# ChatProgram

# PROG2200 – Advanced OOP

# Assignment 1 - Synchronous Chat Program

Assignment Value: 15% of overall course mark

Assignment Purpose:

To build a distributed application. This will meet learning outcome # 3 from the course workplan.

_LO3: Examine the structure of a distributed application by exploring a variety of distributed application models._

Submission Date:

**Feb. 24**** th **** @ 11:59 pm**. But it is still open to Feb 28th.

Submissions:

Submit the solution folder (Visual Studio Project) of this assignment **only** to the Brightspace Dropbox. Send the recording demonstration via MS-Teams private chat.

**Instructions:**

Watch the posted walkthrough video on Teams.

For this assignment, you are tasked to create a simple, console‐based, chat program that will allow the user to communicate, over TCP/IP, to another instance of the chat program.

The program will consist of two parts, the server and the client. When the application is run with the "‐server" parameter, the program will run in server mode. When the application is run **without** any parameters, the program will run in client mode.

![](RackMultipart20230302-1-fvulw3_html_7510b03d3c471269.png)

![](RackMultipart20230302-1-fvulw3_html_23d04b5bb00f9557.png)

![](RackMultipart20230302-1-fvulw3_html_156b75c2d68fcc23.png)

When the chat program is run in server mode, the application will wait for connection from an instance of the application running in client mode.

![](RackMultipart20230302-1-fvulw3_html_7f2300f81304908d.png)

When the chat program is run in client mode, the application will try and connect to the server instance.

![](RackMultipart20230302-1-fvulw3_html_f785f88a001698b0.png)

**Note:** _For the purposes of this assignment, you can assume both the server and client are running on the same machine and that only one client can connect to the server at a time._

Once the client has connected, you can begin sending messages from either the server instance or the client instance. See the following:

![](RackMultipart20230302-1-fvulw3_html_8814baa6a9ddba86.png)

To begin sending messages, you will press the **I** key on the keyboard which will put the chat program into "Input Mode" and display " **\>\>**" as a prompt. Once in "Input Mode", the user can type a message and it will be sent when the user finishes typing and presses the enter key.

![](RackMultipart20230302-1-fvulw3_html_cba2ef50738973b3.png)

When either server or client is **not** in "Insert mode", the program will be constantly checking for input from the other program. (e.g. the client checks for message from the server and vice versa)

![](RackMultipart20230302-1-fvulw3_html_2865a3b653b0c16d.png)

The following representing the recived message inside the server object(node):

![](RackMultipart20230302-1-fvulw3_html_e95edbc76f4bb15e.png)

The program will exit when the word "quit" is entered as a message or you can use the "Escape" Key as following in the client object (App)(Node):

.

![](RackMultipart20230302-1-fvulw3_html_26c48599901ee66d.png)

The following is representing the server after the Client being disconnected (Or closing the Client App)

![](RackMultipart20230302-1-fvulw3_html_e2bd4e4b22e24993.png)

**Sample Chat Session:**

![](RackMultipart20230302-1-fvulw3_html_8039c82a5bb54e7c.png)

![](RackMultipart20230302-1-fvulw3_html_a393c1ccf8329e37.png)

**After the Client disconnected:**

![](RackMultipart20230302-1-fvulw3_html_d7d2495daa356530.png)

Assignment Rubric is posted to BS in another PDF file.

**All the Best!**

2
