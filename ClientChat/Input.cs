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
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Message = ProtocolsMessages.Message;

namespace ClientChat
{
    public partial class Input : Form
    {
        public User user { get; set; }
        public TcpClient tcp;

        public Input()
        {
            InitializeComponent();
           
        }
            
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Regex regLog = new Regex("^[A-ZА-Я]{1}\\S{1,8}$");
            Regex regPass = new Regex("^\\S{1,8}$");
            user = new User();
            if (!regLog.IsMatch(textBoxLogin.Text))
            {
                MessageBox.Show("Login entered incorrectly", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                user.Login = textBoxLogin.Text;
            user.Tag = "avtorization";
            if (!regPass.IsMatch(textBoxPass.Text))
            {
                MessageBox.Show("Password entered incorrectly", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                user.Password = Hash(textBoxPass.Text);
            Message answer = new Message();
            Transfer.SendTCP(tcp, new DataUser() { User = user });
            Data data = Transfer.ReceiveTCP(tcp);
            if(data is DataMessage)
                answer = ((DataMessage)data).Message;
            if (answer.Answer == "No")
            {
                MessageBox.Show("Login or password is not correct, or such a login is already in the chat", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLogin.Clear();
                textBoxPass.Clear();
                return;
            }
            else
                DialogResult = DialogResult.OK;
        }

        public string Hash(string password)
        {
            byte[] data = Encoding.Default.GetBytes(password);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] hash = sha256.ComputeHash(data);
            string pass = Convert.ToBase64String(hash);
            return pass;
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Registration reg = new Registration();
            if (reg.ShowDialog(this) == DialogResult.OK)
            {
                user = reg.user;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (tcp.Connected)
            {
                tcp.SendTimeout = 500;
                tcp.Close();
                Close();
            }
            else
                Close();
        }

        private void Input_Load(object sender, EventArgs e)
        {
            tcp = (Owner as Form1).socket;
        }
    }
}
