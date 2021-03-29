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
using DataBaseProtocol;


namespace ClientChat
{
    public partial class Form1 : Form
    {

        private IPAddress iP;
        private IPEndPoint endPoint;
        public TcpClient socket;
        private string address = "127.0.0.1";
        private int port = 1250;
        private string[] login;
        public string[] message;
        public List<Contact> contacts;
        public List<Contact> contactsGroup;
        private List<Contact> tempContacts;
        private Random rand;
        public Contact Cont;
        private bool sort = true;
        private int countMess = 0;
        private string logs;
        private string exp;
        private string tagMessage;
        private List<byte[]> fileByte;
        public DataClient data;
        public Client client;
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
                color = color
            };
            tempContacts.Add(cnt);
        }

        public Color ContactColor(string buff, Color color)
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

        public async void ReceiveAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        int count = listViewMessages.Items.Count;
                        string[] mess = new string[4];
                        Data data = Transfer.ReceiveTCP(socket);
                        if (data is DataFile)
                        {
                            fileByte = ((DataFile)data).FileByte;
                            LoadFileByte(fileByte);
                        }
                        else
                        {
                            mess = ((DataMessage)data).Array;
                            CheckReceives(mess);
                            string buff = mess[0];
                            Color color = RandColor();
                            color = ContactColor(mess[2], color);
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

        public void CheckReceives(string[] mess)
        {
            string messCheck = mess[1];
            switch (messCheck)
            {
                case "group":
                    string log = mess[2];
                    if (log == login[0])
                    {
                        logs = mess[3];
                        string[] messAns = new string[4];
                        string regex = "\\b" + $"{login[0]}" + "\\b";
                        logs = Regex.Replace(logs, regex, log);
                        messAns[1] = "group";
                        messAns[2] = login[0];
                        messAns[3] = logs;
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
                                string[] messAns = new string[4];
                                logs = mess[3];
                                string regex = "\\b" + $"{login[0]}" + "\\b";
                                logs = Regex.Replace(logs, regex, log);
                                messAns[1] = "group";
                                messAns[2] = login[0];
                                messAns[3] = logs;
                                message = messAns;

                            }
                            else
                            {
                                string[] messAns = new string[4];
                                messAns[0] = "Sorry, i can't, busy!";
                                messAns[1] = "private";
                                messAns[2] = login[0];
                                messAns[3] = log;
                                Transfer.SendTCP(socket, new DataMessage() { Array = messAns });
                            }
                        }
                    }
                    break;
                case "close":
                    {
                        string[] messAns = new string[4];
                        messAns[0] = $"Sorry, group \"{nameGroup}\" CLOSE!";
                        messAns[1] = "other";
                        messAns[2] = login[0];
                        messAns[3] = "";
                        message = messAns;
                        Transfer.SendTCP(socket, new DataMessage() { Array = message });
                        break;
                    }
                default:
                    break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            login = new string[5];
            ConnectServer();
            Input inp = new Input();
            try
            {
                if (inp.ShowDialog(this) == DialogResult.OK)
                {
                    if (socket.Connected)
                    {
                        login = inp.login;
                        if (login[0] == "Admin")
                        {
                            data = new DataClient();
                            data.clientsFile = new List<Client>();
                            listViewContacts.Visible = false;
                            listViewClients.Visible = true;
                            groupBox2.Text = "CONTACTS CLIENTS";
                        }
                        else
                        {
                            listViewContacts.Visible = true;
                            listViewClients.Visible = false;
                            groupBox2.Text = "MY CONTACTS";
                        }
                        message = new string[4];
                        contacts = new List<Contact>();
                        tempContacts = new List<Contact>();
                        fileByte = new List<byte[]>();
                        message[1] = "other";
                        message[2] = login[0];
                        message[3] = "";
                        listViewMessages.Items.Clear();
                        listViewMessages.Columns[0].Width = listViewMessages.Width - 5;
                        listViewMessages.Columns[0].Text = "SERVER CONNEСTOR: " + DateTime.Now.ToString();
                        buttonSend.Enabled = false;
                        buttonSaveFile.Enabled = false;
                        textBoxMessage.Enabled = true;
                        buttonSaveContacts.Enabled = false;
                        textBoxFileName.Enabled = false;
                        buttonCloseGroup.Enabled = false;
                        this.Text = "MYOWNCHAT: " + $"Login - {login[0]}";
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

        public void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                message[0] = textBoxMessage.Text;
                Transfer.SendTCP(socket, new DataMessage() { Array = message });
                textBoxMessage.Clear();
            }
            catch (Exception) { throw; }
        }

        public void buttonExit_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                message[0] = "";
                message[1] = "exit";
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
            foreach (var item in data.clientsFile)
            {
                ListViewItem it = new ListViewItem(item.Name);
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
            if (login[0] == "Admin")
            {
                List<Client> newList = new List<Client>();
                foreach (ListViewItem item in listViewClients.Items)
                {
                    Client client = new Client();
                    client.Name = item.SubItems[0].Text;
                    client.Password = data.clientsFile[data.clientsFile.FindIndex((x) => x.Name == client.Name)].Password;
                    client.CountBadWord = Convert.ToInt32(item.SubItems[1].Text);
                    client.Birthday = item.SubItems[2].Text;
                    newList.Add(client);
                }
                data.clientsFile.Clear();
                foreach (var item in newList)
                {
                    data.clientsFile.Add(item);
                }
                BinarySaveLoad.SaveClients(data);
                MessageBox.Show("Clients Saved!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
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
        }

        private void buttonLoadContacts_Click(object sender, EventArgs e)
        {
            if (login[0] == "Admin")
            {
                BinarySaveLoad.LoadClients(data);
                AddToListViewClients();
            }
            else
            {
                openFileDialog1.Filter = "All files (*.*)|*.*|DAT File (*.dat)|*.dat";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFile = openFileDialog1.FileName;
                    LoadContacts(pathFile);
                }
            }
        }

        public void listViewContacts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (login[0] == "Admin")
            {
                if (e.Column == 0 && sort == true)
                {
                    data.clientsFile.Sort(new LogClComparer());
                    sort = false;
                }
                else if (e.Column == 0 && sort == false)
                {
                    data.clientsFile.Sort(new LogClComparer());
                    data.clientsFile.Reverse();
                    sort = true;
                }
                if (e.Column == 1 && sort == true)
                {
                    data.clientsFile.Sort(new CountClComparer());
                    sort = false;
                }
                else if (e.Column == 1 && sort == false)
                {
                    data.clientsFile.Sort(new CountClComparer());
                    data.clientsFile.Reverse();
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
            if (login[0] == "Admin")
            {
                bool res = true;
                int k = 0;
                AddClient edit = new AddClient(); ;
                while (res)
                {
                    client = new Client();
                    ListViewItem item = listViewClients.SelectedItems[0];
                    client.Name = item.SubItems[0].Text;
                    string name = client.Name;
                    client.CountBadWord = Convert.ToInt32(item.SubItems[1].Text);
                    client.Birthday = item.SubItems[2].Text; ;
                    client.Password = data.clientsFile[data.clientsFile.FindIndex((x) => x.Name == client.Name)].Password;
                    if (edit.ShowDialog(this) == DialogResult.OK)
                    {
                        client = edit.client;
                        if (client.Name != name)
                        {
                            if (!data.clientsFile.Exists((x) => x.Name == client.Name))
                            {
                                k = data.clientsFile.FindIndex((x) => x.Name == client.Name);
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
                            k = data.clientsFile.FindIndex((x) => x.Name == client.Name);
                            res = false;
                        }
                    }
                }
                data.clientsFile.RemoveAt(k);
                data.clientsFile.Add(client);
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
                Cont.color = contacts[contacts.FindIndex((x) => x.Login == Cont.Login)].color;
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
            if (login[0] == "Admin")
            {
                client = new Client();
                ListViewItem it = listViewClients.SelectedItems[0];
                client.Name = it.SubItems[0].Text;
                int k = data.clientsFile.FindIndex((x) => x.Name == client.Name);
                data.clientsFile.RemoveAt(k);
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
            if (login[0] == "Admin")
            {
                client = new Client();
                AddClient ad = new AddClient();
                if (ad.ShowDialog(this) == DialogResult.OK)
                {
                    login = ad.login;
                    client.Name = login[0];
                    client.Password = login[2];
                    client.CountBadWord = 0;
                    client.Birthday = login[3];
                    data.clientsFile.Add(client);
                    AddToListViewClients();
                    MessageBox.Show("Client Addet!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Cont = new Contact();
                Cont.Login = "";
                Cont.Name = "";
                Cont.Tag = "";
                Cont.color = RandColor();
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
            message[1] = "group";
            message[2] = login[0];
            if (logins != "")
                message[3] = logins;
            else
                message[3] = logs;
            message[0] = $"Wellcome to group \"{nameGroup}\"";
            Transfer.SendTCP(socket, new DataMessage() { Array = message });
        }

        private void buttonCloseGroup_Click(object sender, EventArgs e)
        {
            buttonCloseGroup.Enabled = false;
            buttonGroupChat.Enabled = true;
            message[0] = "Start to close group!"; 
            message[1] = "close";
            message[2] = login[0];
            message[3] = "";
            countMess = 0;
            Transfer.SendTCP(socket, new DataMessage() { Array = message });
        }

        public void SendFile(string log)
        {
            if (fileByte.Count > 0)
                fileByte.Clear();
            string lg = log + " " + login[0] + " " + tagMessage;
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
            if (MessageBox.Show($"You received a file \"{name}\" from \"{message[2]}\", do you want to keep it?", "Messsage",
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
            message[3] = ReceiveLogin();
            PrivateMessage prive = new PrivateMessage();
            if (prive.ShowDialog(this) == DialogResult.OK)
            {
                message[0] = prive.message;
                message[1] = "private";
                message[2] = login[0];

                Transfer.SendTCP(socket, new DataMessage() { Array = message });
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

    class LogClComparer : IComparer<Client>
    {
        public int Compare(Client x, Client y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }

    class CountClComparer : IComparer<Client>
    {
        public int Compare(Client x, Client y)
        {
            return string.Compare(x.CountBadWord.ToString(), y.CountBadWord.ToString());
        }
    }


}
