using Snow.Events;
using Snow.Formats;
using Snow.Items;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Servers;
using Snow.Worlds;
using System;
namespace Snow.Entities
{
    public class Entity
    {
        private UUID uuid;
        public void SetUUID(UUID uuid)
        {
            this.uuid = uuid;
        }
        public UUID GetUUID()
        {
            return uuid;
        }

        private int id;
        public void SetId(int id)
        {
            this.id = id;
        }
        public int GetId()
        {
            return id;
        }

        private double x = 0;
        private double y = 0;
        private double z = 0;

        private int pitch;
        private int yaw;

        public int GetPitch()
        {
            return pitch;
        }

        public int GetYaw()
        {
            return yaw;
        }

        public World GetWorld()
        {
            return world;
        }

        public Vector3 GetPosistion()
        {
            return new Vector3(x, y, z);
        }

        World world;

        internal void SetWorld(World world)
        {
            this.world = world;
        }

        public Vector3 GetLocation()
        {
            return new Vector3(x, y, z);
        }

        Server server;

        public void SetServer(Server server)
        {
            this.server = server;
        }

        public void Remove()
        {
            int[] ids = new int[] { this.id };
            RemoveEntitiesPacket removeEntitiesPacket = new RemoveEntitiesPacket(ids);
            GetWorld().RemoveFromEntities(this);
        }

        public int type;

        public void Teleport(World world, double x, double y, double z, int yaw, int pitch)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.yaw = yaw;
            this.pitch = pitch;

            if (OnEntityMove != null) 
                OnEntityMove.Invoke(this, new OnEntityMoveArgs(world, new Vector3(x, y, z)));
            SetWorld(world);
           


            world.BroadcastPacket(new TeleportEntityPacket(this, x, y, z, yaw, pitch));
        }

        public EventHandler<OnEntityMoveArgs> OnEntityMove;
    }
}
