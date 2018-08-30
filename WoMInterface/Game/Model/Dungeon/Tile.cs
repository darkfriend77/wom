using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMInterface.Game.Model
{
    public abstract class Tile
    {
        public Room Parent;

        public int Height;
        public readonly Wall[] Sides = new Wall[6];

        public abstract bool IsSolid { get; }

        public Wall NorthWestSide
        {
            get => Sides[0];
            set => Sides[0] = value;
        }
        public Wall NorthSide
        {
            get => Sides[1];
            set => Sides[1] = value;
        }
        public Wall NorthEastSide
        {
            get => Sides[2];
            set => Sides[2] = value;
        }
        public Wall SouthEastSide
        {
            get => Sides[3];
            set => Sides[3] = value;
        }
        public Wall SouthSide
        {
            get => Sides[4];
            set => Sides[4] = value;
        }
        public Wall SouthWestSide
        {
            get => Sides[5];
            set => Sides[5] = value;
        }
    }

    public class StoneTile : Tile
    {
        public override bool IsSolid => true;
    }

    public class WaterTile : Tile
    {
        public override bool IsSolid => false;
    }
}
