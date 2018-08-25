using System.Collections.Generic;

namespace WoMInterface.Game.Model
{
    public class Stats
    {
        public Attribute LuckAttr = AttributBuilder.Create("Luck")
            .Salted(true).Position(42).Size(2).Creation(32).MinRange(1).MaxRange(256)
            .Description("A measure of a character's luck. Luck might influence anything, but mostly random items, encounters and outstanding successes/failures (such as critical hits).").Build();
        public int Luck => LuckAttr.GetValue();

        public Attribute AllignmentAttr = AttributBuilder.Create("Allignment")
            .Salted(true).Position(44).Size(1).Creation(16).MaxRange(16)
            .Description("A creature's general moral and personal attitudes are represented by its alignment: lawful good, neutral good, chaotic good, lawful neutral, neutral, chaotic neutral, lawful evil, neutral evil, or chaotic evil.").Build();
        public int Allignment => AllignmentAttr.GetValue();

        public List<Attribute> All => new List<Attribute>() { LuckAttr, AllignmentAttr };

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