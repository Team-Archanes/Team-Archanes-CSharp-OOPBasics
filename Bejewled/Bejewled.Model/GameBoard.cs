using System.Threading;

namespace Bejewled.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bejewled.Model.Enums;
    using Bejewled.Model.Interfaces;

    public class GameBoard : IGameBoard
    {
        private const int NumberOfColumn = 8;

        private const int NumberOfRows = 8;

        private readonly List<ITile> firstTileList;

        private readonly ITile[,] gameBoard;

        private readonly List<ITile> secondTileList;

        private readonly TileGenerator tileGenerator;

        public GameBoard()
        {
            this.gameBoard = new ITile[NumberOfRows, NumberOfColumn];
            this.firstTileList = new List<ITile>();
            this.secondTileList = new List<ITile>();
            this.tileGenerator = new TileGenerator();
        }

        public void AvaliableMoves()
        {
            this.HorizontalCheck();
            this.CheckVertical();
        }


        private void CheckForMatch()
        {
            var allTileMatches = this.GetAllTileMatches();

            this.RemoveMatchedTiles(allTileMatches);

            this.MoveDownTiles();

            this.GenerateTilesOnEmptySpots();
        }

        // Checks if move is valid
        public void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile)
        {
            var differenceX = Math.Abs(firstClickedTile.Position.X - secondClickedTile.Position.X);
            var differenceY = Math.Abs(firstClickedTile.Position.Y - secondClickedTile.Position.Y);

            // todo: Needs to be implemented better
            if (differenceX + differenceY == 1)
            {
                this.SwapTiles(firstClickedTile, secondClickedTile);

                var allTileMatches = this.GetAllTileMatches();
                if (allTileMatches.Count == 0)
                {
                    this.SwapTiles(firstClickedTile, secondClickedTile);
                }
                else
                {
                    this.RemoveMatchedTiles(allTileMatches);

                    this.MoveDownTiles();

                    this.GenerateTilesOnEmptySpots();
                }
               // CheckForMatch();
                return;
                if (this.firstTileList.Contains(
                     this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y]))
                {
                    if (this.secondTileList[this.firstTileList.IndexOf(this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y])] == this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y])
                    {
                        this.SwapTiles(firstClickedTile, secondClickedTile);
                    }
                    /*IList<int> allIndexOf = new List<int>();
                    var index =
                        this.firstTileList.IndexOf(
                            this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y]);
                    
                    while (index != -1)
                    {
                        allIndexOf.Add(index);
                        index =
                            this.firstTileList.IndexOf(
                                this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y],
                                index + 1);
                    }
                    if (allIndexOf.Any(indexx => this.secondTileList.ElementAt(indexx).Equals(secondClickedTile)))
                    {
                        this.SwapTiles(firstClickedTile, secondClickedTile);
                    }*/
                }
                else if (this.secondTileList.Contains(
                      this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y]))
                {
                    if (this.firstTileList[this.secondTileList.IndexOf(this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y])] == this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y])
                    {
                        this.SwapTiles(firstClickedTile, secondClickedTile);
                    }
                    /*IList<int> allIndexOf = new List<int>();
                    var index =
                        this.secondTileList.IndexOf(
                            this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y]);

                    while (index != -1)
                    {
                        allIndexOf.Add(index);
                        index =
                            this.secondTileList.IndexOf(
                                firstClickedTile,
                                index + 1);
                    }
                   if (allIndexOf.Any(indexx => this.firstTileList.ElementAt(indexx).Equals(secondClickedTile)))
                    {
                        this.SwapTiles(firstClickedTile, secondClickedTile);
                    }*/

                }
            }
        }

        public IEnumerable<ITile> GetHint()
        {
            // List that stores all the matches we find 
            var matches = new List<ITile>();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                {
                    var currentTile = this.gameBoard[row, col];
                    var rightNeighbourTile = this.gameBoard[row, col + 1];
                    var downNeighbourTile = this.gameBoard[row + 1, col];

                    var twoInRowMatch = currentTile.TileType.Equals(rightNeighbourTile.TileType);
                    var twoInColumnMatch = currentTile.TileType.Equals(downNeighbourTile.TileType);

                    // Six horizontal cases for potential match
                    // 1. Check for: * & * & & * * * case
                    if (col > 1 && twoInRowMatch && currentTile.TileType.Equals(this.gameBoard[row, col - 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col - 2]);
                    }

                    // 2. Check for: * & & * & * * * case
                    if (col < NumberOfColumn - 4 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row, col + 3].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col + 3]);
                    }

                    // 3. Check for case:
                    // * & * * * *  
                    // * * & & * * 
                    if (col > 0 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col - 1]);
                    }

                    // 4. Check for case:
                    // * * * * & *  
                    // * * & & * * 
                    if (col < NumberOfColumn - 2 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col + 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col + 2]);
                    }

                    // 5. Check for case:
                    // * * & & * *  
                    // * & * * * *
                    if (row < NumberOfRows - 2 && col > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row + 1, col - 1]);
                    }

                    // 6. Check for case:
                    // * * & & * *  
                    // * * * * & * 
                    if (row < NumberOfRows - 2 && col < NumberOfColumn - 2 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 1, col + 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row + 1, col + 2]);
                    }

                    // Six vertical cases for potential match

                    // * * & * *  
                    // * * & * * 
                    // * * * * *  --- vertical case 1 
                    // * * & * *
                    if (row < NumberOfRows - 3 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 3, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 3, col]);
                    }

                    // * * & * * 
                    // * * * * *
                    // * * & * *  --- vertical case 2
                    // * * & * * 
                    if (row > 1 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 2, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 2, col]);
                    }

                    // * & * *
                    // * * & *   --- vertical case 3
                    // * * & *
                    if (row > 0 && col > 0 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col - 1]);
                    }

                    // * * & *
                    // * & * *   --- vertical case 4
                    // * & * * 
                    if (row > 0 && col < NumberOfColumn - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col + 1]);
                    }

                    // * * & *
                    // * * & *   --- vertical case 5
                    // * & * *
                    if (col > 0 && row < NumberOfRows - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 2, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col - 1]);
                    }

                    // * & * *
                    // * & * *   --- vertical case 6
                    // * * & *
                    if (row < NumberOfRows - 2 && col < NumberOfColumn - 1 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 2, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col + 1]);
                    }
                }
            }

            if (matches.Count >= 3)
            {
                var rand = new Random();
                yield return matches[rand.Next(matches.Count - 1)];
            }
            else
            {
                yield return null;
            }
        }

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

            this.AvaliableMoves();
            return this.GenerateNumericGameBoard();
        }



        private void CheckVertical()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0) - 2; row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1) - 1; column++)
                {
                    if (this.gameBoard[row, column].TileType == this.gameBoard[row + 1, column].TileType)
                    {
                        if (column - 1 >= 0)
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 2, column - 1].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 2, column - 1]);
                            }
                        }

                        if (column + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 2, column + 1].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 2, column + 1]);
                            }
                        }

                        if (row + 3 < this.gameBoard.GetLength(1))
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 3, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 3, column]);
                            }
                        }
                    }
                    else if (this.gameBoard[row, column].TileType == this.gameBoard[row + 2, column].TileType)
                    {
                        if (column + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column]);
                            }
                        }

                        if (column - 1 >= 0)
                        {
                            if (this.gameBoard[row + 1, column - 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column - 1]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column]);
                            }
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

        // On all empty spots of the gameboard we generate new tiles // ATanev
        private void GenerateTilesOnEmptySpots()
        {
            var rand = new Random();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                {
                    if (this.gameBoard[row, col].TileType == TileType.Empty)
                    {
                        //  var newTile = this.tileGenerator.CreateRandomTile(row, col);
                        // this.gameBoard[row, col] = newTile;
                        var tileNum = rand.Next(7);

                        switch (tileNum)
                        {
                            case 0:
                                this.gameBoard[row, col] = new Tile(TileType.Red, new TilePosition { X = row, Y = col });
                                break;
                            case 1:
                                this.gameBoard[row, col] = new Tile(TileType.Green, new TilePosition { X = row, Y = col });
                                break;
                            case 2:
                                this.gameBoard[row, col] = new Tile(TileType.Blue, new TilePosition { X = row, Y = col });
                                break;
                            case 3:
                                this.gameBoard[row, col] = new Tile(TileType.RainBow, new TilePosition { X = row, Y = col });
                                break;
                            case 4:
                                this.gameBoard[row, col] = new Tile(TileType.Purple, new TilePosition { X = row, Y = col });
                                break;
                            case 5:
                                this.gameBoard[row, col] = new Tile(TileType.White, new TilePosition { X = row, Y = col });
                                break;
                            case 6:
                                this.gameBoard[row, col] = new Tile(TileType.Yellow, new TilePosition { X = row, Y = col });
                                break;
                        }
                    }
                }
            }

            var allTileMatches = this.GetAllTileMatches();
            
            if (allTileMatches.Count != 0)
            {
                this.RemoveMatchedTiles(allTileMatches);

                this.MoveDownTiles();

                this.GenerateTilesOnEmptySpots();
            }
        }

        // Getting all horizontal and vertical matches on the GameBoard // ATanev
        private List<ITile[]> GetAllTileMatches()
        {
            var allTileMatches = new List<ITile[]>();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                // tempStackOfTiles.Push(this.gameBoard[row, 0]);
                tempStackOfTiles.Push(new Tile(gameBoard[row, 0].TileType, gameBoard[row, 0].Position));

                for (var col = 1; col < this.gameBoard.GetLength(1); col++)
                {
                    if (this.gameBoard[row, col].TileType.Equals(tempStackOfTiles.Peek().TileType))
                    {
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                        // tempStackOfTiles.Push(new Tile(gameBoard[row, col].TileType, new TilePosition(row, col)));
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }

                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                        //tempStackOfTiles.Push(new Tile(gameBoard[row, col].TileType, gameBoard[row, col].Position));
                    }
                }

                if (tempStackOfTiles.Count >= 3)
                {
                    allTileMatches.Add(tempStackOfTiles.ToArray());
                }
            }

            for (var col = 0; col < this.gameBoard.GetLength(1); col++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                tempStackOfTiles.Push(this.gameBoard[0, col]);

                for (var row = 1; row < this.gameBoard.GetLength(0); row++)
                {
                    if (this.gameBoard[row, col].TileType.Equals(tempStackOfTiles.Peek().TileType))
                    {
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }

                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                    }
                }

                if (tempStackOfTiles.Count >= 3)
                {
                    allTileMatches.Add(tempStackOfTiles.ToArray());
                }
            }

            return allTileMatches.Distinct().ToList();
        }

        private void HorizontalCheck()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0) - 1; row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1) - 2; column++)
                {
                    if (this.gameBoard[row, column].TileType == this.gameBoard[row, column + 1].TileType)
                    {
                        if (row - 1 >= 0)
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row - 1, column + 2].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row - 1, column + 2]);
                            }
                        }

                        if (row + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row + 1, column + 2].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column + 2]);
                            }
                        }

                        if (column + 3 < this.gameBoard.GetLength(1))
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row, column + 3].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row, column + 3]);
                            }
                        }
                    }
                    else if (this.gameBoard[row, column].TileType == this.gameBoard[row, column + 2].TileType)
                    {
                        if (row + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row, column + 1]);
                            }
                        }

                        if (row - 1 >= 0)
                        {
                            if (this.gameBoard[row - 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row - 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row, column + 1]);
                            }
                        }
                    }
                }
            }
        }

        // Moving everything down, if possible // ATanev
        private void MoveDownTiles()
        {
            // Moving tiles down until there are no changes // ATanev
            var thereIsChange = true;
            while (thereIsChange)
            {
                thereIsChange = false;

                for (var row = 0; row < this.gameBoard.GetLength(0) - 1; row++)
                {
                    for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                    {
                        if (this.gameBoard[row, col].TileType != TileType.Empty
                            && this.gameBoard[row + 1, col].TileType == TileType.Empty)
                        {
                            this.SwapTiles(this.gameBoard[row, col], this.gameBoard[row + 1, col]);
                            thereIsChange = true;
                        }
                    }
                }
            }
        }

        // Removing matched Tiles by replacing them with an Empty one // ATanev
        private void RemoveMatchedTiles(List<ITile[]> matchesToRemove)
        {
            GlobalScore.globalScore += matchesToRemove.Count;

            foreach (var match in matchesToRemove)
            {
                foreach (var tile in match)
                {
                    if (this.gameBoard[tile.Position.X, tile.Position.Y].TileType != TileType.Empty)
                    {
                        this.gameBoard[tile.Position.X, tile.Position.Y] = new Tile(
                            TileType.Empty,
                            this.gameBoard[tile.Position.X, tile.Position.Y].Position);
                    }
                }
            }
        }

        private void SwapTiles(ITile firstClickedTile, ITile secondClickedTile)
        {
            this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y] = secondClickedTile;
            this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y] = firstClickedTile;

            int x = firstClickedTile.Position.X;
            int y = firstClickedTile.Position.Y;

            firstClickedTile.Position.X = secondClickedTile.Position.X;
            firstClickedTile.Position.Y = secondClickedTile.Position.Y;

            secondClickedTile.Position.X = x;
            secondClickedTile.Position.Y = y;






            /* bool isVertical = false;
             int matchesLenght = this.CheckForHorizontalMatch(firstClickedTile);
             if (matchesLenght > 3)
             {
                 this.RemoveTiles(isVertical, secondClickedTile);
             }
             else
             {
                 matchesLenght = this.CheckForVerticalMatch(firstClickedTile);
             }
             if (matchesLenght>3)
             {
                 isVertical = true;
                 this.RemoveTiles(isVertical, firstClickedTile);
             }*/
            this.CheckForMatch();
        }

        private void RemoveTiles(bool isVertical, ITile firstClickedTile)
        {
            if (isVertical)
            {

            }
        }

        private int CheckForVerticalMatch(ITile firstClickedTile)
        {
            int matchesLenght = 1;
            for (int i = firstClickedTile.Position.Y; i < this.gameBoard.GetLength(0); i++)
            {
                if (this.gameBoard[i, firstClickedTile.Position.Y].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            for (int i = firstClickedTile.Position.Y - 1; i >= 0; i--)
            {
                if (this.gameBoard[i, firstClickedTile.Position.Y].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            return matchesLenght;
        }

        private int CheckForHorizontalMatch(ITile firstClickedTile)
        {
            int matchesLenght = 1;
            for (int i = firstClickedTile.Position.X; i < this.gameBoard.GetLength(1); i++)
            {
                if (this.gameBoard[firstClickedTile.Position.X, i].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            for (int i = firstClickedTile.Position.X - 1; i >= 0; i--)
            {
                if (this.gameBoard[firstClickedTile.Position.X, i].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            return matchesLenght;
        }
    }
}