using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer
{
    public static class ConsoleLogger
    {

        public static void Write(string message, string type, Form1 form)
        {
            if (type == "server")
            {
                form.rtbMain.Text += $"[SERVER] {DateTime.Now.ToLongTimeString()}: {message}\n";
            }
            else
            {
                form.rtbMain.Text += $"{DateTime.Now.ToLongTimeString()}: {message}\n";
            }

        }
    }
}
