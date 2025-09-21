# Chat Service

**Client-server chat application for text communication over the network.**

---

## Project Modules

- **Client/** – connects to the server, sends and receives messages  
- **Server/** – manages multiple clients and message routing  
- **Console/** – monitoring and management console for server or client  
- **Packets/** – shared data structures and packet types for communication  
- **Server Scripts/** – additional scripts supporting server operations  

---

## Features

- Supports multiple simultaneous clients  
- Real-time text messaging  
- Custom packet types for communication  
- Connection management via sockets  

---

## Project Structure

Each module has its own `.csproj` and dedicated classes for communication and application logic.

---

## Setup & Run

1. Build the project using .NET (`dotnet build`)  
2. Start the server (`Server/Main.cs`)  
3. Start the client (`Client/Main.cs`) and connect to the server  

---

## Requirements

- .NET Core / .NET Framework  
- OS: Linux / Windows  

---

## Author

Project created for learning network communication in C#.
