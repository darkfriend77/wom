using System;

namespace WoMInterface.Game.Model
{
    /// <summary>
    /// Cubic coordinate implementation of hex grid
    /// </summary>
    public struct Coordinate : IEquatable<Coordinate>
    {
        // flat-top style directions
        private static readonly Coordinate[] _directions = new Coordinate[]
        {
            new Coordinate(0, 1, -1), new Coordinate(1, 0, -1), new Coordinate(1, -1, 0),
            new Coordinate(0, -1, 1), new Coordinate(-1, 0, 1), new Coordinate(-1, 1, 0)
        };

        //public int RoomNumber;
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Coordinate(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new ArgumentOutOfRangeException("The sum of the coordinates must equal to 0.");

            X = x;
            Y = y;
            Z = z;
        }

        public static int Length(Coordinate coordinate)
        {
            return (Math.Abs(coordinate.X) + Math.Abs(coordinate.Y) + Math.Abs(coordinate.Z)) / 2;
        }

        public static int Distance(Coordinate a, Coordinate b)
        {
            return Length(a - b);
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Coordinate operator *(Coordinate a, int k)
        {
            return new Coordinate(a.X * k, a.Y * k, a.Z * k);
        }

        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2)
        {
            return coordinate1.Equals(coordinate2);
        }

        public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2)
        {
            return !(coordinate1 == coordinate2);
        }

        public static Coordinate Direction(int direction)
        {
            if (direction > 5 || direction < 0)
                throw new ArgumentOutOfRangeException();

            return _directions[direction];
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate coordinate && Equals(coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }
}