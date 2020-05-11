using LWorkshopServer.domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//пофиксить логгер
//уведомление всех пользователей об изменении списка книг


namespace LWorkshopServer
{
    public class Server
    {
        static LibraryContext context = new LibraryContext();
        private TcpListener _listener;
        private int _port;
        private IPEndPoint serverIP;


        private List<TcpClient> clients;

        public Server(int port)
        {
            _port = port;
            serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
        }

        public void Start()
        {
            SocketString listener = new SocketString(new Socket(SocketType.Stream, ProtocolType.Tcp));
            listener.Bind(serverIP);
            listener.Listen(10);



            while (true)
            {
                SocketString handler = listener.Accept();
                Logger.Log.Add($"new client {handler.RemoteEndPoint}");
                new Task(new ClientHandler(handler).Run).Start();
            }
        }

        class ClientHandler
        {
            SocketString handler;
            byte[] buffer = new byte[1048576];
            string SRequest;
            int byteCount;
            Request r;

            public ClientHandler(SocketString handler)
            {
                this.handler = handler;
            }

            public void Run()
            {
                while ((SRequest = handler.Receive()).Length != 0)
                {
                    r = JsonConvert.DeserializeObject<Request>(SRequest);

                    Logger.Log.Add($"Запрос от {r.Login.Login}: {r.Query}");
                    var rsp = GetResponse(r);

                    Logger.Log.Add($"Ответ {r.Login.Login}: {(rsp != "InvalidQuery")}");

                    handler.Send(rsp);

                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

            private static bool isRegistered(UserLogin ul)
            {
                foreach (var login in context.Logins)
                {
                    if (ul.canLogin(ul.Login, ul.Password))
                    {
                        return true;
                    }
                }

                return false;
            }

            private string GetResponse(Request r)
            {
                Regex word = new Regex("\\w+");
                Regex num = new Regex("[0-9]+");
                Regex json = new Regex("({.+})|([.+])|(\".+\")");
                string query = r.Query;
                bool isReg = isRegistered(r.Login);
                bool isLib = isReg ? r.Login.IsLibrarian : false;
                string defaultresp = "AccessDenied";
                string response;
                string q = word.Match(query).Value;

                switch (q)
                {
                    case "Login":
                        string login = Regex.Match(query, "\\|.+\\|").Value;
                        string pass = Regex.Match(query, "\\|.+?$", RegexOptions.RightToLeft).Value;
                        response = Login(login.Substring(1, login.Length - 2), pass.Substring(1));
                        break;
                    case "GetBooks":
                        response = isReg ? GetBooks() : defaultresp;
                        break;
                    case "GetUsers":
                        response = isLib ? GetUsers() : defaultresp;
                        break;
                    case "DelUser":
                        response = isLib ? DelUser(int.Parse(num.Match(query).Value)) : defaultresp;
                        break;
                    case "DelBook":
                        response = isLib ? DelBook(int.Parse(num.Match(query).Value)) : defaultresp;
                        break;
                    case "AddBook":
                        response = isLib
                            ? AddBook(JsonConvert.DeserializeObject<Book>(json.Match(query).Value))
                            : defaultresp;
                        break;
                    case "AddUser":
                        response = isLib
                            ? AddUser(JsonConvert.DeserializeObject<UserLogin>(json.Match(query).Value))
                            : defaultresp;
                        break;
                    case "GiveBook":
                        response = isLib
                            ? GiveBook(int.Parse(num.Match(query).Value), int.Parse(num.Matches(query)[1].Value),
                            JsonConvert.DeserializeObject<DateTime>(json.Match(query).Value))
                            : defaultresp;
                        break;
                    case "GetBook":
                        response = isLib
                            ? GetBook(int.Parse(num.Match(query).Value), int.Parse(num.Matches(query)[1].Value))
                            : defaultresp;
                        break;
                    default:
                        response = "InvalidQuery";
                        break;
                }

                return response;
            }
        }

        public static string Login(string login, string pass)
        {
            bool flag = false;
            UserLogin ul = null;
            foreach (var l in context.Logins)
            {
                if (l.canLogin(login, pass))
                {
                    flag = true;
                    ul = l;
                    break;
                }
            }

            return $"{flag}|{(flag ? JsonConvert.SerializeObject(ul) : "")}";
        }

        public static string GiveBook(int userId, int bookId, DateTime deadLine)
        {
            User user = context.Users.Find(userId);
            Book book = context.Books.Find(bookId);

            Issuance issuance = UserForGrid.FindUserIssuance(user, book);
            if (issuance != null || book == null || user == null || book.NumOfBooks == 0)
            {
                return "CantGiveBook";
            }

            issuance = new Issuance() { Book = book, IssuanceDate = DateTime.Now, Deadline = deadLine };
            book.NumOfBooks--;
            user.Issuances.Add(issuance);
            context.SaveChanges();

            return "BookWasGiven";
        }

        public static string GetBook(int userId, int bookId)
        {
            int ddays = 0;

            User user = context.Users.Find(userId);
            Book book = context.Books.Find(bookId);

            Issuance issuance = UserForGrid.FindUserIssuance(user, book);
            if (issuance == null)
            {
                ddays = -1;
            }

            if (new UserForGrid(user).HasDebts())
            {
                ddays = issuance.GetDebtDays();
            }

            book.NumOfBooks++;
            context.Issuances.Remove(issuance);
            user.Issuances.Remove(issuance);

            context.SaveChanges();

            return $"BookWasReturned|{ddays}";
        }

        public static string AddBook(Book b)
        {
            context.Books.Add(b);
            context.SaveChanges();

            return "BookWasAdded";
        }

        public static string AddUser(UserLogin l)
        {
            context.Users.Add(l.User);
            context.Logins.Add(l);
            context.SaveChanges();

            return "UserWasAdded";
        }

        public static string DelUser(int id)
        {
            User delUser = context.Users.Find(id);
            foreach (Issuance i in delUser.Issuances)
            {
                context.Issuances.Remove(i);
            }

            foreach (var login in context.Logins)
            {
                if (login.User == delUser)
                {
                    context.Logins.Remove(login);
                    break;
                }
            }

            context.Users.Remove(delUser);
            context.SaveChanges();
            return "UserWasDeleted";
        }

        public static string DelBook(int id)
        {
            Book delBook = context.Books.Find(id);
            foreach (Issuance i in context.Issuances)
            {
                if (i.Book == delBook)
                    context.Issuances.Remove(i);
            }
            context.Books.Remove(delBook);
            context.SaveChanges();

            return "BookWasDeleted";
        }

        public static string GetUsers()
        {
            var users = LoadUsers();
            return JsonConvert.SerializeObject(users);
        }

        public static string GetBooks()
        {

            return JsonConvert.SerializeObject(context.Books.ToList());

        }

        private static ObservableCollection<UserForGrid> LoadUsers()
        {
            context.Users.Load();
            context.Issuances.Load();

            UserForGrid userForGreed;
            ObservableCollection<UserForGrid> bind = new ObservableCollection<UserForGrid>();
            var users = context.Users.Local.ToBindingList();
            var libraryian = context.Logins.Where(u => u.IsLibrarian).First();
            foreach (User u in users)
            {
                if (u == libraryian.User)
                    continue;
                userForGreed = new UserForGrid() { User = u, Books = new List<Book>() };
                foreach (Issuance i in u.Issuances)
                {
                    userForGreed.Books.Add(i.Book);
                }

                bind.Add(userForGreed);
            }

            return bind;
        }

        public void Close()
        {
            //SqlDependency.Stop(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);
            _listener.Stop();
        }
    }
}
