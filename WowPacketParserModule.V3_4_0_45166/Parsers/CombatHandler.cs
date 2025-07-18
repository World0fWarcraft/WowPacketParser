using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Proto;

namespace WowPacketParserModule.V3_4_0_45166.Parsers
{
    public static class CombatHandler
    {
        [Parser(Opcode.SMSG_ATTACK_START, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleAttackStartStart(Packet packet)
        {
            packet.ReadPackedGuid128("Attacker Guid");
            packet.ReadPackedGuid128("Victim Guid");
        }

        [Parser(Opcode.SMSG_AI_REACTION, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleAIReaction(Packet packet)
        {
            PacketAIReaction aiReaction = packet.Holder.AiReaction = new();
            aiReaction.UnitGuid = packet.ReadPackedGuid128("UnitGUID").ToUniversalGuid();
            aiReaction.Reaction = (WowPacketParser.Proto.AIReaction)packet.ReadInt32E<WowPacketParser.Enums.AIReaction>("Reaction");
        }

        [Parser(Opcode.SMSG_ATTACK_STOP, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleAttackStartStop(Packet packet)
        {
            packet.ReadPackedGuid128("Attacker Guid");
            packet.ReadPackedGuid128("Victim Guid");

            packet.ReadBit("NowDead");
        }

        [Parser(Opcode.SMSG_ATTACK_SWING_ERROR, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleAttackSwingError(Packet packet)
        {
            packet.ReadBitsE<AttackSwingErr>("Reason", 3);
        }

        [Parser(Opcode.SMSG_BREAK_TARGET, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleBreakTarget(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
        }

        [Parser(Opcode.SMSG_CAN_DUEL_RESULT, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleCanDuelResult(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadBit("Result");
        }

        [Parser(Opcode.SMSG_CANCEL_AUTO_REPEAT, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleCancelAutoRepeat(Packet packet)
        {
            packet.ReadPackedGuid128("Guid");
        }

        [Parser(Opcode.SMSG_HIGHEST_THREAT_UPDATE, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleHighestThreatlistUpdate(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            packet.ReadPackedGuid128("HighestThreatGUID");

            var count = packet.ReadUInt32("ThreatListCount");

            // ThreatInfo
            for (var i = 0; i < count; i++)
            {
                packet.ReadPackedGuid128("UnitGUID", i);
                packet.ReadInt64("Threat", i);
            }
        }

        [Parser(Opcode.SMSG_THREAT_UPDATE, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleThreatlistUpdate(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            var count = packet.ReadInt32("ThreatListCount");

            for (int i = 0; i < count; i++)
            {
                packet.ReadPackedGuid128("TargetGUID", i);
                packet.ReadInt64("Threat", i);
            }
        }

        [Parser(Opcode.SMSG_DUEL_COMPLETE, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleDuelComplete(Packet packet)
        {
            packet.ReadBool("Started");
        }

        [Parser(Opcode.SMSG_DUEL_COUNTDOWN, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleDuelCountDown(Packet packet)
        {
            packet.ReadInt32("Countdown");
        }

        [Parser(Opcode.SMSG_DUEL_REQUESTED, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleDuelRequested(Packet packet)
        {
            packet.ReadPackedGuid128("ArbiterGUID");
            packet.ReadPackedGuid128("RequestedByGUID");
            packet.ReadPackedGuid128("RequestedByWowAccount");
            packet.ReadBit("ToTheDeath");
        }

        [Parser(Opcode.SMSG_DUEL_WINNER, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleDuelWinner(Packet packet)
        {
            var beatenNameLength = packet.ReadBits(6);
            var winnerNameLength = packet.ReadBits(6);

            packet.ReadBit("Fled");

            // Order guessed
            packet.ReadUInt32("BeatenVirtualRealmAddress");
            packet.ReadUInt32("WinnerVirtualRealmAddress");

            // Order guessed
            packet.ReadWoWString("BeatenName", beatenNameLength);
            packet.ReadWoWString("WinnerName", winnerNameLength);
        }

        [Parser(Opcode.SMSG_PVP_CREDIT, ClientVersionBuild.V3_4_4_59817)]
        public static void HandlePvPCredit(Packet packet)
        {
            packet.ReadInt32("OriginalHonor");
            packet.ReadInt32("Honor");
            packet.ReadPackedGuid128("Target");
            packet.ReadByte("Rank");

            packet.ResetBitReader();
            packet.ReadBit("ForceHonorable");
        }

        [Parser(Opcode.SMSG_THREAT_CLEAR, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleClearThreatlist(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.SMSG_THREAT_REMOVE, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleRemoveThreatlist(Packet packet)
        {
            packet.ReadPackedGuid128("UnitGUID");
            packet.ReadPackedGuid128("AboutGUID");
        }

        [Parser(Opcode.CMSG_ATTACK_SWING, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleAttackSwing(Packet packet)
        {
            packet.ReadPackedGuid128("Victim");
        }

        [Parser(Opcode.CMSG_CAN_DUEL, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleCanDuel(Packet packet)
        {
            packet.ReadPackedGuid128("TargetGUID");
            packet.ReadBit("ToTheDeath");
        }

        [Parser(Opcode.CMSG_SET_SHEATHED, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleSetSheathed(Packet packet)
        {
            packet.ReadInt32E<SheathState>("CurrentSheathState");
            packet.ReadBit("Animate");
        }

        [Parser(Opcode.SMSG_CANCEL_COMBAT, ClientVersionBuild.V3_4_4_59817)]
        [Parser(Opcode.SMSG_DUEL_IN_BOUNDS, ClientVersionBuild.V3_4_4_59817)]
        [Parser(Opcode.SMSG_DUEL_OUT_OF_BOUNDS, ClientVersionBuild.V3_4_4_59817)]
        [Parser(Opcode.CMSG_ATTACK_STOP, ClientVersionBuild.V3_4_4_59817)]
        [Parser(Opcode.CMSG_TOGGLE_PVP, ClientVersionBuild.V3_4_4_59817)]
        public static void HandleCombatNull(Packet packet)
        {
        }
    }
}
