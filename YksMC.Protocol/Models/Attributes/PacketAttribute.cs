using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketAttribute : Attribute
    {
        public int Id { get; set; }
        public ConnectionState ConnectionState { get; set; }
        public BoundTo BoundTo { get; set; }

        public PacketAttribute(int id, ConnectionState connectionState, BoundTo boundTo)
        {
            Id = id;
            ConnectionState = connectionState;
            BoundTo = boundTo;
        }
    }
}
