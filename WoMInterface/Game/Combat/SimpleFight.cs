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
            CommandLine.InGameMessage($"SimpleFight: {hero.Entity.Name}[{hero.InititativeValue},{hero.Entity.Dexterity}] vs. {sheep.Entity.Name}[{sheep.InititativeValue},{sheep.Entity.Dexterity}]", ConsoleColor.Cyan, true);
            // let's start the rounds ...
            Fighter winner = null;
            for (currentRound = 1; currentRound < maxRounds && winner == null; currentRound++)
            {
                CommandLine.InGameMessage($"ROUND [{currentRound}] --------", ConsoleColor.Green, true);

                foreach (var combatant in inititiveOrder)
                {
                    if (combatant.Entity.CurrentHitPoints < 1)
                    {
                        continue;
                    }

                    Entity target = combatant.Enemies.Where(p => p.CurrentHitPoints > 0).FirstOrDefault();

                    CommandLine.InGameMessage($"-> TURN");
                    CommandLine.InGameMessage($" - [{combatant.Entity.Name}, HP:");
                    CommandLine.InGameMessage($"{combatant.Entity.CurrentHitPoints}", combatant.Entity.CurrentHitPoints > 0 ? ConsoleColor.Green : ConsoleColor.Red);
                    CommandLine.InGameMessage($"]", true);

                    int attack = combatant.Entity.AttackRoll(combatant.Dice);
                    CommandLine.InGameMessage($"   attacking with [");
                    CommandLine.InGameMessage($"{combatant.Entity.Equipment.PrimaryWeapon.Name}", ConsoleColor.Gray);
                    CommandLine.InGameMessage($"] target[{target.Name}, AC:");
                    CommandLine.InGameMessage($"{target.ArmorClass}", ConsoleColor.Yellow);
                    CommandLine.InGameMessage($"] with ");
                    CommandLine.InGameMessage($"{attack}", ConsoleColor.Yellow);
                    CommandLine.InGameMessage($" roll!", true);

                    if (attack > target.ArmorClass)
                    {
                        int damage = combatant.Entity.DamageRoll(combatant.Dice);
                        CommandLine.InGameMessage($"   successful ", ConsoleColor.Yellow);
                        CommandLine.InGameMessage($"hitting target[{target.Name}, HP:");
                        CommandLine.InGameMessage($"{target.CurrentHitPoints}", target.CurrentHitPoints > 0 ? ConsoleColor.Green : ConsoleColor.Red);
                        CommandLine.InGameMessage($"] for ");
                        CommandLine.InGameMessage($"-{damage}", ConsoleColor.Red);
                        CommandLine.InGameMessage($" damage!", true);
                        target.CurrentHitPoints -= damage;
                    }
                    else
                    {
                        CommandLine.InGameMessage($"   failed ", ConsoleColor.Red);
                        CommandLine.InGameMessage($"to attack target[{target.Name}]!", true);
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
                CommandLine.InGameMessage($"Combat is over! The winner is {winner.Entity.Name}", ConsoleColor.Cyan, true);

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
                CommandLine.InGameMessage($"No winner, no loser, this fight was a draw!", ConsoleColor.Cyan, true);
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
                    CommandLine.InGameMessage($"The ");
                    CommandLine.InGameMessage($"{p.Name}", ConsoleColor.DarkGray);
                    CommandLine.InGameMessage($" gave you ");
                    CommandLine.InGameMessage($"+{expReward}", ConsoleColor.Green);
                    CommandLine.InGameMessage($"!", ConsoleColor.Cyan, true);
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
