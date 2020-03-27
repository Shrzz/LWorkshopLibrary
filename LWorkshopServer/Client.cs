using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;



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


        public async Task<string> SendMessage(string query)          //клиентская часть
        {
            string serverResponse = "";
            using (var client = new TcpClient())
            {
                ConsoleLogger.Write("Подключение к серверу", 1, formMain);
                await client.ConnectAsync(_ip, _port);
                ConsoleLogger.Write("Успешно подключён", 1, formMain);
                byte[] byteQuery = Encoding.UTF8.GetBytes(query);
                using (var networkStream = client.GetStream())
                {
                    ConsoleLogger.Write("Oтправка сообщения", 1, formMain);
                    await networkStream.WriteAsync(byteQuery, 0, byteQuery.Length);
                    var buffer = new byte[2048];
                    var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    serverResponse = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    ConsoleLogger.Write("Oтвет убил: " + Encoding.UTF8.GetString(buffer, 0, byteCount), 1, formMain);
                }
            }
            return serverResponse;
        }

    }
}
