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
        private Tile _currentTile;

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

        public Tile CurrentTile
        {
            get => _currentTile;
            set
            {
                _currentTile = value;
                value.IsOccupied = true;
            }
        }

        public bool TryMoveTo(Direction direction)
        {
            if (CurrentTile.Sides[(int)direction]?.IsBlocked ?? false)
                return false;

            if (CurrentRoom.TryGetTile(CurrentTile.Coordinate + Coordinate.Direction(direction), out Tile destination))
            {
                CurrentTile = destination;
                return true;
            }

            return false;
        }

        // simple implementation of Dijkstra's algorithm
        public void GetShortestPath(Tile destination)
        {
            
        }
    }
}
