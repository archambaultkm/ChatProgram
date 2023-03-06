using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
{
    public abstract class ChatBase
    {
        public string localIP { get; set; }
        public Int32 port { get; set; }
        public bool status = true;
        public NetworkStream networkStream { get; set; }
        public StreamReader streamReader { get; set; }
        public StreamWriter streamWriter { get; set; }

        public abstract void StartChat();
        public abstract void Connect();
        public abstract void OpenStreams();
        public bool SendMessage(string outgoingMessage)
        {
            if (string.Equals(outgoingMessage, "quit", StringComparison.OrdinalIgnoreCase))
            {
                streamWriter.WriteLine("quit"); //this is one way to make sure "quit"/escape have the same effect, I can change this
                                
                streamWriter.Flush();
                status = false;

                return false;
            }
                            
            streamWriter.WriteLine(outgoingMessage);
            streamWriter.Flush();

            return true;
        }

        public string ListenForMessage()
        {
            string incomingMessage;
            //this is from the reader, so the message the client sent to the stream
            incomingMessage = streamReader.ReadLine();
            
            return incomingMessage;
        }
        
        public virtual void DisconnectChat()
        {
            status = false;
            
            networkStream.Close();
            streamWriter.Close();
            streamReader.Close();
            Environment.Exit(1);
        }
    }
}