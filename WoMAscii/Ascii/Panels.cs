using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMAscii.Tool;
using WoMInterface.Game.Model;
using WoMInterface.Tool;

namespace WoMInterface.Game.Ascii
{
    public class Panels
    {
        public static string[] CharacterPanel(Mogwai mogwai)
        {
            var template = new string[]
            {
                 @"  .+------------------------------+.                         ¬", // 0
                 @".:-| <name        >    [Lvl. <l>] |-:. Class: <class       > ¬", // 1
                 @"+-stats-------+-hitpoints---------+--+-experience-----------+¬", // 2
                 @"| STR: <str > ¦ <chp > / <mhp > HP   ¦ <cex > / <nex > EXP  |¬", // 3
                 @"| DEX: <dex > ¦ <hpbar        > <h%> ¦ <exbar        > <e%> |¬", // 4
                 @"| CON: <con > +-combat------+-weapon-+-----+---+------------+¬", // 5
                 @"| INT: <int > ¦ INI: <ini > ¦ P: <pweap      > dmg <pdmg  > |¬", // 6
                 @"| WIS: <wis > ¦ AT:  <atb > ¦ S: <sweap      > dmg <sdmg  > |¬", // 7
                 @"| CHA: <cha > ¦ AC:  <acl > +-armor-------------------------+¬", // 8
                 @"+-------------+ SP:  <sp  > ¦ <barm          >  AC:  <baac> |¬", // 9
                 @"| FOR: <for > +-------------+ ................  AC:  <... > |¬", //10
                 @"| REF: <ref > ¦ CMB: <cmb > ¦ ................  AC:  <... > |¬", //11
                 @"| WIL: <wil > ¦ CMD: <cmd > ¦ ................  AC:  <... > |¬", //12
                 @"+-------------+-------------+-------------------------------+¬", //13
                 @"| <address                         > ¦ <alligen           > |¬", //14
                 @"+---------------+-----------+-------------------------------+¬", //15
                 @"| HISTORY PANEL | POINTER <pnow         > / <tnow         > |¬", //16
                 @"+---------------+-------------------------------------------+¬", //17
                 @"| <logentr8                                              > +|¬", //18
                 @"| <logentr7                                              > ¦|¬", //19
                 @"| <logentr6                                              > ¦|¬", //20
                 @"| <logentr5                                              > ¦|¬", //21
                 @"| <logentr5                                              > ¦|¬", //22
                 @"| <logentr4                                              > ¦|¬", //23
                 @"| <logentr3                                              > ¦|¬", //24
                 @"| <logentr2                                              > ¦|¬", //25
                 @"| <logentr1                                              > ¦|¬", //26
                 @"| <logentr0                                              > +|¬", //27
                 @"+-----------------------------------------------------------+¬"  //28
             };

            template[1] = template[1].Replace("<name        >", string.Format("¬C{0}§", mogwai.Name.PadRight(14)));
            template[1] = template[1].Replace("<class       >", string.Format("¬c{0}§", (mogwai.CurrentClass != null ? mogwai.CurrentClass.Name : "none").PadRight(14)));
            template[1] = template[1].Replace("<l>", string.Format("¬Y{0:##0}§", mogwai.CurrentLevel.ToString().PadLeft(3)));
            template[2] = template[2].Replace("<gen    >", string.Format("¬W{0}§", mogwai.MapGender.PadLeft(9)));
            template[3] = template[3].Replace("<str >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Strength, mogwai.StrengthMod > 0 ? "G" : mogwai.StrengthMod < 0 ? "R" : "Y", mogwai.StrengthMod.ToString("+0;-#")));

            template[3] = template[3].Replace("<chp >", string.Format("¬W{0}§", mogwai.CurrentHitPoints.ToString().PadLeft(6)));
            template[3] = template[3].Replace("<mhp >", string.Format("¬W{0}§", mogwai.MaxHitPoints.ToString().PadRight(6)));

            template[4] = template[4].Replace("<hpbar        >", string.Format("{0}", AsciiHelpers.GetBar(mogwai.CurrentHitPoints, mogwai.MaxHitPoints, out int hpPerc)));
            template[4] = template[4].Replace("<h%>", string.Format("¬W{0:##0}§%", hpPerc.ToString().PadLeft(3)));

            template[3] = template[3].Replace("<cex >", string.Format("¬W{0}§", mogwai.Exp.ToString().PadLeft(6)));
            template[3] = template[3].Replace("<nex >", string.Format("¬W{0}§", mogwai.XpToLevelUp.ToString().PadRight(6)));

            template[4] = template[4].Replace("<exbar        >", string.Format("{0}", AsciiHelpers.GetBar(mogwai.Exp, mogwai.XpToLevelUp, out int exPerc)));
            template[4] = template[4].Replace("<e%>", string.Format("¬W{0:##0}§%", exPerc.ToString().PadLeft(3)));

            template[4] = template[4].Replace("<dex >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Dexterity, mogwai.DexterityMod > 0 ? "G" : mogwai.DexterityMod < 0 ? "R" : "Y", mogwai.DexterityMod.ToString("+0;-#")));
            template[5] = template[5].Replace("<con >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Constitution, mogwai.ConstitutionMod > 0 ? "G" : mogwai.ConstitutionMod < 0 ? "R" : "Y", mogwai.ConstitutionMod.ToString("+0;-#")));
            template[6] = template[6].Replace("<int >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Inteligence, mogwai.InteligenceMod > 0 ? "G" : mogwai.InteligenceMod < 0 ? "R" : "Y", mogwai.InteligenceMod.ToString("+0;-#")));
            template[7] = template[7].Replace("<wis >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Wisdom, mogwai.WisdomMod > 0 ? "G" : mogwai.WisdomMod < 0 ? "R" : "Y", mogwai.WisdomMod.ToString("+0;-#")));
            template[8] = template[8].Replace("<cha >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.Charisma, mogwai.CharismaMod > 0 ? "G" : mogwai.CharismaMod < 0 ? "R" : "Y", mogwai.CharismaMod.ToString("+0;-#")));

            template[6] = template[6].Replace("<ini >", string.Format("¬W{0}§", mogwai.Initiative.ToString().PadLeft(2).PadRight(6)));
            int addAtb = mogwai.AttackBonus(0) - mogwai.BaseAttackBonus[0];
            template[7] = template[7].Replace("<atb >", string.Format("¬W{0,2}§[¬{1}{2,2}§]", mogwai.BaseAttackBonus[0], addAtb > 0 ? "G" : addAtb < 0 ? "R" : "Y", addAtb.ToString("+0;-#")));
            template[8] = template[8].Replace("<acl >", string.Format("¬W{0}§", mogwai.ArmorClass.ToString().PadLeft(2).PadRight(6)));
            template[9] = template[9].Replace("<sp  >", string.Format("¬W{0}§ft ", mogwai.BaseSpeed.ToString().PadRight(1).PadRight(3)));

            template[6] = template[6].Replace("<pweap      >", string.Format("¬W{0}§", mogwai.Equipment.PrimaryWeapon.Name.PadRight(13)));
            template[6] = template[6].Replace("<pdmg  >", "¬Y" + string.Format("{0}-{1}", mogwai.Equipment.PrimaryWeapon.MinDmg, mogwai.Equipment.PrimaryWeapon.MaxDmg).PadRight(8) + "§");
            if (mogwai.Equipment.SecondaryWeapon != null)
            {
                template[7] = template[7].Replace("<sweap      >", string.Format("¬W{0}§", mogwai.Equipment.SecondaryWeapon.Name.PadRight(13)));
                template[7] = template[7].Replace("<sdmg  >", "¬Y" + string.Format("{0}-{1}", mogwai.Equipment.SecondaryWeapon.MinDmg, mogwai.Equipment.SecondaryWeapon.MaxDmg).PadRight(8) + "§");
            }
            else
            {
                template[7] = template[7].Replace("<sweap      >", "none         ");
                template[7] = template[7].Replace("<sdmg  >", "        ");
            }

            if (mogwai.Equipment.Armor != null)
            {
                template[9] = template[9].Replace("<barm          >", string.Format("¬W{0}§", mogwai.Equipment.Armor.Name.PadRight(16)));
                template[9] = template[9].Replace("<baac>", "¬Y" + string.Format("{0}", mogwai.Equipment.Armor.ArmorBonus).PadRight(6) + "§");
            }
            else
            {
                template[9] = template[9].Replace("<barm          >", "none            ");
                template[9] = template[9].Replace("<sdmg  >", "        ");
            }

            template[14] = template[14].Replace("<address                         >", "¬G" + string.Format("{0,34}", mogwai.Key) + "§");
            template[14] = template[14].Replace("<alligen           >", "¬W" + string.Format("{0} {1}", mogwai.Stats.MapAllignment(), mogwai.MapGender).PadRight(20) + "§");

            return template;
        }
    }
}
