using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    public enum GenderType
    {
        MALE,
        FEMALE
    }

    public abstract class Entity
    {
        public string Name { get; set; }

        public int Gender { get; set; }
        public string MapGender => ((GenderType)Gender).ToString();

        public int Strength { get; set; }
        public int StrengthMod => Modifier(Strength);

        public int Dexterity { get; set; }
        public int DexterityMod => Modifier(Dexterity);

        public int Constitution { get; set; }
        public int ConstitutionMod => Modifier(Constitution);

        public int Inteligence { get; set; }
        public int InteligenceMod => Modifier(Inteligence);

        public int Wisdom { get; set; }
        public int WisdomMod => Modifier(Wisdom);

        public int Charisma { get; set; }
        public int CharismaMod => Modifier(Charisma);

        // armorclass = 10 + armor bonus + shield bonus + dex modifier + size modifier + natural armor + deflection + misc modifier
        public int ArmorClass => 10 + DexterityMod;

        public int HitPointa => 8 + 0;

        // initiative = dex modifier + misc modifier
        public int Initiative => DexterityMod;

        // base attack bonus = class dependent value
        public int BaseAttackBonus => 1;

        // attackbonus = base attack bonus + strength modifier + size modifier
        public int AttackBonus => BaseAttackBonus + StrengthMod;

        public Experience Experience { get; set; }
        
        public Entity()
        {
            
        }

        private int Modifier(int ability)
        {
            return (int)Math.Floor(ability / 10D) - 5;
        }
        
    }
}
