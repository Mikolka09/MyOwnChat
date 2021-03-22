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
using System.Net.Sockets;
using ProtocolsMessages;


namespace ClientChat
{
    public partial class Form1 : Form
    {
        private IPAddress iP;
        private IPEndPoint endPoint;
        public TcpClient socket;
        private string address = "127.0.0.1";
        private int port = 1250;
        private string[] login;
        private string[] message;

        public Form1()
        {

            InitializeComponent();

        }

        private void ConnectServer()
        {
            try
            {
                iP = IPAddress.Parse(address);
                endPoint = new IPEndPoint(iP, port);
                socket = new TcpClient();
                socket.Connect(endPoint);
            }
            catch (Exception)
            {
                MessageBox.Show("SERVER IS NOT CONNECTOR", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void StartChat()
        {
            buttonSend.Enabled = true;
            textBoxMessage.Enabled = true;
            ReceiveAsync();
        }

        private async void ReceiveAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            string[] mess = new string[4];
                            mess = ((DataMessage)Transfer.ReceiveTCP(socket)).Array;
                            listViewMessages.Items.Add(mess[0]);
                        }
                        catch (Exception) { }
                    }
                });
            }
            catch (Exception) { throw; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            login = new string[2];
            ConnectServer();
            Input inp = new Input();
            if (inp.ShowDialog(this) == DialogResult.OK)
            {
                if (socket.Connected)
                {
                    login = inp.login;
                    message = new string[4];
                    message[1] = "other";
                    message[2] = login[0];
                    message[3] = "";
                    listViewMessages.Items.Clear();
                    listViewMessages.Items.Add("SERVER CONNEСTOR");
                    listViewMessages.Items[0].ForeColor = Color.Red;
                    buttonSend.Enabled = true;
                    textBoxMessage.Enabled = true;
                    this.Text = "MYOWNCHAT: " + $"Login - {login[0]}";
                    StartChat();
                }
            }
            else
            {
                inp.Close();
                Close();
            }

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                message[0] = textBoxMessage.Text;
                Transfer.SendTCP(socket, new DataMessage() { Array = message });
                textBoxMessage.Clear();
            }
            catch (Exception) { throw; }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                message[0] = "";
                message[1] = "exit";
                buttonSend_Click(this, new EventArgs());
                socket.SendTimeout = 500;
                socket.Close();
                Close();
            }
            else
                Close();
        }

        private int FindIndexListView(string mess)
        {
            int index = 0;
            foreach (ListViewItem item in listViewMessages.Items)
            {
                if (item.ToString() == mess)
                    index = listViewMessages.Items.IndexOf(item);
            }
            return index;
        }

        private void buttonInquiry_Click(object sender, EventArgs e)
        {
            message[1] = "private";
            message[2] = login[0];
            message[3] = textBoxName.Text;
            textBoxName.Enabled = false;
            buttonInquiry.Enabled = false;
            buttonCancelPrivateChat.Enabled = true;
        }

        private void buttonCancelPrivateChat_Click(object sender, EventArgs e)
        {
            message[1] = "other";
            message[2] = login[0];
            message[3] = "";
            textBoxName.Clear();
            textBoxName.Enabled = true;
            buttonInquiry.Enabled = true;
            buttonCancelPrivateChat.Enabled = false;
        }
    }


}
