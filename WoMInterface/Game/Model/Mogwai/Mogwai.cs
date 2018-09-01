using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WoMInterface.Game.Generator;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Interaction;
using WoMInterface.Tool;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Model
{
    public partial class Mogwai : Entity
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int blockHeight;

        private Shift currentShift;

        private List<Shift> Shifts { get; }

        public MogwaiState MogwaiState { get; set; }

        public Dictionary<int, Shift> LevelShifts { get; }

        public int Pointer { get; private set; }

        public string Key { get; }

        public Coat Coat { get; }

        public Body Body { get; }

        public Stats Stats { get; }

        public Experience Experience { get; set; }

        public double Exp { get; private set; } = 0;

        public int CurrentLevel { get; private set; } = 1;

        public double XpToLevelUp => CurrentLevel * 1000;

        public Adventure Adventure { get; set; }

        public override Dice Dice => currentShift.MogwaiDice;

        public static GameLog History = new GameLog();

        public Mogwai(string key, List<Shift> shifts)
        {
            Key = key;
            Shifts = shifts;
            LevelShifts = new Dictionary<int, Shift>() { { CurrentLevel, currentShift } };

            var creationShift = shifts[0];

            blockHeight = creationShift.Height;
            Pointer = creationShift.Height;

            // create appearance           
            var hexValue = new HexValue(creationShift);
            Name = NameGen.GenerateName(hexValue);
            Body = new Body(hexValue);
            Coat = new Coat(hexValue);
            Stats = new Stats(hexValue);

            // create abilities
            int[] rollEvent = new int[] {4,6,3};
            Gender = creationShift.MogwaiDice.Roll(2, -1);
            Strength = creationShift.MogwaiDice.Roll(rollEvent);
            Dexterity = creationShift.MogwaiDice.Roll(rollEvent);
            Constitution = creationShift.MogwaiDice.Roll(rollEvent);
            Inteligence = creationShift.MogwaiDice.Roll(rollEvent);
            Wisdom = creationShift.MogwaiDice.Roll(rollEvent);
            Charisma = creationShift.MogwaiDice.Roll(rollEvent);

            BaseSpeed = 30;

            NaturalArmor = 0;
            SizeType = SizeType.MEDIUM;

            BaseAttackBonus = new int[] { 0 };

            // create experience
            Experience = new Experience(creationShift);

            // add simple rapier as weapon
            Equipment.PrimaryWeapon = Weapons.Rapier;

            // add simple rapier as weapon
            Equipment.Armor = Armors.StuddedLeather;

            HitPointDice = 8;
            CurrentHitPoints = MaxHitPoints;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockHeight"></param>
        public void Evolve(int blockHeight = 0)
        {
            int oldPointer = Pointer;

            foreach(var shift in Shifts)
            {
                // set current shift to the actual shift we process
                currentShift = shift;

                // only evolve to the target block height
                if (blockHeight != 0 && shift.Height > blockHeight)
                {
                    break;
                }

                // only proccess shifts that aren't proccessed before ...
                if (shift.Height <= Pointer)
                {
                    continue;
                }

                // setting pointer to the actual shift
                Pointer = shift.Height;

                // first we always calculated current lazy experience
                double lazyExp = Experience.GetExp(CurrentLevel, shift);
                if (lazyExp > 0)
                {
                    AddExp(Experience.GetExp(CurrentLevel, shift));
                }

                // we go for the adventure if there is one up
                if (Adventure != null && Adventure.IsActive)
                {
                    Adventure.NextStep(this, shift);
                    continue;
                }

                Adventure = null;


                if (!shift.IsSmallShift)
                {
                    switch (shift.Interaction.InteractionType)
                    {
                        case InteractionType.ADVENTURE:
                            Adventure = AdventureGenerator.Create(shift, (AdventureAction) shift.Interaction);
                            break;
                        case InteractionType.LEVELING:
                            Console.WriteLine("Received a leveling action!");
                            break;
                        default:
                            break;
                    }
                }

                // lazy health regeneration
                if (MogwaiState == MogwaiState.NONE)
                {
                    Heal(shift.IsSmallShift ? 2 * CurrentLevel : CurrentLevel, HealType.REST);
                }
            }

            // no more shifts to proccess
            currentShift = null;

            StringHelpers.InfoMsg($"Evolved {Name} from ¬G{oldPointer}§  to ¬G{Pointer}§!¬");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="monster"></param>
        public override void AddExp(double exp, Monster monster = null)
        {
            if (monster == null)
            {
                StringHelpers.InfoMsg($"You just earned ¬G+{exp}§ experience!¬");
            }
            else
            {
                StringHelpers.InfoMsg($"The ¬C{monster.Name}§ gave you ¬G+{exp}§!¬");
            }

            Exp += exp;

            if (Exp >= XpToLevelUp)
            {
                CurrentLevel += 1;
                LevelShifts.Add(CurrentLevel, currentShift);
                LevelUp(currentShift);
            }
        }

        /// <summary>
        /// Passive level up, includes for example hit point roles.
        /// </summary>
        /// <param name="shift"></param>
        private void LevelUp(Shift shift)
        {
            StringHelpers.InfoMsg($"¬YYou're mogwai suddenly feels an ancient power around him.§¬");
            StringHelpers.InfoMsg($"¬YCongratulations he just made the§ ¬G{CurrentLevel}§ ¬Yth level!§¬");

            // hit points roll
            HitPointLevelRolls.Add(shift.MogwaiDice.Roll(HitPointDice));
            
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