using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace App.Clients
{
    public class ClientStructure
    {
        private int id;
        private string name;
        private Socket socket;
        public ManualResetEvent clientEvent;
        public ClientStateObject state;

        public ClientStructure(int id, string name, Socket socketHandler)
        {
            this.id = id;
            this.name = name;
            socket = socketHandler;
            clientEvent = new ManualResetEvent(false);
            state = new ClientStateObject();
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } set { name = value; } }
        public Socket Socket { get { return socket; } }
    }
}
