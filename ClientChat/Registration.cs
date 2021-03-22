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
            tcp = (Owner as Input).tcp;
            login = new string[2];
            Regex regLog = new Regex("^[A-ZА-Я]{1}\\S{1,8}$");
            Regex regPass = new Regex("^\\S{1,8}$");
            bool res = true;
            while (res)
            {
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
                {
                    while (textBoxPass.Text != textBoxRepeat.Text)
                    {
                        MessageBox.Show("Enter your password again", "Warning",
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxPass.Clear();
                        textBoxRepeat.Clear();
                    }
                    login[1] = Hash(textBoxPass.Text);
                }
                
                Transfer.SendTCP(tcp, new DataMessage() { Array = login });
                if (((DataMessage)Transfer.ReceiveTCP(tcp)).Message == "No")
                {
                    MessageBox.Show("There is such a Login. Please enter a different Login", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    res = true;
                    textBoxLogin.Clear();
                    textBoxPass.Clear();
                    textBoxRepeat.Clear();
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
