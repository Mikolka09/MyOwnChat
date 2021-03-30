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
    public partial class Registration : Form
    {
        public User user { get; set; }
        private TcpClient tcp;

        public Registration()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Regex regLog = new Regex("^[A-ZА-Я]{1}\\S{1,8}$");
            Regex regPass = new Regex("^\\S{1,8}$");

            tcp = (Owner as Input).tcp;

            user = new User();

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
                    user.Login = textBoxLogin.Text;
                user.Tag = "avtorization";
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
                    user.Password = Hash(textBoxPass.Text);
                }
                user.Birthday = dateTimePickerBirthday.Value.ToShortDateString();

                Message answer = new Message();
                Transfer.SendTCP(tcp, new DataUser() { User = user });
                Data data = Transfer.ReceiveTCP(tcp);
                if (data is DataMessage)
                    answer = ((DataMessage)data).Message;
                if (answer.Answer == "No")
                {
                    MessageBox.Show("There is such a Login. Please enter a different Login", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    res = true;
                    textBoxLogin.Clear();
                    textBoxPass.Clear();
                    textBoxRepeat.Clear();
                    dateTimePickerBirthday.Format = DateTimePickerFormat.Custom;
                }
                else
                    res = false;
            }
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

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
