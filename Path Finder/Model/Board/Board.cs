﻿using System.Collections.Generic;

using Path_Finder.Constants;
using Path_Finder.Algorithms;

namespace Path_Finder.Grid
{
    class Board
    {
        private readonly Cell[,] grid;

        public bool BombSet { get; private set; }

        public bool pathFound { get; private set; }

        private bool bfs, dfs, aStar, dijkstra;
       
        private Position previousPosition = new Position(0, 0);

        private Position startPosition = new Position
            (
                BoardConstants.STARTXSQUARE, BoardConstants.YSQUARE
            );
        private Position endPosition = new Position
            (
                BoardConstants.ENDXSQUARE, BoardConstants.YSQUARE
            );
        private Position bombPosition = new Position
            (
                BoardConstants.BOMBXSQUARE, BoardConstants.BOMBQSQUARE
            );

        public Board()
        {
            grid = new Cell[BoardConstants.ROWSIZE, BoardConstants.COLUMNSIZE];
            for (int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    SetCell(i, j, CellType.EMPTY);
                }
            }
            SetStartPosition(BoardConstants.STARTXSQUARE, BoardConstants.YSQUARE);
            SetEndPosition(BoardConstants.ENDXSQUARE, BoardConstants.YSQUARE);
        } 

        public Cell[,] GetGrid() => grid;

        public Position GetStartingPosition() => startPosition;

        public Position GetEndPosition() => endPosition;

        public Position GetBombPosition() => bombPosition;

        public bool InsideTheBoard(int posX, int posY)
        {
            if(posX >= BoardConstants.MARGIN && posX <= BoardConstants.WIDTH - BoardConstants.MARGIN && 
                posY >= 85 && posY <= BoardConstants.HEIGHT - BoardConstants.MARGIN)
            {
                return true;
            }
            return false;
        }
        private void AssignValues(bool bBFS, bool bDFS, bool bASTAR, bool bDIJKSTRA)
        {
            (bfs, dfs, aStar, dijkstra) = (bBFS, bDFS, bASTAR, bDIJKSTRA);
        }

        public void SetAlgorithm(string algoName)
        {
            switch (algoName)
            {
                case "BFS":
                    AssignValues(true, false, false, false);
                    break;
                case "DFS":
                    AssignValues(false, true, false, false);
                    break;
                case "AStar":
                    AssignValues(false, false, true, false);
                    break;
                case "Dijkstra":
                    AssignValues(false, false, false, true);
                    break;
                default:
                    break;
            }
        }

        public bool AnyAlgorithmSet()
        {
            return bfs || dfs || aStar || dijkstra;
        }

        // Return true if the position is taken by the start or end position
        public bool IsTaken(int posX, int posY)
        {
            if (grid[posY, posX].type == CellType.START || grid[posY, posX].type == CellType.END)
            {
                return true;
            }
            return false;
        }

        public bool IsStartPosition(int posX, int posY)
        {
            if(startPosition == new Position(posX, posY))
            {
                return true;
            }
            return false;
        }

        public bool IsEndPosition(int posX, int posY)
        {
            if (endPosition == new Position(posX, posY))
            {
                return true;
            }
            return false;
        }

        public bool IsBombPosition(int posX, int posY)
        {
            if (bombPosition == new Position(posX, posY))
            {
                return true;
            }
            return false;
        }

        public bool IsWall(int posX, int posY)
        {
            if (grid[posY, posX].type == CellType.WALL)
            {
                return true;
            }
            return false;
        }

        public bool IsEmpty(int posX, int posY)
        {
            if(grid[posY, posX].type == CellType.EMPTY)
            {
                return true;
            }
            return false;
        }
/*
        public bool IsValidPosition(int posX, int posY)
        {
            if(IsStartPosition(posX, posY) || IsEndPosition(posX, posY) || IsWall(posX,posY))
            {
                return false;
            }
            return true;
        }
*/
        public void SetCell(int row, int column, CellType givenType)
        {
            grid[row, column] = new Cell
            {
                position = new Position(column, row),
                type = givenType
            };
        }

