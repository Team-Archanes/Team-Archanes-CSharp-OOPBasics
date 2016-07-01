namespace Bejewled.Model.Interfaces
{
    public interface IGameBoard
    {
        int[,] InitializeGameBoard();

        void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile);

        int[,] GenerateNumericGameBoard();
    }
}