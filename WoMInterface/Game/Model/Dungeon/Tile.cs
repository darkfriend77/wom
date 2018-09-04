using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Model
{
    /// <summary>
    /// Represents the basic hex tile.
    /// </summary>
    public abstract class Tile
    {
        private int d = 0;
        private Tile p = null;

        public readonly Room Parent;
        public readonly Coordinate Coordinate;
        public readonly Wall[] Sides = new Wall[4];

        public int Height;

        protected Tile(Room parent, Coordinate coordinate)
        {
            Parent = parent;
            Coordinate = coordinate;
        }

        public abstract bool IsSolid { get; }

        //public abstract void Interact(Mogwai mog);

        public bool IsOccupied { get; set; }

        public Wall GetSide(Direction direction)
        {
            return Sides[(int) direction];
        }

        public Tile[] GetShortestPath(Tile destination)
        {
            var floor = Parent.Floor;
            var width = Parent.Width;
            var length = Parent.Length;

            var list = new List<Tile>(width * length);
            for (int i = 0; i < length; i++)
                for (int j = 0; j < width; j++)
                {
                    var tile = floor[i, j];
                    tile.p = null;
                    tile.d = int.MaxValue;
                    list.Add(tile);
                }

            var current = this;
            d = 0;
            while (list.Count > 0)
            {
                //for (int i = 0; i < 4; i++)
                //    if (!(current.Sides[i]?.IsBlocked ?? false)&&
                //        Parent.TryGetTile(Coordinate.Neighbour((Direction) i), out Tile t))
                //    {
                //        current = t;
                //        break;
                //    }

                var cd = int.MaxValue;
                foreach (var t in list)
                {
                    if (cd <= t.d) continue;
                    cd = t.d;
                    current = t;
                }

                list.Remove(current);
                if (current == destination)
                    break;

                foreach (var neighbour in Parent.GetNeighbours(current.Coordinate))
                {
                    var alt = current.d + 1;
                    if (alt < neighbour.d)
                    {
                        neighbour.d = alt;
                        neighbour.p = current;
                    }
                }
            }

            var s = new Stack<Tile>();
            if (destination.p != null || destination == this)
            {
                while (current != null)
                {
                    s.Push(current);
                    current = current.p;
                }

                return s.ToArray();
            }

            return new Tile[0];
        }
    }

    public class StoneTile : Tile
    {
        public override bool IsSolid => true;

        public StoneTile(Room parent, Coordinate coordinate) : base(parent, coordinate)
        {
        }
    }

    public class WaterTile : Tile
    {
        public override bool IsSolid => false;

        public WaterTile(Room parent, Coordinate coordinate) : base(parent, coordinate)
        {
        }
    }
}
