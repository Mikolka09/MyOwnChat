﻿using System;
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
using System.Runtime.Serialization.Formatters.Binary;
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
        private Random rand;
        public Contact Cont;
        private bool sort = true;

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

        private void CreateTempContact(string log, string nm, string tg, Color color)
        {
            Contact cnt = new Contact()
            {
                Login = log,
                Name = nm,
                Tag = tg,
                color = color
            };
            tempContacts.Add(cnt);
        }

        private Color ContactColor(string buff, Color color)
        {
            if (tempContacts.Count != 0)
            {
                if (tempContacts.Exists((x) => x.Login == buff))
                    return color = tempContacts.Find((x) => x.Login == buff).color;
                else if (!tempContacts.Exists((x) => x.color == color))
                {
                    CreateTempContact(buff, "", "", color);
                    return color;
                }
                else
                {
                    while (!tempContacts.Exists((x) => x.color == color) && color != Color.White)
                    {
                        color = RandColor();
                    }
                    CreateTempContact(buff, "", "", color);
                    return color;
                }
            }
            else
            {
                CreateTempContact(buff, "", "", color);
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
                        color = ContactColor(mess[2], color);
                        string dateTime = DateTime.Now.ToLongTimeString();

                        if (listViewMessages.InvokeRequired)
                        {
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items.Add(" ")));
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items.Add(dateTime)));
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items.Add(buff)));
                            listViewMessages.Invoke(new Action(() => listViewMessages.Items[count + 2].ForeColor = color));
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
                    message[1] = "other";
                    message[2] = login[0];
                    message[3] = "";
                    listViewMessages.Items.Clear();
                    listViewMessages.Items.Add("SERVER CONNEСTOR: " + DateTime.Now.ToString() + "          ");
                    listViewMessages.Items[0].ForeColor = Color.Red;
                    buttonSend.Enabled = true;
                    textBoxMessage.Enabled = true;
                    buttonSaveContacts.Enabled = false;
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
                ListViewItem it = new ListViewItem(item.Login);
                it.SubItems.Add(item.Name);
                listViewContacts.Items.Add(it);
            }
            buttonSaveContacts.Enabled = true;
            buttonLoadContacts.Enabled = false;
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            Cont = new Contact();
            string buff = listViewMessages.Items[listViewMessages.FocusedItem.Index].Text;
            string[] str = buff.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            Cont.Login = str[0];
            Cont.color = tempContacts[tempContacts.FindIndex((x) => x.Login == Cont.Login)].color;
            EditContact editContact = new EditContact();
            if (editContact.ShowDialog(this) == DialogResult.OK)
            {
                Cont = editContact.contact;
            }
            if (contacts.Count == 0)
            {
                contacts.Add(Cont);
            }
            else if (!contacts.Exists((x) => x.Login == Cont.Login))
            {
                while (contacts.Exists((x) => x.color == Cont.color))
                {
                    Cont.color = RandColor();
                }
                contacts.Add(Cont);
            }
            AddToListViewContacts();
        }

        private void SaveContacts(string pathFile)
        {
            BinaryFormatter br = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                br.Serialize(fs, contacts);
            }
        }

        private void LoadContacts(string pathFile)
        {
            BinaryFormatter br = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathFile, FileMode.Open))
            {
                var cnt = (List<Contact>)br.Deserialize(fs);
                contacts = cnt;
                AddToListViewContacts();
            }
        }

        private void buttonSaveContacts_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".dat";
            saveFileDialog1.Filter = "All files (*.*)|*.*|DAT File (*.dat)|*.dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFile = saveFileDialog1.FileName;
                SaveContacts(pathFile);
                MessageBox.Show("Contacts Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonLoadContacts_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*|DAT File (*.dat)|*.dat";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFile = openFileDialog1.FileName;
                LoadContacts(pathFile);
            }
        }

        private void listViewContacts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0 && sort == true)
            {
                contacts.Sort(new LoginComparer());
                sort = false;
            }
            else if (e.Column == 0 && sort == false)
            {
                contacts.Sort(new LoginComparer());
                contacts.Reverse();
                sort = true;
            }
            if (e.Column == 1 && sort == true)
            {
                contacts.Sort(new NameComparer());
                sort = false;
            }
            else if (e.Column == 1 && sort == false)
            {
                contacts.Sort(new NameComparer());
                contacts.Reverse();
                sort = true;
            }
            AddToListViewContacts();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cont = new Contact();
            ListViewItem item = listViewContacts.SelectedItems[0];
            Cont.Login = item.SubItems[0].Text;
            Cont.Name = item.SubItems[1].Text;
            Cont.Tag = contacts[contacts.FindIndex((x) => x.Login == Cont.Login)].Tag;
            Cont.color = contacts[contacts.FindIndex((x) => x.Login == Cont.Login)].color;
            EditContact edit = new EditContact();
            if (edit.ShowDialog(this) == DialogResult.OK)
            {
                int k = 0;
                for (int i = 0; i < contacts.Count; i++)
                {
                    if (contacts[i].Login == Cont.Login)
                    {
                        k = i;
                        break;
                    }
                }
                contacts.RemoveAt(k);
                contacts.Add(edit.contact);
                AddToListViewContacts();
                MessageBox.Show("Contact Edited!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cont = new Contact();
            ListViewItem item = listViewContacts.SelectedItems[0];
            Cont.Login = item.SubItems[0].Text;
            int k = 0;
            for (int i = 0; i < contacts.Count; i++)
            {
                if (contacts[i].Login == Cont.Login)
                {
                    k = i;
                    break;
                }
            }
            contacts.RemoveAt(k);
            AddToListViewContacts();
            MessageBox.Show("Contact Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cont = new Contact();
            Cont.Login = "";
            Cont.Name = "";
            Cont.Tag = "";
            Cont.color = RandColor(); ;
            EditContact edit = new EditContact();
            if (edit.ShowDialog(this) == DialogResult.OK)
            {
                contacts.Add(edit.contact);
                AddToListViewContacts();
                MessageBox.Show("Contact Addet!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void groupChatToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    [Serializable]
    public class Contact
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public Color color { get; set; }
    }

    class LoginComparer : IComparer<Contact>
    {
        public int Compare(Contact x, Contact y)
        {
            return string.Compare(x.Login, y.Login);
        }
    }

    class NameComparer : IComparer<Contact>
    {
        public int Compare(Contact x, Contact y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }

}
