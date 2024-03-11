using Snow.Formats;
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

        private double x;
        private double y;
        private double z;

        public World GetWorld()
        {
            return world;
        }

        World world;

        internal void SetWorld(World world)
        {
            this.world = world;
        }

        public Vector GetLocation()
        {
            return new Vector(x, y, z);
        }

        Server server;

        public void SetServer(Server server)
        {
            this.server = server;
        }

        public int type;

        public void Teleport(double x, double y, double z)
        {
            float distance = (float)Math.Sqrt((x - this.x) * (x - this.x) + (y - this.y) * (y - this.y) + (z - this.z) * (z - this.z));

            if(distance < -1)
            {
                MoveClose((float) (x - this.x), (float) (y - this.y), (float) (z - this.z));
            }
            else
            {
                MoveFar(x, y, z, 0, 0);
            }
        }


        private void MoveClose(float deltaX, float deltaY, float deltaZ)
        {
            short encodedDeltaX = (short) (4095.875 * deltaX);
            short encodedDeltaY = (short)(4095.875 * deltaY);
            short encodedDeltaZ = (short)(4095.875 * deltaZ);

            UpdateEntityPositionPacket updateEntityPosition = new UpdateEntityPositionPacket(this, encodedDeltaX, encodedDeltaY, encodedDeltaZ);
            server.BroadcastPacket(updateEntityPosition);

            this.x += deltaX;
            this.y += deltaY;
            this.z += deltaZ;
        }

        private void MoveFar(double x, double y, double z, float yaw, float pitch)
        {
            TeleportEntityPacket teleportEntity = new TeleportEntityPacket(this, x, y, z, yaw, pitch);
            server.BroadcastPacket(teleportEntity);

            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
