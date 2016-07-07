namespace Bejewled.Model
{
    using System.Threading;

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
            this.view.OnSecondTileClicked += this.SecondTileClicked;
            this.gameBoard.OnValidMove += this.OnValidMove;
            this.view.OnExplosionFinished += this.OnExplosionFinished;
            this.gameBoard.OnTileRemoved += this.OnTileRemoved;
            this.gameBoard.OnTileFocused += this.OnTileFocused;
            this.view.OnFirstTileClicked += this.OnFirstTileClicked;
            this.view.OnHintClicked += this.OnHintClicked;
            this.gameBoard.OnGameOver += this.OnGameOver;
        }

        private void OnGameOver(object sender, System.EventArgs e)
        {
            this.view.DisplayGameEndMessage();
            this.view.Score = "0";
        }

        private void OnFirstTileClicked(object sender, TileEventArgs tileEventArgs)
        {
            var firstTilePosition = new TilePosition { X = tileEventArgs.FirstTileX, Y = tileEventArgs.FirstTileY };
            var firstTile = new Tile((TileType)tileEventArgs.FirstTileTypeIndex, firstTilePosition);
            this.gameBoard.FirstTileClicked(firstTile);
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
        }

        private void OnHintClicked(object sender, System.EventArgs e)
        {
            this.gameBoard.GetHint();
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
        }

        private void OnTileFocused(object sender, System.EventArgs e)
        {
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
        }

        private void OnTileRemoved(object sender, ScoreEventArgs scoreEventArgs)
        {
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
            this.view.Score = scoreEventArgs.GainedScore;
            //this.gameBoard.CheckForGameOver();
        }

        private void OnExplosionFinished(object sender, System.EventArgs e)
        {
            Thread.Sleep(50);
            this.gameBoard.RemoveMatchedTiles();
            this.view.Tiles = this.gameBoard.GenerateNumericGameBoard();
            this.view.DrawGameBoard();
        }

        private void OnValidMove(object sender, TileEventArgs tilesInfo)
        {
            this.view.Tiles[tilesInfo.FirstTileX, tilesInfo.FirstTileY] = tilesInfo.SecondTileTypeIndex;
            this.view.Tiles[tilesInfo.SecondTileX, tilesInfo.SecondTileY] = tilesInfo.FirstTileTypeIndex;
            this.view.DrawGameBoard();
        }

        private void GameLoaded(object sender, System.EventArgs eventArgs)
        {
            this.view.Tiles = this.gameBoard.InitializeGameBoard();
            this.view.Score = "0";
        }

        private void SecondTileClicked(object sender, TileEventArgs tileEventArgs)
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