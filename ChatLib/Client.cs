using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
{
    public class Client
    {
        public string localIP { get; private set; }
        public int port { get; set; }
        public bool clientStatus = true;
        
        public NetworkStream networkStream { get; private set; }
        public StreamWriter streamWriter { get; private set; }
        public StreamReader streamReader { get; private set; }

        public Client(string localIP, int port)
        {
            this.localIP = localIP;
            this.port = port;
        }

        public void ConnectToServer()
        {
            try
            {
                //program will fail if ip isn't same as server
                TcpClient client = new TcpClient(localIP, port);
                
                networkStream = client.GetStream();
                streamReader = new StreamReader(networkStream);
                streamWriter = new StreamWriter(networkStream);
                
                Console.WriteLine("Connected to server");
            }
            catch
            {
                //maybe this is the validation?
                Console.WriteLine("Error connecting client to server");
            }
        }

        public void disconnect()
        {
            streamWriter.Close();
            networkStream.Close();
            streamWriter.Close();
        }
    }
}