        public void SetStartPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                if(grid[startPosition.y, startPosition.x].type == CellType.START)
                {
                    grid[startPosition.y, startPosition.x].type = CellType.EMPTY;
                }
                grid[posY, posX].type = CellType.START;
                startPosition = new Position(posX, posY);
            }
        }

        public void SetEndPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                if(grid[endPosition.y, endPosition.x].type == CellType.END)
                {
                    grid[endPosition.y, endPosition.x].type = CellType.EMPTY;
                }
                grid[posY, posX].type = CellType.END;
                endPosition = new Position(posX, posY);
            }
        }

        public void SetBombPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                grid[bombPosition.y, bombPosition.x].type = CellType.EMPTY;
                grid[posY, posX].type = CellType.BOMB;
                bombPosition = new Position(posX, posY);
            }
        }

        public void SetWall(int posX, int posY, bool justPress = true)
        {
            if (justPress)
            {
                grid[posY, posX].type = CellType.WALL;
            }
            else
            {
                if (previousPosition.x != posX || previousPosition.y != posY)
                {
                    grid[posY, posX].type = CellType.WALL;
                    previousPosition = new Position(posX, posY);
                }
            }
        }

        public void RemoveWall(int posX, int posY, bool justPress = true)
        {
            if(justPress)
            {
                grid[posY, posX].type = CellType.EMPTY;
            }else
            {
                if(previousPosition.x != posX || previousPosition.y != posY)
                {
                    grid[posY, posX].type = CellType.EMPTY;
                    previousPosition = new Position(posX, posY);
                }
            }
        }

        public void ClearBoard()
        {
            // Clear the board back to EMPTY squares
            for(int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    if(grid[i, j].type != CellType.EMPTY)
                    {
                        grid[i, j].type = CellType.EMPTY;
                    }
                }
            }

            // Set the Start position back to an inital place
            SetStartPosition(BoardConstants.STARTXSQUARE, BoardConstants.YSQUARE);
            // Set the End position back to an inital place
            SetEndPosition(BoardConstants.ENDXSQUARE, BoardConstants.YSQUARE);
            // Remove the bomb from the Grid
            RemoveBomb();
        }

        public bool IsBombSet()
        {
            if (BombSet)
            {
                return true;
            }
            return false;
        }

        public void RemoveBomb()
        {
            grid[bombPosition.y, bombPosition.x].type = CellType.EMPTY;
            // Set the bomb to its original spot
            bombPosition = new Position((BoardConstants.STARTXSQUARE + BoardConstants.ENDXSQUARE) / 2, 
                                                                        BoardConstants.YSQUARE / 2);
            BombSet = false;
        }

        public void AddBomb()
        {
            if(!IsTaken(bombPosition.x, bombPosition.y))
            {
                grid[bombPosition.y, bombPosition.x].type = CellType.BOMB;
                BombSet = true;
            }
        }

        private Algorithm SelectedAlgorithm()
        {
            if (bfs) return Algorithm.BFS;
            else if (dfs) return Algorithm.DFS;
            else if (aStar) return Algorithm.AStar;
            else return Algorithm.Dijkstra;
        }

        public (List<Position>, List<Position>) GetSearchPath()
        {
            Algorithm searchAlgo = SelectedAlgorithm();
            List<Position> finalPath = new List<Position>();
            List<Position> visitedPositions = new List<Position>();

            switch (searchAlgo)
            {
                case Algorithm.BFS:
                    (finalPath, visitedPositions) = BFS();
                    break;
                case Algorithm.DFS:
                    (finalPath, visitedPositions) = DFS();
                    break;
                default:
                    break;
            }
            if(finalPath.Count != 0)
            {
                pathFound = true;
            }else
            {
                pathFound = false;
            }
            return (finalPath, visitedPositions);
        }

        public (List<Position>, List<Position>) BFS()
        {
            BreadthFirst bfsSearch = new BreadthFirst();
            return bfsSearch.Search(startPosition, endPosition, grid);
        }

        public (List<Position>, List<Position>) DFS()
        {
            DepthFirst dfsSearch = new DepthFirst();
            return dfsSearch.Search(startPosition, endPosition, grid);
        }
    }
}