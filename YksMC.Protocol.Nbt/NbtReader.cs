using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt
{
    public class NbtReader : INbtReader
    {
        private readonly IPacketReader _reader;
        private readonly Func<BaseTag>[] _tagGetters;

        public NbtReader(IPacketReader reader)
        {
            _reader = reader;

            _tagGetters = new Func<BaseTag>[12];
            _tagGetters[(int)TagType.End] = GetEndTag;
            _tagGetters[(int)TagType.Byte] = GetByteTag;
            _tagGetters[(int)TagType.Short] = GetShortTag;
            _tagGetters[(int)TagType.Int] = GetIntTag;
            _tagGetters[(int)TagType.Long] = GetLongTag;
            _tagGetters[(int)TagType.Float] = GetFloatTag;
            _tagGetters[(int)TagType.Double] = GetDoubleTag;
            _tagGetters[(int)TagType.ByteArray] = GetByteArrayTag;
            _tagGetters[(int)TagType.String] = GetStringTag;
            _tagGetters[(int)TagType.List] = null;
            _tagGetters[(int)TagType.Compound] = GetCompoundTag;
            _tagGetters[(int)TagType.IntArray] = null;
        }

        public T GetTag<T>() where T : BaseTag
        {
            byte tagType = _reader.GetByte();
            if (tagType > _tagGetters.Length)
                throw new ArgumentException($"Invalid tag type: {tagType}");
            return (T)_tagGetters[tagType]();
        }

        private ByteArrayTag GetByteArrayTag()
        {
            string name = GetString();
            int length = _reader.GetInt();
            byte[] value = _reader.GetBytes(length);
            return new ByteArrayTag(name, value);
        }

        private BaseTag GetEndTag()
        {
            return null;
        }

        private ByteTag GetByteTag()
        {
            string name = GetString();
            byte value = _reader.GetByte();
            return new ByteTag(name, value);
        }

        private CompoundTag GetCompoundTag()
        {
            string name = GetString();
            List<BaseTag> tags = new List<BaseTag>();
            while(true)
            {
                BaseTag tag = GetTag<BaseTag>();
                if (tag == null)
                    break;
                tags.Add(tag);
            }
            return new CompoundTag(name, tags);
        }

        private DoubleTag GetDoubleTag()
        {
            string name = GetString();
            double value = _reader.GetDouble();
            return new DoubleTag(name, value);
        }

        private FloatTag GetFloatTag()
        {
            string name = GetString();
            float value = _reader.GetFloat();
            return new FloatTag(name, value);
        }

        private IntTag GetIntTag()
        {
            string name = GetString();
            int value = _reader.GetInt();
            return new IntTag(name, value);
        }

        private LongTag GetLongTag()
        {
            string name = GetString();
            long value = _reader.GetLong();
            return new LongTag(name, value);
        }

        private ShortTag GetShortTag()
        {
            string name = GetString();
            short value = _reader.GetShort();
            return new ShortTag(name, value);
        }

        private StringTag GetStringTag()
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
    }
}
