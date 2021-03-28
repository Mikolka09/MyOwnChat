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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyOwnChat
{

    class Program
    {
        private static IPAddress iP;
        private static IPEndPoint endPoint;
        private static TcpListener server;
        private static string address = "127.0.0.1";
        private static int port = 1250;
        public static DataClient dataClient;
        private static List<Client> clients;
        private static object lck;
        private static CancellationTokenSource tokenStop;
        private static string[] messEveryone;

        static void Main(string[] args)
        {
            iP = IPAddress.Parse(address);
            endPoint = new IPEndPoint(iP, port);
            server = new TcpListener(endPoint);
            clients = new List<Client>();
            dataClient = new DataClient();
            dataClient.clientsFile = new List<Client>();
            lck = new object();
            tokenStop = new CancellationTokenSource();
            messEveryone = new string[5];
            if (File.Exists(dataClient.pathFile))
                BinarySaveLoad.LoadClients(dataClient);
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
                    else
                        ServerMediator(socket, ((DataMessage)data).Array);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception) { }
        }

        public static int CountOneWord(string txt, string reg)
        {
            int cnt = 0;
            MatchCollection matchs = Regex.Matches(txt, reg, RegexOptions.IgnoreCase);
            cnt = matchs.Count;
            return cnt;
        }

        private static string CensoringPosts(string message, Client client)
        {
            string[] badWord = {"fuck", "shit", "son of a bitch", "asshole", "bint", "bollocks", "munter", "bastard", "snatch", "dick",
                                "козел", "сука", "урод", "дерьмо", "член", "ублюдок"};
            string change = "#######";
            string txt = message;
            foreach (var item in badWord)
            {
                string reg = "\\b" + $"{item}" + "\\b";
                client.CountBadWord += CountOneWord(txt, reg);
                txt = Regex.Replace(txt, reg, change, RegexOptions.IgnoreCase);
            }
            return txt;
        }

        private static void ServerMediator(TcpClient socket, string[] message)
        {
            Client client = new Client();
            string messCheck = message[1];
            switch (messCheck)
            {
                case "avtorization":
                    if (LoginCheck(socket, message))
                    {
                        client = clients[clients.FindIndex((x) => x.Name == message[0])];
                        Console.WriteLine($"Client connected with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                        messEveryone[0] = $"{client.Name} joined to chat";
                        SendToEveryone(client, messEveryone);
                    }
                    break;
                case "create":
                    {
                        client = clients[clients.FindIndex((x) => x.Name == message[0])];
                        Console.WriteLine($"Client created Admin with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                        break;
                    }
                case "other":
                    {
                        client = clients[clients.FindIndex((x) => x.Name == message[2])];
                        string messCen = CensoringPosts(message[0], client);
                        string mess = $"[{client.Name}]: " + messCen;
                        message[0] = mess;
                        Console.WriteLine($"Message has been received: {client.Name} and IP: {client.EndPointClient.Address}");
                        SendToEveryone(client, message);
                        break;
                    }
                case "private":
                    {
                        Client clientSend = new Client();
                        clientSend = clients[clients.FindIndex((x) => x.Name == message[2])];
                        Client clientReceive = new Client();
                        clientReceive = clients[clients.FindIndex((x) => x.Name == message[3])];
                        string messCen = CensoringPosts(message[0], clientSend);
                        string mess = $"[Private: {clientSend.Name}]: " + messCen;
                        message[0] = mess;
                        SendToPrivateMessage(clientSend, clientReceive, message);
                        break;
                    }
                case "group":
                    {
                        Client clientSend = new Client();
                        clientSend = clients[clients.FindIndex((x) => x.Name == message[2])];
                        string[] clientsR = message[3].Split();
                        Client[] clientsReceive = new Client[clientsR.Length];
                        for (int i = 0; i < clientsReceive.Length; i++)
                        {
                            clientsReceive[i] = clients[clients.FindIndex((x) => x.Name == clientsR[i])];
                        }
                        string messCen = CensoringPosts(message[0], clientSend);
                        string mess = $"[Groups: {clientSend.Name}]: " + messCen;
                        message[0] = mess;
                        SendToGroups(clientSend, clientsReceive, message);
                        break;
                    }
                case "exit":
                    {
                        client = clients[clients.FindIndex((x) => x.Name == message[2])];
                        Console.WriteLine($"Client passed out with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                        messEveryone[0] = $"{client.Name} left to chat";
                        SendToEveryone(client, messEveryone);
                        clients.RemoveAt(clients.FindIndex((x) => x.Name == message[2]));
                        BinarySaveLoad.SaveClients(dataClient);
                        // Cancel();
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
            Client clientSend = new Client();
            Client clientReceive = new Client();
            mess = name.Split();
            switch (mess[2])
            {
                case "private":
                    {
                        clientSend = new Client();
                        clientSend = clients[clients.FindIndex((x) => x.Name == mess[1])];
                        clientReceive = new Client();
                        clientReceive = clients[clients.FindIndex((x) => x.Name == mess[0])];
                        string mes = $"[SendFile: {clientSend.Name}]: File sent!";
                        string[] sub = new string[4];
                        sub[0] = mes;
                        sub[1] = mess[2];
                        sub[2] = mess[1];
                        sub[3] = mess[0];
                        SendToFile(clientSend, clientReceive, fileByte, sub);
                        break;
                    }
                case "other":
                    {
                        clientSend = new Client();
                        clientSend = clients[clients.FindIndex((x) => x.Name == mess[1])];
                        string mes = $"[SendFile: {clientSend.Name}]: File sent!";
                        string[] sub = new string[4];
                        sub[0] = mes;
                        sub[1] = mess[2];
                        sub[2] = mess[1];
                        sub[3] = "";
                        SendToOtherFile(clientSend, fileByte, sub);
                        break;
                    }
                default:
                    break;
            }
        }

        private static void SendToOtherFile(Client clientSend, List<byte[]> fileByte, string[] sub)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(clientSend.ClientTcp, new DataMessage() { Array = sub }));
                foreach (var item in clients)
                {
                    if (item.Name != clientSend.Name)
                    {
                        Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Array = sub }));
                        Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataFile() { FileByte = fileByte }));
                    }
                }
                Console.WriteLine($"File has been sent: {clientSend.Name} and IP: {clientSend.EndPointClient.Address}");
            }
        }


        private static void SendToFile(Client clientSend, Client clientReceive, List<byte[]> fileByte, string[] sub)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(clientSend.ClientTcp, new DataMessage() { Array = sub }));
                Task.Run(() => Transfer.SendTCP(clientReceive.ClientTcp, new DataMessage() { Array = sub }));
                Task.Run(() => Transfer.SendTCP(clientReceive.ClientTcp, new DataFile() { FileByte = fileByte }));

                Console.WriteLine($"File has been sent: {clientSend.Name} from: {clientReceive.Name}");
            }
        }

        private static void SendToPrivateMessage(Client clientSend, Client clientReceive, string[] message)
        {
            lock (lck)
            {
                Task.Run(() => Transfer.SendTCP(clientSend.ClientTcp, new DataMessage() { Array = message }));
                Task.Run(() => Transfer.SendTCP(clientReceive.ClientTcp, new DataMessage() { Array = message }));

                Console.WriteLine($"Private message has been sent: {clientSend.Name} from: {clientReceive.Name}");
            }
        }

        private static void SendToGroups(Client clientSend, Client[] clientsReceive, string[] message)
        {
            lock (lck)
            {
                foreach (var item in clientsReceive)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Array = message }));
                    Console.WriteLine($"Private message has been sent: {clientSend.Name} from: {item.Name}");
                }
                Task.Run(() => Transfer.SendTCP(clientSend.ClientTcp, new DataMessage() { Array = message }));
            }
        }

        private static bool LoginCheck(TcpClient socket, string[] login)
        {
            if (dataClient.clientsFile.Count == 0)
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[2],
                    CountBadWord = 0,
                    Birthday = login[3],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                dataClient.clientsFile.Add(client);
                clients.Add(client);
                BinarySaveLoad.SaveClients(dataClient);
                return true;
            }
            else if (!ClientExist(login[0]))
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[2],
                    CountBadWord = 0,
                    Birthday = login[3],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                dataClient.clientsFile.Add(client);
                clients.Add(client);
                BinarySaveLoad.SaveClients(dataClient);
                return true;
            }
            else if (IdentificationClient(login) && !clients.Exists((x) => x.Name == login[0]))
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[2],
                    CountBadWord = 0,
                    Birthday = login[3],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clients.Add(client);
                return true;
            }
            else
            {
                string[] mess = new string[5];
                mess[4] = "No";
                Transfer.SendTCP(socket, new DataMessage() { Array = mess });
                return false;
            }

        }

        private static void SendToEveryone(Client client, string[] message)
        {
            lock (lck)
            {
                foreach (var item in clients)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Array = message }));
                }
                Console.WriteLine($"Message has been sent: {client.Name} and IP: {client.EndPointClient.Address}");
            }
        }

        private static bool ClientExist(string name)
        {
            foreach (var item in dataClient.clientsFile)
            {
                if (item.Name == name)
                    return true;
            }
            return false;
        }

        private static bool IdentificationClient(string[] name)
        {
            foreach (var item in dataClient.clientsFile)
            {
                if (item.Name == name[0] && item.Password == name[2])
                    return true;
            }
            return false;
        }
    }


}
