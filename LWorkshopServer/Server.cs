using LWorkshopServer.domain;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static System.Text.Encoding;

namespace LWorkshopServer
{
    public class Server
    {
        LibraryContext context = new LibraryContext();
        private TcpListener _listener;
        private int _port;

        public Server(int port)
        {
            _port = port;
        }

        public async void Start()
        {
            _listener = TcpListener.Create(_port);
            _listener.Start();

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                //Logger.Log.Add($"new client {_listener.LocalEndpoint}");
                new Task(() => { HandleClient(client); }).Start();
            }
        }

        public void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                var buffer = new byte[1024];
                string SRequest;
                int byteCount;
                Request r;

                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    SRequest = UTF8.GetString(buffer, 0, byteCount);
                    r = JsonConvert.DeserializeObject<Request>(SRequest);

                    if (!isRegistered(r.Login))
                    {
                        client.Close();
                        return;
                    }

                    Logger.Log.Add($"Запрос от {r.Login.Login}: {r.Query}");
                    var response = UTF8.GetBytes(GetResponse(r.Query, r.Login.IsLibrarian));
                    Logger.Log.Add($"Ответ {r.Login.Login}: {(UTF8.GetString(response) != "InvalidQuery")}");
                    
                    stream.Write(response, 0, response.Length);
                }

                client.Close();
            }
        }

        private bool isRegistered(UserLogin ul)
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

        private string GetResponse(string query, bool isLibraryian)
        {
            string defaultresp = "AccessDenied";
            string response;
            switch (query)
            {
                case "GetBooks":
                    response = GetBooks();
                    break;
                case "GetUsers":
                    response = isLibraryian ? GetUsers() : defaultresp;
                    break;
                default:
                    response = "InvalidQuery";
                    break;
            }

            return response;
        }

        public string GetUsers()
        {
            return JsonConvert.SerializeObject(context.Users.ToList());
        }

        public string GetBooks()
        {
            return JsonConvert.SerializeObject(context.Books.ToList());
        }

        public void Close()
        {
            _listener.Stop();
        }
    }
}
