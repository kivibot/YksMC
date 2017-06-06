using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Bot.Models
{
    public class PlaceholderBlockType : IBlockType
    {
        public int Id { get; }

        public int Data { get; }

        public string Name => throw new NotImplementedException();

        public double Hardness => throw new NotImplementedException();

        public int StackSize => throw new NotImplementedException();

        public bool Diggable => throw new NotImplementedException();

        public BoundingBoxType BoundingBox => throw new NotImplementedException();

        public bool Transparent => throw new NotImplementedException();

        public int EmitLight => throw new NotImplementedException();

        public int FilterLight => throw new NotImplementedException();

        public PlaceholderBlockType(int id, int data)
        {
            Id = id;
            Data = data;
        }
    }
}
