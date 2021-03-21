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
                if (socket.Connected)
                {
                    listViewMessages.Items.Clear();
                    listViewMessages.Items.Add("SERVER CONNEСTOR");
                    listViewMessages.Items[0].ForeColor = Color.Red;
                    buttonConnect.Enabled = false;  
                    buttonInput.Enabled = true;
                    buttonRegistration.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SERVER IS NOT CONNECTOR", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listViewMessages.Items.Add("SERVER IS NOT CONNECTOR");
                listViewMessages.Items[0].ForeColor = Color.Red;
                buttonInput.Enabled = false;
                buttonRegistration.Enabled = false;
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
                        string message = ((DataMessage)Transfer.ReceiveTCP(socket)).Message;
                        listViewMessages.Items.Add(message);
                    }
                });
            }
            catch (Exception) { throw; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonSend.Enabled = false;
            textBoxMessage.Enabled = false;
            ConnectServer();
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            Input input = new Input();
            if (input.ShowDialog(this) == DialogResult.OK)
            {
                login = input.login;
                buttonInput.Enabled = false;
                buttonRegistration.Enabled = false;
                this.Text = "MYOWNCHAT: " + $"Login - {login[0]}";
                StartChat();
            }
            else
            {
                buttonInput.Enabled = true;
                buttonRegistration.Enabled = true;
            }
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            if (reg.ShowDialog(this) == DialogResult.OK)
            {
                login = reg.login;
                buttonInput.Enabled = false;
                buttonRegistration.Enabled = false;
                this.Text = "MYOWNCHAT: " + $"Login - {login[0]}";
                StartChat();
            }
            else
            {
                buttonInput.Enabled = true;
                buttonRegistration.Enabled = true;
            }


        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = textBoxMessage.Text;
                Transfer.SendTCP(socket, new DataMessage() { Message = message });
                textBoxMessage.Clear();
            }
            catch (Exception) { throw; }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                Transfer.SendTCP(socket, new DataMessage() { Message = "exit" });
                socket.Close();
                Close();
            }
            else
                Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ConnectServer();
        }
    }
}
