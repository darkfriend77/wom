using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Combat;
using WoMInterface.Game.Generator;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Model
{
    public class Dungeon : Adventure
    {
        public int Level { get; protected set; }

        public Room Entrance { get; protected set; }

        //public bool[,] Blueprint { get; protected set; }

        public Dungeon(Mogwai mogwai, Shift shift)
        {
            // create dungeon structure;
        }

        public override void NextStep(Mogwai mogwai, Shift shift)
        {
            if (AdventureState == AdventureState.CREATION)
            {
                Entrance.Initialise(mogwai, shift);
                AdventureState = AdventureState.RUNNING;
            }


        }

        public bool Enter()
        {
            return Entrance.Enter();
        }
        
        /// <summary>
        /// Generates rooms and corridors
        /// </summary>
        public virtual void GenerateRooms(Shift shift)
        {
           
        }
    }

    /// <summary>
    /// Dungeon with one monster room.
    /// Should be removed later.
    /// </summary>
    public class SimpleDungeon : Dungeon
    {
        // generated with a single shift here.
        public SimpleDungeon() : base(null, null)
        {

        }

        public override void NextStep(Mogwai mogwai, Shift shift)
        {
            if (AdventureState == AdventureState.CREATION)
            {
                GenerateRooms(shift);
            }

            if (!Enter())
            {
                AdventureState = AdventureState.FAILED;
                return;
            }

            AdventureState = AdventureState.COMPLETED;
        }

        public override void GenerateRooms(Shift shift)
        {
            // these lines of codes will be generalised

            int n = 1;

            bool[,] blueprint = new bool[n, n];

            // create random connected graph from the blueprint.
            // here, it is obviously { { false } }

            // assign random rooms with probabilities
            // here, the only room is deterministically a monster room
            var rooms = new Room[n];
            for (int i = 0; i < n; i++)
                rooms[i] = new MonsterRoom();

            // specify pointers
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)    // only concern upper diagonal of the matrix
                    if (blueprint[i, j])
                        Room.Connect(rooms[i], rooms[j]);

            // set entrance (or maybe we can create a specific class for entrance)
            Entrance = rooms[0];
        }
    }

    public abstract class Room
    {
        public Dungeon Parent;

        public List<Corridor> OutgoingDoors;

        public List<Corridor> IncomingDoors;

        public int Level { get; private set; }

        public int Size { get; private set; }

        public Dice RoomDice { get; private set; }


        protected readonly bool Trapped;    // TODO

        protected readonly bool Blocked;   // needs door breaching

        // Ideas
        private int layer = 1;  // don't know what to do with this
        private int Illuminance; // affects initiative?
        // random encounters with special environments (e.g. a barrel full of blackpowder)
        // ornaments, furnitures

        //public RoomType Type { get; private set; }

        public Room()
        {

        }

        protected Room(Shift shift)
        {
            var dice = new Dice(shift);
            //Level = mogwai.CurrentLevel + difficulty modifier


            RoomDice = dice;
        }

        // create pointers
        public static void Connect(Room a, Room b)
        {
            var corridor = new Corridor(a, b);
            a.OutgoingDoors.Add(corridor);
            b.IncomingDoors.Add(corridor);
        }

        public abstract bool Enter();

        public abstract void Initialise(Mogwai mogwai, Shift shift);
    }

    // not have to be inherited class of Room 
    public class Corridor : Room
    {
        public Room Entrance { get; }
        public Room Exit { get; }

        public Corridor(Room entrance, Room exit)
        {
            Entrance = entrance;
            Exit = exit;
        }

        public override bool Enter()
        {
            throw new NotImplementedException();
        }

        public override void Initialise(Mogwai mogwai, Shift shift)
        {
            throw new NotImplementedException();
        }
    }

    public class MonsterRoom : Room
    {
        private readonly List<Monster> monsters = new List<Monster>();
        private SimpleFight fight;

        public override void Initialise(Mogwai mogwai, Shift shift)
        {
            // first, generate appropriate monsters
            CreateMonsters(mogwai, shift);

            // second, generate a fight instance
            fight = new SimpleFight(monsters);
            fight.Create(mogwai, shift);
        }

        public override bool Enter()
        {
            if (Blocked)
            {
                //  need breaching
            }

            if (Trapped)
            {
                // TODO: Trap interaction
            }

            // calculate initiate here maybe?

            return fight.Run();
        }

        private void CreateMonsters(Mogwai mogwai, Shift shift)
        {
            // not implemented
            monsters.Add(Animals.Rat);
        }
    }
}
