using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Tool;
using static WoMInterface.Game.AttributBuilder;
using static WoMInterface.Game.Attribute;

namespace WoMInterface.Game
{
    public sealed class StatBuilder
    {
        private readonly string name;
        private int[] creationRollEvent = new int[] { 4,6,3};
        private string description;
        private EvolutionPattern evoPat = Attribute.EvolutionPattern.NONE;

        private StatBuilder(string name) { this.name = name; }

        public static StatBuilder Create(string name)
        {
            return new StatBuilder(name);
        }

        public StatBuilder CreationRollEvent(int[] rollEvent)
        {
            this.creationRollEvent = rollEvent;
            return this;
        }

        public StatBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        public StatBuilder EvolutionPattern(EvolutionPattern evoPat)
        {
            this.evoPat = evoPat;
            return this;
        }

        public Stat Build()
        {
            return new Stat(name, creationRollEvent, description, evoPat);
        }
    }

    public class Stat
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get; }
        public int[] CreationRollEvent { get; }
        public string Description { get; }
        public EvolutionPattern EvoPat { get; set; }

        public bool Valid => value > -1;

        private double value = -1;

        public Stat(string name, int[] creationRollEvent, string description, EvolutionPattern evoPat)
        {
            Name = name;
            CreationRollEvent = creationRollEvent;
            Description = description;
            EvoPat = evoPat;
        }

        public int GetValue()
        {
            return (int)value;
        }

        public bool CreateValue(Dice dice)
        {
            value = dice.Roll(CreationRollEvent);
            return true;
        }

    }
}
