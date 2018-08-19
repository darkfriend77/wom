using System.Collections.Generic;

namespace WoMInterface.Game
{
    public class Stats
    {
        public Attribute StrengthAttr = AttributBuilder.Create("Strength")
            .Salted(true).Position(30).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of how physically strong a character is. Strength often controls the maximum weight the character can carry, melee attack and/or damage, and sometimes hit points.Armor and weapons might also have a Strength requirement.").Build();
        public int Strength => StrengthAttr.GetValue();

        public Attribute ConstitutionAttr = AttributBuilder.Create("Constitution")
            .Salted(true).Position(32).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of how sturdy a character is. Constitution often influences hit points, resistances for special types of damage(poisons, illness, heat etc.) and fatigue.").Build();
        public int Constitution => ConstitutionAttr.GetValue();

        public Attribute DexterityAttr = AttributBuilder.Create("Dexterity")
            .Salted(true).Position(34).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of how agile a character is. Dexterity controls attack and movement speed and accuracy, as well as evading an opponent's attack (see Armor Class).").Build();
        public int Dexterity => DexterityAttr.GetValue();

        public Attribute InteligenceAttr = AttributBuilder.Create("Intelligence")
            .Salted(true).Position(36).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of a character's problem-solving ability. Intelligence often controls a character's ability to comprehend foreign languages and their skill in magic.In some cases, intelligence controls how many skill points the character gets at 'level up'. In some games, it controls the rate at which experience points are earned, or the amount needed to level up.Under certain circumstances, this skill can also negate combat actions between players and NPC enemies.This is sometimes combined with wisdom and/or willpower.").Build();
        public int Inteligence => InteligenceAttr.GetValue();

        public Attribute WisdomAttr = AttributBuilder.Create("Wisdom")
            .Salted(true).Position(38).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of a character's common sense and/or spirituality. Wisdom often controls a character's ability to cast certain spells, communicate to mystical entities, or discern other characters' motives or feelings.").Build();
        public int Wisdom => WisdomAttr.GetValue();

        public Attribute CharismaAttr = AttributBuilder.Create("Charisma")
            .Salted(true).Position(40).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of a character's social skills, and sometimes their physical appearance. Charisma generally influences prices while trading and NPC reactions. Under certain circumstances, this skill can negate combat actions between players and NPC enemies.").Build();
        public int Charisma => CharismaAttr.GetValue();

        public Attribute LuckAttr = AttributBuilder.Create("Luck")
            .Salted(true).Position(42).Size(2).Creation(32).MinRange(1).MaxRange(256).Description("A measure of a character's luck. Luck might influence anything, but mostly random items, encounters and outstanding successes/failures (such as critical hits).").Build();
        public int Luck => LuckAttr.GetValue();

        public Attribute AllignmentAttr = AttributBuilder.Create("Allignment")
            .Salted(true).Position(44).Size(1).Creation(16).MaxRange(16).Description("A creature's general moral and personal attitudes are represented by its alignment: lawful good, neutral good, chaotic good, lawful neutral, neutral, chaotic neutral, lawful evil, neutral evil, or chaotic evil.").Build();
        public int Allignment => AllignmentAttr.GetValue();

        public List<Attribute> All => new List<Attribute>() { StrengthAttr, ConstitutionAttr, DexterityAttr, InteligenceAttr, WisdomAttr, CharismaAttr, LuckAttr, AllignmentAttr };

        public Stats(HexValue hexValue)
        {
            All.ForEach(p => p.CreateValue(hexValue));
        }

        public string MapAllignment() {
            switch(Allignment)
            {
                case 0:
                    return "LG";
                case 1:
                    return "LG";
                case 2:
                    return "LG";
                case 3:
                    return "LG";
                case 4:
                    return "LG";
                case 5:
                    return "NG";
                case 6:
                    return "NG";
                case 7:
                    return "CG";
                case 8:
                    return "LN";
                case 9:
                    return "LN";
                case 10:
                    return "TN";
                case 11:
                    return "TN";
                case 12:
                    return "CN";
                case 13:
                    return "LE";
                case 14:
                    return "NE";
                case 15:
                    return "CE";
                default: 
                    return "LG";
            }
        }
    }
}