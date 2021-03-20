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
    public partial class Registration : Form
    {
        public string[] login { get; set; }
        private TcpClient tcp;

        public Registration()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            tcp = (Owner as Form1).socket;
            login = new string[2];
            bool res = true;
            while (res)
            {            
                login[0] = textBoxLogin.Text;
                while (textBoxPass.Text != textBoxRepeat.Text)
                {
                    MessageBox.Show("Enter your password again", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPass.Clear();
                    textBoxRepeat.Clear();
                }
                login[1] = Hash(textBoxPass.Text);
                Transfer.SendTCP(tcp, new DataMessage() { Array = login });
                if (((DataMessage)Transfer.ReceiveTCP(tcp)).Message == "No")
                {
                    MessageBox.Show("There is such a Login. Please enter a different Login", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    res = true;
                    textBoxLogin.Clear();
                }
                else
                    res = false;
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
