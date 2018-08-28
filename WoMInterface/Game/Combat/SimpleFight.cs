using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
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
                monster.Initialize(dice);
                inititiveOrder.Add(
                    new Fighter(monster)
                    {
                        InititativeValue = monster.InitiativeRoll(dice),
                        Enemies = new List<Entity> { mogwai }
                    });
            };

            inititiveOrder.Add(
                new Fighter(mogwai)
                {
                    IsHero = true,
                    InititativeValue = mogwai.InitiativeRoll(mogwai.Dice),
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
            var heros = string.Join(",", inititiveOrder.Where(p => p.IsHero).Select(p => $"{p.Entity.Name} [{p.InititativeValue}]").ToArray());
            var monsters = string.Join(",", inititiveOrder.Where(p => !p.IsHero).Select(p => $"{p.Entity.Name} [{p.InititativeValue}]").ToArray());
            StringHelpers.Msg($"¬YSimpleFight§: [¬C{heros}§] vs. [¬C{monsters}§]¬");

            Fighter winner = null;

            // let's start the rounds ...
            for (currentRound = 1; currentRound < maxRounds && winner == null; currentRound++)
            {
                int sec = (currentRound - 1) * 6;
                StringHelpers.Msg($"[R¬G{currentRound.ToString("00")}§|¬a{(sec / 60).ToString("00")}§:¬a{(sec % 60).ToString("00")}§]¬");

                for (int turn = 0; turn < inititiveOrder.Count; turn++)
                {
                    Fighter combatant = inititiveOrder[turn];

                    if (combatant.Entity.CurrentHitPoints < 1)
                    {
                        continue;
                    }

                    Entity target = combatant.Enemies.Where(p => p.CurrentHitPoints > -1).FirstOrDefault();

                    // attack
                    combatant.Entity.Attack(turn, target);

                    if (target.CurrentHitPoints < 1)
                    {

                    }

                    if (!combatant.Enemies.Exists(p => p.CurrentHitPoints > -1))
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
