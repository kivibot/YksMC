using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks
{
    public class RespawnTask : BehaviorTask<RespawnCommand>
    {
        private const int _timeout = 5 * 20;

        private int _ticksWaited = 0;

        public override string Name => "Respawn";

        public RespawnTask(RespawnCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }
        
        public override bool IsPossible(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if(player == null)
            {
                return false;
            }
            if (!player.HasEntity)
            {
                return false;
            }
            IEntity entity = world.GetPlayerEntity();
            if (entity.IsAlive)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            ClientStatusPacket reply = new ClientStatusPacket()
            {
                ActionId = ClientStatusPacket.Respawn
            };
            _minecraftClient.SendPacket(reply);
            return Result(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            IEntity entity = world.GetPlayerEntity();
            if (entity.IsAlive)
            {
                return Success(world);
            }
            if (_ticksWaited > _timeout)
            {
                return Failure(world);
            }
            _ticksWaited++;
            return Result(world);
        }
    }
}
