using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Exceptions
{
    public class MinecraftConnectionException : Exception
    {
        public MinecraftConnectionException(string message) : base(message)
        {
        }

        public MinecraftConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
