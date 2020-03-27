using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer
{
    public class Server
    {
        LibraryContext lb = new LibraryContext();
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

        public async void Start()   //серверная часть
        {
            _listener = TcpListener.Create(_port);
            _listener.Start();
            ConsoleLogger.Write("Сервер запущен", 0, formMain);

            while (true)
            {
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
