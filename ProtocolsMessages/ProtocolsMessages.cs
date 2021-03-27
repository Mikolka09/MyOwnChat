﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProtocolsMessages
{
    public interface Data { }


    [Serializable]
    public class DataMessage : Data
    {
        public string Message { get; set; }
        public string[] Array { get; set; }
    }

    [Serializable]
    public class DataFile : Data
    {
        public List<byte[]> FileByte { get; set; }
    }

    public class DataClient : Data
    {
        public string pathFile = @"D:\Users\MIKOLKA\MyOwnChat\MyOwnChat\ProtocolsMessages\bin\Debug\clients.data";
        public List<Client> clientsFile;
    }

    public class Transfer
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public static void SendTCP(TcpClient client, Data data)
        {
            try
            {
                formatter.Serialize(client.GetStream(), data);
            }
            catch { }

        }

        public static Data ReceiveTCP(TcpClient client)
        {
            try
            { 
            return (Data)formatter.Deserialize(client.GetStream());
            }
            catch(IOException) { throw; }


        }
    }

    public class BinarySaveLoad
    {
        private static BinaryFormatter br = new BinaryFormatter();

        public static void SaveClients(DataClient data)
        {
            using (FileStream fs = new FileStream(data.pathFile, FileMode.OpenOrCreate))
            {
                br.Serialize(fs, data.clientsFile);
            }
        }

        public static void LoadClients(DataClient data)
        {
            using (FileStream fs = new FileStream(data.pathFile, FileMode.Open))
            {
                var cnt = (List<Client>)br.Deserialize(fs);
                data.clientsFile = cnt;
            }
        }
    }

    [Serializable]
    public class Client
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int CountBadWord { get; set; }
        public string Birthday { get; set; }
        [NonSerialized]
        public TcpClient ClientTcp;
        [NonSerialized]
        public IPEndPoint EndPointClient;



    }



}
