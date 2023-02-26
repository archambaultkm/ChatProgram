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
                //program will fail if ip isn't same as server
                //TcpClient client = new TcpClient();
                client = new TcpClient();
                client.Connect(localIP, port);

                if (client.Connected)
                {
                    networkStream = client.GetStream();
                    streamReader = new StreamReader(networkStream);
                    streamWriter = new StreamWriter(networkStream);
                    
                    //for testing:
                    Console.WriteLine("Connected to server");
                }
            }
            catch
            {
                //maybe this is the validation?
                Console.WriteLine("Error connecting client to server");
            }
        }

        public void sendMessage(String message)
        {
            if (client.Connected)
            {
                networkStream = client.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message);
                networkStream.Write(outStream, 0, outStream.Length);
                networkStream.Flush();
                    
                //streamReader = new StreamReader(networkStream);
                //streamWriter = new StreamWriter(networkStream);
                
                //for testing (ideally this method would return the value so that it can be printed from the main method)
                Console.WriteLine(message);
            }
        }

        public void recieveMessage()
        {
            //for server response
            byte[] outStream = new byte[256];
            String responseData = String.Empty;
                    
            //read
            Int32 bytes = networkStream.Read(outStream, 0, outStream.Length);
            responseData = System.Text.Encoding.ASCII.GetString(outStream, 0, bytes);
            
            //again just for testing, this method should return that value and it should be printed from main method
            Console.WriteLine("Recieved: {0}", responseData);
        }

        public void disconnect()
        {
            streamWriter.Close();
            networkStream.Close();
            streamWriter.Close();
        }
    }
}