﻿using WowPacketParser.DBC;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_5_0_61735.Parsers
{
    public static class AchievementHandler
    {
        public static void ReadEarnedAchievement(Packet packet, params object[] idx)
        {
            packet.ReadInt32<AchievementId>("Id", idx);
            packet.ReadPackedTime("Date", idx);
            packet.ReadPackedGuid128("Owner", idx);
            packet.ReadInt32("VirtualRealmAddress", idx);
            packet.ReadInt32("NativeRealmAddress", idx);
        }

        public static void ReadCriteriaProgress(Packet packet, params object[] indexes)
        {
            var criteriaId = packet.ReadInt32<CriteriaId>("CriteriaID", indexes);
            var quantity = packet.ReadUInt64("Quantity", indexes);
            packet.ReadPackedGuid128("PlayerGUID", indexes);
            packet.ReadInt32("Unused_10_1_5", indexes);
            packet.ReadInt32("Flags", indexes);
            packet.ReadPackedTime("CurrentTime", indexes);
            packet.ReadTime64("ElapsedTime", indexes);
            packet.ReadTime64("CreationTime", indexes);

            packet.ResetBitReader();

            var hasDynamicID = packet.ReadBit("HasDynamicID", indexes);
            if (hasDynamicID)
                packet.ReadUInt64("DynamicID", indexes);

            if (Settings.UseDBC)
                if (DBC.Criteria.ContainsKey(criteriaId))
                    if (DBC.Criteria[criteriaId].Type == 46)
                        CoreParsers.AchievementHandler.FactionReputationStore[DBC.Criteria[criteriaId].Asset] = quantity;
        }

        public static void ReadAllAchievements(Packet packet, params object[] idx)
        {
            var earnedCount = packet.ReadUInt32("EarnedCount", idx);
            var progressCount = packet.ReadUInt32("ProgressCount", idx);

            for (var i = 0; i < earnedCount; ++i)
                ReadEarnedAchievement(packet, idx, "Earned", i);

            for (var i = 0; i < progressCount; ++i)
                ReadCriteriaProgress(packet, idx, "Progress", i);
        }

        [Parser(Opcode.SMSG_BROADCAST_ACHIEVEMENT)]
        public static void HandleBroadcastAchievement(Packet packet)
        {
            var nameLength = packet.ReadBits(7);
            packet.ReadBit("GuildAchievement");
            packet.ReadPackedGuid128("PlayerGUID");
            packet.ReadUInt32("AchievementID");
            packet.ReadWoWString("Name", nameLength);
        }

        [Parser(Opcode.SMSG_ALL_ACHIEVEMENT_DATA)]
        public static void HandleAllAchievementDataPlayer(Packet packet)
        {
            ReadAllAchievements(packet, "Data");
        }

        [Parser(Opcode.SMSG_ALL_ACCOUNT_CRITERIA)]
        public static void HandleAllAchievementCriteriaDataAccount(Packet packet)
        {
            var count = packet.ReadUInt32("ProgressCount");

            for (var i = 0; i < count; ++i)
                ReadCriteriaProgress(packet, "Progress", i);
        }

        [Parser(Opcode.SMSG_RESPOND_INSPECT_ACHIEVEMENTS)]
        public static void HandleRespondInspectAchievements(Packet packet)
        {
            packet.ReadPackedGuid128("Player");
            ReadAllAchievements(packet, "Data");
        }
    }
}
