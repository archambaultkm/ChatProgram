using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatLib
{
    public class Server : ChatBase
    {
        public Socket socketForClient { get; set; }
        private TcpListener tcpListener { get; set; }
        public Server(string localIp, int port)
        {
            base.localIP = localIp;
            base.port = port;
        }

        public void StartServer()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(localIP), port);
                tcpListener.Start();
            }
            catch
            {
                Console.WriteLine("Error starting server");
            }
        }

        public void AcceptClient()
        {
            try
            {
                //blocking method:
                socketForClient = tcpListener.AcceptSocket();
                Console.WriteLine("Client Connected!");
            }
            catch
            {
                Console.WriteLine("Error accepting client");
            }
        }

        public void OpenStreams()
        {
            //data from the client
            networkStream = new NetworkStream(socketForClient);
            //allows reading/writing client data:
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }
        
        public override void DisconnectChat()
        {
            base.DisconnectChat();
            socketForClient.Close();
        }
    }
}