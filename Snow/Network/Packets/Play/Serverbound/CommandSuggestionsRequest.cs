﻿using Snow.Commands;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class CommandSuggestionsRequest : ServerboundPacket
    {
        int transactionId;
        string text;

        public override void Decode(PacketReader packetReader)
        {
            transactionId = packetReader.ReadVarInt();
            text = packetReader.ReadString();

            Log.Send(text);
        }

        public override void Use(Connection connection)
        {
            CommandManager manager = connection.GetLobby().GetCommandManager();

            string[] suggestions = manager.GetSuggestions(text, connection.GetPlayer());

            CommandSuggestionsResponse response = new CommandSuggestionsResponse(transactionId, text.Length, 0, suggestions);
            connection.SendPacket(response);

        }
    }
}
