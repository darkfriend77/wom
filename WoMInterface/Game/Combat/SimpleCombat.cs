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
    public class SimpleCombat
    {
        private int maxRounds;

        private List<Combatant> inititiveOrder;

        private List<Monster> monsters;

        private int currentRound;

        public List<Entity> Heroes => inititiveOrder.Where(p => p.IsHero).Select(p => p.Entity).ToList();

        public List<Entity> Monsters => inititiveOrder.Where(p => !p.IsHero).Select(p => p.Entity).ToList();

        public SimpleCombat(List<Monster> monsters, int maxRounds = 50)
        {
            this.monsters = monsters;
            this.maxRounds = maxRounds;

            inititiveOrder = new List<Combatant>();
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
                Dice dice = new Dice(shift, m++);
                monster.Initialize(dice);
                inititiveOrder.Add(
                    new Combatant(monster)
                    {
                        InititativeValue = monster.InitiativeRoll(dice),
                        Enemies = new List<Entity> { mogwai }
                    });
            };

            inititiveOrder.Add(
                new Combatant(mogwai)
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
            Mogwai.History.Add(LogType.EVNT, $"¬YSimpleCombat§ [¬C{heros}§] vs. [¬C{monsters}§]¬");

            Combatant winner = null;

            // let's start the rounds ...
            for (currentRound = 1; currentRound < maxRounds && winner == null; currentRound++)
            {
                int sec = (currentRound - 1) * 6;
                Mogwai.History.Add(LogType.EVNT, $"[ROUND ¬G{currentRound.ToString("00")}§] time: ¬a{(sec / 60).ToString("00")}§m:¬a{(sec % 60).ToString("00")}§s Monsters: {Monsters.Count} ({string.Join(",",Monsters.Select(p => p.Name))})¬");

                for (int turn = 0; turn < inititiveOrder.Count; turn++)
                {
                    Combatant combatant = inititiveOrder[turn];

                    // dead targets can't attack any more
                    if (combatant.Entity.CurrentHitPoints < 0)
                    {
                        continue;
                    }

                    Entity target = combatant.Enemies.Where(p => p.CurrentHitPoints > -1).FirstOrDefault();

                    // attack
                    combatant.Entity.Attack(turn, target);

                    if (target.CurrentHitPoints < 0 && target is Monster)
                    {
                        Monster killedMonster = ((Monster)target);
                        int expReward = killedMonster.Experience / Heroes.Count;
                        Heroes.ForEach(p => p.AddExp(expReward, killedMonster));
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
                Mogwai.History.Add(LogType.EVNT, $"¬YSimpleCombat§ Fight is over! The winner is ¬C{winner.Entity.Name}§¬");

                if (winner.IsHero)
                {
                    Loot(Heroes, Monsters);
                    return true;
                }

                return false;
            }
            else
            {
                Mogwai.History.Add(LogType.EVNT, $"¬YSimpleCombat§ No winner, no loser, this fight was a draw!");
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mogwai"></param>
        /// <param name="enemies"></param>
        internal void Loot(List<Entity> mogwais, List<Entity> enemies)
        {
            // award experience for each killed enemy
            enemies.ForEach(p =>
            {
                if (p is Monster)
                {
                    Treasure treasure = ((Monster)p).Treasure;
                    string treasureStr = treasure != null ? "¬Ga Treasure§" : "¬Rno Treasure§";
                    Mogwai.History.Add(LogType.EVNT, $"¬YLooting§ the ¬C{p.Name}§ he has {treasureStr}!¬");
                }
            });
        }

    }
}
