using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
{
    public class Client
    {
        public string localIP { get; private set; }
        public int port { get; set; }
        private TcpClient socketForServer;
        public bool clientStatus = true;
        
        public NetworkStream networkStream { get; set; }
        public StreamWriter streamWriter { get; set; }
        public StreamReader streamReader { get; set; }

        public Client(string localIP, int port)
        {
            this.localIP = localIP;
            this.port = port;
        }

        public void ConnectToServer()
        {
            try
            {
                //maybe add validation that it's the same as server?
                socketForServer = new TcpClient(localIP.ToString(), port);
            }
            catch
            {
                //maybe this is the validation?
                Console.WriteLine("Error connecting client to server");
            }
        }

        public void serverCommunication()
        {
            networkStream = socketForServer.GetStream();
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }

        public void disconnect()
        {
            streamWriter.Close();
            networkStream.Close();
            streamWriter.Close();
            socketForServer.Close();
        }
    }
}