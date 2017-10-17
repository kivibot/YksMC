using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Client;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks
{
    public class RespawnTask : BehaviorTask
    {
        private const int _timeout = 5 * 20;

        private readonly IMinecraftClient _client;

        private int _ticksWaited = 0;

        public RespawnTask(IMinecraftClient client) 
            : base("Respawn")
        {
            _client = client;
        }

        public override IWorld OnStart(IWorld world)
        {
            _client.SendPacket(new ClientStatusPacket() {
                ActionId = ClientStatusPacket.Respawn
            });
            return world;
        }

        public override void OnTick(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            if (entity.IsAlive)
            {
                Complete();
            }
            if(_ticksWaited > _timeout)
            {
                Fail();
            }
            _ticksWaited++;
        }
    }
}
