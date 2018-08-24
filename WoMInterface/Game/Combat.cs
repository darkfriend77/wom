using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game
{
    class Combatant
    {
        public Entity Entity { get; }
        public int InititativeValue { get; set; }
        public Dice Dice { get; set; }
        
        public Combatant(Entity entity)
        {
            Entity = entity;
        }

    }
    class Combat
    {
        private List<Combatant> InititiveOrder;

        public Combat(Mogwai mogwai, Dice mogwaiDice, Monster monster, Dice monsterDice)
        {
            InititiveOrder = new List<Combatant>() {
                new Combatant(mogwai) { InititativeValue = mogwai.InitiativeRoll(mogwaiDice), Dice = mogwaiDice },
                new Combatant(monster) { InititativeValue = monster.InitiativeRoll(monsterDice), Dice = monsterDice }
            };

            InititiveOrder.OrderBy(s => s.InititativeValue).ThenBy(s => s.Entity.Dexterity);

            Start();
        }

        private void Start()
        {
            
        }
    }
}
