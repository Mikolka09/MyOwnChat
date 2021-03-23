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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading;


namespace ClientChat
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]

        private static extern bool ShowScrollBar(IntPtr hwnd, int wBar,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool bShow);

        private IPAddress iP;
        private IPEndPoint endPoint;
        public TcpClient socket;
        private string address = "127.0.0.1";
        private int port = 1250;
        private string[] login;
        private string[] message;
        private List<Contact> contacts;
        private List<Contact> tempContacts;
        private string pathFile = "contacts.json";
        private Random rand;

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

        private void HideHorizontalScrollBar()
        {
            ShowScrollBar(listViewMessages.Handle, 0, false);
        }

        private void StartChat()
        {
            buttonSend.Enabled = true;
            textBoxMessage.Enabled = true;
            ReceiveAsync();
        }

        private Color ContactColor(string buff, Color color)
        {
            if (tempContacts.Count != 0)
            {
                if (tempContacts.Exists((x) => x.Name == buff))
                    return color = tempContacts.Find((x) => x.Name == buff).color;
                else if (!tempContacts.Exists((x) => x.color == color))
                {
                    Contact cnt1 = new Contact()
                    {
                        Name = buff,
                        color = color
                    };
                    tempContacts.Add(cnt1);
                    return color;
                }
                else
                {
                    while (!tempContacts.Exists((x) => x.color == color) && color != Color.White)
                    {
                        color = RandColor();
                    }
                    Contact cnt2 = new Contact()
                    {
                        Name = buff,
                        color = color
                    };
                    tempContacts.Add(cnt2);
                    return color;
                }
            }
            else
            {
                Contact cnt = new Contact()
                {
                    Name = buff,
                    color = color
                };
                tempContacts.Add(cnt);
                return color;
            }
        }


        private async void ReceiveAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        int count = listViewMessages.Items.Count;
                        string[] mess = new string[4];
                        mess = ((DataMessage)Transfer.ReceiveTCP(socket)).Array;
                        string buff = mess[0];
                        Color color = RandColor();
                        while (color == Color.White)
                        {
                            color = RandColor();
                        }
                        color = ContactColor(mess[2], color);


                        if (listViewMessages.InvokeRequired)
                        {
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items.Add(buff)));
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items[count].ForeColor = color));
                        }
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
                    contacts = new List<Contact>();
                    tempContacts = new List<Contact>();
                    if (File.Exists(pathFile))
                    {
                        LoadContacts();
                        foreach (var item in contacts)
                        {
                            tempContacts.Add(item);
                        }
                    }
                    message[1] = "other";
                    message[2] = login[0];
                    message[3] = "";
                    listViewMessages.Items.Clear();
                    listViewMessages.Items.Add(
                  "SERVER CONNEСTOR                                                                                                         ");
                    listViewMessages.Items[0].ForeColor = Color.Red;
                    buttonSend.Enabled = true;
                    textBoxMessage.Enabled = true;
                    this.Text = "MYOWNCHAT: " + $"Login - {login[0]}";
                    HideHorizontalScrollBar();
                    if (contacts.Count != 0)
                        AddToListViewContacts();
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

        public Color RandColor()
        {
            rand = new Random();
            KnownColor[] names = new KnownColor[]{ KnownColor.Aqua, KnownColor.Aquamarine, KnownColor.Blue, KnownColor.BlueViolet, KnownColor.Brown,
                KnownColor.BurlyWood, KnownColor.CadetBlue, KnownColor.Chartreuse, KnownColor.Chocolate, KnownColor.Coral, KnownColor.CornflowerBlue,
                KnownColor.Crimson, KnownColor.Cyan, KnownColor.DarkBlue, KnownColor.DarkCyan, KnownColor.DarkGoldenrod, KnownColor.DarkGray, KnownColor.DarkGreen,
                KnownColor.DarkKhaki, KnownColor.DarkMagenta, KnownColor.DarkOliveGreen, KnownColor.DarkOrange, KnownColor.DarkOrchid, KnownColor.DarkRed,
                KnownColor.DarkSalmon, KnownColor.DarkSeaGreen, KnownColor.DarkSlateBlue, KnownColor.DarkSlateGray, KnownColor.DarkTurquoise, KnownColor.DarkViolet,
                KnownColor.DeepPink, KnownColor.DeepSkyBlue, KnownColor.DimGray, KnownColor.DodgerBlue, KnownColor.Firebrick, KnownColor.ForestGreen, KnownColor.Fuchsia,
                KnownColor.Gold, KnownColor.Goldenrod, KnownColor.Gray, KnownColor.Green, KnownColor.HotPink, KnownColor.IndianRed, KnownColor.Indigo, KnownColor.Magenta,
                KnownColor.Maroon, KnownColor.MediumAquamarine, KnownColor.MediumBlue, KnownColor.MediumOrchid, KnownColor.MediumPurple, KnownColor.MediumSeaGreen,
                KnownColor.MediumSlateBlue, KnownColor.MediumSpringGreen, KnownColor.MediumTurquoise, KnownColor.MediumVioletRed, KnownColor.MidnightBlue, KnownColor.Navy,
                KnownColor.Olive, KnownColor.Orange, KnownColor.OrangeRed, KnownColor.Orchid, KnownColor.Peru, KnownColor.Plum, KnownColor.Purple, KnownColor.Red,
                KnownColor.RoyalBlue, KnownColor.SaddleBrown, KnownColor.SeaGreen, KnownColor.Sienna, KnownColor.SlateBlue, KnownColor.SlateGray, KnownColor.SteelBlue,
                KnownColor.Teal, KnownColor.Tomato, KnownColor.Violet, KnownColor.YellowGreen };
            KnownColor randColorName = names[rand.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randColorName);
            return randomColor;
        }

        private void AddToListViewContacts()
        {
            listViewContacts.Items.Clear();
            foreach (var item in contacts)
            {
                listViewContacts.Items.Add(item.Name);
                listViewContacts.Items[contacts.FindIndex((x) => x.Name == item.Name)].ForeColor = item.color;
            }
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            Contact contact = new Contact();
            string buff = listViewMessages.Items[listViewMessages.FocusedItem.Index].Text;
            string[] str = buff.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            contact.Name = str[0];
            contact.color = tempContacts[tempContacts.FindIndex((x) => x.Name == contact.Name)].color;
            if (contacts.Count == 0)
            {
                contacts.Add(contact);
                SaveContacts();
            }
            else if (!contacts.Exists((x) => x.Name == contact.Name))
            {
                while (contacts.Exists((x) => x.color == contact.color))
                {
                    contact.color = RandColor();
                }
                contacts.Add(contact);
                SaveContacts();
            }

            AddToListViewContacts();
        }

        private async void SaveContacts()
        {
            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fs, contacts);
            }
        }

        private async void LoadContacts()
        {
            using (FileStream fs = new FileStream(pathFile, FileMode.Open))
            {
                var cnt = await JsonSerializer.DeserializeAsync<List<Contact>>(fs);
                contacts = cnt;
            }
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public Color color { get; set; }
    }
}
