namespace Bejewled.Model.EventArgs
{
    using System;

    public class TileEventArgs : EventArgs
    {
        public TileEventArgs(
            int firsttileTypeIndex,
            int firstTileX,
            int firstTileY,
            int secondTileTypeIndex,
            int secondTileX,
            int secondTileY)
            : this(firsttileTypeIndex, firstTileX, firstTileY)
        {
            this.SecondTileX = secondTileX;
            this.SecondTileY = secondTileY;
            this.SecondTileTypeIndex = secondTileTypeIndex;
        }

        public TileEventArgs(int firsttileTypeIndex, int firstTileX, int firstTileY)
        {
            this.FirstTileX = firstTileX;
            this.FirstTileY = firstTileY;
            this.FirstTileTypeIndex = firsttileTypeIndex;
        }

        public int FirstTileTypeIndex { get; set; }

        public int FirstTileX { get; set; }

        public int FirstTileY { get; set; }

        public int SecondTileTypeIndex { get; set; }

        public int SecondTileX { get; set; }

        public int SecondTileY { get; set; }
    }
}