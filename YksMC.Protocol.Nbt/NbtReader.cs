using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt
{
    public class NbtReader : INbtReader
    {
        private delegate BaseTag TagGetter(INbtDataReader reader);

        private readonly TagGetter[] _tagGetters;

        public NbtReader()
        {
            _tagGetters = new TagGetter[12];
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

        public T GetTag<T>(INbtDataReader reader) where T : BaseTag
        {
            byte tagType = reader.GetByte();
            if (tagType == (int)TagType.End)
                return null;

            TagGetter getter = GetTagGetter(tagType);

            string name = GetString(reader);

            T tag = (T)getter(reader);
            tag.Name = name;

            return tag;
        }

        private TagGetter GetTagGetter(byte tagType)
        {
            if (tagType > _tagGetters.Length)
                throw new ArgumentException($"Invalid tag type: {tagType}");
            return _tagGetters[tagType];
        }

        private ByteArrayTag GetByteArrayTag(INbtDataReader reader)
        {
            int length = reader.GetInt();
            byte[] value = reader.GetBytes(length);
            return new ByteArrayTag(value);
        }

        private BaseTag GetEndTag(INbtDataReader reader)
        {
            return null;
        }

        private ByteTag GetByteTag(INbtDataReader reader)
        {
            byte value = reader.GetByte();
            return new ByteTag(value);
        }

        private CompoundTag GetCompoundTag(INbtDataReader reader)
        {
            List<BaseTag> tags = new List<BaseTag>();
            while (true)
            {
                BaseTag tag = GetTag<BaseTag>(reader);
                if (tag == null)
                    break;
                tags.Add(tag);
            }
            return new CompoundTag(tags);
        }

        private DoubleTag GetDoubleTag(INbtDataReader reader)
        {
            double value = reader.GetDouble();
            return new DoubleTag(value);
        }

        private FloatTag GetFloatTag(INbtDataReader reader)
        {
            float value = reader.GetFloat();
            return new FloatTag(value);
        }

        private IntTag GetIntTag(INbtDataReader reader)
        {
            int value = reader.GetInt();
            return new IntTag(value);
        }

        private LongTag GetLongTag(INbtDataReader reader)
        {
            long value = reader.GetLong();
            return new LongTag(value);
        }

        private ShortTag GetShortTag(INbtDataReader reader)
        {
            short value = reader.GetShort();
            return new ShortTag(value);
        }

        private StringTag GetStringTag(INbtDataReader reader)
        {
            string value = GetString(reader);
            return new StringTag(value);
        }

        private IntArrayTag GetIntArrayTag(INbtDataReader reader)
        {
            int length = reader.GetInt();
            int[] values = new int[length];
            for (int i = 0; i < length; i++)
                values[i] = reader.GetInt();
            return new IntArrayTag(values);
        }

        private ListTag GetListTag(INbtDataReader reader)
        {
            byte tagType = reader.GetByte();
            TagGetter getter = GetTagGetter(tagType);
            int length = reader.GetInt();
            List<BaseTag> tags = new List<BaseTag>();
            for (int i = 0; i < length; i++)
                tags.Add(getter(reader));
            return new ListTag(tags);
        }

        private string GetString(INbtDataReader reader)
        {
            short length = reader.GetShort();
            byte[] data = reader.GetBytes(length);
            return Encoding.UTF8.GetString(data);
        }
    }
}
