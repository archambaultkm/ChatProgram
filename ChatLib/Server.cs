using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatLib
{
    public class Server
    {
        public IPAddress localIP { get; }
        public int port { get; }
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
                //Console.WriteLine("Server started successfully");
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
                //blocking method:
                socketForClient = tcpListener.AcceptSocket();
                return true;
            }
            catch
            {
                Console.WriteLine("Error accepting client");
                return false;
            }
        }

        public void startCommunication()
        {
            //data from the client
            networkStream = new NetworkStream(socketForClient);
            //allows reading/writing client data:
            streamReader = new StreamReader(networkStream);
            streamWriter = new StreamWriter(networkStream);
        }

        public void disconnectChat()
        {
            serverStatus = false;
            
            networkStream.Close();
            streamWriter.Close();
            streamReader.Close();
            socketForClient.Close();
        }
        
        public bool sendMessage(string outgoingMessage)
        {
            if (string.Equals(outgoingMessage, "quit", StringComparison.OrdinalIgnoreCase))
            {
                streamWriter.WriteLine("quit"); //this is one way to make sure "quit"/escape have the same effect, I can change this
                                
                streamWriter.Flush();
                serverStatus = false;

                return false;
            }
                            
            streamWriter.WriteLine(outgoingMessage);
            streamWriter.Flush();

            return true;
        }
        
        public string listenForMessage()
        {
            string incomingMessage;
            //this is from the reader, so the message the client sent to the stream
            incomingMessage = streamReader.ReadLine();

            //Console.WriteLine("Client: " + incomingMessage);
            return incomingMessage;
        }
    }
}