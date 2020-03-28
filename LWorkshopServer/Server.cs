using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LWorkshopServer
{
    public class Server
    {
        List<NetworkStream> clients = new List<NetworkStream>();
        LibraryContext lb = new LibraryContext();
        private Form1 formMain;
        private TcpListener _listener;
        private int _port;

        public Server(int port, Form1 formMain)
        {
            _port = port;
            this.formMain = formMain;
        }

        public async void Start(  )   //серверная часть
        {
            _listener = TcpListener.Create(_port);
            _listener.Start();

            ConsoleLogger.Write("Сервер запущен", 0, formMain);

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                
                ConsoleLogger.Write("Клиент подключился", 0, formMain);

                new Thread(() => { HandleClient(client); }).Start(); 
            }
        }

        public void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                clients.Add(stream);

                var buffer = new byte[1024];
                int byteCount = 0;
                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string clientRequest = Encoding.UTF8.GetString(buffer, 0, byteCount);

                    if (clientRequest.ToLower().Contains("get"))
                    {
                        if (clientRequest.ToLower().Contains("book"))
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
        }


        public void SendBooksList(NetworkStream stream)       //метод для сервера
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetBooksList()));
            stream.Write(responseBytes, 0, responseBytes.Length);      //был асинхронным
            ConsoleLogger.Write($"Отправлен список книг", 0, formMain);
        }

        public void SendUsersList(NetworkStream stream)           //метод для сервера
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GetUsersList()));
            stream.Write(responseBytes, 0, responseBytes.Length);          //был асинхронным
            ConsoleLogger.Write($"Отправлен список пользователей", 0, formMain);
        }

        public List<User> GetUsersList()
        {
            return lb.Users.ToList();
        }

        public List<Book> GetBooksList()
        {
            return lb.Books.ToList();
        }

        public void Close()
        {
            _listener.Stop();
        }
    }
}
