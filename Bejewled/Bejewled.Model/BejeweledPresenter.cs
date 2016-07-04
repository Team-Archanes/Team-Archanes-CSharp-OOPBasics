namespace Bejewled.Model
{
    using Bejewled.Model.Enums;
    using Bejewled.Model.EventArgs;
    using Bejewled.Model.Interfaces;

    public class BejeweledPresenter
    {
        private readonly IGameBoard gameBoard;

        private readonly IView view;

        public BejeweledPresenter(IView view, IGameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
            this.view = view;
            this.view.OnLoad += this.GameLoaded;
            this.view.OnTileClicked += this.TileClicked;
            this.gameBoard.OnValidMove += this.OnValidMove;
        }

        void OnValidMove(object sender, TileEventArgs tilesInfo)
        {
            this.view.Tiles[tilesInfo.FirstTileX, tilesInfo.FirstTileY] = tilesInfo.SecondTileTypeIndex;
            this.view.Tiles[tilesInfo.SecondTileX, tilesInfo.SecondTileY] = tilesInfo.FirstTileTypeIndex;
            this.view.DrawGameBoard();
        }

        private void GameLoaded(object sender, System.EventArgs eventArgs)
        {
            this.view.Tiles = this.gameBoard.InitializeGameBoard();
        }

        private void TileClicked(object sender, TileEventArgs tileEventArgs)
        {
            var firstTilePosition = new TilePosition { X = tileEventArgs.FirstTileX, Y = tileEventArgs.FirstTileY };
            var firstTile = new Tile((TileType)tileEventArgs.FirstTileTypeIndex, firstTilePosition);
            var secondTilePosition = new TilePosition { X = tileEventArgs.SecondTileX, Y = tileEventArgs.SecondTileY };
            var secondTile = new Tile((TileType)tileEventArgs.SecondTileTypeIndex, secondTilePosition);
            this.gameBoard.CheckForValidMove(firstTile, secondTile);
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
        }
    }
}