# ChatProgram
Assignment 1 - Synchronous Chat Program

Assignment Value: 15% of overall course mark
Assignment Purpose: 
To build a distributed application. This will meet learning outcome # 3 from the course workplan.
LO3: Examine the structure of a distributed application by exploring a variety of distributed application models.
Submission Date: 
Feb. 24th @ 11:59 pm. But it is still open to Feb 28th. 
Submissions: 
Submit the solution folder (Visual Studio Project) of this assignment only to the Brightspace Dropbox. Send the recording demonstration via MS-Teams private chat. 
Instructions: 
Watch the posted walkthrough video on Teams. 
For this assignment, you are tasked to create a simple, console‐based, chat program that will allow the user to communicate, over TCP/IP, to another instance of the chat program.
The program will consist of two parts, the server and the client. When the application is run with the "‐server" parameter, the program will run in server mode. When the application is run without any parameters, the program will run in client mode.
 
 
 

When the chat program is run in server mode, the application will wait for connection from an instance of the application running in client mode.
 

When the chat program is run in client mode, the application will try and connect to the server instance.
 

Note: For the purposes of this assignment, you can assume both the server and client are running on the same machine and that only one client can connect to the server at a time.
Once the client has connected, you can begin sending messages from either the server instance or the client instance. See the following:

 
To begin sending messages, you will press the I key on the keyboard which will put the chat program into "Input Mode" and display ">>" as a prompt. Once in "Input Mode", the user can type a message and it will be sent when the user finishes typing and presses the enter key.
 

When either server or client is not in "Insert mode", the program will be constantly checking for input from the other program. (e.g. the client checks for message from the server and vice versa)
 
The following representing the recived message inside the server object(node):
 
The program will exit when the word "quit" is entered as a message or you can use the “Escape” Key as following in the client object (App)(Node): 
.
 
The following is representing the server after the Client being disconnected (Or closing the Client App)

 
 
Sample Chat Session: 
 
 
After the Client disconnected:
 

![image](https://user-images.githubusercontent.com/97715354/222510163-f123de6d-2ebf-472a-b236-48a96c7d47c4.png)
