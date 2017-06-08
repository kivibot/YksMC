namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class PlayerPositionLookPacketFlags
    {
        private const int _xBit = 0x1;
        private const int _yBit = 0x2;
        private const int _zBit = 0x4;
        private const int _yRotBit = 0x8;
        private const int _xRotBit = 0x10;

        public byte Data { private get; set; }

        public bool RelativeX => (Data & _xBit) != 0;
        public bool RelativeY => (Data & _yBit) != 0;
        public bool RelativeZ => (Data & _zBit) != 0;
        public bool RelativePitch => (Data & _yRotBit) != 0;
        public bool RelativeYaw => (Data & _xRotBit) != 0;
    }
}