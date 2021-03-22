﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ProtocolsMessages;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MyOwnChat
{

    class Program
    {
        private static IPAddress iP;
        private static IPEndPoint endPoint;
        private static TcpListener server;
        private static string address = "127.0.0.1";
        private static int port = 1250;
        private static string pathFile = "clients.dat";
        private static List<Client> clientsFile;
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
            clientsFile = new List<Client>();
            lck = new object();
            tokenStop = new CancellationTokenSource();
            messEveryone = new string[4];
            if (File.Exists(pathFile))
                LoadClients();
            server.Start(50);

            Console.WriteLine($"Server started: {endPoint.Address} : {endPoint.Port}");

            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {

                        TcpClient socket = server.AcceptTcpClient();
                        Task.Run(() =>
                        {
                            string[] login = new string[2];
                            login = ((DataMessage)Transfer.ReceiveTCP(socket)).Array;
                            if (login != null)
                                LoginCheck(socket, login);
                            else
                                return;

                            Client client = new Client();
                            client = clients[clients.FindIndex((x) => x.Name == login[0])];
                            Console.WriteLine($"Client connected with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                            messEveryone[0] = $"{client.Name} joined to chat";
                            SendToEveryone(client, messEveryone);

                            try
                            {
                                while (!tokenStop.Token.IsCancellationRequested)
                                {
                                    string[] message = new string[4];
                                    message = ((DataMessage)Transfer.ReceiveTCP(socket)).Array;
                                    if (message[1] == "other")
                                    {
                                        string mess = $"[{client.Name}]: " + message[0];
                                        message[0] = mess;
                                        Console.WriteLine($"Message has been received: {client.Name} and IP: {client.EndPointClient.Address}");
                                        SendToEveryone(client, message);
                                    }
                                    else if (message[1] == "private")
                                    {
                                        Client clientSend = new Client();
                                        clientSend = clients[clients.FindIndex((x) => x.Name == message[2])];
                                        Client clientReceive = new Client();
                                        clientReceive = clients[clients.FindIndex((x) => x.Name == message[3])];
                                        SendToPrivateMessage(clientSend, clientReceive, message);
                                    }
                                    else if (message[1] == "exit")
                                    {
                                        Console.WriteLine($"Client passed out with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                                        messEveryone[0] = $"{client.Name} left to chat";
                                        SendToEveryone(client, messEveryone);
                                        tokenStop.Cancel();
                                    }
                                }
                            }
                            catch (SocketException e)
                            {
                                if (e.SocketErrorCode == SocketError.Interrupted)
                                    throw;
                            }

                        });

                    }
                }
                catch (Exception)
                {
                    throw;
                }
            });
            Console.ReadKey();
            Console.WriteLine("Server stoping...");
            server.Stop();
        }

        private static void SendToPrivateMessage(Client clientSend, Client clientReceive, string[] message)
        {
            lock (lck)
            {
                foreach (var item in clients)
                {
                    if (item == clientReceive)
                        Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Array = message }));
                    break;
                }
                Console.WriteLine($"Private message has been sent: {clientSend.Name} from: {clientReceive.Name}");
            }
        }


        private static void LoginCheck(TcpClient socket, string[] login)
        {
            if (clientsFile.Count == 0)
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[1],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clientsFile.Add(client);
                clients.Add(client);
                SaveClients();
            }
            else if (!ClientExist(login[0]))
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[1],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clientsFile.Add(client);
                clients.Add(client);
                SaveClients();
            }
            else if (IdentificationClient(login))
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[1],
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clients.Add(client);
            }
            else
                Transfer.SendTCP(socket, new DataMessage() { Message = "No" });
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

        private static void SaveClients()
        {
            BinaryFormatter binary = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                binary.Serialize(fs, clientsFile);
            }
        }

        private static void LoadClients()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
            {
                clientsFile = (List<Client>)formatter.Deserialize(fs);
            }
        }

        private static bool ClientExist(string name)
        {
            foreach (var item in clientsFile)
            {
                if (item.Name == name)
                    return true;
            }
            return false;
        }

        private static bool IdentificationClient(string[] name)
        {
            foreach (var item in clientsFile)
            {
                if (item.Name == name[0] || item.Password == name[1])
                    return true;
            }
            return false;
        }
    }


    [Serializable]
    public class Client
    {
        public string Name { get; set; }
        public string Password { get; set; }
        [NonSerialized]
        public TcpClient ClientTcp;
        [NonSerialized]
        public IPEndPoint EndPointClient;

    }

}
