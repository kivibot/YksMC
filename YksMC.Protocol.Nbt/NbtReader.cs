using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt
{
    public class NbtReader : INbtReader
    {
        private readonly IPacketReader _reader;

        public NbtReader(IPacketReader reader)
        {
            _reader = reader;
        }

        public ByteArrayTag GetByteArrayTag()
        {
            string name = GetString();
            int length = _reader.GetInt();
            byte[] value = _reader.GetBytes(length);
            return new ByteArrayTag(name, value);
        }

        public ByteTag GetByteTag()
        {
            string name = GetString();
            byte value = _reader.GetByte();
            return new ByteTag(name, value);
        }

        public CompoundTag GetCompoundTag()
        {
            string name = GetString();
            List<BaseTag> tags = new List<BaseTag>();
            while(true)
            {
                BaseTag tag = GetTag();
                if (tag is EndTag)
                    break;
                tags.Add(tag);
            }
            return new CompoundTag(name, tags);
        }

        public DoubleTag GetDoubleTag()
        {
            string name = GetString();
            double value = _reader.GetDouble();
            return new DoubleTag(name, value);
        }

        public FloatTag GetFloatTag()
        {
            string name = GetString();
            float value = _reader.GetFloat();
            return new FloatTag(name, value);
        }

        public IntTag GetIntTag()
        {
            string name = GetString();
            int value = _reader.GetInt();
            return new IntTag(name, value);
        }

        public LongTag GetLongTag()
        {
            string name = GetString();
            long value = _reader.GetLong();
            return new LongTag(name, value);
        }

        public ShortTag GetShortTag()
        {
            string name = GetString();
            short value = _reader.GetShort();
            return new ShortTag(name, value);
        }

        public StringTag GetStringTag()
        {
            string name = GetString();
            string value = GetString();
            return new StringTag(name, value);
        }

        private string GetString()
        {
            short length = _reader.GetShort();
            byte[] data = _reader.GetBytes(length);
            return Encoding.UTF8.GetString(data);
        }

        private BaseTag GetTag()
        {
            return new EndTag();
        }
    }
}
