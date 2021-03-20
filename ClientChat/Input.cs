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

namespace ClientChat
{
    public partial class Input : Form
    {
        public string login { get; set; }
        private TcpClient tcp;

        public Input()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            tcp = (Owner as Form1).socket;
            login = textBoxLogin.Text;
            Transfer.SendTCP(tcp, new DataMessage() { Message = login });
            string pass = Hash(textBoxPass.Text);
            Transfer.SendTCP(tcp, new DataMessage() { Message = pass });
            if (((DataMessage)Transfer.ReceiveTCP(tcp)).Message == "No")
            {
                MessageBox.Show("Login or password is not correct, please try again", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLogin.Clear();
                return;
            }
            DialogResult = DialogResult.OK;
        }

        public string Hash(string password)
        {
            byte[] data = Encoding.Default.GetBytes(password);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(data);
            password = Convert.ToBase64String(result);
            return password;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
