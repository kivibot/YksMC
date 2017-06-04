using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt
{
    public interface INbtDataReader
    {        
        byte GetByte();
        short GetShort();
        int GetInt();
        long GetLong();
        float GetFloat();
        double GetDouble();

        byte[] GetBytes(int count);
    }
}
