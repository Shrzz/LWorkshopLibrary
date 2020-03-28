using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWorkshopServer
{
    public static class ConsoleLogger
    {

        public static void Write(string message, int type, Form1 form)
        {
            if (type == 0)
            {
                form.rtbMain.Text += $"{DateTime.Now.ToLongTimeString()} [SERVER]: {message}\n";
            }
            else
            {
                form.rtbMain.Text += $"{DateTime.Now.ToLongTimeString()}: {message}\n";
            }
        }
    }
}
