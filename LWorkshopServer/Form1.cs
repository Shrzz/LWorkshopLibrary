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
using Newtonsoft.Json;


namespace LWorkshopServer
{
    public partial class Form1 : Form
    {
        Server server;
        List<Book> books = new List<Book>();
        List<User> users = new List<User>();
        public Form1()
        {
            InitializeComponent();
            server = new Server("127.0.0.1", 708, this);
            server.Start();
        }

        private void btnClearGrid_Click(object sender, EventArgs e)
        {
            dgMain.DataSource = null; 
            while (dgMain.Rows.Count > 0)
            {
                dgMain.Rows.RemoveAt(dgMain.Rows.Count-1);
            }
        }


        //чисто чтоб было, обработка с текстбокса
        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            string response = await server.Client(textBox1.Text);
        }

        private async void btnGetUsersList_Click(object sender, EventArgs e)
        {
            string response = await server.Client("get users");
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private async void btnGetBooksList_Click(object sender, EventArgs e)
        {
            string response = await server.Client("get books");
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbMain.Text = "";
        }
    }
}
