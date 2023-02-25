using System;
using System.IO;
using System.Threading;
using ChatLib;
using System.Linq;
using System.Net;
using System.Net.Sockets;

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
            //ip might need to be changed to string value for client
            string localIP = IPAddress.Any.ToString();
            int port = 8888;

            Client client = new Client(localIP, port);
            
            client.ConnectToServer();
            Console.WriteLine("Client connected successfully to server");
            
            Thread.Sleep(500);
            Console.Clear();
            
            client.serverCommunication();

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
                        client.clientStatus = false;
                        //send the server a message to say the client has left
                        client.streamWriter.WriteLine("Client has left the chat");
                        
                        client.streamWriter.Flush();
                        client.streamWriter.Close();
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
            
            client.disconnect();
        }

        private static void runAppAsServer()
        {
            Console.Title = "Running as Server";
                
            //idk the rules around these so look into whether there's a better way to do it:
            IPAddress localIP = IPAddress.Any;
            int port = 8888;
            
            Server server = new Server(localIP, port);
            
            server.startServer();
            Console.WriteLine("Server started successfully");
            
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Waiting for client to connect...");
            
            //if a client is trying to connect
            server.acceptClient();
            Console.WriteLine("Client Connected");

            try
            {
                string clientMessage = "";
                string serverMessage = "";
                
                server.clientCommunication();
                
                //run until the client tries to exit
                while (server.serverStatus)
                {
                    if (server.socketForClient.Connected)
                    {
                        clientMessage = server.streamReader.ReadLine();
                        //print their message to the console
                        Console.WriteLine("Client: " + clientMessage);

                        //check if they've entered any variation of the word "quit"
                        if (string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase))
                        {
                            server.serverStatus = false;
                            //could be an error with calling this rather than closing the necessary ones, idk if closing the client socket will work
                            server.disconnectChat();
                            return;
                        }

                        //indicate to the server they're in insert mode
                        Console.Write(">>");
                        serverMessage = Console.ReadLine();

                        //send to the client
                        server.streamWriter.WriteLine(serverMessage);

                        //clean the buffer to prevent errors
                        server.streamWriter.Flush();

                    }//end if(server.socketForClient.Connected)
                } //end while loop
                
                server.disconnectChat();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }
    }
}


