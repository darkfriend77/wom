using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Combat;

namespace WoMInterface.Game.Model
{
    public abstract class Room
    {
        public Dungeon Parent;

        public List<Corridor> OutgoingDoors;

        public List<Corridor> IncomingDoors;

        public int Level { get; protected set; }

        public int Width { get; protected set; }

        public int Length { get; protected set; }

        public Tile[][] Floor { get; protected set; }

        protected readonly bool IsHidden;

        protected readonly bool Trapped;    // TODO

        protected readonly bool Blocked;    // needs door breaching

        // Ideas
        private int layer = 1;              // don't know what to do with this
        private int Illuminance;            // affects initiative?
        // random encounters with special environments (e.g. a barrel full of blackpowder)
        // ornaments, furnitures

        //public RoomType Type { get; private set; }

        public abstract bool Enter();

        public virtual void Initialise(Mogwai mogwai)
        {
            
        }

        // create pointers
        public static void Connect(Room a, Room b)
        {
            var corridor = new Corridor(a, b);
            a.OutgoingDoors.Add(corridor);
            b.IncomingDoors.Add(corridor);
        }
    }

    // not have to be inherited class of Room 
    public class Corridor : Room
    {
        private int[] _corners;     // indices of corner tiles 

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

        public override void Initialise(Mogwai mogwai)
        {
            throw new NotImplementedException();
        }
    }

    public class MonsterRoom : Room
    {
        private readonly List<Monster> _monsters = new List<Monster>();
        private SimpleCombat _fight;

        public override void Initialise(Mogwai mogwai)
        {
            // first, generate appropriate monsters
            CreateMonsters(mogwai);

            // second, generate a fight instance
            _fight = new SimpleCombat(_monsters);
            _fight.Create(mogwai, Parent.CreationShift);
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

            return _fight.Run();
        }

        private void CreateMonsters(Mogwai mogwai)
        {
            // not implemented
            _monsters.Add(Monsters.Rat);

            // dispose the created monsters to floor
        }
    }
}
