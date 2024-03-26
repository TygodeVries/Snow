
using Snow.Events;
using Snow.Formats;
using Snow.Items;
using Snow.Items.Containers;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snow.Entities
{
    public class Player : Entity
    {
        public Player(Connection connection)
        {
            this.connection = connection;
            this.type = 87;

            this.SetUUID(UUID.Random());

            this.OnEntityMove += ChunkSectionUpdate;
            this.OnUseItem += BlockPlaceExecutor;

        }

        private Connection connection;
        public Connection GetConnection() { return connection; }

        private string name = "unnamed";
        public string GetName()
        {
            return name;
        }
        public void SetName(string str)
        {
            name = str;
        }


        private Inventory inventory = new Inventory(44);
        public Inventory GetInventory() { return inventory; }

        public void UpdateInventory()
        {
            ClientboundPacket clientboundPacket = new SetContainerContentPacket(0x00, inventory);
            connection.SendPacket(clientboundPacket);
        }

        /// <summary>
        /// Spawn the client into the world
        /// </summary>
        internal void SyncClient()
        {
            UpdateInventory();

            foreach(Entity entity in GetWorld().GetEntities())
            {
                if (entity != this)
                {
                    SpawnEntityPacket packet = new SpawnEntityPacket(entity);
                    connection.SendPacket(packet);
                }
            }

            if (GetConnection().GetServer().OnPlayerJoin != null)
            {
                GetConnection().GetServer().OnPlayerJoin.Invoke(this, new OnPlayerJoinArgs(this));
            }
        }

        public ItemStack GetItemMainInHand()
        {
            return inventory.GetItem(36 + GetSelectedHotbarSlot());
        }

        internal int selectedHotbarSlot = 0;
        public int GetSelectedHotbarSlot()
        {
            return selectedHotbarSlot;
        }


        int chunkSectionX = 0;
        int chunkSectionZ = 0;
        private void ChunkSectionUpdate(object sender, OnEntityMoveArgs args)
        {
            int newChunkSectionX = (int) Math.Floor(GetPosistion().x / 16);
            int newChunkSectionZ = (int)Math.Floor(GetPosistion().z / 16);

            if (chunkSectionX != newChunkSectionX || chunkSectionZ != newChunkSectionZ)
            {
                chunkSectionX = newChunkSectionX;
                chunkSectionZ = newChunkSectionZ;

                SetCenterChunkPacket packet = new SetCenterChunkPacket(chunkSectionX, chunkSectionZ);
                GetConnection().SendPacket(packet);

                connection.SendRenderDistance(GetWorld(), 7, new Vector3(chunkSectionX, 0, chunkSectionZ));
            }
        }

        public EventHandler<OnUseItemsArgs> OnUseItem;

        public void Kick(string message)
        {
            Thread thr = new Thread(() =>
            {
                DisconnectPacket packet = new DisconnectPacket(new TextComponent(message));
                GetConnection().SendPacket(packet);
                GetConnection().Flush();
                Thread.Sleep(100);
                GetConnection().Disconnected();
            });

            thr.Start();
        }

        private void BlockPlaceExecutor(object sender, OnUseItemsArgs args)
        {
            ItemStack itemStack = args.player.GetItemMainInHand();
            if (itemStack == null)
                return;

            if (args.insideblock)
                return;

            if (itemStack.GetItemType().IsBlock)
            {
                int face = args.face;

                Position blockPos = args.position.GetAdjacent(face);
                GetWorld().OnBlockPlace.Invoke(this, new OnBlockPlaceArgs(this, blockPos, itemStack.GetItemType().blockType));
            }
        }
    }
}
