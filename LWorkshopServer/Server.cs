using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LWorkshopServer 
{
    public class Server 
    {
        private Form1 formMain;
        private TcpListener _listener;

        private string _ip;
        private int _port;

        public Server(string ip, int port, Form1 formMain)
        {
            _ip = ip;
            _port = port;
            this.formMain = formMain;
        }
        private static readonly string ServerResponseString = "чё те надо блять";
        private static readonly byte[] ServerResponseBytes = Encoding.UTF8.GetBytes(ServerResponseString);

        private static readonly string ClientRequestString = "get users";
        private static readonly byte[] ClientRequestBytes = Encoding.UTF8.GetBytes(ClientRequestString);

        async public void Start()
        {
            TcpListener _listener = TcpListener.Create(_port);
            _listener.Start();
            ConsoleLogger.Write("Сервер запущен", "server", formMain);

            TcpClient client = await _listener.AcceptTcpClientAsync();
            ConsoleLogger.Write("Клиент подключился", "server", formMain);
            using (NetworkStream stream = client.GetStream())
            {
                var buffer = new byte[1024];
                ConsoleLogger.Write("Получает информацию от клиента", "server", formMain);
                var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                var request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                ConsoleLogger.Write($"Клиент отправил сообщение '{request}'", "server", formMain);
                await stream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                ConsoleLogger.Write($"Ответ отправлен","server", formMain);
            }  
        }

        public async void Client()
        {
            using (var client = new TcpClient())
            {
                ConsoleLogger.Write("Подключение к серверу", "client", formMain);
                await client.ConnectAsync(IPAddress.Parse(_ip), _port);
                ConsoleLogger.Write("Успешно подключён", "client", formMain);
                using (var networkStream = client.GetStream())
                {
                    ConsoleLogger.Write("Oтправка сообщения", "client", formMain);
                    await networkStream.WriteAsync(ClientRequestBytes, 0, ClientRequestBytes.Length);
                    var buffer = new byte[4096];
                    var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    var response = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ConsoleLogger.Write("Oтвет сервера: "+ response, "client", formMain);
                }
            }
        }

        /*public void SendUsersList(int index)
        {
            NetworkStream stream = clients[index].GetStream();
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetUsers()));
            stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Отправлен список пользователей");
            stream.Close();
            clients[index].Close();
            clients.Remove(clients[index]);
        }*/

       /* public void SendBooksList(int index)
        {
            NetworkStream stream = clients[index].GetStream();
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetBooks()));
            stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Отправлен список книг");
            stream.Close();

        }*/

        public List<User> GetUsers()
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
        }

        public void Close()
        {
            _listener.Stop();
        }
    }
}
