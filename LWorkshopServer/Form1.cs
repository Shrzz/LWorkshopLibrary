using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LWorkshopServer
{
    public partial class Form1 : Form
    {
        Server server;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new Server("127.0.0.1", 8888, this);
            }

            server.Start();
            ReceiveData();
        }

        public async void ReceiveData()
        {
            while (true)
            {
                await Task.Run(() => server.GetQuery());
            }
        }


    }
}
