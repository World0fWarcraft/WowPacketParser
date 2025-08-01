﻿
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_5_0_61735.Parsers
{
    public static class GroupHandler
    {
        [Parser(Opcode.SMSG_CHANGE_PLAYER_DIFFICULTY_RESULT)]
        public static void HandleChangePlayerDifficultyResult(Packet packet)
        {
            var result = packet.ReadBitsE<DifficultyChangeResult>("Result", 4);

            switch (result)
            {
                case DifficultyChangeResult.Cooldown:
                    packet.ReadBit("InCombat");
                    packet.ReadInt64("Cooldown");
                    break;
                case DifficultyChangeResult.LoadingScreenEnable:
                    packet.ReadBit("Unused");
                    packet.ReadInt64("NextDifficultyChangeTime");
                    break;
                case DifficultyChangeResult.MapDifficultyConditionNotSatisfied:
                    packet.ReadInt32E<MapDifficulty>("MapDifficultyID");
                    break;
                case DifficultyChangeResult.PlayerAlreadyLockedToDifferentInstance:
                    packet.ReadPackedGuid128("PlayerGUID");
                    break;
                case DifficultyChangeResult.Success:
                    packet.ReadInt32<MapId>("MapID");
                    packet.ReadInt32<DifficultyId>("DifficultyID");
                    break;
                default:
                    break;
            }
        }

        [Parser(Opcode.SMSG_ROLE_CHANGED_INFORM)]
        public static void HandleRoleChangedInform(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadPackedGuid128("From");
            packet.ReadPackedGuid128("ChangedUnit");
            packet.ReadByteE<LfgRoleFlag>("OldRole");
            packet.ReadByteE<LfgRoleFlag>("NewRole");
        }

        [Parser(Opcode.SMSG_ROLE_POLL_INFORM)]
        public static void HandleRolePollInform(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadPackedGuid128("From");
        }

        [Parser(Opcode.SMSG_RAID_MARKERS_CHANGED)]
        public static void HandleRaidMarkersChanged(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadInt32("ActiveMarkers");

            var count = packet.ReadBits(4);
            for (int i = 0; i < count; i++)
            {
                packet.ReadPackedGuid128("TransportGUID");
                packet.ReadInt32("MapID");
                packet.ReadVector3("Position");
            }
        }
    }
}
