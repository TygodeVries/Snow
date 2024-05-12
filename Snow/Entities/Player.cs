
using Snow.Events;
using Snow.Events.Arguments;
using Snow.Formats;
using Snow.Items;
using Snow.Items.Containers;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Network.Packets.Play.Serverbound;
using Snow.Worlds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Snow.Entities
{
    public class Player : Entity
    {
        public Player(Connection connection)
        {
            this.inventory = new Inventory(44, this, InventoryType.PlayerInventory);

            this.connection = connection;
            this.type = 124;

            this.OnEntityMove += ChunkSectionUpdate;
            this.OnUseItem += BlockPlaceExecutor;

        }

        private Connection connection;
        public Connection GetConnection() { return connection; }

        private string name = "mrnoname";
        public string GetName()
        {
            return name;
        }
        public void SetName(string str)
        {
            name = str;
        }


        private Inventory inventory;
        public Inventory GetInventory() { return inventory; }

        public void UpdateInventory()
        {
            ClientboundPacket clientboundPacket = new SetContainerContentPacket(0x00, inventory);
            connection.SendPacket(clientboundPacket);
        }

        public override void Spawn()
        {
            PlayerInfoUpdatePacket updatePacket = new PlayerInfoUpdatePacket(this.GetUUID());
            updatePacket.SetAddPlayerPayload(GetName());
            GetWorld().BroadcastPacket(updatePacket, new List<Connection>() { this.GetConnection() });   
        }

        /// <summary>
        /// Spawn the client into the world
        /// </summary>
        internal void SyncClient()
        {
            UpdateInventory();

            foreach(Entity entity in GetWorld().GetEntities())
            {
                if (entity.GetType() == typeof(Player))
                {
                    Player player = (Player)entity;

                    PlayerInfoUpdatePacket updatePacket = new PlayerInfoUpdatePacket(player.GetUUID());
                    updatePacket.SetAddPlayerPayload(GetName());
                    GetConnection().SendPacket(updatePacket);
                }

                if (entity != this)
                {
                    SpawnEntityPacket packet = new SpawnEntityPacket(entity);
                    GetConnection().SendPacket(packet);
                }

            }

            SetGamemode((Gamemode)Enum.Parse(typeof(Gamemode), GetConnection().GetServer().GetSettings().GetString("default-gamemode")));

            GetConnection().GetServer().GetEventManager().PlayerJoinEvent?.Invoke(null, new OnPlayerJoinArgs(this));
        }

        public void PlaySound(Identifier identifier, SoundSource soundSource, float volume, float pitch)
        {
            SoundEffectPacket packet = new SoundEffectPacket(identifier, soundSource, volume, pitch, 0);
            GetConnection().SendPacket(packet);
        }

        public ItemStack GetItemMainInHand()
        {
            return inventory.GetItem(36 + GetSelectedHotbarSlot());
        }
        
        public void SendSystemMessage(TextComponent message)
        {
            SystemChatMessagePacket packet = new SystemChatMessagePacket(message, false);
            GetConnection().SendPacket(packet);
        }

        internal int selectedHotbarSlot = 0;
        public int GetSelectedHotbarSlot()
        {
            return selectedHotbarSlot;
        }

        Gamemode gamemode;
        public Gamemode GetGamemode()
        { return gamemode; }

        public void SetGamemode(Gamemode gamemode)
        {
            this.gamemode = gamemode;
            GameEventPacket packet = new GameEventPacket(0x03, (int) gamemode);
            GetConnection().SendPacket(packet);

            if(GetGamemode() == Gamemode.CREATIVE)
            {
                SetAllowFlying(true);
            }
            else if(GetGamemode() == Gamemode.SPECTATOR)
            {
                SetAllowFlying(false);
                SetFlying(true);
            }
            else
            {
                SetAllowFlying(false);
                SetFlying(false);
            }
        }


        public void SetCamera(Entity entity)
        {
            SetCameraPacket packet = new SetCameraPacket(entity.GetId());
            GetConnection().SendPacket(packet);
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

            if (itemStack.GetItemType().GetType() == typeof(BlockItemType))
            {
                BlockItemType blockItemType = (BlockItemType) itemStack.GetItemType();
                int face = args.face;

                Position blockPos = args.position.GetAdjacent(face);
                GetWorld().OnBlockPlace?.Invoke(this, new OnBlockPlaceArgs(this, blockPos, blockItemType.blockType));
                GetWorld().SetBlockAt(blockPos.x, blockPos.y, blockPos.z, blockItemType.blockType);
            }
        }

        private void UpdatePlayerAbilities()
        {
            byte flags = 0x00;

            if(flying) flags |= 0x02;
            if(flyingAllowed) flags |= 0x04;

            PlayerAbilitiesPacket packet = new PlayerAbilitiesPacket(flags, flyingSpeed, 0.1f);
        }

        private bool flying = false;
        public void SetFlying(bool flying)
        {
            this.flying = flying;
            UpdatePlayerAbilities();
        }

        Position _currentDiggingPosition = null;
        public Position GetCurrentDigggingPosition()
        {
            return _currentDiggingPosition;
        }

        public bool IsDigging()
        {
            return _currentDiggingPosition != null;
        }

        public void StartDigging(Position position)
        {
            _currentDiggingPosition = position;

            if(GetGamemode() == Gamemode.CREATIVE)
            {
                BreakBlock(_currentDiggingPosition);
                _currentDiggingPosition = null;
            }
        }

        public void CancelDigging()
        {
            _currentDiggingPosition = null;
        }

        public void BreakBlock(Position position)
        {
            GetWorld().SetBlockAt(position.x, position.y, position.z, BlockType.AIR);
        }

        private bool flyingAllowed = false;
        public void SetAllowFlying(bool flyingAllowed)
        {
            this.flyingAllowed = flyingAllowed;
            UpdatePlayerAbilities();
        }
        
        private float flyingSpeed = 0.05f;
        /// <summary>
        /// 0.05 by default.
        /// </summary>
        /// <param name="speed"></param>
        public void SetFlyingSpeed(float flyingSpeed)
        {
            this.flyingSpeed = flyingSpeed;
            UpdatePlayerAbilities();
        }


        public void SetTitleAnimationTimes(int fadein, int stay, int fadeout)
        {
            SetTitleAnimationTimesPacket packet = new SetTitleAnimationTimesPacket(fadein, stay, fadeout);
            GetConnection().SendPacket(packet);
        }
        public void SendTitle(TextComponent textComponent)
        {
            SetTitleTextPacket setTitleTextPacket = new SetTitleTextPacket(textComponent);
            GetConnection().SendPacket(setTitleTextPacket);
        }
    }

    public enum Gamemode
    {
        SURVIVAL,
        CREATIVE,
        ADVENTURE,
        SPECTATOR
    }
}
