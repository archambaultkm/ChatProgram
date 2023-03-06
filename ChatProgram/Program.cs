using System;
using System.Threading;
using ChatLib;
using System.Linq;

namespace ChatProgram
{
    public class Program
    {
        private const Int32 port = 8888;
        private const string ipAddr = "127.0.0.1";
        public static void Main(string[] args)
        {
            if (args.Contains("-server"))
            {
                Console.Title = "Running as Server";
                Server server = new Server(ipAddr, port);
                RunChat(server, "Client");
            }
            else
            {
                Console.Title = "Running as Client";
                Client client = new Client(ipAddr, port);
                RunChat(client, "Server");
            }
        }//end main

        private static void RunChat(ChatBase user, string otherUser)
        {
            //make tcpclient/tcplistener
            user.StartChat();
            
            //client will connect to server, server will accept client socket
            Console.WriteLine("Waiting for connection...");
            user.Connect();
            
            //display info 
            Thread.Sleep(1000);
            Console.Clear();
            
            Console.WriteLine("Press \"I\" to enter insert mode.");
            Console.WriteLine("Type \"quit\" or press Escape to close the application.");

            try
            {
                string outgoingMessage;
                string incomingMessage;

                user.OpenStreams();

                while (user.status)
                {
                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key. Intercept:true prevents them typing the entered letter to console          
                        ConsoleKeyInfo userKey = Console.ReadKey(true); //Blocking statement
                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write(">>");
                            outgoingMessage = Console.ReadLine();

                            //client.sendMessage returns false if they enter a string to quit the program
                            if (!user.SendMessage(outgoingMessage))
                            {
                                Console.WriteLine("You disconnected the chat. Bye!");
                            }
                        }
                        else if (userKey.Key == ConsoleKey.Escape)
                        {
                            //send the server a message to say the client has left
                            Console.WriteLine("You disconnected the chat. Bye!");
                            user.SendMessage("quit");
                            user.DisconnectChat();

                            return;
                        }
                    }

                    //listening mode
                    if (user.networkStream.DataAvailable)
                    {
                        incomingMessage = user.ListenForMessage();

                        //check if they've entered any variation of the word "quit", and if not print it
                        if (!string.Equals(incomingMessage, "quit", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Server: " + incomingMessage);
                        }
                        else
                        {
                            Console.WriteLine(otherUser + " has disconnected the chat.");
                            Console.WriteLine("Exiting...");
                            user.DisconnectChat();
                        }
                    }
                } //end while(client.clientStatus)
            }
            catch
            {
                Console.WriteLine("Problem reading from " + otherUser);
            }
        } //end RunChat
    }//end main class
}//end namespace