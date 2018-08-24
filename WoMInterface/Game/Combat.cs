using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    class Combatant
    {
        public bool IsHero { get; set; } = false;
        public Entity Entity { get; }
        public int InititativeValue { get; set; }
        public Dice Dice { get; set; }
        public List<Entity> Enemies { get; set; }
        
        public Combatant(Entity entity)
        {
            Entity = entity;
        }

    }
    class Combat
    {
        private int maxRounds;
        private List<Combatant> InititiveOrder;

        public Combat(Mogwai mogwai, Dice mogwaiDice, Monster monster, Dice monsterDice, int maxRounds = 100)
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

        public void Start(bool verbose = true)
        {
            Console.WriteLine("Start Combat");
            var hero = InititiveOrder.Where(p => p.IsHero).First();
            Console.WriteLine($"{hero.Entity.Name}");

        }
    }
}
