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
    public partial class AddClient : Form
    {
        public Client client { get; set; }
        public string[] login { get; set; }
        private TcpClient tcp;

        public AddClient()
        {
            InitializeComponent();
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Regex regLog = new Regex("^[A-ZА-Я]{1}\\S{1,8}$");
            Regex regPass = new Regex("^\\S{1,8}$");

            if (client.Name == null)
            {
                tcp = (Owner as Form1).socket;
                login = new string[5];
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
                    login[1] = "create";
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
                        login[2] = Hash(textBoxPass.Text);
                    }
                    login[3] = dateTimePickerBirthday.Value.ToShortDateString();

                    string[] answer = new string[5];
                    Transfer.SendTCP(tcp, new DataMessage() { Array = login });
                    answer = ((DataMessage)Transfer.ReceiveTCP(tcp)).Array;
                    if (answer[4] == "No")
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
            }
            else
            {

                if (!regLog.IsMatch(textBoxLogin.Text))
                {
                    MessageBox.Show("Login entered incorrectly", "Warning",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                    client.Name = textBoxLogin.Text;
                if(client.Name == "Admin")
                {
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
                        client.Password = Hash(textBoxPass.Text);
                    }
                }
                client.Birthday = dateTimePickerBirthday.Value.ToShortDateString();

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

        private void AddClient_Load(object sender, EventArgs e)
        {
            client = new Client();
            client = (Owner as Form1).client;
            if (client.Name != null)
            {
                textBoxLogin.Text = client.Name;
                if(client.Name == "Admin")
                {
                    textBoxPass.Enabled = true;
                    textBoxRepeat.Enabled = true;
                }
                else
                {
                    textBoxPass.Enabled = false;
                    textBoxRepeat.Enabled = false;
                } 
                if (client.Birthday != "")
                    dateTimePickerBirthday.Value = Convert.ToDateTime(client.Birthday);
            }
        }
    }
}
