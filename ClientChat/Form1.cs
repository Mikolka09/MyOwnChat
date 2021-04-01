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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Message = ProtocolsMessages.Message;
using MyOwnChat;

namespace ClientChat
{
    public partial class Form1 : Form
    {

        private IPAddress iP;
        private IPEndPoint endPoint;
        public TcpClient socket;
        private string address = "127.0.0.1";
        private int port = 1250;
        public Message message;
        public List<Contact> contacts;
        public List<Contact> contactsGroup;
        private List<Contact> tempContacts;
        private User user;
        private Random rand;
        public Contact Cont;
        private bool sort = true;
        private int countMess = 0;
        private string logs;
        private string exp;
        private string tagMessage;
        private List<byte[]> fileByte;
        private List<byte[]> contByte;
        private List<byte[]> listByte;
        public List<User> usersB;
        public List<Message> listMess;
        public User client;
        public List<string> contCheck;
        public string nameGroup;






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
                Close();
            }
        }

        public void StartChat()
        {
            buttonSend.Enabled = false;
            textBoxMessage.Enabled = true;
            ReceiveAsync();
        }

        public void CreateTempContact(string log, string nm, string tg, Color color)
        {
            Contact cnt = new Contact()
            {
                Login = log,
                Name = nm,
                Tag = tg,
                Color = color
            };
            tempContacts.Add(cnt);
        }

        public Color ContactColor(string buff, Color color)
        {
            if (tempContacts.Count != 0)
            {
                if (tempContacts.Exists((x) => x.Login == buff))
                    return color = tempContacts.Find((x) => x.Login == buff).Color;
                else if (!tempContacts.Exists((x) => x.Color == color))
                {
                    CreateTempContact(buff, "", "", color);
                    return color;
                }
                else
                {
                    while (!tempContacts.Exists((x) => x.Color == color) && color != Color.White)
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

        public async void ReceiveAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        int count = listViewMessages.Items.Count;
                        Message mess = new Message();
                        Data data = Transfer.ReceiveTCP(socket);
                        if (data is DataFile)
                        {
                            fileByte = ((DataFile)data).FileByte;
                            LoadFileByte(fileByte);
                        }
                        else
                        {
                            mess = ((DataMessage)data).Message;
                            CheckReceives(mess);
                            string buff = mess.Text;
                            Color color = RandColor();
                            color = ContactColor(mess.LoginSend, color);
                            string dateTime = DateTime.Now.ToLongTimeString();
                            string print = "";
                            for (int i = 0; i < 3; i++)
                            {
                                if (i == 0) print = "";
                                if (i == 1) print = dateTime;
                                if (i == 2) print = buff;
                                ListViewItem it = new ListViewItem(print);
                                if (i == 2) it.ForeColor = color;
                                if (listViewMessages.InvokeRequired)
                                {
                                    listViewMessages.Invoke(new Action(() => listViewMessages.Items.Add(it)));
                                    listViewMessages.Invoke(new Action(() => listViewMessages.EnsureVisible(listViewMessages.Items.Count - 1)));
                                }
                            }
                        }

                    }
                });
            }
            catch (Exception) { throw; }
        }

        public void CheckReceives(Message mess)
        {
            string messCheck = mess.Priorety;
            switch (messCheck)
            {
                case "group":
                    string log = mess.LoginSend;
                    if (log == user.Login)
                    {
                        logs = mess.LoginReceive;
                        Message messAns = new Message();
                        string regex = "\\b" + $"{user.Login}" + "\\b";
                        logs = Regex.Replace(logs, regex, log);
                        messAns.Priorety = "group";
                        messAns.LoginSend = user.Login;
                        messAns.LoginReceive = logs;
                        messAns.Moment = DateTime.Now.ToLongTimeString();
                        message = messAns;
                    }
                    else
                    {
                        if (countMess == 0)
                        {
                            if (MessageBox.Show($"{log} sent a request for a group chat", "Inquiry", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                countMess++;
                                Message messAns = new Message();
                                logs = mess.LoginReceive;
                                string regex = "\\b" + $"{user.Login}" + "\\b";
                                logs = Regex.Replace(logs, regex, log);
                                messAns.Priorety = "group";
                                messAns.LoginSend = user.Login;
                                messAns.LoginReceive = logs;
                                messAns.Moment = DateTime.Now.ToLongTimeString();
                                message = messAns;

                            }
                            else
                            {
                                Message messAns = new Message();
                                messAns.Text = "Sorry, i can't, busy!";
                                messAns.Priorety = "private";
                                messAns.LoginSend = user.Login;
                                messAns.LoginReceive = log;
                                messAns.Moment = DateTime.Now.ToLongTimeString();
                                Transfer.SendTCP(socket, new DataMessage() { Message = messAns });
                            }
                        }
                    }
                    break;
                case "close":
                    {
                        Message messAns = new Message();
                        messAns.Text = $"Sorry, group \"{nameGroup}\" CLOSE!";
                        messAns.Priorety = "other";
                        messAns.LoginSend = user.Login;
                        messAns.LoginReceive = "";
                        messAns.Answer = "";
                        messAns.Moment = DateTime.Now.ToLongTimeString();
                        message = messAns;
                        Transfer.SendTCP(socket, new DataMessage() { Message = message });
                        break;
                    }
                default:
                    break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user = new User();
            ConnectServer();
            Input inp = new Input();
            try
            {
                if (inp.ShowDialog(this) == DialogResult.OK)
                {
                    if (socket.Connected)
                    {
                        user = inp.user;
                        if (user.Login == "Admin")
                        {
                            listByte = new List<byte[]>();
                            usersB = new List<User>();
                            usersB = ReceiveUsers();
                            listMess = new List<Message>();
                            listMess = ReceiveMessages();
                            listViewContacts.Visible = false;
                            listViewClients.Visible = true;
                            listViewStatistic.Visible = false;
                            buttonCloseGroup.Visible = false;
                            buttonGroupChat.Visible = false;
                            buttonClearSt.Enabled = false;
                            buttonBackChat.Enabled = false;
                            groupBox2.Text = "CONTACTS CLIENTS";
                        }
                        else
                        {
                            listViewContacts.Visible = true;
                            listViewClients.Visible = false;
                            listViewStatistic.Visible = false;
                            buttonBackChat.Visible= false;
                            buttonClearSt.Visible = false;
                            statisticToolStripMenuItem.Visible = false;
                            groupBox2.Text = "MY CONTACTS";
                        }
                        message = new Message();
                        contacts = new List<Contact>();
                        contacts = ReceiveContacts();
                        tempContacts = new List<Contact>();
                        contByte = new List<byte[]>();
                        fileByte = new List<byte[]>();
                        message.Priorety = "other";
                        message.LoginSend = user.Login;
                        message.LoginReceive = "";
                        message.Answer = "";
                        listViewMessages.Items.Clear();
                        listViewMessages.Columns[0].Width = listViewMessages.Width - 5;
                        listViewMessages.Columns[0].Text = "SERVER CONNEСTOR: " + DateTime.Now.ToString();
                        buttonSend.Enabled = false;
                        buttonSaveFile.Enabled = false;
                        textBoxMessage.Enabled = true;
                        buttonSaveContacts.Enabled = false;
                        textBoxFileName.Enabled = false;
                        buttonCloseGroup.Enabled = false;
                        this.Text = "MYOWNCHAT: " + $"Login - {user.Login}";
                        StartChat();
                    }
                }
                else
                {
                    inp.Close();
                    Close();
                }
            }
            catch { }
        }

        public List<User> ReceiveUsers()
        {
            Message mess = new Message();
            mess.Text = "";
            mess.Priorety = "loadUsers";
            mess.LoginSend = user.Login;
            mess.LoginReceive = user.Login;
            mess.Moment = DateTime.Now.ToLongTimeString();
            mess.Answer = "";
            Transfer.SendTCP(socket, new DataMessage() { Message = mess });
            Data data = Transfer.ReceiveTCP(socket);
            if (data is DataUsers)
                return ((DataUsers)data).ListU;
            else
                return null;
        }

        public List<Message> ReceiveMessages()
        {
            Message mess = new Message();
            mess.LoginSend = user.Login;
            mess.LoginReceive = user.Login;
            mess.Priorety = "statistic";
            mess.Text = "";
            mess.Moment = DateTime.Now.ToLongTimeString();
            mess.Answer = "";
            Transfer.SendTCP(socket, new DataMessage() { Message = mess });
            Data data = Transfer.ReceiveTCP(socket);
            if (data is DataMessages)
                return ((DataMessages)data).ListM;
            else
                return null;
        }

        public List<Contact> ReceiveContacts()
        {
            Message mess = new Message();
            mess.Text = "";
            mess.Priorety = "loadContacts";
            mess.LoginSend = user.Login;
            mess.LoginReceive = user.Login;
            mess.Moment = DateTime.Now.ToLongTimeString();
            mess.Answer = "";
            Transfer.SendTCP(socket, new DataMessage() { Message = mess });
            Data data = Transfer.ReceiveTCP(socket);
            if (data is DataContacts)
                return ((DataContacts)data).ListC;
            else
                return null;
        }

        public void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                message.Text = textBoxMessage.Text;
                message.Moment = DateTime.Now.ToLongTimeString();
                Transfer.SendTCP(socket, new DataMessage() { Message = message });
                textBoxMessage.Clear();
            }
            catch (Exception) { throw; }
        }

        public void buttonExit_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                if (message == null)
                    message = new Message();
                message.Text = "";
                message.Priorety = "exit";
                message.Answer = "";
                message.Moment = DateTime.Now.ToLongTimeString();
                buttonSend_Click(this, new EventArgs());
                Close();
            }
            else
                Close();
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
                it.SubItems.Add(item.Tag);
                listViewContacts.Items.Add(it);
            }
            buttonSaveContacts.Enabled = true;
            buttonLoadContacts.Enabled = false;
        }

        private void AddToListViewClients()
        {
            listViewClients.Items.Clear();
            foreach (var item in usersB)
            {
                ListViewItem it = new ListViewItem(item.Login);
                it.SubItems.Add(item.CountBadWord.ToString());
                it.SubItems.Add(item.Birthday);
                listViewClients.Items.Add(it);
            }
            buttonSaveContacts.Enabled = true;
            buttonLoadContacts.Enabled = false;
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            Cont = new Contact();
            string buff = listViewMessages.Items[listViewMessages.FocusedItem.Index].Text;
            string[] str = buff.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (str.Length > 2)
                Cont.Login = str[0].Split()[1];
            else
                Cont.Login = str[0];
            Cont.Color = tempContacts[tempContacts.FindIndex((x) => x.Login == Cont.Login)].Color;
            Cont.LoginAdd = user.Login;
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
                while (contacts.Exists((x) => x.Color == Cont.Color))
                {
                    Cont.Color = RandColor();
                }
                contacts.Add(Cont);
            }
            AddToListViewContacts();
        }

        public void SaveUsers(List<User> users)
        {
            Transfer.SendTCP(socket, new DataUsers() { ListU = users });
        }

        public void SaveContacts(List<Contact> list)
        {
            Transfer.SendTCP(socket, new DataContacts() { ListC = list });
        }

        private void buttonSaveContacts_Click(object sender, EventArgs e)
        {
            if (user.Login == "Admin")
            {
                List<User> newList = new List<User>();
                foreach (ListViewItem item in listViewClients.Items)
                {
                    User client = new User();
                    client.Login = item.SubItems[0].Text;
                    client.Password = usersB[usersB.FindIndex((x) => x.Login == client.Login)].Password;
                    client.CountBadWord = Convert.ToInt32(item.SubItems[1].Text);
                    client.Birthday = item.SubItems[2].Text;
                    client.Tag = usersB[usersB.FindIndex((x) => x.Login == client.Login)].Tag;
                    client.IPClient = usersB[usersB.FindIndex((x) => x.Login == client.Login)].IPClient;
                    newList.Add(client);
                }
                usersB.Clear();
                foreach (var item in newList)
                {
                    usersB.Add(item);
                }
                SaveUsers(usersB);
                MessageBox.Show("Clients Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SaveContacts(contacts);
                MessageBox.Show("Contacts Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonLoadContacts_Click(object sender, EventArgs e)
        {
            if (user.Login == "Admin")
            {
                AddToListViewClients();
            }
            else
            {
                AddToListViewContacts();
            }
        }

        public void listViewContacts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (user.Login == "Admin")
            {
                if (e.Column == 0 && sort == true)
                {
                    usersB.Sort(new LogClComparer());
                    sort = false;
                }
                else if (e.Column == 0 && sort == false)
                {
                    usersB.Sort(new LogClComparer());
                    usersB.Reverse();
                    sort = true;
                }
                if (e.Column == 1 && sort == true)
                {
                    usersB.Sort(new CountClComparer());
                    sort = false;
                }
                else if (e.Column == 1 && sort == false)
                {
                    usersB.Sort(new CountClComparer());
                    usersB.Reverse();
                    sort = true;
                }
                AddToListViewClients();
            }
            else
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
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.Login == "Admin")
            {
                bool res = true;
                int k = 0;
                AddClient edit = new AddClient(); ;
                while (res)
                {
                    client = new User();
                    ListViewItem item = listViewClients.SelectedItems[0];
                    client.Login = item.SubItems[0].Text;
                    string name = client.Login;
                    client.CountBadWord = Convert.ToInt32(item.SubItems[1].Text);
                    client.Birthday = item.SubItems[2].Text; ;
                    client.Password = usersB[usersB.FindIndex((x) => x.Login == client.Login)].Password;
                    client.Tag = usersB[usersB.FindIndex((x) => x.Login == client.Login)].Tag;
                    client.IPClient = usersB[usersB.FindIndex((x) => x.Login == client.Login)].IPClient;
                    if (edit.ShowDialog(this) == DialogResult.OK)
                    {
                        client = edit.client;
                        if (client.Login != name)
                        {
                            if (!usersB.Exists((x) => x.Login == client.Login))
                            {
                                k = usersB.FindIndex((x) => x.Login == client.Login);
                                res = false;
                            }
                            else
                            {
                                MessageBox.Show("There is already a user with this login, please try again!", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                res = true;
                            }
                        }
                        else
                        {
                            k = usersB.FindIndex((x) => x.Login == client.Login);
                            res = false;
                        }
                    }
                    else
                        res = false;
                }
                usersB.RemoveAt(k);
                usersB.Add(client);
                AddToListViewClients();
                MessageBox.Show("Client Edited!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cont = new Contact();
                ListViewItem it = listViewContacts.SelectedItems[0];
                Cont.Login = it.SubItems[0].Text;
                Cont.Name = it.SubItems[1].Text;
                Cont.Tag = it.SubItems[2].Text;
                Cont.Color = contacts[contacts.FindIndex((x) => x.Login == Cont.Login)].Color;
                Cont.LoginAdd = contacts[contacts.FindIndex((x) => x.Login == Cont.Login)].LoginAdd;
                EditContact ed = new EditContact();
                if (ed.ShowDialog(this) == DialogResult.OK)
                {
                    int k = contacts.FindIndex((x) => x.Login == Cont.Login);

                    contacts.RemoveAt(k);
                    contacts.Add(ed.contact);
                    AddToListViewContacts();
                    MessageBox.Show("Contact Edited!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.Login == "Admin")
            {
                client = new User();
                ListViewItem it = listViewClients.SelectedItems[0];
                client.Login = it.SubItems[0].Text;
                int k = usersB.FindIndex((x) => x.Login == client.Login);
                usersB.RemoveAt(k);
                AddToListViewClients();
                MessageBox.Show("Client Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cont = new Contact();
                ListViewItem item = listViewContacts.SelectedItems[0];
                Cont.Login = item.SubItems[0].Text;

                int k = contacts.FindIndex((x) => x.Login == Cont.Login);

                contacts.RemoveAt(k);
                AddToListViewContacts();
                MessageBox.Show("Contact Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user.Login == "Admin")
            {
                client = new User();
                AddClient ad = new AddClient();
                if (ad.ShowDialog(this) == DialogResult.OK)
                {
                    user = ad.user;
                    client.Login = user.Login;
                    client.Password = user.Password;
                    client.CountBadWord = 0;
                    client.Birthday = user.Birthday;
                    client.Tag = "";
                    client.IPClient = "";
                    usersB.Add(client);
                    AddToListViewClients();
                    MessageBox.Show("Client Addet!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Cont = new Contact();
                Cont.Login = "";
                Cont.LoginAdd = user.Login;
                Cont.Name = "";
                Cont.Tag = "";
                Cont.Color = RandColor();
                EditContact edit = new EditContact();
                if (edit.ShowDialog(this) == DialogResult.OK)
                {
                    contacts.Add(edit.contact);
                    AddToListViewContacts();
                    MessageBox.Show("Contact Addet!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void checkBoxGroups_CheckedChanged(object sender, EventArgs e)
        {
            string logins = "";
            for (int i = 0; i < contactsGroup.Count; i++)
            {
                if (i < contactsGroup.Count - 1)
                    logins += contactsGroup[i].Login + " ";
                else
                    logins += contactsGroup[i].Login;
            }
            message.Priorety = "group";
            message.LoginSend = user.Login;
            if (logins != "")
                message.LoginReceive = logins;
            else
                message.LoginReceive = logs;
            message.Text = $"Wellcome to group \"{nameGroup}\"";
            message.Answer = "";
            message.Moment = DateTime.Now.ToLongTimeString();
            Transfer.SendTCP(socket, new DataMessage() { Message = message });
        }

        private void buttonCloseGroup_Click(object sender, EventArgs e)
        {
            buttonCloseGroup.Enabled = false;
            buttonGroupChat.Enabled = true;
            message.Text = "Start to close group!";
            message.Priorety = "close";
            message.LoginSend = user.Login;
            message.LoginReceive = "";
            message.Answer = "";
            message.Moment = DateTime.Now.ToLongTimeString();
            countMess = 0;
            Transfer.SendTCP(socket, new DataMessage() { Message = message });
        }

        public void SendFile(string log)
        {
            if (fileByte.Count > 0)
                fileByte.Clear();
            string lg = log + " " + user.Login + " " + tagMessage;
            byte[] lgB = Encoding.Default.GetBytes(lg);
            fileByte.Add(lgB);
            string name = openFileDialog1.SafeFileName;
            byte[] lenB = Encoding.Default.GetBytes(name);
            fileByte.Add(lenB);
            byte[] fileB = File.ReadAllBytes(openFileDialog1.FileName);
            fileByte.Add(fileB);
            Transfer.SendTCP(socket, new DataFile() { FileByte = fileByte });
            MessageBox.Show($"File sent from \"{log}\"!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void buttonSendFileAll_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*|Text File (*.txt)|*.txt";
            tagMessage = "other";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                SendFile("");
        }

        public void LoadFileByte(List<byte[]> file)
        {
            string name = Encoding.Default.GetString(file[1]);
            if (MessageBox.Show($"You received a file \"{name}\" from \"{message.LoginSend}\", do you want to keep it?", "Messsage",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (textBoxFileName.InvokeRequired)
                    textBoxFileName.Invoke(new Action(() => textBoxFileName.Text = name));
                exp = "." + name.Split('.')[1];
                buttonSaveFile.Enabled = true;
            }
        }


        public void SaveFile()
        {
            try
            {
                saveFileDialog1.DefaultExt = exp;
                saveFileDialog1.Filter = "All files (*.*)|*.*|ext File (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFile = saveFileDialog1.FileName;
                    File.WriteAllBytes(pathFile, fileByte[2]);
                    MessageBox.Show("File Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                textBoxFileName.Clear();
                buttonSaveFile.Enabled = false;
            }
            catch { throw; }

        }

        public string ReceiveLogin()
        {
            string buff = listViewMessages.Items[listViewMessages.FocusedItem.Index].Text;
            string[] str = buff.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0].Split();
            if (str.Length == 2)
                return str[1];
            else
                return str[0];
        }


        private void sendPrivateMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            message.LoginReceive = ReceiveLogin();
            PrivateMessage prive = new PrivateMessage();
            if (prive.ShowDialog(this) == DialogResult.OK)
            {
                message.Text = prive.message;
                message.Priorety = "private";
                message.LoginSend = user.Login;
                message.Answer = "";
                message.Moment = DateTime.Now.ToLongTimeString();

                Transfer.SendTCP(socket, new DataMessage() { Message = message });
            }
        }

        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "All files (*.*)|*.*|Text File (*.txt)|*.txt";
            tagMessage = "private";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                SendFile(ReceiveLogin());
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            buttonSend.Enabled = true;
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void buttonGroupChat_Click(object sender, EventArgs e)
        {
            contCheck = new List<string>();
            contactsGroup = new List<Contact>();
            nameGroup = "";
            buttonLoadContacts_Click(this, new EventArgs());
            CreateGroup create = new CreateGroup();
            if (create.ShowDialog(this) == DialogResult.OK)
            {
                contCheck = create.contactsCheck;
                nameGroup = create.nameGroup;
                foreach (var it in contCheck)
                {
                    contactsGroup.Add(contacts[contacts.FindIndex((x) => x.Login == it)]);
                }
                buttonCloseGroup.Enabled = true;
                buttonGroupChat.Enabled = false;
                checkBoxGroups_CheckedChanged(this, new EventArgs());

            }
        }

        private void statisticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem it = listViewClients.SelectedItems[0];
            string log = it.SubItems[0].Text;
            listViewMessages.Visible = false;
            buttonBackChat.Visible = true;
            listViewStatistic.Visible = true;
            label1.Text = $"STATISTIC MESSAGES LOGIN - {it.SubItems[0].Text}";
            foreach (var item in listMess)
            {
                if (item.LoginSend == log)
                {
                    ListViewItem itt = new ListViewItem(item.LoginSend);
                    itt.SubItems.Add(item.LoginReceive);
                    itt.SubItems.Add(item.Text);
                    itt.SubItems.Add(item.Moment);
                    listViewStatistic.Items.Add(itt);
                }
                buttonClearSt.Enabled = true;
                buttonBackChat.Enabled = true;
            }
            MessageBox.Show("Statistics ready", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonClearSt_Click(object sender, EventArgs e)
        {
            listViewStatistic.Items.Clear();
            buttonClearSt.Enabled = false;
        }

        private void buttonBackChat_Click(object sender, EventArgs e)
        {
            listViewMessages.Visible = true;
            listViewStatistic.Visible = false;
            buttonBackChat.Enabled = false;
        }
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

    class LogClComparer : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            return string.Compare(x.Login, y.Login);
        }
    }

    class CountClComparer : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            return string.Compare(x.CountBadWord.ToString(), y.CountBadWord.ToString());
        }
    }


}
