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
        protected class Map
        {
            private readonly int _sideLength;
            private readonly Tile[] _data;

            private readonly Dictionary<Coordinate, Tile> data;

            public Map(Room room, int sideLength)
            {
                _sideLength = sideLength;
                // simplest parallelogram map
                _data = new Tile[sideLength * sideLength];
                int d = sideLength / 2;
                int i = 0;
                for (int z = -d; z < d; z++)
                    for (int x = -d; x < d; x++)
                        _data[i++] = new StoneTile(room, new Coordinate(x, 1 - x - z, z));
            }

            public Tile this[int index]
            {
                get => _data[index];
                set => _data[index] = value;
            }

            public bool TryGetTile(Coordinate coordinate, out Tile tile)
            {
                int d = _sideLength / 2;
                if (Math.Abs(coordinate.X) > d || Math.Abs(coordinate.Z) > d || Math.Abs(coordinate.Y) >= _sideLength)
                {
                    tile = null;
                    return false;
                }

                tile = _data[5 * (coordinate.Z + d) + coordinate.X + d];
                return true;
            }
        }

        public Dungeon Parent;

        public List<Corridor> OutgoingDoors;

        public List<Corridor> IncomingDoors;

        public int Level { get; protected set; }

        public int MaxWidth { get; protected set; }

        //public Tile[,] Floor { get; protected set; }
        protected Map _map;

        protected readonly bool IsHidden;

        protected readonly bool Trapped;    // TODO

        protected readonly bool Blocked;    // needs door breaching

        // Ideas
        private int layer = 1;              // don't know what to do with this
        private int Illuminance;            // affects initiative?
        // random encounters with special environments (e.g. a barrel full of blackpowder)
        // ornaments, furnitures

        //public RoomType Type { get; private set; }

        protected Room(Dungeon parent)
        {
            Parent = parent;
        }

        public bool TryGetTile(Coordinate coord, out Tile tile)
        {
            return _map.TryGetTile(coord, out tile);
        }

        public IEnumerable<Tile> GetNeighbours(Coordinate coordinate)
        {
            if (!TryGetTile(coordinate, out Tile tile)) yield break;
            for (int i = 0; i < 6; i++)
                if (TryGetTile(tile.Coordinate + Coordinate.Direction(i), out Tile neighbour))
                    yield return neighbour;
        }

        public abstract bool Enter();

        public abstract void Initialise(Mogwai mogwai);

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

        public Corridor(Room entrance, Room exit) : base(entrance.Parent)
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
        protected SimpleCombat _fight;


        public MonsterRoom(Dungeon parent) : base(parent)
        {
        }

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

    public class SimpleRoom : MonsterRoom
    {
        private readonly Dungeoneer _mog;
        private readonly Dungeoneer[] _mobs;

        public SimpleRoom(Dungeon parent, Mogwai mogwai) : base(parent)
        {
            const int maxWidth = 5;

            //var floor = Floor;

            _map = new Map(this, maxWidth);

            // Initialise Dungeoneers
            _mog = new Dungeoneer(_fight.Heroes[0]);
            _mog.CurrentTile = _map[0];

            var monsters = _fight.Monsters;
            _mobs = monsters.Select(p => new Dungeoneer(p)).ToArray();
            for (int i = maxWidth * maxWidth - 1, j = _mobs.Length; j > 0; i++, j--)
            {
                _mobs[j].CurrentTile = _map[i];
            }
        }

        public override bool Enter()
        {
            return false;
        }

    }
}
