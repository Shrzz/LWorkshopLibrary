using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LWorkshopServer
{
    public class Server
    {
        private Form1 formMain;
        private TcpListener _listener;
        private List<Book> books;
        private List<User> users;


        private string _ip;
        private int _port;

        public Server(string ip, int port, Form1 formMain)
        {
            _ip = ip;
            _port = port;
            this.formMain = formMain;
            books = new List<Book>();
            for (int i = 0; i < 10; i++)
            {
                Book b = new Book();
                b.Author = "a" + 1;
                b.Id = i;
                b.Name = "name" + i;
                b.NumOfBooks = i * i;
                b.PublishingDate = DateTime.Now;
                books.Add(b);
            }

            users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                User u = new User();
                u.Id = i;
                u.Issuances = null;
                u.Name = "name" + i;
                users.Add(u);
            }
        }

        public async void Start()   //серверная часть
        {
            TcpListener _listener = TcpListener.Create(_port);
            _listener.Start();
            ConsoleLogger.Write("Сервер запущен", 0, formMain);

            TcpClient client = await _listener.AcceptTcpClientAsync();
            ConsoleLogger.Write("Клиент подключился", 0, formMain);
            using (NetworkStream stream = client.GetStream())
            {
                var buffer = new byte[1024];
                ConsoleLogger.Write("Получает информацию от клиента", 0, formMain);
                var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                string clientRequest = Encoding.UTF8.GetString(buffer, 0, byteCount);
                ConsoleLogger.Write($"Клиент отправил сообщение '{clientRequest}'", 0, formMain);

                //обработчик запроса сюды//

                if (clientRequest.Contains("get"))
                {
                    if (clientRequest.Contains("book"))
                    {
                        SendBooksList(stream);
                    }
                    else
                    {
                        SendUsersList(stream);
                    }        
                }
            }
        }

        public async Task<string> Client(string query)          //клиентская часть
        {
            string serverResponse;
            using (var client = new TcpClient())
            {
                ConsoleLogger.Write("Подключение к серверу", 1, formMain);
                await client.ConnectAsync(IPAddress.Parse(_ip), _port);
                ConsoleLogger.Write("Успешно подключён", 1, formMain);
                byte[] byteQuery = Encoding.UTF8.GetBytes(query);
                using (var networkStream = client.GetStream())
                {
                    ConsoleLogger.Write("Oтправка сообщения", 1, formMain);
                    await networkStream.WriteAsync(byteQuery, 0, byteQuery.Length);
                    var buffer = new byte[4096];
                    var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    serverResponse = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ConsoleLogger.Write("Oтвет убил: " + Encoding.UTF8.GetString(buffer, 0, byteCount), 1, formMain);
                }
            }
            return serverResponse;
        }

         public async void SendBooksList(NetworkStream stream)       //метод для сервера
         {
             byte[] responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(books).ToString());
             await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
             ConsoleLogger.Write($"Отправлен список книг", 0, formMain);
         }

         public async void SendUsersList(NetworkStream stream)           //метод для сервера
         {
             byte[] responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(users).ToString());
             await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
             ConsoleLogger.Write($"Отправлен список пользователей", 0, formMain);
         }

        //это потом
        /*  public List<User> GetUsers()
          {
              using (LibraryContext lb = new LibraryContext())
              {
                  return lb.Users.ToList<User>();
              }

          }

          public List<Book> GetBooks()
          {
              using (LibraryContext lb = new LibraryContext())
              {
                  return lb.Books.ToList<Book>();
              }
          }*/

        public void Close()
        {
            _listener.Stop();
        }
    }
}
