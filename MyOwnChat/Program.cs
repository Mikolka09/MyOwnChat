using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ProtocolsMessages;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace MyOwnChat
{
    [Serializable]
    class Program
    {
        private static IPAddress iP;
        private static IPEndPoint endPoint;
        private static TcpListener server;
        private static string address = "127.0.0.1";
        private static int port = 1250;
        private static string pathFile = "clients.json";
        private static ListClient listClient;
        private static Client client;
        private static object lck;

        static void Main(string[] args)
        {
            iP = IPAddress.Parse(address);
            endPoint = new IPEndPoint(iP, port);
            server = new TcpListener(endPoint);
            listClient = new ListClient();
            listClient.clients = new List<Client>();
            //clients = new List<Client>();
            lck = new object();
            //if (File.Exists(pathFile))
               // LoadClients();
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
                            //string pass = ((DataMessage)Transfer.ReceiveTCP(socket)).Message;
                            if (listClient.clients.Count == 0)
                            {
                                client = new Client()
                                {
                                    Name = login[0],
                                    Password = login[1],
                                    ClientTcp = socket,
                                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                                };
                                listClient.clients.Add(client);
                                //SaveClients();
                            }
                            else if (!ClientExist(login[0]))
                            {
                                client = new Client()
                                {
                                    Name = login[0],
                                    Password = login[1],
                                    ClientTcp = socket,
                                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                                };
                                listClient.clients.Add(client);
                                //SaveClients();
                            }
                            else if (!IdentificationClient(login))
                            {
                                Transfer.SendTCP(socket, new DataMessage() { Message = "No" });
                            }

                            Console.WriteLine($"Client connected with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                            SendToEveryone(client, $"{client.Name} joined to chat");

                            while (true)
                            {
                                string message = $"[{client.Name}]: " + ((DataMessage)Transfer.ReceiveTCP(client.ClientTcp)).Message;
                                Console.WriteLine($"Message has been received: {client.Name} and IP: {client.EndPointClient.Address}");
                                SendToEveryone(client, message);
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

        private static void SendToEveryone(Client client, string message)
        {
            lock (lck)
            {
                foreach (var item in listClient.clients)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = message }));
                }
                Console.WriteLine($"Message has been sent: {client.Name} and IP: {client.EndPointClient.Address}");
            }
        }

        private static async void SaveClients()
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
 await JsonSerializer.SerializeAsync(fs, listClient.clients);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //formatter.Serialize(fs, listClient.clients);
               
            }
        }

        private static void LoadClients()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
            {
                listClient = (ListClient)formatter.Deserialize(fs);
            }
        }

        private static bool ClientExist(string name)
        {
            foreach (var item in listClient.clients)
            {
                if (item.Name == name)
                {
                    Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = "No" });
                    return true;
                }
            }
            return false;
        }

        private static bool IdentificationClient(string[] name)
        {
            foreach (var item in listClient.clients)
            {
                if (item.Name != name[0] || item.Password != name[1])
                    return false;
            }
            return true;
        }
    }


   // [Serializable]
    public class Client
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public TcpClient ClientTcp { get; set; }
        public IPEndPoint EndPointClient { get; set; }
    }

    //[Serializable]
    public class ListClient
    {
        public List<Client> clients { get; set; }
    }

}
