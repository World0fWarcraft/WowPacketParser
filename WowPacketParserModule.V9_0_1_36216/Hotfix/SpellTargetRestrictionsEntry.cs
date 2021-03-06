using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V9_0_1_36216.Hotfix
{
    [HotfixStructure(DB2Hash.SpellTargetRestrictions, HasIndexInData = false)]
    public class SpellTargetRestrictionsEntry
    {
        public byte DifficultyID { get; set; }
        public float ConeDegrees { get; set; }
        public byte MaxTargets { get; set; }
        public uint MaxTargetLevel { get; set; }
        public short TargetCreatureType { get; set; }
        public int Targets { get; set; }
        public float Width { get; set; }
        public uint SpellID { get; set; }
    }
}
