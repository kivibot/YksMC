﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Tests.Fakes
{
    public class FakePacketReader : IPacketReader
    {
        private readonly Queue<object> _queue;

        public FakePacketReader(params object[] objects)
        {
            _queue = new Queue<object>(objects);
        }

        public Angle GetAngle()
        {
            return Get<Angle>();
        }

        public bool GetBool()
        {
            return Get<bool>();
        }

        public byte GetByte()
        {
            return Get<byte>();
        }

        public byte[] GetBytes(int count)
        {
            return Get<byte[]>();
        }

        public Chat GetChat()
        {
            return Get<Chat>();
        }

        public double GetDouble()
        {
            return Get<double>();
        }

        public float GetFloat()
        {
            return Get<float>();
        }

        public Guid GetGuid()
        {
            return Get<Guid>();
        }

        public int GetInt()
        {
            return Get<int>();
        }

        public long GetLong()
        {
            return Get<long>();
        }

        public Position GetPosition()
        {
            return Get<Position>();
        }

        public int GetRemainingBytes()
        {
            throw new NotImplementedException();
        }

        public short GetShort()
        {
            return Get<short>();
        }

        public sbyte GetSignedByte()
        {
            return Get<sbyte>();
        }

        public string GetString()
        {
            return Get<string>();
        }

        public uint GetUnsignedInt()
        {
            return Get<uint>();
        }

        public ulong GetUnsignedLong()
        {
            return Get<ulong>();
        }

        public ushort GetUnsignedShort()
        {
            return Get<ushort>();
        }

        public VarInt GetVarInt()
        {
            return Get<VarInt>();
        }

        public VarLong GetVarLong()
        {
            return Get<VarLong>();
        }

        public void ResetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetPacket(byte[] packet)
        {
            throw new NotImplementedException();
        }

        private T Get<T>()
        {
            return (T)_queue.Dequeue();
        }
    }
}
