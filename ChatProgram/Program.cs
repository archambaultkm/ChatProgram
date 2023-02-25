using System;
using System.Threading;
using ChatLib;
using System.Linq;
using System.Net;

namespace ChatProgram
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-server"))
            {
                runAppAsServer();
            }
            else
            {
                runAppAsClient();
            }
        }//end main

        private static void runAppAsClient()
        {
            Console.Title = "Running as Client";
            string localIP = "localhost";
            //I just picked a random port number, idk what it should be
            int port = 8888;

            Client client = new Client(localIP, port);
            
            //makes a new tcpClient/stream objects
            client.ConnectToServer();
            
            Thread.Sleep(500);
            Console.Clear();

            try
            {
                string clientMessage = "";
                string serverMessage = "";

                while (client.clientStatus)
                {
                    Console.Write(">>");
                    clientMessage = Console.ReadLine();

                    if (string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase))
                    {
                        
                        //send the server a message to say the client has left
                        client.streamWriter.WriteLine(clientMessage);
                        client.streamWriter.WriteLine("Client has left the chat");

                        client.streamWriter.Flush();
                        client.clientStatus = false;
                    }
                    else
                    {
                        client.streamWriter.WriteLine(clientMessage);
                        client.streamWriter.Flush();
                        
                        //this is from the reader, so the message the server sent to the stream
                        serverMessage = client.streamReader.ReadLine();
                        Console.WriteLine("Server: " + serverMessage);
                    }
                }//end while(client.clientStatus)
            }
            
            catch
            {
                Console.WriteLine("Problem reading from server");
            }
            
            //closes stream objects
            client.disconnect();
        }

        private static void runAppAsServer()
        {
            Console.Title = "Running as Server";
                
            //idk the rules around these so look into whether there's a better way to do it:
            IPAddress localIP = IPAddress.Any;
            int port = 8888;
            
            Server server = new Server(localIP, port);
            
            //creates and starts new tcpListener:
            server.startServer();

            bool clientConnected = false;

            while (!clientConnected)
            {
                Thread.Sleep(500);
                Console.Clear();
                Console.WriteLine("Waiting for client to connect...");
            
                //if a client is trying to connect
                //accept client has server.acceptsocket method
                if (server.acceptClient())
                {
                    clientConnected = true;
                    Console.WriteLine("Client Connected");
                }
            }
            

            try
            {
                string clientMessage = "";
                string serverMessage = "";
                
                //create networkstream and readers/writers
                server.clientCommunication();
                
                //run until the client tries to exit
                while (server.serverStatus)
                {
                    if (server.socketForClient.Connected)
                    {
                        serverMessage = server.streamReader.ReadLine();
                        //print their message to the console
                        Console.WriteLine("Client: " + serverMessage);

                        //check if they've entered any variation of the word "quit"
                        if (string.Equals(serverMessage, "quit", StringComparison.OrdinalIgnoreCase))
                        {
                            server.serverStatus = false;
                            
                            server.streamReader.Close();
                            server.networkStream.Close();
                            server.streamWriter.Close();
                            
                            return;
                        }

                        //indicate to the server they're in insert mode
                        Console.Write(">>");
                        clientMessage = Console.ReadLine();

                        //send to the client
                        server.streamWriter.WriteLine(clientMessage);

                        //clean the buffer to prevent errors
                        server.streamWriter.Flush();

                    }//end if(server.socketForClient.Connected)
                } //end while loop
                
                //closes stream objects and client socket
                Console.WriteLine("Exiting...");
                server.disconnectChat();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }
    }
}


