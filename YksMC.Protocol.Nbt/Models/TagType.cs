using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public enum TagType
    {
        End = 0x00,
        Byte = 0x01,
        Short = 0x02,
        Int = 0x03,
        Long = 0x04,
        Float = 0x05,
        Double = 0x06,
        ByteArray = 0x07,
        String = 0x08,
        List = 0x09,
        Compound = 0x0A,
        IntArray = 0x0B
    }
}
