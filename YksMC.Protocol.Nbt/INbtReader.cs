using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt
{
    public interface INbtReader
    {
        ByteTag GetByteTag();
        ShortTag GetShortTag();
        IntTag GetIntTag();
        LongTag GetLongTag();
        FloatTag GetFloatTag();
        DoubleTag GetDoubleTag();
        ByteArrayTag GetByteArrayTag();
        StringTag GetStringTag();

        CompoundTag GetCompoundTag();
    }
}
