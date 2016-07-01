namespace Bejewled.Model.Interfaces
{
    using Bejewled.Model.Enums;

    public interface ITile
    {
        TilePosition Position { get; set; }

        TileType TileType { get; }
    }
}