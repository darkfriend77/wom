using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Combat;

namespace WoMInterface.Game.Model
{
    /// <summary>
    /// Wrapper class for an combatant.
    /// Should be removed and merged after
    /// </summary>
    public class Dungeoneer
    {
        public readonly Entity Entity;
        public readonly bool IsHero;

        public int MoveRange { get; set; } = 1;
        public int AttackRange { get; set; } = 1;

        public Dungeoneer(Entity entity)
        {
            Entity = entity;
        }

        public Dungeon CurrentDungeon => CurrentTile.Parent.Parent;
        public Room CurrentRoom => CurrentTile.Parent;
        public Tile CurrentTile { get; set; }

        public bool TryMoveTo(int direction)
        {
            if (CurrentTile.Sides[direction]?.IsBlocked ?? false)
                return false;

            if (CurrentRoom.TryGetTile(CurrentTile.Coordinate + Coordinate.Direction(direction), out Tile destination))
            {
                CurrentTile = destination;
                return true;
            }

            return false;
        }
    }
}
