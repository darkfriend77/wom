using System;
using System.Collections.Generic;

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

        public static Coordinate Round(double x, double y, double z)
        {
            int rx = (int) Math.Round(x);
            int ry = (int) Math.Round(y);
            int rz = (int) Math.Round(z);

            double xDiff = Math.Abs(rx - x);
            double yDiff = Math.Abs(ry - y);
            double zDiff = Math.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (yDiff > zDiff)
                ry = -rx - rz;
            else
                rz = -rx - ry;
           
            return new Coordinate(rx, ry, rz);
        }

        public static IEnumerable<Coordinate> GetLine(Coordinate a, Coordinate b)
        {
            double Lerp(int x, int y, double t) => x + (y - x) * t;

            int d = Distance(a, b);

            for (int i = 0 ; i < d; i++)
            {
                double t = (double)i / d;
                yield return Round(Lerp(a.X, a.X, t), Lerp(a.Y, b.Y, t), Lerp(a.Z, b.Z, t));
            }
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