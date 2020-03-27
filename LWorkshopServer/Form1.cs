﻿using System;
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
        
        public Form1()
        {
            InitializeComponent();
            server = new Server("127.0.0.1", 708, this);
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
