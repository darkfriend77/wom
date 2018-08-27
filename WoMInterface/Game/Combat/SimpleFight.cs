using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;
using WoMInterface.Game.Random;
using WoMInterface.Tool;

namespace WoMInterface.Game.Combat
{
    public class SimpleFight
    {
        private int maxRounds;

        private List<Fighter> inititiveOrder;

        private List<Monster> monsters;

        private int currentRound;

        public SimpleFight(List<Monster> monsters, int maxRounds = 50)
        {
            this.monsters = monsters;
            this.maxRounds = maxRounds;

            inititiveOrder = new List<Fighter>();

            //    new Fighter(mogwai) {
            //        IsHero = true,
            //        InititativeValue = mogwai.InitiativeRoll(mogwaiDice),
            //        Dice = mogwaiDice,
            //        Enemies = new List<Entity> { monster } },

            //    new Fighter(monster) {
            //        InititativeValue = monster.InitiativeRoll(monsterDice),
            //        Dice = monsterDice,
            //        Enemies = new List<Entity> { mogwai }
            //    }
            //};

            //inititiveOrder.OrderBy(s => s.InititativeValue).ThenBy(s => s.Entity.Dexterity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Create(Mogwai mogwai, Shift shift)
        {
            int m = 1;
            foreach (var monster in monsters)
            {
                Dice dice = new Dice(shift, m);
                monster.Create(dice);
                inititiveOrder.Add(
                    new Fighter(monster)
                    {
                        InititativeValue = monster.InitiativeRoll(dice),
                        Dice = dice,
                        Enemies = new List<Entity> { mogwai }
                    });
            };

            inititiveOrder.Add(
                new Fighter(mogwai)
                {
                    IsHero = true,
                    InititativeValue = mogwai.InitiativeRoll(shift.MogwaiDice),
                    Dice = shift.MogwaiDice,
                    Enemies = monsters.Select(p => p as Entity).ToList()
                });

            inititiveOrder.OrderBy(s => s.InititativeValue).ThenBy(s => s.Entity.Dexterity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            var hero = inititiveOrder.Where(p => p.IsHero).First();
            var sheep = inititiveOrder.Where(p => !p.IsHero).First();
            StringHelpers.Msg($"¬YSimpleFight§: ¬C{hero.Entity.Name}§[¬Y{hero.InititativeValue}§] vs. ¬C{sheep.Entity.Name}§[¬Y{sheep.InititativeValue}§]¬");
            // let's start the rounds ...
            Fighter winner = null;
            for (currentRound = 1; currentRound < maxRounds && winner == null; currentRound++)
            {
                int sec = (currentRound - 1) * 6;
                StringHelpers.Msg($"[R¬G{currentRound.ToString("00")}§|¬a{(sec/60).ToString("00")}§:¬a{(sec%60).ToString("00")}§]¬");

                for (int i = 0; i < inititiveOrder.Count; i++)
                {
                    Fighter combatant = inititiveOrder[i];
                    if (combatant.Entity.CurrentHitPoints < 1)
                    {
                        continue;
                    }

                    Entity target = combatant.Enemies.Where(p => p.CurrentHitPoints > 0).FirstOrDefault();

                    int attack = combatant.Entity.AttackRoll(combatant.Dice);

                    StringHelpers.Msg($" + ¬g{i.ToString("00")}§: ¬C{combatant.Entity.Name}§ attacks ¬C{target.Name}§ with ¬c{combatant.Entity.Equipment.PrimaryWeapon.Name}§ roll ¬Y{attack}§[¬a{target.ArmorClass}§]:");

                    if (attack > target.ArmorClass)
                    {
                        int damage = combatant.Entity.DamageRoll(combatant.Dice);
                        StringHelpers.Msg($" ¬Gsucced§ ¬R{damage}§ damage!¬");
                        target.CurrentHitPoints -= damage;
                    }
                    else
                    {
                        StringHelpers.Msg($" ¬Rfailed§!¬");
                    }

                    if (!combatant.Enemies.Exists(p => p.CurrentHitPoints > 0))
                    {
                        winner = combatant;
                        break;
                    }
                }
            }

            if (winner != null)
            {
                StringHelpers.Msg($"¬CCombat is over! The winner is {winner.Entity.Name}§¬");

                if (winner.IsHero)
                {
                    Reward(winner.Entity as Mogwai, winner.Enemies);
                    Loot(winner.Entity as Mogwai, winner.Enemies);
                    return true;
                }

                return false;
            }
            else
            {
                 StringHelpers.Msg($"¬CNo winner, no loser, this fight was a draw!§");
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwai"></param>
        /// <param name="enemies"></param>
        private void Reward(Mogwai mogwai, List<Entity> enemies)
        {
            CommandLine.InGameMessage($"Rewarding ", ConsoleColor.Cyan);
            CommandLine.InGameMessage($"{mogwai.Name}");
            CommandLine.InGameMessage($" for the victory.", ConsoleColor.Cyan, true);

            // award experience for each killed enemy
            enemies.ForEach(p =>
            {
                if (p is Monster)
                {
                    int expReward = ((Monster)p).Experience;
                    mogwai.AddExp(expReward);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwai"></param>
        /// <param name="enemies"></param>
        internal void Loot(Mogwai mogwai, List<Entity> enemies)
        {
            // award experience for each killed enemy
            enemies.ForEach(p =>
            {
                if (p is Monster)
                {
                    Treasure treasure = ((Monster)p).Treasure;
                    CommandLine.InGameMessage($"Looting the ");
                    CommandLine.InGameMessage($"{p.Name}", ConsoleColor.DarkGray);
                    CommandLine.InGameMessage($" he has ");
                    if (treasure != null)
                    {
                        CommandLine.InGameMessage($"a Treasure", ConsoleColor.Green);
                    }
                    else
                    {
                        CommandLine.InGameMessage($"no Treasure", ConsoleColor.Red);
                    }

                    CommandLine.InGameMessage($"!", true);
                }
            });
        }

    }
}
