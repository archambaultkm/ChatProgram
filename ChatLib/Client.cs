using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
//at this point the client program will crash if run before server
{
    public class Client : ChatBase
    {
        public TcpClient client;
        public Client(string localIP, int port)
        {
            base.localIP = localIP;
            base.port = port;
        }
        
        public void ConnectToServer()
        {
            try
            {
                client = new TcpClient();
                client.Connect(localIP, port);

                if (client.Connected)
                {
                    networkStream = client.GetStream();
                    streamReader = new StreamReader(networkStream);
                    streamWriter = new StreamWriter(networkStream);
                    
                    Console.WriteLine("Connected to server.");
                }
            }
            catch
            {
                Console.WriteLine("Error connecting client to server");
            }
        }
    }
}