using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Interaction;
using WoMInterface.Tool;

namespace WoMInterface.Game.Model
{
    public class Mogwai : Entity
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int blockHeight;

        private List<Shift> Shifts { get; }

        public MogwaiState MogwaiState { get; set; }

        public Dictionary<int, Shift> LevelShifts { get; }

        public string Key { get; }

        public Coat Coat { get; }

        public Body Body { get; }

        public Stats Stats { get; }

        public Experience Experience { get; set; }

        public double Exp { get; private set; } = 0;

        public int CurrentLevel { get; private set; } = 1;

        public double XpToLevelUp => CurrentLevel * 1000;

        public Mogwai(string key, List<Shift> shifts)
        {
            Key = key;
            Shifts = shifts;
            LevelShifts = new Dictionary<int, Shift>();

            var creationShift = shifts[0];

            blockHeight = creationShift.Height;

            // create appearance           
            var hexValue = new HexValue(creationShift);
            Name = NameGen.GenerateName(hexValue);
            Body = new Body(hexValue);
            Coat = new Coat(hexValue);
            Stats = new Stats(hexValue);

            // create abilities
            int[] rollEvent = new int[] {4,6,3};
            Gender = creationShift.Dice.Roll(2, -1);
            Strength = creationShift.Dice.Roll(rollEvent);
            Dexterity = creationShift.Dice.Roll(rollEvent);
            Constitution = creationShift.Dice.Roll(rollEvent);
            Inteligence = creationShift.Dice.Roll(rollEvent);
            Wisdom = creationShift.Dice.Roll(rollEvent);
            Charisma = creationShift.Dice.Roll(rollEvent);

            NaturalArmor = 0;
            SizeType = SizeType.MEDIUM;

            BaseAttackBonus = 1;

            // create experience
            Experience = new Experience(creationShift);

            // add simple hand as weapon
            Equipment.BaseWeapon = new Fist();

            HitPointDice = 8;
            CurrentHitPoints = MaxHitPoints;

            // evolve
            Evolve(shifts);
        }

        private void Evolve(List<Shift> shifts)
        {
            foreach(var shift in shifts.Skip(1))
            {
                // first we always calculated current lazy experience
                AddExp(Experience.GetExp(CurrentLevel, shift), shift);

                // lazy health regeneration
                if (MogwaiState == MogwaiState.NONE)
                {
                    int naturalHealing = shift.IsSmallShift ? 2 * CurrentLevel : CurrentLevel;
                    CurrentHitPoints += naturalHealing;
                    if (CurrentHitPoints > MaxHitPoints)
                    {
                        CurrentHitPoints = MaxHitPoints;
                    }
                }
            }
        }

        public void AddExp(double exp, Shift shift)
        {
            Exp += exp;

            if (Exp >= XpToLevelUp)
            {
                CurrentLevel += 1;
                LevelShifts.Add(CurrentLevel, shift);
                LevelUp(shift);
            }
        }

        /// <summary>
        /// Passive level up, includes for example hit point roles.
        /// </summary>
        /// <param name="shift"></param>
        private void LevelUp(Shift shift)
        {
            // hit points roll
            HitPointLevelRolls.Add(shift.Dice.Roll(HitPointDice));
            
            // leveling up will heal you to max hitpoints
            CurrentHitPoints = MaxHitPoints;
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