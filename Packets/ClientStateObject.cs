using App.Packets;
using System.Text;

namespace App.Clients
{
    public class ClientStateObject
    {
        public int BufferSize = Packet.GetSize();
        public byte[] Buffer;
        public StringBuilder StringBuilder;

        public ClientStateObject()
        {
            Buffer = new byte[BufferSize];
            StringBuilder = new StringBuilder();
        }
    }
}
