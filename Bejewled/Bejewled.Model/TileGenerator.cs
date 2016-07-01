namespace Bejewled.Model
{
    using System;

    using Bejewled.Model.Enums;
    using Bejewled.Model.Interfaces;

    public class TileGenerator
    {
        public ITile CreateRandomTile(int row, int column)
        {
            var rand = new Random();
            var tileNum = rand.Next(7);
            Tile tile = null;
            switch (tileNum)
            {
                case 0:
                    tile = new Tile(TileType.Red, new TilePosition { X = row, Y = column });
                    break;
                case 1:
                    tile = new Tile(TileType.Green, new TilePosition { X = row, Y = column });
                    break;
                case 2:
                    tile = new Tile(TileType.Blue, new TilePosition { X = row, Y = column });
                    break;
                case 3:
                    tile = new Tile(TileType.RainBow, new TilePosition { X = row, Y = column });
                    break;
                case 4:
                    tile = new Tile(TileType.Purple, new TilePosition { X = row, Y = column });
                    break;
                case 5:
                    tile = new Tile(TileType.White, new TilePosition { X = row, Y = column });
                    break;
                case 6:
                    tile = new Tile(TileType.Yellow, new TilePosition { X = row, Y = column });
                    break;
            }

            return tile;
        }
    }
}