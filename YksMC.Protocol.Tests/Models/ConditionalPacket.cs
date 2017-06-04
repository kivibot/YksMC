using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Tests.Models
{
    public class ConditionalPacket
    {
        public int Action { get; set; }
        public string Filler { get; set; }
        [Conditional(nameof(Action), Condition.Is, 2, 3)]
        public int IntValue { get; set; }
        [Conditional(nameof(Action), Condition.IsNot, 2, 3)]
        public string StringValue { get; set; }
    }
}
