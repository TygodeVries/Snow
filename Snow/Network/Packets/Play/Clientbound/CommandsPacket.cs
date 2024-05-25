using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class CommandsPacket : ClientboundPacket
    {
        string[] commandsList;
        public CommandsPacket(string[] commandsList)
        { 
            this.commandsList = commandsList;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            List<CommandNode> commandNodes = new List<CommandNode>();

            CommandNode rootCommandNode = new CommandNode().SetFlag(NodeFlags.Root);
            commandNodes.Add(rootCommandNode);

            List<int> baseCommandIndexes = new List<int>();
            for(int i = 0; i < commandsList.Length; i++)
            {
                baseCommandIndexes.Add(commandNodes.Count);
                commandNodes.Add(new CommandNode().SetFlag(NodeFlags.Literal).SetName(commandsList[i]));
            }

            rootCommandNode.SetIndexes(baseCommandIndexes.ToArray());

            packetWriter.WriteVarInt(commandNodes.Count);
            foreach (CommandNode commandNode in commandNodes) 
            {
                // Write flag
                packetWriter.WriteByte((byte) commandNode.Flags);
                
                // Write Children
                packetWriter.WriteVarInt(commandNode.childrenIndexes.Length);
                foreach(int i in commandNode.childrenIndexes)
                {
                    packetWriter.WriteVarInt(i);
                }

                // Write name
                if(commandNode.name != null)
                {
                    packetWriter.WriteString(commandNode.name);
                }
            }

            // Root index
            packetWriter.WriteVarInt(0);
        }
    }

    class CommandNode
    {
        public NodeFlags Flags { get; private set; }
        public int[] childrenIndexes = new int[0];

        public string name = null;

        public CommandNode SetFlag(NodeFlags flag)
        {
            Flags |= flag;
            return this;
        }

        public CommandNode SetIndexes(int[] ints)
        {
            this.childrenIndexes = ints;
            return this;
        }

        public CommandNode SetName(string name)
        {
            this.name = name;
            return this;
        }
    }

    [Flags]
    enum NodeFlags
    {
        None = 0x00,
        NodeTypeMask = 0x03, // Mask for extracting node type
        Root = 0x00,         // Node type 0: root
        Literal = 0x01,      // Node type 1: literal
        Argument = 0x02,     // Node type 2: argument
        IsExecutable = 0x04, // Set if the node stack to this point constitutes a valid command
        HasRedirect = 0x08,  // Set if the node redirects to another node
        HasSuggestionsType = 0x10 // Only present for argument nodes
    }
}
