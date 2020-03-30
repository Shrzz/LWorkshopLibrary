using LWorkshopServer.domain;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace LWorkshopServer
{
    public partial class Form1 : Form
    {
        Server server;
        LibraryContext context = new LibraryContext(); // отладка

        public Form1()
        {
            InitializeComponent();
            Logger.Log.ListChanged += LogChangedEventHandler;
            new Thread(() =>
            {
                server = new Server(8888);
                server.Start();
            }).Start();
            
            Logger.Log.Add("Сервер запущен на порте 8888");
        }

        private void LogChangedEventHandler(object sender, ListChangedEventArgs e)
        {
            try
            {
                rtbMain.Text += Logger.Log[e.NewIndex] + "\n";
            }
            catch (Exception ex)
            {
                
            }
        }

        private void BtnClearGrid_Click(object sender, EventArgs e)
        {
            dgMain.DataSource = null;
            while (dgMain.Rows.Count > 0)
            {
                dgMain.Rows.RemoveAt(dgMain.Rows.Count - 1);
            }
        }

        //обработка запроса из текстбокса
        private async void BtnSendMessage_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 8888);
            string response = await c.SendMessage(textBox1.Text, context.Logins.Where((l) => l.IsLibrarian == true).FirstOrDefault());
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private async void BtnGetUsersList_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 8888);
            string response = await c.SendMessage("GetUsers", context.Logins.Where((l) => l.IsLibrarian == true).FirstOrDefault());
            var result = JsonConvert.DeserializeObject<ObservableCollection<UserForGrid>>(response);
            dgMain.DataSource = result;
        }

        private async void BtnGetBooksList_Click(object sender, EventArgs e)
        {
            Client c = new Client(this, "127.0.0.1", 8888);
            string response = await c.SendMessage("GetBooks", context.Logins.Where((l) => l.IsLibrarian == true).FirstOrDefault());
            var result = JsonConvert.DeserializeObject(response);
            dgMain.DataSource = result;
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            rtbMain.Text = "";
        }
    }
}
