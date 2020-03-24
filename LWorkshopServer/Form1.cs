using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;


namespace LWorkshopServer
{
    public partial class Form1 : Form
    {
        Server server;  
        
        public Form1()
        {
            InitializeComponent();
            server = new Server("127.0.0.1", 708, this);
            server.Start();
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {


            //GetQuery();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.Client();
        }
    }
}
