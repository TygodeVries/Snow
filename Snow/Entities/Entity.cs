using Snow.Events;
using Snow.Events.Arguments;
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

        private float pitch;
        private float yaw;

        public float GetPitch()
        {
            return pitch;
        }

        public float GetYaw()
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
            GetWorld().RemoveEntity(this);

            GetWorld().BroadcastPacket(removeEntitiesPacket);
        }

        public int type;

        public void Teleport(World world, double x, double y, double z, float yaw, float pitch)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.yaw = yaw;
            this.pitch = pitch;

            SetWorld(world);

            if (OnEntityMove != null) 
                OnEntityMove.Invoke(this, new OnEntityMoveArgs(world, new Vector3(x, y, z), this));

            world.BroadcastPacket(new TeleportEntityPacket(this, x, y, z, yaw, pitch));
            world.BroadcastPacket(new SetHeadRotationPacket(this, yaw));
        }

        public EventHandler<OnEntityMoveArgs> OnEntityMove;

        public virtual void Spawn()
        {

        }
    }
}
