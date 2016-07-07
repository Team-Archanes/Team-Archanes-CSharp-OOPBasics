namespace Bejewled.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bejewled.Model.Interfaces;

    public class Hint : IHint
    {
        private readonly Random random;

        public Hint()
        {
            this.random = new Random();
        }

        public IEnumerable<ITile> GetAllPossibleTiles(ITile[,] gameboard)
        {
            // List that stores all the matches we find 
            var matches = new List<ITile>();

            for (var row = 0; row < gameboard.GetLength(0) - 1; row++)
            {
                for (var col = 0; col < gameboard.GetLength(1) - 1; col++)
                {
                    var currentTile = gameboard[row, col];
                    var rightNeighbourTile = gameboard[row, col + 1];
                    var downNeighbourTile = gameboard[row + 1, col];

                    var twoInRowMatch = currentTile.TileType.Equals(rightNeighbourTile.TileType);
                    var twoInColumnMatch = currentTile.TileType.Equals(downNeighbourTile.TileType);

                    // Six horizontal cases for potential match
                    // 1. Check for: * & * & & * * * case
                    if (col > 1 && twoInRowMatch && currentTile.TileType.Equals(gameboard[row, col - 2].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row, col - 2]);
                    }

                    // 2. Check for: * & & * & * * * case
                    if (col < gameboard.GetLength(1) - 4 && twoInRowMatch
                        && currentTile.TileType.Equals(gameboard[row, col + 3].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row, col + 3]);
                    }

                    // 3. Check for case:
                    // * & * * * *  
                    // * * & & * * 
                    if (col > 0 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(gameboard[row - 1, col - 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row - 1, col - 1]);
                    }

                    // 4. Check for case:
                    // * * * * & *  
                    // * * & & * * 
                    if (col < gameboard.GetLength(1) - 2 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(gameboard[row - 1, col + 2].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row - 1, col + 2]);
                    }

                    // 5. Check for case:
                    // * * & & * *  
                    // * & * * * *
                    if (row < gameboard.GetLength(0) - 2 && col > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(gameboard[row + 1, col - 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row + 1, col - 1]);
                    }

                    // 6. Check for case:
                    // * * & & * *  
                    // * * * * & * 
                    if (row < gameboard.GetLength(0) - 2 && col < gameboard.GetLength(1) - 2 && twoInRowMatch
                        && currentTile.TileType.Equals(gameboard[row + 1, col + 2].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);*/
                        matches.Add(gameboard[row + 1, col + 2]);
                    }

                    // Six vertical cases for potential match

                    // * * & * *  
                    // * * & * * 
                    // * * * * *  --- vertical case 1 
                    // * * & * *
                    if (row < gameboard.GetLength(0) - 3 && twoInColumnMatch
                        && currentTile.TileType.Equals(gameboard[row + 3, col].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row + 3, col]);
                    }

                    // * * & * * 
                    // * * * * *
                    // * * & * *  --- vertical case 2
                    // * * & * * 
                    if (row > 1 && twoInColumnMatch && currentTile.TileType.Equals(gameboard[row - 2, col].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row - 2, col]);
                    }

                    // * & * *
                    // * * & *   --- vertical case 3
                    // * * & *
                    if (row > 0 && col > 0 && twoInColumnMatch
                        && currentTile.TileType.Equals(gameboard[row - 1, col - 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row - 1, col - 1]);
                    }

                    // * * & *
                    // * & * *   --- vertical case 4
                    // * & * * 
                    if (row > 0 && col < gameboard.GetLength(1) - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(gameboard[row - 1, col + 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row - 1, col + 1]);
                    }

                    // * * & *
                    // * * & *   --- vertical case 5
                    // * & * *
                    if (col > 0 && row < gameboard.GetLength(0) - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(gameboard[row + 2, col - 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row + 2, col - 1]);
                    }

                    // * & * *
                    // * & * *   --- vertical case 6
                    // * * & *
                    if (row < gameboard.GetLength(0) - 2 && col < gameboard.GetLength(1) - 1 && twoInColumnMatch
                        && currentTile.TileType.Equals(gameboard[row + 2, col + 1].TileType))
                    {
                        /*matches.Add(currentTile);
                        matches.Add(downNeighbourTile);*/
                        matches.Add(gameboard[row + 2, col + 1]);
                    }
                }
            }

            if (matches.Count >= 1)
            {
                var rand = new Random();
                yield return matches[rand.Next(matches.Count - 1)];
            }
            else
            {
                yield return null;
            }
        }

        public ITile GetPossibleTile(ITile[,] gameboard)
        {
            var possibleMoves = this.GetAllPossibleTiles(gameboard);
            var randomIndex = this.random.Next(possibleMoves.Count());
            var possibleTile = possibleMoves.ElementAt(randomIndex);
            return possibleTile;
        }
    }
}