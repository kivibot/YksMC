using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public sealed class Chat
    {
        public string Value { get; set; }

        public Chat(string value)
        {
            Value = value;
        }
        public override bool Equals(object obj)
        {
            Chat other = obj as Chat;
            if (other == null)
                return false;
            if (Value != other.Value)
                return false;
            return true;
        }
    }
}
