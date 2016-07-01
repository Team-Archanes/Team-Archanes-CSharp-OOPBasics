namespace Bejewled.Model
{
    using System;

    using Bejewled.Model.Enums;
    using Bejewled.Model.Interfaces;

    public class Tile : ITile, IEquatable<ITile>
    {
        public Tile(TileType tileType, TilePosition position)
        {
            this.TileType = tileType;
            this.Position = position;
        }

        public TilePosition Position { get; set; }

        public TileType TileType { get; private set; }

        public bool Equals(ITile other)
        {
            if (this.TileType == other.TileType)
            {
                if (this.Position.X == other.Position.X)
                {
                    if (this.Position.Y == other.Position.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}