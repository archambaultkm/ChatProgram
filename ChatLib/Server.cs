using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatLib
{
    public class Server
    {
        public IPAddress localIP { get; private set; }
        public int port { get; private set; }
        public bool serverStatus = true;
        public Socket socketForClient { get; set; }
        
        public NetworkStream networkStream { get; set; }
        public StreamReader streamReader { get; set; }
        public StreamWriter streamWriter { get; set; }
        
        private TcpListener tcpListener { get; set; }
        public Server(IPAddress localIp, int port)
        {
            this.localIP = localIp;
            this.port = port;
        }

        public void startServer()
        {
            try
            {
                tcpListener = new TcpListener(localIP, port);
                tcpListener.Start();
                Console.WriteLine("Server started successfully");
            }
            catch
            {
                Console.WriteLine("Error starting server");
            }
        }

        public bool acceptClient()
        {
            try
            {
                socketForClient = tcpListener.AcceptSocket();
                return true;
            }
            catch
            {
                Console.WriteLine("Error accepting client");
                return false;
            }
        }

        public void clientCommunication()
        {
            //data from the client
            networkStream = new NetworkStream(socketForClient);
            //allows reading/writing client data:
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }

        public void disconnectChat()
        {
            networkStream.Close();
            streamWriter.Close();
            streamReader.Close();
            socketForClient.Close();
        }
    }
}