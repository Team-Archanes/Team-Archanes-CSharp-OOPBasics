namespace Bejewled.Model.Interfaces
{
    using System;

    using Bejewled.Model.EventArgs;

    public interface IGameBoard
    {
        int[,] InitializeGameBoard();

        void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile);

        event EventHandler<TileEventArgs> OnValidMove;

        int[,] GenerateNumericGameBoard();
    }
}