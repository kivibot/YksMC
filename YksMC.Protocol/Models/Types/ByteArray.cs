using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class ByteArray
    {
        public VarInt Length { get; set; }
        public byte[] Data { get; set; }

        public ByteArray() { }

        public ByteArray(byte[] data)
        {
            Data = data;
            Length = new VarInt(data.Length);
        }

        public override bool Equals(object obj)
        {
            ByteArray other = obj as ByteArray;
            if (other == null)
                return false;
            if (!object.Equals(Length, other.Length))
                return false;
            if (!Enumerable.SequenceEqual(Data, other.Data))
                return false;
            return true;
        }
    }
}
