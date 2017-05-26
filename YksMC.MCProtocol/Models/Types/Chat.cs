using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class Chat
    {
        public string Value { get; set; }

        public Chat(string value)
        {
            Value = value;
        }
    }
}
