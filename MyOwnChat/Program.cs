using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ProtocolsMessages;
using System.Threading;
using DataBaseProtocol;
using System.Text.RegularExpressions;


namespace MyOwnChat
{

    public class Programs
    {
        private static IPAddress iP;
        private static IPEndPoint endPoint;
        private static TcpListener server;
        private static string address = "127.0.0.1";
        private static int port = 1250;
        private static List<User> users;
        public static List<User> usersBase;
        public static List<Contact> contactsBase;
        public static List<Message> messages;
        private static object lck;
        private static CancellationTokenSource tokenStop;
        private static Message messEveryone;



        static void Main(string[] args)
        {
            iP = IPAddress.Parse(address);
            endPoint = new IPEndPoint(iP, port);
            server = new TcpListener(endPoint);
            contactsBase = new List<Contact>();
            messages = new List<Message>();
            messages = LoadData.LoadMessage();
            contactsBase = LoadData.LoadContact();
            users = new List<User>();
            usersBase = new List<User>();
            usersBase = LoadData.LoadUser();
            lck = new object();
            tokenStop = new CancellationTokenSource();
            messEveryone = new Message();
            server.Start(50);

            Console.WriteLine($"Server started: {endPoint.Address} : {endPoint.Port}");

            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {

                        TcpClient socket = server.AcceptTcpClient();
                        var task = Task.Run(() => Start(socket, tokenStop.Token));
                    }
                }
                catch { }

            });
            Console.ReadKey();
            Console.WriteLine("Server stoping...");
            server.Stop();
        }

        public static void Cancel()
        {
            tokenStop.Cancel();
        }

        private static async void Start(TcpClient socket, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(500, token);
                    Data data = Transfer.ReceiveTCP(socket);
                    if (data is DataFile)
                        ServerCheckSendFile(((DataFile)data).FileByte);
                    else if (data is DataUsers)
                        SaveListUser(((DataUsers)data).ListU);
                    else if (data is DataContacts)
                        SaveListContact(((DataContacts)data).ListC);
                    else
                        ServerMediator(socket, data);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception) { }
        }

        private static bool EqualsContact(Contact cont1, Contact cont2)
        {
            if (cont1.Login == cont2.Login && cont1.LoginAdd == cont2.LoginAdd && cont1.Name == cont2.Name
                && cont1.Tag == cont2.Tag && cont1.Color.Name == cont2.Color.Name)
                return true;
            else
                return false;
        }

        private static void SaveListContact(List<Contact> list)
        {
            int count = 0;
            foreach (var item in list)
            {
                foreach (var it in contactsBase)
                {
                    if (EqualsContact(item, it))
                        count++;
                }
                if (count == 0)
                    contactsBase.Add(item);
                else
                    count = 0;
            }
            SaveData.SaveContacts(contactsBase);
        }

        public static void SaveListUser(List<User> list)
        {
            usersBase.Clear();
            foreach (var item in list)
            {
                usersBase.Add(item);
            }
            SaveData.SaveListUser(usersBase);
        }

        public static void SaveUsers(List<User> list)
        {
            SaveData.SaveListUser(list);
        }

        public static int CountOneWord(string txt, string reg)
        {
            int cnt = 0;
            MatchCollection matchs = Regex.Matches(txt, reg, RegexOptions.IgnoreCase);
            cnt = matchs.Count;
            return cnt;
        }

        private static string CensoringPosts(User user, string message)
        {
            string[] badWord = {"fuck", "shit", "son of a bitch", "asshole", "bint", "bollocks", "munter", "bastard", "snatch", "dick",
                                "козел", "сука", "урод", "дерьмо", "член", "ублюдок"};
            string change = "#######";
            string txt = message;
            foreach (var item in badWord)
            {
                string reg = "\\b" + $"{item}" + "\\b";
                user.CountBadWord += CountOneWord(txt, reg);
                txt = Regex.Replace(txt, reg, change, RegexOptions.IgnoreCase);
            }
            return txt;
        }

        private static void ServerMediator(TcpClient socket, Data data)
        {
            User user = new User();
            string messCheck = "";
            if (data is DataMessage)
                messCheck = ((DataMessage)data).Message.Priorety;
            else if (data is DataUser)
                messCheck = ((DataUser)data).User.Tag;
            switch (messCheck)
            {
                case "avtorization":

                    if (!IdentificationClient(((DataUser)data).User))
                    {
                        Message mess = new Message();
                        mess.Answer = "No";
                        Transfer.SendTCP(socket, new DataMessage() { Message = mess });
                    }
                    else if (EntryCheck(socket, (DataUser)data))
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataUser)data).User.Login)];
                        Console.WriteLine($"Client connected with Name: {user.Login} and IP: {user.EndPointClient.Address}");
                        messEveryone.Text = $"{user.Login} joined to chat";
                        messEveryone.Moment = DateTime.Now.ToLongTimeString();
                        messEveryone.LoginSend = user.Login;
                        messEveryone.Priorety = "avtorization";
                        messEveryone.LoginReceive = "";
                        messEveryone.Answer = "";
                        SaveData.SaveMessage(messEveryone);
                        SendToEveryone(user, messEveryone);
                    }
                    break;
                case "registration":
                    if (LoginCheck(socket, (DataUser)data))
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataUser)data).User.Login)];
                        Console.WriteLine($"Client connected with Name: {user.Login} and IP: {user.EndPointClient.Address}");
                        messEveryone.Text = $"{user.Login} joined to chat";
                        messEveryone.Moment = DateTime.Now.ToLongTimeString();
                        messEveryone.LoginSend = user.Login;
                        messEveryone.Priorety = "avtorization";
                        messEveryone.LoginReceive = "";
                        messEveryone.Answer = "";
                        SaveData.SaveMessage(messEveryone);
                        SendToEveryone(user, messEveryone);
                    }
                    break;
                case "loadUsers":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        List<User> list = new List<User>();
                        foreach (var item in usersBase)
                        {
                            list.Add(item);
                        }
                        Transfer.SendTCP(user.ClientTcp, new DataUsers() { ListU = list });
                        break;
                    }
                case "statistic":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        List<Message> list = new List<Message>();
                        foreach (var item in messages)
                        {
                            if (item.LoginReceive == "0" && item.Answer == "")
                            {
                                item.Answer = "no answer";
                                item.LoginReceive = "other";
                                list.Add(item);
                            }
                        }
                            Transfer.SendTCP(user.ClientTcp, new DataMessages() { ListM = list });
                        break;
                    }
                case "loadContacts":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        List<Contact> list = new List<Contact>();
                        foreach (var item in contactsBase)
                        {
                            if (item.LoginAdd == user.Login)
                                list.Add(item);
                        }
                        Transfer.SendTCP(user.ClientTcp, new DataContacts() { ListC = list });
                        break;
                    }
                case "create":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataUser)data).User.Login)];
                        Console.WriteLine($"Client created Admin with Name: {user.Login} and IP: {user.EndPointClient.Address}");
                        break;
                    }
                case "other":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        string messCen = CensoringPosts(user, ((DataMessage)data).Message.Text);
                        string mess = $"[{user.Login}]: " + messCen;
                        ((DataMessage)data).Message.Text = mess;
                        Console.WriteLine($"Message has been received: {user.Login} and IP: {user.EndPointClient.Address}");
                        SaveData.SaveMessage(((DataMessage)data).Message);
                        SendToEveryone(user, ((DataMessage)data).Message);
                        break;
                    }
                case "close":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        string messCen = CensoringPosts(user, ((DataMessage)data).Message.Text);
                        string mess = $"[{user.Login}]: " + messCen;
                        ((DataMessage)data).Message.Text = mess;
                        SaveData.SaveMessage(((DataMessage)data).Message);
                        Console.WriteLine($"Message has been received: {user.Login} and IP: {user.EndPointClient.Address}");
                        SendToEveryone(user, ((DataMessage)data).Message);
                        break;
                    }
                case "private":
                    {
                        User userSend = new User();
                        userSend = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        User userReceive = new User();
                        userReceive = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginReceive)];
                        string messCen = CensoringPosts(user, ((DataMessage)data).Message.Text);
                        string mess = $"[Private: {userSend.Login}]: " + messCen;
                        ((DataMessage)data).Message.Text = mess;
                        SaveData.SaveMessage(((DataMessage)data).Message);
                        SendToPrivateMessage(userSend, userReceive, ((DataMessage)data).Message);
                        break;
                    }
                case "group":
                    {
                        User userSend = new User();
                        userSend = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        string[] usersR = ((DataMessage)data).Message.LoginReceive.Split();
                        User[] usersReceive = new User[usersR.Length];
                        for (int i = 0; i < usersReceive.Length; i++)
                        {
                            usersReceive[i] = users[users.FindIndex((x) => x.Login == usersR[i])];
                        }
                        string messCen = CensoringPosts(user, ((DataMessage)data).Message.Text);
                        string mess = $"[Groups: {userSend.Login}]: " + messCen;
                        ((DataMessage)data).Message.Text = mess;
                        SaveData.SaveMessage(((DataMessage)data).Message);
                        SendToGroups(userSend, usersReceive, ((DataMessage)data).Message);
                        break;
                    }
                case "exit":
                    {
                        user = users[users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend)];
                        Console.WriteLine($"Client passed out with Name: {user.Login} and IP: {user.EndPointClient.Address}");
                        messEveryone.Text = $"{user.Login} left to chat";
                        messEveryone.Moment = DateTime.Now.ToLongTimeString();
                        messEveryone.LoginSend = user.Login;
                        messEveryone.Priorety = "exit";
                        messEveryone.LoginReceive = "";
                        messEveryone.Answer = "";

                        foreach (var item in users)
                        {
                            foreach (var it in usersBase)
                            {
                                if (item.Login == it.Login)
                                    if (item.CountBadWord != it.CountBadWord)
                                        it.CountBadWord = item.CountBadWord;
                            }
                        }

                        SaveUsers(usersBase);
                        SendToEveryone(user, messEveryone);
                        users.RemoveAt(users.FindIndex((x) => x.Login == ((DataMessage)data).Message.LoginSend));
                        break;
                    }
                default:
                    break;
            }

        }

        private static void ServerCheckSendFile(List<byte[]> fileByte)
        {
            string name = Encoding.Default.GetString(fileByte[0]);
            string[] mess = new string[3];
            User userSend = new User();
            User userReceive = new User();
            mess = name.Split();
            switch (mess[2])
            {
                case "private":
                    {
                        userSend = new User();
                        userSend = users[users.FindIndex((x) => x.Login == mess[1])];
                        userReceive = new User();
                        userReceive = users[users.FindIndex((x) => x.Login == mess[0])];
                        string mes = $"[SendFile: {userSend.Login}]: File sent!";
                        Message messP = new Message();
                        messP.Text = mes;
                        messP.Priorety = mess[2];
                        messP.LoginSend = mess[1];
                        messP.LoginReceive = mess[0];
                        messP.Answer = "";
                        SendToFile(userSend, userReceive, fileByte, messP);
                        break;
                    }
                case "other":
                    {
                        userSend = new User();
                        userSend = users[users.FindIndex((x) => x.Login == mess[1])];
                        string mes = $"[SendFile: {userSend.Login}]: File sent!";
                        Message messP = new Message();
                        messP.Text = mes;
                        messP.Priorety = mess[2];
                        messP.LoginSend = mess[1];
                        messP.LoginReceive = "";
                        messP.Answer = "";
                        SendToOtherFile(userSend, fileByte, messP);
                        break;
                    }
                default:
                    break;
            }
        }

        private static void SendToOtherFile(User userSend, List<byte[]> fileByte, Message messP)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(userSend.ClientTcp, new DataMessage() { Message = messP }));
                foreach (var item in users)
                {
                    if (item.Login != userSend.Login)
                    {
                        Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = messP }));
                        Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataFile() { FileByte = fileByte }));
                    }
                }
                Console.WriteLine($"File has been sent: {userSend.Login} and IP: {userSend.EndPointClient.Address}");
            }
        }


        private static void SendToFile(User userSend, User userReceive, List<byte[]> fileByte, Message messP)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(userSend.ClientTcp, new DataMessage() { Message = messP }));
                Task.Run(() => Transfer.SendTCP(userReceive.ClientTcp, new DataMessage() { Message = messP }));
                Task.Run(() => Transfer.SendTCP(userReceive.ClientTcp, new DataFile() { FileByte = fileByte }));

                Console.WriteLine($"File has been sent: {userSend.Login} from: {userReceive.Login}");
            }
        }

        private static void SendToPrivateMessage(User userSend, User userReceive, Message message)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(userSend.ClientTcp, new DataMessage() { Message = message }));
                Task.Run(() => Transfer.SendTCP(userReceive.ClientTcp, new DataMessage() { Message = message }));

                Console.WriteLine($"Private message has been sent: {userSend.Login} from: {userReceive.Login}");
            }
        }

        private static void SendToGroups(User userSend, User[] usersReceive, Message message)
        {
            lock (lck)
            {
                foreach (var item in usersReceive)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = message }));
                    Console.WriteLine($"Group message has been sent: {userSend.Login} from: {item.Login}");
                }
                Task.Run(() => Transfer.SendTCP(userSend.ClientTcp, new DataMessage() { Message = message }));
            }
        }

        private static bool EntryCheck(TcpClient socket, DataUser data)
        {
            if (IdentificationClient(data.User) && !users.Exists((x) => x.Login == data.User.Login))
            {
                User user = new User()
                {
                    Login = data.User.Login,
                    Password = data.User.Password,
                    Tag = data.User.Tag,
                    CountBadWord = data.User.CountBadWord,
                    Birthday = usersBase[usersBase.FindIndex((x) => x.Login == data.User.Login)].Birthday,
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint,
                    IPClient = ((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString()
                };

                users.Add(user);
                return true;
            }
            else
            {
                Message mess = new Message();
                mess.Answer = "No";
                Transfer.SendTCP(socket, new DataMessage() { Message = mess });
                return false;
            }

        }

        private static bool LoginCheck(TcpClient socket, DataUser data)
        {
            if (usersBase.Count == 0)
            {
                User user = new User()
                {
                    Login = data.User.Login,
                    Password = data.User.Password,
                    Tag = data.User.Tag,
                    CountBadWord = data.User.CountBadWord,
                    Birthday = data.User.Birthday,
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint,
                    IPClient = ((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString()
                };

                usersBase.Add(user);
                users.Add(user);
                SaveData.SaveUser(user);
                return true;
            }
            else if (!ClientExist(data.User.Login))
            {
                User user = new User()
                {
                    Login = data.User.Login,
                    Password = data.User.Password,
                    Tag = data.User.Tag,
                    CountBadWord = data.User.CountBadWord,
                    Birthday = data.User.Birthday,
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint,
                    IPClient = ((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString()
                };

                usersBase.Add(user);
                users.Add(user);
                SaveData.SaveUser(user);
                return true;
            }
            else
            {
                Message mess = new Message();
                mess.Answer = "No";
                Transfer.SendTCP(socket, new DataMessage() { Message = mess });
                return false;
            }

        }

        private static void SendToEveryone(User user, Message message)
        {
            lock (lck)
            {
                foreach (var item in users)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = message }));
                }
                Console.WriteLine($"Message has been sent: {user.Login} and IP: {user.EndPointClient.Address}");
            }
        }

        private static bool ClientExist(string login)
        {
            foreach (var item in usersBase)
            {
                if (item.Login == login)
                    return true;
            }
            return false;
        }

        private static bool IdentificationClient(User user)
        {
            foreach (var item in usersBase)
            {
                if (item.Login == user.Login && item.Password == user.Password)
                    return true;
            }
            return false;
        }
    }


}
