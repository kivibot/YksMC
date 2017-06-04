using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class Vector<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }
    }
}
