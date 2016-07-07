namespace Bejewled.Model.Interfaces
{
    using System;

    using Bejewled.Model.EventArgs;

    public interface IGameBoard
    {
        int[,] InitializeGameBoard();

        void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile);

        event EventHandler<TileEventArgs> OnValidMove;

        event EventHandler<ScoreEventArgs> OnTileRemoved;

        event EventHandler OnTileFocused;

        event EventHandler OnGameOver;

        ITile GetHint();

        void NormalizeFocusedTile(ITile firstClickedTile);

        void CheckForGameOver();

        void FirstTileClicked(ITile firstClickedTile);

        void RemoveMatchedTiles();

        int[,] GenerateNumericGameBoard();
    }
}