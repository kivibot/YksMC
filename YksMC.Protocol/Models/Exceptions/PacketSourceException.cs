using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Exceptions
{
    public class PacketSourceException : Exception
    {
        public PacketSourceException(string message) : base(message)
        {
        }

        public PacketSourceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
