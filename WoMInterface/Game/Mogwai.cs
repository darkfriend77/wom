using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WoMInterface.Tool;

namespace WoMInterface.Game
{
    public class Mogwai : Entity
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int blockHeight;

        private List<Shift> Shifts { get; }

        public string Key { get; }

        public Coat Coat { get; }

        public Body Body { get; }

        public Stats Stats { get; }

        public Mogwai(string key, List<Shift> shifts)
        {
            Key = key;
            Shifts = shifts;

            var creationShift = shifts[0];

            blockHeight = creationShift.Height;

            // create appearance           
            var hexValue = new HexValue(creationShift);
            Name = NameGen.GenerateName(hexValue);
            Body = new Body(hexValue);
            Coat = new Coat(hexValue);
            Stats = new Stats(hexValue);

            // create abilities
            Dice dice = new Dice(creationShift);
            int[] rollEvent = new int[] {4,6,3};
            Gender = dice.Roll(2, -1);
            Strength = dice.Roll(rollEvent);
            Dexterity = dice.Roll(rollEvent);
            Constitution = dice.Roll(rollEvent);
            Inteligence = dice.Roll(rollEvent);
            Wisdom = dice.Roll(rollEvent);
            Charisma = dice.Roll(rollEvent);

            // create experience
            Experience = new Experience(creationShift);

            // evolve
            Evolve(shifts);
        }

        private void Evolve(List<Shift> shifts)
        {
            foreach(var shift in shifts.Skip(1))
            {
                // first we always calculated current lazy experience
                Experience.LazyExperience(shift);
                
            }
        }

        public void Print()
        {
            Shift shift = Shifts[0];

            Console.WriteLine("*** Mogwai Nascency Transaction ***");
            Console.WriteLine($"- Time: {shift.Time}");
            Console.WriteLine($"- Index: {shift.BkIndex}");
            Console.WriteLine($"- Amount: {shift.Amount}");
            Console.WriteLine($"- Height: {shift.Height}");
            Console.WriteLine($"- AdHex: {shift.AdHex}");
            Console.WriteLine($"- BlHex: {shift.BkHex}");
            Console.WriteLine($"- TxHex: {shift.TxHex}");

            Console.WriteLine();
            Console.WriteLine("*** Mogwai Attributes ***");
            Console.WriteLine("- Body:");
            Body.All.ForEach(p => Console.WriteLine($"{p.Name}: {p.GetValue()} [{p.MinRange}-{p.Creation-1}] Var:{p.MaxRange}-->{p.Valid}"));
            Console.WriteLine("- Coat:");
            Coat.All.ForEach(p => Console.WriteLine($"{p.Name}: {p.GetValue()} [{p.MinRange}-{p.Creation-1}] Var:{p.MaxRange}-->{p.Valid}"));
            Console.WriteLine("- Stats:");
            Stats.All.ForEach(p => Console.WriteLine($"{p.Name}: {p.GetValue()} [{p.MinRange}-{p.Creation-1}] Var:{p.MaxRange}-->{p.Valid}"));
            Experience.Print();
        }
    }
}