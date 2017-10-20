using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.ItemStack
{
    [DebuggerDisplay("{Name}, {Size}, {Durability}")]
    public class ItemStack : IItemStack
    {
        protected readonly string _name;
        protected readonly int _size;
        protected readonly int _maxSize;
        protected readonly int _durability;
        protected readonly int _maxDurability;

        public string Name => _name;
        public int Size => _size;
        public int MaxSize => _maxSize;
        public int Durability => _durability;
        public int MaxDurability => _maxDurability;

        public ItemStack(string name, int maxSize, int maxDurability)
            : this(name, 0, maxSize, maxDurability, maxDurability)
        {

        }

        protected ItemStack(string name, int size, int maxSize, int durability, int maxDurability)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }
            if (maxSize < 0)
            {
                throw new ArgumentException(nameof(maxSize));
            }
            if (size < 0 || size > maxSize)
            {
                throw new ArgumentException(nameof(size));
            }
            if(maxDurability < 0)
            {
                throw new ArgumentException(nameof(maxDurability));
            }
            if (durability < 0 || durability > maxDurability)
            {
                throw new ArgumentException(nameof(durability));
            }

            _name = name;
            _size = size;
            _maxSize = maxSize;
            _durability = durability;
            _maxDurability = maxDurability;
        }

        public IItemStack ChangeDurability(int damage)
        {
            int durability = _maxDurability - damage;
            return CreateItemStack(_name, _size, _maxSize, durability, _maxDurability);
        }

        public IItemStack ChangeSize(int count)
        {
            return CreateItemStack(_name, count, _maxSize, _durability, _maxDurability);
        }

        protected virtual IItemStack CreateItemStack(string name, int size, int maxSize, int durability, int maxDurability)
        {
            return new ItemStack(name, size, maxSize, durability, maxDurability);
        }
    }
}
