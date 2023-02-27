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
                Console.Title = "Running as Client";
                string localIP = "localhost";
                //I just picked a random port number, idk what it should be
                int port = 8888;

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

            Console.WriteLine("Connected to server.");
            Thread.Sleep(1000);
            
            Console.Clear();
            
            Console.WriteLine("Press \"I\" to enter insert mode.");
            Console.WriteLine("Type \"quit\" or press Escape to close the application.");

            try
            {
                string clientMessage;
                string serverMessage;

                while (client.clientStatus)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key.            
                        ConsoleKeyInfo userKey = Console.ReadKey(true); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            clientMessage = Console.ReadLine();
                            client.sendMessage(clientMessage);
                            
                        } else if (userKey.Key == ConsoleKey.Escape)
                        {
                            //send the server a message to say the client has left
                            Console.WriteLine("You disconnected the chat. Bye!");
                            client.disconnect();

                            return;
                        }
                    }
                    //listening mode
                    if (client.networkStream.DataAvailable)
                    {
                        client.listenForMessage();
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
            int port = 8888;
            
            Server server = new Server(localIP, port);
            
            //creates and starts new tcpListener:
            server.startServer();
            
            Thread.Sleep(1000);
            Console.Clear();
            
            Console.WriteLine("Waiting for client to connect...");

            //if a client is trying to connect
            //accept client has server.acceptsocket method
            if (server.acceptClient())
            {
                Console.WriteLine("Client Connected!");
                
                Thread.Sleep(1000);
                Console.Clear();
                
                Console.WriteLine("Press \"I\" to enter insert mode.");
                Console.WriteLine("Type \"quit\" or press Escape to close the application.");
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
                        ConsoleKeyInfo userKey = Console.ReadKey(true); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            serverMessage = Console.ReadLine();
                            
                            server.sendMessage(serverMessage);
                            
                        } else if (userKey.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("You disconnected the chat. Bye!");
                            server.disconnectChat();

                            return;
                        }
                    }
                    
                    //listening mode
                    if (server.networkStream.DataAvailable)//this will circumvent blocking if there is no new data
                    {
                        server.listenForMessage();
                    }
                } //end while loop
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }
    }//end runAppAsServer
}