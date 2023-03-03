using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
{
    public class Client
    {
        public TcpClient client;
        public string localIP { get; }
        public int port { get; }
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
                client = new TcpClient();
                client.Connect(localIP, port);

                if (client.Connected)
                {
                    networkStream = client.GetStream();
                    streamReader = new StreamReader(networkStream);
                    streamWriter = new StreamWriter(networkStream);
                }
            }
            catch
            {
                Console.WriteLine("Error connecting client to server");
            }
        }

        public bool sendMessage(string clientMessage)
        {
            if (string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase))
            {
                //send the server a message to say the client has left
                streamWriter.WriteLine("quit"); //this is one way to make sure "quit"/escape have the same effect, I can change this
                streamWriter.Flush();
                clientStatus = false;

                return false;
            }
            
            streamWriter.WriteLine(clientMessage);
            streamWriter.Flush();

            return true;
        }

        public string listenForMessage()
        {
            string serverMessage;
            //this is from the reader, so the message the server sent to the stream
            serverMessage = streamReader.ReadLine();

            //Console.WriteLine("Server: " + serverMessage);
            return serverMessage;
        }

        public void disconnect()
        {
            clientStatus = false;
            
            streamWriter.Close();
            networkStream.Close();
            streamWriter.Close();
        }
    }
}