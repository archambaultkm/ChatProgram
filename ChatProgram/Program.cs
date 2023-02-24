using System;
using System.Threading;
using ChatLib;

namespace ChatProgram
{
    public class Program
    {
        static void Main(string args)
        {
            if (args.Contains("-server")) // if args[0] = "-server"
            {
                Console.WriteLine("Server");
                Server server = new Server(); //create a new object
            }
            else
            {
                Console.WriteLine("Client");
                Client client = new Client(); //create a new object
            }

            Console.ReadLine(); //Blocking statement

            //Run as Client vs Server
            while (true)
            {
                Console.WriteLine("Listening for messages");
                if (!Console.KeyAvailable) continue;
                
                //User input mode: when user press "I" key.            
                ConsoleKeyInfo userKey = Console.ReadKey(); //Blocking statement
                if (userKey.Key == ConsoleKey.I)
                {
                    Console.Write("'I' is PRESSED >>");
                    Console.ReadLine();
                    //break;
                }
                else
                {
                    Console.WriteLine($"You typed {userKey.Key}");
                    Thread.Sleep(500);
                }
            }
        }
    }
}


