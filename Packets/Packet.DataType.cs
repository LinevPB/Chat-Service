namespace App.Packets
{
    public partial class Packet
    {
        public enum DataType
        {
            String = 's',
            Int = 'd',
            Float = 'f'
        }

        private static bool ValidateType(char ctype)
        {
            if (ctype == (char)DataType.Int ||
                        ctype == (char)DataType.Float ||
                        ctype == (char)DataType.String)
                return true;
            return false;
        }
    }
}