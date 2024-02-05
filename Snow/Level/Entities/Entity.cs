﻿using Snow.Formats;
using Snow.Level;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities
{
    public class Entity
    {
        public int EntityID { get; set; }
        internal UUID uuid;

        private double x;
        private double y;
        private double z;

        /// <summary>
        /// Returns a COPY of the location, use Teleport(x, y, z) to move the entity.
        /// </summary>
        /// <returns></returns>
        public Vector GetLocation()
        {
            return new Vector(x, y, z);
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

        /// <summary>
        /// Move up to 8 blocks
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="deltaZ"></param>
        private void MoveClose(float deltaX, float deltaY, float deltaZ)
        {
            short encodedDeltaX = (short) (4095.875 * deltaX);
            short encodedDeltaY = (short)(4095.875 * deltaY);
            short encodedDeltaZ = (short)(4095.875 * deltaZ);

            UpdateEntityPosition updateEntityPosition = new UpdateEntityPosition(this, encodedDeltaX, encodedDeltaY, encodedDeltaZ);
            world.BroadcastPacket(updateEntityPosition);

            this.x += deltaX;
            this.y += deltaY;
            this.z += deltaZ;
        }

        /// <summary>
        /// Move more then 8 blocks
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        private void MoveFar(double x, double y, double z, float yaw, float pitch)
        {
            TeleportEntity teleportEntity = new TeleportEntity(this, x, y, z, yaw, pitch);
            world.BroadcastPacket(teleportEntity);

            this.x = x;
            this.y = y;
            this.z = z;
        }

        internal World world;
        public World GetWorld()
        {
            return world;
        }
    }
}
