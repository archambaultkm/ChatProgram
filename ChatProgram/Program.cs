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
            
            //at this point the client program will crash if run before server
            Thread.Sleep(500);
            Console.Clear();

            try
            {
                string clientMessage;
                string serverMessage;

                while (client.clientStatus)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key.            
                        ConsoleKeyInfo userKey = Console.ReadKey(); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            clientMessage = Console.ReadLine();
                            //break;
                            
                            if (string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase) || (userKey.Key == ConsoleKey.Escape))
                            {
                        
                                //send the server a message to say the client has left
                                client.streamWriter.WriteLine("quit"); //this is one way to make sure "quit"/escape have the same effect, I can change this

                                client.streamWriter.Flush();
                                client.clientStatus = false;

                                return;
                            }
                            
                            client.streamWriter.WriteLine(clientMessage);
                            client.streamWriter.Flush();

                            //this is from the reader, so the message the server sent to the stream
                            serverMessage = client.streamReader.ReadLine();
                            Console.WriteLine("Server: " + serverMessage);
                            
                        }
                        else
                        {
                            Console.WriteLine($"You typed {userKey.Key}");
                            Console.WriteLine("Press \"I\" to enter insert mode.");
                            Console.WriteLine("Type \"quit\" or press Escape to close the application.");
                            Thread.Sleep(1000);
                        }
                    }
                }//end while(client.clientStatus)
            }
            catch
            {
                Console.WriteLine("Problem reading from server");
            }
            
            //closes stream objects
            client.disconnect();
        } //end runAppAsClient

        private static void runAppAsServer()
        {
            Console.Title = "Running as Server";
                
            //use IPAddress.Any to accept any connection (localhost wil work)
            IPAddress localIP = IPAddress.Any;
            //I just picked a random port
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
                string clientMessage;
                string serverMessage;
                
                //create networkstream and readers/writers
                server.clientCommunication();
                
                //run until the client tries to exit
                while (server.serverStatus)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key.            
                        ConsoleKeyInfo userKey = Console.ReadKey(); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            serverMessage = Console.ReadLine();
                            //send to the client
                            server.streamWriter.WriteLine(serverMessage);

                            //clean the buffer to prevent errors
                            server.streamWriter.Flush();
                        }
                    }

                    clientMessage = server.streamReader.ReadLine(); //this will block

                    //check if they've entered any variation of the word "quit"
                    if (string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Client has disconnected from the chat.");
                        Console.WriteLine("Exiting...");
                        //closes stream objects and client socket
                        server.disconnectChat();
                        server.serverStatus = false;

                        return;
                    }

                    //print their message to the console
                    Console.WriteLine("Client: " + clientMessage);
                    
                    //end if(server.socketForClient.Connected)
                } //end while loop
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }
    }//end runAppAsServer
}