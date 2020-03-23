using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace LWorkshopServer 
{
    public class Server 
    {
        private Form1 formMain;
        private TcpListener _listener;
        private List<TcpClient> clients;
        private string _ip;
        private int _port;
        private readonly int _connectionsAmount = 10;

        public Server(string ip, int port, Form1 formMain)
        {
            _ip = ip;
            _port = port;
            this.formMain = formMain;
        }

        public void Start()
        {
            TcpListener t = new TcpListener(IPAddress.Parse(_ip), _port);
            t.Start(_connectionsAmount);
            t.AcceptTcpClientAsync();
            formMain.rtbMain.Text += DateTime.Now.ToShortTimeString() + ": Сервер запущен для " + _connectionsAmount + "соединений";
            clients = new List<TcpClient>();
        }

        public void GetQuery()   ////////////////
        {
            TcpClient client = _listener.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine(DateTime.Now.ToShortTimeString() + "Клиент подключен");
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytes = 0;
            StringBuilder builder = new StringBuilder();

            do
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
                builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
            }
            while (stream.DataAvailable);

            formMain.rtbMain.Text += ("Получено " + DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
        }

        public void SendUsersList(int index)
        {
            NetworkStream stream = clients[index].GetStream();
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetUsers()));
            stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Отправлен список пользователей");
            stream.Close();
            clients[index].Close();
            clients.Remove(clients[index]);
        }

        public void SendBooksList(int index)
        {
            NetworkStream stream = clients[index].GetStream();
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetBooks()));
            stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Отправлен список книг");
            stream.Close();
            clients[index].Close();
            clients.Remove(clients[index]);
        }

        public DbSet<User> GetUsers()
        {
            using (UserContext uc = new UserContext())
            {
                return uc.Users;
            }

        }

        public DbSet<Book> GetBooks()
        {
            using (BookContext bc = new BookContext())
            {
                return bc.Books;
            }
        }

    }
}
