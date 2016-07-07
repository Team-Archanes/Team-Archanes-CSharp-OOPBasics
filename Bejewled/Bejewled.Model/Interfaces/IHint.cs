namespace Bejewled.Model.Interfaces
{
    using System.Collections.Generic;

    public interface IHint
    {
        IEnumerable<ITile> GetAllPossibleTiles(ITile[,] gameboard);

        ITile GetPossibleTile(ITile[,] gameboard);
    }
}