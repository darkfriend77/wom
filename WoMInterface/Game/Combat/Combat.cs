using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Model;
using WoMInterface.Game.Random;
using WoMInterface.Tool;

namespace WoMInterface.Game.Combat
{
    public class Combat
    {
        private int maxRounds;
        private List<Combatant> InititiveOrder;
        private int currentRound;

        public Combat(Mogwai mogwai, Dice mogwaiDice, Monster monster, Dice monsterDice, int maxRounds = 50)
        {
            this.maxRounds = maxRounds;

            InititiveOrder = new List<Combatant>() {
                new Combatant(mogwai) {
                    IsHero = true,
                    InititativeValue = mogwai.InitiativeRoll(mogwaiDice),
                    Dice = mogwaiDice,
                    Enemies = new List<Entity> { monster } },
                new Combatant(monster) {
                    InititativeValue = monster.InitiativeRoll(monsterDice),
                    Dice = monsterDice,
                    Enemies = new List<Entity> { mogwai }
                }
            };

            InititiveOrder.OrderBy(s => s.InititativeValue).ThenBy(s => s.Entity.Dexterity);
        }

        public void Start()
        {
            var hero = InititiveOrder.Where(p => p.IsHero).First();
            var sheep = InititiveOrder.Where(p => !p.IsHero).First();
            CommandLine.ColorWriteLine($"Shave combat: {hero.Entity.Name}[{hero.InititativeValue},{hero.Entity.Dexterity}] vs. {sheep.Entity.Name}[{sheep.InititativeValue},{sheep.Entity.Dexterity}]", ConsoleColor.Cyan);
            // let's start the rounds ...
            Combatant winner = null;
            for (currentRound = 1; currentRound < maxRounds && winner == null; currentRound++)
            {
                CommandLine.ColorWriteLine($"ROUND [{currentRound}] --------", ConsoleColor.Green);

                foreach (var combatant in InititiveOrder)
                {
                    if (combatant.Entity.CurrentHitPoints < 1)
                    {
                        continue;
                    }

                    Entity target = combatant.Enemies.Where(p => p.CurrentHitPoints > 0).FirstOrDefault();

                    CommandLine.ColorWrite($"-> TURN");
                    CommandLine.ColorWrite($" - [{combatant.Entity.Name}, HP:");
                    CommandLine.ColorWrite($"{combatant.Entity.CurrentHitPoints}", combatant.Entity.CurrentHitPoints > 0 ? ConsoleColor.Green : ConsoleColor.Red);
                    CommandLine.ColorWrite($"]");
                    Console.WriteLine();
                    int attack = combatant.Entity.AttackRoll(combatant.Dice);
                    CommandLine.ColorWrite($"   attacking target[{target.Name}, AC:");
                    CommandLine.ColorWrite($"{target.ArmorClass}", ConsoleColor.Yellow);
                    CommandLine.ColorWrite($"] with ");
                    CommandLine.ColorWrite($"{attack}", ConsoleColor.Yellow);
                    CommandLine.ColorWrite($" roll!");
                    Console.WriteLine();
                    if (attack > target.ArmorClass)
                    {
                        int damage = combatant.Entity.DamageRoll(combatant.Dice);
                        CommandLine.ColorWrite($"   successful ", ConsoleColor.Yellow);
                        CommandLine.ColorWrite($"hitting target[{target.Name}, HP:");
                        CommandLine.ColorWrite($"{target.CurrentHitPoints}", target.CurrentHitPoints > 0 ? ConsoleColor.Green : ConsoleColor.Red);
                        CommandLine.ColorWrite($"] for ");
                        CommandLine.ColorWrite($"-{damage}", ConsoleColor.Red);
                        CommandLine.ColorWrite($" damage!");
                        Console.WriteLine();
                        target.CurrentHitPoints -= damage;
                    }
                    else
                    {
                        CommandLine.ColorWrite($"   failed ", ConsoleColor.Red);
                        CommandLine.ColorWrite($"to attack target[{target.Name}]!");
                        Console.WriteLine();
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
                CommandLine.ColorWriteLine($"Combat is over! The winner is {winner.Entity.Name}", ConsoleColor.Cyan);
            }
            else
            {
                CommandLine.ColorWriteLine($"No winner, no loser, this fight was a draw!", ConsoleColor.Cyan);
            }

        }

        public void Finish()
        {

        }
    }
}
