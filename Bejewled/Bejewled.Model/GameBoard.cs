namespace Bejewled.Model
{
    using System;
    using System.Collections.Generic;

    using Bejewled.Model.Enums;
    using Bejewled.Model.EventArgs;
    using Bejewled.Model.Interfaces;

    public class GameBoard : IGameBoard
    {
        private const int NumberOfColumn = 8;

        private const int NumberOfRows = 8;

        private readonly ITile[,] gameBoard;

        private readonly IHint hint;

        private readonly TileGenerator tileGenerator;

        private ITile possibleTile;

        private IScore score;

        public GameBoard()
        {
            this.gameBoard = new ITile[NumberOfRows, NumberOfColumn];
            this.tileGenerator = new TileGenerator();
            this.hint = new Hint();
            this.score = new Score();
        }

        public void FirstTileClicked(ITile firstClickedTile)
        {
            if ((int)this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType < 7)
            {
                this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType =
                    (TileType)(int)firstClickedTile.TileType + 8;
            }
        }

        public event EventHandler OnGameOver;

        public ITile GetHint()
        {
            this.possibleTile = this.hint.GetPossibleTile(this.gameBoard);
            this.GameOver();
            this.FirstTileClicked(this.possibleTile);
            return this.possibleTile;
        }

        private void GameOver()
        {
            if (this.possibleTile == null)
            {
                if (this.OnGameOver != null)
                {
                    this.OnGameOver(this, System.EventArgs.Empty);
                    this.score = new Score();
                }
            }
        }

        public void NormalizeFocusedTile(ITile firstClickedTile)
        {
            this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType =
                this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType - 8;
        }

        public void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile)
        {
            /*if (this.hint.GetPossibleTile(this.gameBoard) == null)
            {
               this.GameOver();
            }*/
            if (this.possibleTile != null)
            {
                this.NormalizeFocusedTile(this.possibleTile);
                this.possibleTile = null;
            }
            if (this.OnTileFocused != null)
            {
                this.OnTileFocused(this, System.EventArgs.Empty);
            }
            if ((int)this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType > 7)
            {
                this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType =
                    this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y].TileType - 8;
            }

            if (Math.Abs(firstClickedTile.Position.X - secondClickedTile.Position.X)
                + Math.Abs(firstClickedTile.Position.Y - secondClickedTile.Position.Y) == 1)
            {
                var tempTile = this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y];
                this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y] =
                    this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y];
                this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y] = tempTile;
                var matches = this.Matches(this.gameBoard);
                if (matches > 0)
                {
                    this.ValidMoveMade(firstClickedTile, secondClickedTile);
                }
                else
                {
                    tempTile = this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y];
                    this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y] =
                        this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y];
                    this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y] = tempTile;
                }
            }
        }

        public event EventHandler<TileEventArgs> OnValidMove;

        public event EventHandler<ScoreEventArgs> OnTileRemoved;

        public event EventHandler OnTileFocused;

        public int[,] InitializeGameBoard()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1); column++)
                {
                    var tile = this.tileGenerator.CreateRandomTile(row, column);
                    if (row < 2 && column >= 2)
                    {
                        while (tile.TileType.Equals(this.gameBoard[row, column - 1].TileType)
                               && tile.TileType.Equals(this.gameBoard[row, column - 2].TileType))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    if (row >= 2 && column < 2)
                    {
                        while (tile.TileType.Equals(this.gameBoard[row - 1, column].TileType)
                               && tile.TileType.Equals(this.gameBoard[row - 2, column].TileType))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    if (row >= 2 && column >= 2)
                    {
                        while ((tile.TileType.Equals(this.gameBoard[row - 1, column].TileType)
                                && tile.TileType.Equals(this.gameBoard[row - 2, column].TileType))
                               || (tile.TileType.Equals(this.gameBoard[row, column - 1].TileType)
                                   && tile.TileType.Equals(this.gameBoard[row, column - 2].TileType)))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    this.gameBoard[row, column] = tile;
                }
            }
            return this.GenerateNumericGameBoard();
        }

        public void RemoveMatchedTiles()
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (this.gameBoard[i, j].TileType == TileType.Explosive)
                    {
                        var index = i;
                        while (index > 0)
                        {
                            this.gameBoard[index, j].TileType = this.gameBoard[index - 1, j].TileType;
                            index--;
                            this.score.IncreaseScore();
                            this.TileRemoved();
                            this.CheckForGameOver();
                        }
                        this.gameBoard[index, j] = this.tileGenerator.CreateRandomTile(index, j);
                        this.TileRemoved();
                        if (this.Matches(this.gameBoard) > 0)
                        {
                            this.RemoveMatchedTiles();
                        }
                    }
                }
            }
        }

        public int[,] GenerateNumericGameBoard()
        {
            var otherGameBoard = new int[NumberOfRows, NumberOfColumn];
            for (var i = 0; i < this.gameBoard.GetLength(0); i++)
            {
                for (var j = 0; j < this.gameBoard.GetLength(1); j++)
                {
                    otherGameBoard[i, j] = (int)this.gameBoard[i, j].TileType;
                }
            }
            return otherGameBoard;
        }

        public void CheckForGameOver()
        {
            this.possibleTile = this.hint.GetPossibleTile(this.gameBoard);
            this.GameOver();
            this.possibleTile = null;
        }

        private void ValidMoveMade(ITile firstClickedTile, ITile secondClickedTile)
        {
            if (this.OnValidMove != null)
            {
                var firstTileTypeIndex = (int)firstClickedTile.TileType;
                var firstTileX = firstClickedTile.Position.X;
                var firstTileY = firstClickedTile.Position.Y;
                var secondTileTypeIndex = (int)secondClickedTile.TileType;
                var secondTileX = secondClickedTile.Position.X;
                var secondTileY = secondClickedTile.Position.Y;
                this.OnValidMove(
                    this,
                    new TileEventArgs(
                        firstTileTypeIndex,
                        firstTileX,
                        firstTileY,
                        secondTileTypeIndex,
                        secondTileX,
                        secondTileY));
            }
        }

        private int Matches(ITile[,] tempGameBoard)
        {
            var matched = 0;
            var tiles = new List<ITile>();
            for (var i = 0; i < tempGameBoard.GetLength(0); i++)
            {
                for (var j = 0; j < tempGameBoard.GetLength(1) - 2; j++)
                {
                    if (tempGameBoard[i, j].TileType == tempGameBoard[i, j + 1].TileType
                        && tempGameBoard[i, j + 1].TileType == tempGameBoard[i, j + 2].TileType)
                    {
                        tiles.Add(tempGameBoard[i, j]);
                        tiles.Add(tempGameBoard[i, j + 1]);
                        tiles.Add(tempGameBoard[i, j + 2]);
                        var index = j + 2;
                        while (index < tempGameBoard.GetLength(1) - 2
                               && tempGameBoard[i, j].TileType == tempGameBoard[i, index].TileType)
                        {
                            tiles.Add(tempGameBoard[i, index]);
                            index++;
                        }
                        matched++;
                    }
                }
            }
            for (var i = 0; i < tempGameBoard.GetLength(0) - 2; i++)
            {
                for (var j = 0; j < tempGameBoard.GetLength(1); j++)
                {
                    if (tempGameBoard[i, j].TileType == tempGameBoard[i + 1, j].TileType
                        && tempGameBoard[i + 1, j].TileType == tempGameBoard[i + 2, j].TileType)
                    {
                        tiles.Add(tempGameBoard[i, j]);
                        tiles.Add(tempGameBoard[i + 1, j]);
                        tiles.Add(tempGameBoard[i + 2, j]);
                        var index = i + 2;

                        while (index < tempGameBoard.GetLength(0) - 2
                               && tempGameBoard[i, j].TileType == tempGameBoard[index, j].TileType)
                        {
                            tiles.Add(tempGameBoard[index, j]);
                            index++;
                        }
                        matched++;
                    }
                }
            }
            foreach (var tile in tiles)
            {
                tile.TileType = TileType.Explosive;
            }
            return matched;
        }

        private void TileRemoved()
        {
            if (this.OnTileRemoved != null)
            {
                this.OnTileRemoved(this, new ScoreEventArgs(this.score));
            }
        }
    }
}