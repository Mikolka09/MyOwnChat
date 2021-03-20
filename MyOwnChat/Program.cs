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
        private static List<Client> clients;
        private static object lck;

        static void Main(string[] args)
        {
            iP = IPAddress.Parse(address);
            endPoint = new IPEndPoint(iP, port);
            server = new TcpListener(endPoint);
            clients = new List<Client>();
            lck = new object();
            //if (File.Exists(pathFile))
            //    LoadClients();
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
                            LoginCheck(socket, login);

                            Client client = new Client();
                            client = clients[clients.FindIndex((x) => x.Name == login[0])];
                            Console.WriteLine($"Client connected with Name: {client.Name} and IP: {client.EndPointClient.Address}");
                            SendToEveryone(client, $"{client.Name} joined to chat");

                            try
                            {
                                while (true)
                                {
                                    string message = $"[{client.Name}]: " + ((DataMessage)Transfer.ReceiveTCP(socket)).Message;
                                    Console.WriteLine($"Message has been received: {client.Name} and IP: {client.EndPointClient.Address}");
                                    SendToEveryone(client, message);
                                }
                            }
                            catch (Exception)
                            {
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

        private static void LoginCheck(TcpClient socket, string[] login)
        {
            if (clients.Count == 0)
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[1],
                    //Ip = ((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString()
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clients.Add(client);
                //SaveClients();
            }
            else if (!ClientExist(login[0]))
            {
                Client client = new Client()
                {
                    Name = login[0],
                    Password = login[1],
                    //Ip = ((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString()
                    ClientTcp = socket,
                    EndPointClient = (IPEndPoint)socket.Client.LocalEndPoint
                };

                clients.Add(client);
                //SaveClients();
            }
            else if (!IdentificationClient(login))
            {
                Transfer.SendTCP(socket, new DataMessage() { Message = "No" });
            }
        }

        private static void SendToEveryone(Client client, string message)
        {
            lock (lck)
            {
                foreach (var item in clients)
                {
                    Task.Run(() => Transfer.SendTCP(item.ClientTcp, new DataMessage() { Message = message }));
                }
                Console.WriteLine($"Message has been sent: {client.Name} and IP: {client.EndPointClient.Address}");
            }
        }

        private static void SaveClients()
        {
            BinaryFormatter binary = new BinaryFormatter();
            using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate))
            {
                binary.Serialize(fs, clients);
            }
        }

        private static void LoadClients()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
            {
                clients = (List<Client>)formatter.Deserialize(fs);
            }
        }

        private static bool ClientExist(string name)
        {
            foreach (var item in clients)
            {
                if (item.Name == name)
                    return true;
            }
            return false;
        }

        private static bool IdentificationClient(string[] name)
        {
            foreach (var item in clients)
            {
                if (item.Name == name[0] || item.Password == name[1])
                    return true;
            }
            return false;
        }
    }


    //[Serializable]
    public class Client
    {
        public string Name { get; set; }
        public string Password { get; set; }
        //[NonSerialized]
        public TcpClient ClientTcp { get; set; }
        //[NonSerialized]
        public IPEndPoint EndPointClient { get; set; }
        //public string Ip { get; set; }

    }

}
