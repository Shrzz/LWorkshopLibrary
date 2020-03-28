using Newtonsoft.Json;
using System;
using System.Windows.Forms;


namespace LWorkshopServer
{
    public partial class Form1 : Form
    {
        Server server;
        
        public Form1()
        {
            InitializeComponent();
            server = new Server(708, this);
            server.Start();
        }

        private void BtnClearGrid_Click(object sender, EventArgs e)
        {
            dgMain.DataSource = null; 
            while (dgMain.Rows.Count > 0)
            {
                dgMain.Rows.RemoveAt(dgMain.Rows.Count-1);
            }
        }

        //обработка запроса из текстбокса
        private async void BtnSendMessage_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 708);
            string response = await c.SendMessage(textBox1.Text);
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private async void BtnGetUsersList_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 708);
            string response = await c.SendMessage("get users");
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private async void BtnGetBooksList_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 708);
            string response = await c.SendMessage("get books");
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            rtbMain.Text = "";
        }
    }
}
