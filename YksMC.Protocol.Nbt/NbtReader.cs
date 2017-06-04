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
            _tagGetters[(int)TagType.List] = GetListTag;
            _tagGetters[(int)TagType.Compound] = GetCompoundTag;
            _tagGetters[(int)TagType.IntArray] = GetIntArrayTag;
        }

        public T GetTag<T>() where T : BaseTag
        {
            byte tagType = _reader.GetByte();
            if (tagType == (int)TagType.End)
                return null;

            Func<BaseTag> getter = GetTagGetter(tagType);

            string name = GetString();

            T tag = (T)getter();
            tag.Name = name;

            return tag;
        }

        private Func<BaseTag> GetTagGetter(byte tagType)
        {
            if (tagType > _tagGetters.Length)
                throw new ArgumentException($"Invalid tag type: {tagType}");
            return _tagGetters[tagType];
        }

        private ByteArrayTag GetByteArrayTag()
        {
            int length = _reader.GetInt();
            byte[] value = _reader.GetBytes(length);
            return new ByteArrayTag(value);
        }

        private BaseTag GetEndTag()
        {
            return null;
        }

        private ByteTag GetByteTag()
        {
            byte value = _reader.GetByte();
            return new ByteTag(value);
        }

        private CompoundTag GetCompoundTag()
        {
            List<BaseTag> tags = new List<BaseTag>();
            while (true)
            {
                BaseTag tag = GetTag<BaseTag>();
                if (tag == null)
                    break;
                tags.Add(tag);
            }
            return new CompoundTag(tags);
        }

        private DoubleTag GetDoubleTag()
        {
            double value = _reader.GetDouble();
            return new DoubleTag(value);
        }

        private FloatTag GetFloatTag()
        {
            float value = _reader.GetFloat();
            return new FloatTag(value);
        }

        private IntTag GetIntTag()
        {
            int value = _reader.GetInt();
            return new IntTag(value);
        }

        private LongTag GetLongTag()
        {
            long value = _reader.GetLong();
            return new LongTag(value);
        }

        private ShortTag GetShortTag()
        {
            short value = _reader.GetShort();
            return new ShortTag(value);
        }

        private StringTag GetStringTag()
        {
            string value = GetString();
            return new StringTag(value);
        }

        private IntArrayTag GetIntArrayTag()
        {
            int length = _reader.GetInt();
            int[] values = new int[length];
            for (int i = 0; i < length; i++)
                values[i] = _reader.GetInt();
            return new IntArrayTag(values);
        }

        private ListTag GetListTag()
        {
            byte tagType = _reader.GetByte();
            Func<BaseTag> getter = GetTagGetter(tagType);
            int length = _reader.GetInt();
            List<BaseTag> tags = new List<BaseTag>();
            for (int i = 0; i < length; i++)
                tags.Add(getter());
            return new ListTag(tags);
        }

        private string GetString()
        {
            short length = _reader.GetShort();
            byte[] data = _reader.GetBytes(length);
            return Encoding.UTF8.GetString(data);
        }
    }
}
