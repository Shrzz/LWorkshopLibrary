using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace LWorkshopServer.domain
{
    public class SocketString 
    {
        public Encoding CurrentEncoding { get; set; }
        byte[] buffer = new byte[102400];
        Socket socket;


        public SocketString(Socket socket)
        {
            CurrentEncoding = Encoding.UTF8;
            this.socket = socket;
            
        }
        public EndPoint RemoteEndPoint
        {
            get => socket.RemoteEndPoint;
        }

        public void Listen(int amount)
        {
            socket.Listen(amount);
        }

        public void Bind(IPEndPoint ipPoint)
        {
            socket.Bind(ipPoint);
        }



        public void ChangeBuffer(int size)
        {
            if (size > 0)
            {
                buffer = new byte[size];
            }
        }

        public void Send(string message)
        {
            socket.Send(CurrentEncoding.GetBytes(message));
        }

        public string Receive()
        {
            int bytes;
            StringBuilder result = new StringBuilder();
            do
            {
                bytes = socket.Receive(buffer);
                result.Append(CurrentEncoding.GetString(buffer, 0, bytes));
            } while (socket.Available > 0);

            return result.ToString();
        }

        public SocketString Accept()
        {
            return new SocketString(socket.Accept());
        }  

        public void Shutdown(SocketShutdown s)
        {
            socket.Shutdown(s);
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
