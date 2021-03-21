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

namespace ClientChat
{
    public partial class Input : Form
    {
        public string[] login { get; set; }
        private TcpClient tcp;

        public Input()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            tcp = (Owner as Form1).socket;
            Regex regLog = new Regex("^[A - zА - я]([A - z0 - 9А - я]{ 1, 9 })$");
            Regex regPass = new Regex("^[A - zА - я]([A - z0 - 9А - я]{ 2, 8 })$");
            login = new string[2];
            if (!regLog.IsMatch(textBoxLogin.Text))
            {
                MessageBox.Show("Login entered incorrectly", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                login[0] = textBoxLogin.Text;
            if (!regPass.IsMatch(textBoxPass.Text))
            {
                MessageBox.Show("Password entered incorrectly", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
                login[1] = Hash(textBoxPass.Text);

            Transfer.SendTCP(tcp, new DataMessage() { Array = login });
            if (((DataMessage)Transfer.ReceiveTCP(tcp)).Message == "No")
            {
                MessageBox.Show("Login or password is not correct, please try again", "Warning",
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
