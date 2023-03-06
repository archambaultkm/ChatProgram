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
        
        public override void StartChat()
        {
            client = new TcpClient();
        }
        
        public override void Connect()
        {
            try
            {
                //client = new TcpClient();
                client.Connect(localIP, port);
            }
            catch
            {
                Console.WriteLine("Error connecting client to server. \nEnsure an instance of ChatProgram.exe is running in server mode (-server) \nExiting...");
                Environment.Exit(1);
            }
        }
        
        public override void OpenStreams()
        {
            networkStream = client.GetStream();
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }
    }
}