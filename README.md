# DOSP Project - Team 27

## COP5615 - Distributed Operating System Principles  
### Fall 2023  
### Programming Assignment #1  

---

## Project Overview  
This project implements a simple client-server system using **F#**. The server listens for incoming client connections, processes arithmetic operations, and manages communication asynchronously. Clients connect to the server, send commands, and receive responses.

---

## Environment Setup

### Requirements
- **Editor**: Visual Studio Code (VS Code)
- **Programming Language**: F#
- **Supported OS**: Windows, macOS
- **Dependencies**:
  - Install [Ionide for F# Plugin](https://marketplace.visualstudio.com/items?itemName=Ionide.Ionide-fsharp) in VS Code
  - Install [.NET SDK](https://dotnet.microsoft.com/download)

---

## Compilation & Execution

### Steps:
1. Download and extract the `PA1_Team27.zip` file.
2. Open the `server` and `client` folders in VS Code.
3. Update the **IP address** in `client` code (Line 8) to match the system running the server.
4. Open multiple terminals:
   - Navigate to the **server** folder in one terminal.
   - Navigate to the **client** folder in another terminal.

### Commands:

#### **Compilation**
Run the following command in both the **server** and **client** terminals:  
```sh
dotnet build
```
If successful, it displays "Build Success".  

## Execution
Run the following command in the server terminal first:  

```sh
dotnet run
```
Then, run the same command in the client terminal(s):  

```sh
dotnet run
```
This initiates an interactive session between the server and clients.

## Code Structure
# Server:  
1. handleClient Function:

Manages client communication asynchronously.  
Handles commands for arithmetic operations (add, subtract, multiply, divide).  
Implements error handling for invalid inputs.  
Manages client disconnection (bye) and server termination (terminate).  

2. Main Function:

Starts a TcpListener for client connections.  
Uses acceptClients to manage incoming connections asynchronously.  

# Client
Connects to the server and sends commands.  
Receives and handles server responses.  
Implements error handling for invalid inputs and network issues.  
Terminates when receiving an exit command (bye or terminate).  

## Execution Results
1. Establishing Connection:
Server starts listening for clients.  
Clients receive a "Hello!" message upon connection.  

2. Arithmetic Operations:  
Commands like add 5 3 return 8, while invalid operations return error messages.  

3. Error Handling:
Invalid commands: "qw" → "Incorrect operation command"  
Too many arguments: "add 1 2 3 4 5" → "Number of inputs is more than four"  
Non-numeric input: "add a b" → "One or more inputs contain(s) non-number(s)"  

4. Abnormal Cases:  
Trailing spaces in input → Recognized as an additional input and handled as an error.  

## References
F# Documentation  
.NET SDK  
