using Snow.Formats;
using Snow.Formats.Nbt.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities.Types
{
    public abstract class PhysicsEntity : Entity
    {
        private Vector3d velocity;

        public Vector3d GetVelocity()
        { return velocity; }

        public PhysicsEntity()
        {
            
        }

        public void EnablePhysics()
        {
            this.OnEntityTick += Physics;
        }

        public void SetVelocity(Vector3d vector)
        {
            this.velocity = vector;
        }

        float gravity = -0.2f;
        public void SetGravity(float gravity)
        {
            this.gravity = gravity;
        }

        public void Physics(object sender, EventArgs eventArgs)
        {
            velocity.y += gravity;

            Vector3d newLocation = this.GetLocation() + velocity;

            this.Teleport(this.GetWorld(), newLocation.x, newLocation.y, newLocation.z, this.GetYaw(), this.GetPitch());
        }
    }
}
