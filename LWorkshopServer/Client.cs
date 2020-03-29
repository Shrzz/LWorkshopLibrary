using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LWorkshopServer.domain;
using Newtonsoft.Json;

namespace LWorkshopServer
{
    public class Client
    {
        private Form1 formMain;
        private IPAddress _ip;
        private int _port;

        public Client(Form1 formMain, string ip, int port)
        {
            this.formMain = formMain;
            _ip = IPAddress.Parse(ip);
            _port = port;
        }

        public async Task<string> SendMessage(string query, UserLogin login)          //клиентская часть
        {
            string serverResponse = "";
            using (var client = new TcpClient())
            {

                await client.ConnectAsync(_ip, _port);

                Request r = new Request() { Query = query, Login = login};
                byte[] byteQuery = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(r));
                
                using (var networkStream = client.GetStream())
                {
                    await networkStream.WriteAsync(byteQuery, 0, byteQuery.Length);
                    var buffer = new byte[1024];
                    var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    serverResponse = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Logger.Log.Add($"Ответ получен : {login.Login}");
                }
            }
            return serverResponse;
        }
    }
}
