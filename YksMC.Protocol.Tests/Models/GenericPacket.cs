using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Tests.Models
{
    public class GenericPacket<T>
    {
        public T Value { get; set; }
    }
}
