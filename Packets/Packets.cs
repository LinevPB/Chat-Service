using System.Text;

namespace App.Packets
{
    public class Packet
    {
        public enum DataType
        {
            String = 's',
            Int = 'd',
            Float = 'f'
        }

        private const int BYTE_SIZE = 2048;
        public string strData;
        private byte[] data;

        public Packet()
        {
            strData = String.Empty;
            data = new byte[BYTE_SIZE];
        }

        public byte[] Parse()
        {
            data = Encoding.ASCII.GetBytes(strData);
            return data;
        }

        public byte[] GetData()
        {
            return data;
        }


        private string ConstructArg(DataType type, string value)
        {
            // eg. 4d2137
            return String.Format("{0}{1}{2}", value.Length, (char)type, value);
        }

        public void WriteInt(int value)
        {
            strData += ConstructArg(DataType.Int, value.ToString());
        }

        public void WriteFloat(float value)
        {
            strData += ConstructArg(DataType.Float, value.ToString());
        }

        public void WriteString(string value)
        {
            strData += ConstructArg((DataType)DataType.String, value);
        }
    }
}