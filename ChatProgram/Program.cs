using System;
using System.Threading;
using ChatLib;
using System.Linq;
using System.Net;

namespace ChatProgram
{
    public class Program

    {
        private const Int32 port = 8888;
        private const string ipAddr = "127.0.0.1";
        static void Main(string[] args)
        {
            if (args.Contains("-server"))
            {
                Console.Title = "Running as Server";
                runAppAsServer();
            }
            else
            {
                Console.Title = "Running as Client";
                runAppAsClient();
            }
        }//end main

        private static void runAppAsClient()
        {
            Client client = new Client(ipAddr, port);
            
            //makes a new tcpClient/stream objects
            client.ConnectToServer();

            Console.WriteLine("Connected to server.");
            Thread.Sleep(1000);
            
            Console.Clear();
            
            Console.WriteLine("Press \"I\" to enter insert mode.");
            Console.WriteLine("Type \"quit\" or press Escape to close the application.");

            try
            {
                string clientMessage;
                string serverMessage;
                
                while (client.status)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key. Intercept:true prevents them typing the entered letter to console          
                        ConsoleKeyInfo userKey = Console.ReadKey(true); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            clientMessage = Console.ReadLine();
                            
                            //client.sendMessage returns false if they enter a string to quit the program
                            if (!client.SendMessage(clientMessage))
                            {
                                Console.WriteLine("You disconnected the chat. Bye!");
                            }
                            
                        } else if (userKey.Key == ConsoleKey.Escape)
                        {
                            //send the server a message to say the client has left
                            Console.WriteLine("You disconnected the chat. Bye!");
                            client.SendMessage("quit");
                            client.DisconnectChat();

                            return;
                        }
                    }
                    //listening mode
                    if (client.networkStream.DataAvailable)
                    {
                        serverMessage = client.ListenForMessage();

                        //check if they've entered any variation of the word "quit", and if not print it
                        if (!string.Equals(serverMessage, "quit", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Server: " + serverMessage);
                        }
                        else
                        {
                            Console.WriteLine("Server has disconnected the chat.");
                            Console.WriteLine("Exiting...");
                            client.DisconnectChat();
                        }
                    }
                }//end while(client.clientStatus)
            }
            catch
            {
                Console.WriteLine("Problem reading from server");
            }
        } //end runAppAsClient

        private static void runAppAsServer()
        {
            Server server = new Server(ipAddr, port);
            
            //creates and starts new tcpListener:
            server.StartServer();
            
            Thread.Sleep(1000);
            Console.Clear();
            
            Console.WriteLine("Waiting for client to connect...");

            //accept client has server.acceptsocket method
            if (server.AcceptClient())
            {
                Console.WriteLine("Client Connected!");
                
                Thread.Sleep(1000);
                Console.Clear();
                
                Console.WriteLine("Press \"I\" to enter insert mode.");
                Console.WriteLine("Type \"quit\" or press Escape to close the application.");
            }
            
            try
            {
                string serverMessage;
                string clientMessage;
                
                //create networkstream and readers/writers
                server.OpenStreams();
                
                //run until the somebody tries to exit
                while (server.status)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user presses "I" key.            
                        ConsoleKeyInfo userKey = Console.ReadKey(true); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            serverMessage = Console.ReadLine();

                            if (!server.SendMessage(serverMessage))
                            {
                                Console.WriteLine("You disconnected the chat. Bye!");
                            }
                            
                        } else if (userKey.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("You disconnected the chat. Bye!");
                            server.SendMessage("quit");
                            server.DisconnectChat();

                            return;
                        }
                    }
                    
                    //listening mode
                    if (server.networkStream.DataAvailable)//this will circumvent blocking if there is no new data
                    {
                        clientMessage = server.ListenForMessage();

                        //check if they've entered any variation of the word "quit", and if not print it
                        if (!string.Equals(clientMessage, "quit", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Client: " + clientMessage);
                        }
                        else
                        {
                            Console.WriteLine("Client has disconnected the chat.");
                            Console.WriteLine("Exiting...");
                            server.DisconnectChat();
                        }
                    }
                } //end while loop
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
            }
        }//end runAppAsServer
    }//end main class
}//end namespace