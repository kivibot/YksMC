﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Dtos;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol
{
    public class MCStatusClient : IMCStatusClient
    {
        private readonly IMCPacketReader _packetReader;
        private readonly IMCPacketWriter _packetWriter;
        private readonly IMCPacketDeserializer _deserializer;
        private readonly IMCPacketSerializer _serializer;

        public MCStatusClient(IMCPacketReader packetReader, IMCPacketWriter packetWriter)
        {
            _packetReader = packetReader;
            _packetWriter = packetWriter;
            _deserializer = new MCPacketDeserializer();
            _serializer = new MCPacketSerializer();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<TimeSpan> GetPingAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            await SendPingAsync();
            return await ReceivePongAsync();
        }

        private async Task SendPingAsync()
        {
            PingPacket pingPacket = new PingPacket() { Payload = DateTimeOffset.Now.Ticks };
            _packetWriter.PutVarInt(new VarInt(0x01));
            _serializer.Serialize(pingPacket, _packetWriter);
            await _packetWriter.SendPacketAsync();
        }

        private async Task<TimeSpan> ReceivePongAsync()
        {
            await _packetReader.NextAsync();
            VarInt id2 = _packetReader.GetVarInt();
            PongPacket packet = _deserializer.Deserialize<PongPacket>(_packetReader);

            return TimeSpan.FromTicks(DateTimeOffset.Now.Ticks - packet.Payload);
        }

        public async Task<StatusDto> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            await SendHandshakeAsync();
            await SendStatusRequestAsync();
            return await ReceiveStatusAsync();
        }

        private async Task SendHandshakeAsync()
        {
            HandshakePacket handshakePacket = new HandshakePacket()
            {
                NextState = new VarInt(0x01),
                ProtocolVersion = new VarInt(316),
                ServerAddress = "localhost",
                ServerPort = 25565
            };
            _packetWriter.PutVarInt(new VarInt(0));
            _serializer.Serialize(handshakePacket, _packetWriter);
            await _packetWriter.SendPacketAsync();
        }

        private async Task SendStatusRequestAsync()
        {
            _packetWriter.PutVarInt(new VarInt(0));
            await _packetWriter.SendPacketAsync();
        }

        private async Task<StatusDto> ReceiveStatusAsync()
        {
            await _packetReader.NextAsync();
            VarInt id = _packetReader.GetVarInt();
            if (id.Value != 0)
                throw new Exception("Unexpected packet id: " + id);
            StatusResponsePacket packet = _deserializer.Deserialize<StatusResponsePacket>(_packetReader);

            return JsonConvert.DeserializeObject<StatusDto>(packet.JsonData);
        }
    }
}
