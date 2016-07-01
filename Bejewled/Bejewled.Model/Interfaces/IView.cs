namespace Bejewled.Model.Interfaces
{
    using System;

    using Bejewled.Model.EventArgs;

    public interface IView
    {
        int[,] Tiles { get; set; }

        event EventHandler OnLoad;

        event EventHandler<TileEventArgs> OnTileClicked;

        void DrawGameBoard();
    }
}