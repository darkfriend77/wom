using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WoMInterface.Tool;

namespace WoMInterface.Game
{
    public class Mogwai
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<Shift> shifts;

        private readonly int blockHeight;

        private HexValue hexValue;

        public string Key { get; }

        public string Name { get; }

        public enum GenderType { MALE, FEMALE }

        public Coat Coat { get; }

        public Body Body { get; }

        public Stats Stats { get; }

        public Abilities Abilities { get; }

        public Experience Experience { get; }

        public Mogwai(string key, List<Shift> shifts)
        {
            Key = key;
            this.shifts = shifts;

            var creationShift = shifts[0];

            blockHeight = creationShift.Height;
           
            Experience = new Experience(creationShift);

            hexValue = new HexValue(creationShift);

            Name = NameGen.GenerateName(hexValue);
            Body = new Body(hexValue);
            Coat = new Coat(hexValue);
            Stats = new Stats(hexValue);

            Abilities = new Abilities(creationShift);

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
            Shift shift = shifts[0];

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