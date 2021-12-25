namespace Packets
{
    public class Packet
    {
        private const int BYTE_SIZE = 2048;
        private String content;
        private byte[] data;

        public Packet()
        {
            content = String.Empty;
            data = new byte[BYTE_SIZE];
        }
    }
}