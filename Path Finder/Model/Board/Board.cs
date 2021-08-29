using System;
using System.Collections.Generic;

using Path_Finder.Constants;
using Path_Finder.Model.Algorithms;

namespace Path_Finder.Grid
{
    class Board
    {
        private readonly Cell[,] grid;

        public bool BombSet { get; set; }
        public bool PathFound { get; private set; }
        public bool PathToBombFound { get; private set; }

        private bool bfs, dfs, dijkstra, aStarEuclidean, aStarManhattan, smartDFS;
       
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
                    SetCell(i, j, CellType.EMPTY, false);
                }
            }
            SetStartPosition(BoardConstants.STARTXSQUARE, BoardConstants.YSQUARE);
            SetEndPosition(BoardConstants.ENDXSQUARE, BoardConstants.YSQUARE);
        } 

        public Cell[,] GetGrid() => grid;

        public Position GetStartingPosition() => startPosition;

        public Position GetEndPosition() => endPosition;

        public Position GetBombPosition() => bombPosition;

        public bool InsideTheBoard(int posXPixel, int posYPixel)
        {
            if(posXPixel >= BoardConstants.MARGIN && posXPixel < BoardConstants.WIDTH - BoardConstants.MARGIN && 
               posYPixel >= ViewConstants.LEFTOVER && posYPixel < BoardConstants.HEIGHT - BoardConstants.MARGIN)
            {
                return true;
            }
            return false;
        }

        #region Checks Regarding the Board Cells

        public bool IsTaken(int posX, int posY)
        {
            if (IsTakenByStartAndEnd(posX, posY) || IsWall(posX, posY))
            {
                return true;
            }
            return false;
        }

        // Return true if the position is taken by the start or end position
        public bool IsTakenByStartAndEnd(int posX, int posY)
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
        #endregion

        public void ResetSearching()
        {
            for(int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    if (grid[i, j].visited) 
                    {
                        grid[i, j].visited = false;
                    }
                }
            }
        }

        #region Setting the Cell

        public void SetCell(int row, int column, CellType givenType, bool isVisited = false)
        {
            grid[row, column] = new Cell
            {
                // Did not define the parent property of the Cell
                // as it does not have any for now.
                position = new Position(column, row),
                type = givenType,
                visited = isVisited
            };
        }

        public void SetStartPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                if(grid[startPosition.y, startPosition.x].type == CellType.START)
                {
                    SetCell(startPosition.y, startPosition.x, CellType.EMPTY);
                }
                SetCell(posY, posX, CellType.START);
                startPosition = new Position(posX, posY);
            }
        }

        public void SetEndPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                if(grid[endPosition.y, endPosition.x].type == CellType.END)
                {
                    SetCell(endPosition.y, endPosition.x, CellType.EMPTY);
                }
                SetCell(posY, posX, CellType.END);
                endPosition = new Position(posX, posY);
            }
        }

        public void SetBombPosition(int posX, int posY)
        {
            if (IsEmpty(posX, posY))
            {
                SetCell(bombPosition.y, bombPosition.x, CellType.EMPTY);
                SetCell(posY, posX, CellType.BOMB);
                bombPosition = new Position(posX, posY);
            }
        }

        public void SetWall(int posX, int posY, bool justPress = true)
        {
            if (justPress)
            {
                SetCell(posY, posX, CellType.WALL);
            }
            else
            {
                if (previousPosition.x != posX || previousPosition.y != posY)
                {
                    SetCell(posY, posX, CellType.WALL);
                    previousPosition = new Position(posX, posY);
                }
            }
        }

        #endregion

        public void RemoveWall(int posX, int posY, bool justPress = true)
        {
            if(justPress)
            {
                SetCell(posY, posX, CellType.EMPTY);
            }else
            {
                if(previousPosition.x != posX || previousPosition.y != posY)
                {
                    SetCell(posY, posX, CellType.EMPTY);
                    previousPosition = new Position(posX, posY);
                }
            }
        }

        private void RemoveAllWalls()
        {
            for(int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for(int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    if(IsWall(j, i))
                    {
                        grid[i, j].type = CellType.EMPTY;
                    }
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
                    if (grid[i, j].type != CellType.EMPTY || grid[i, j].visited)
                    {
                        SetCell(i, j, CellType.EMPTY, false);
                    }
                }
            }

            // Set the Start position back to an inital place
            SetStartPosition(BoardConstants.STARTXSQUARE, BoardConstants.YSQUARE);
            // Set the End position back to an inital place
            SetEndPosition(BoardConstants.ENDXSQUARE, BoardConstants.YSQUARE);
            // Remove the bomb from the Grid
            RemoveBomb();

            BombSet = false;
            PathFound = false;
            AssignAlgoValues(false, false, false, false, false, false);
        }

        public void RemoveBomb()
        {
            SetCell(bombPosition.y, bombPosition.x, CellType.EMPTY);
            // Set the bomb to its original spot
            bombPosition = new Position((BoardConstants.STARTXSQUARE + BoardConstants.ENDXSQUARE) / 2, 
                                                                        BoardConstants.YSQUARE / 2);
            BombSet = false;
        }

        public void AddBomb()
        {
            if(!IsTakenByStartAndEnd(bombPosition.x, bombPosition.y))
            {
                SetCell(bombPosition.y, bombPosition.x, CellType.BOMB);
                BombSet = true;
            }
        }

        #region Working with Maze Generators
        public bool IsPathFound()
        {
            ResetSearching();

            return BFS(startPosition, endPosition).Item1.Count != 0;
        }

        public void GenerateRandomMaze()
        {
            int rNumber;
            RemoveAllWalls();

            Random r = new Random();

            for(int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                for (int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                {
                    if(!IsTaken(j, i))
                    {
                        rNumber = r.Next(0, 100);
                        if(rNumber <= 35)
                        {
                            grid[i, j].type = CellType.WALL;
                        }
                    }
                }
            }
        }

        public void GenerateRecursiveMaze()
        {
            Random n = new Random();

            RemoveAllWalls();

            // Add outer walls
            for (int i = 0; i < BoardConstants.ROWSIZE; i++)
            {
                if (i == 0 || i == (BoardConstants.ROWSIZE - 1))
                {
                    for (int j = 0; j < BoardConstants.COLUMNSIZE; j++)
                    {
                        grid[i, j].type = CellType.WALL;
                    }
                } else
                {
                    grid[i, 0].type = CellType.WALL;
                    grid[i, BoardConstants.COLUMNSIZE - 1].type = CellType.WALL;
                }
            }

            RecursiveDivision(1, 1, BoardConstants.COLUMNSIZE - 1, BoardConstants.ROWSIZE - 1, n);
        }

        // To choose horizontal or vertical direction. 0 is vertical 1 is horizontal
        private int ChooseDirection(int width, int height)
        {
            Random dir = new Random();
            if (width < height)
                return 0;
            else if (width > height)
                return 1;
            else
                return dir.Next(0, 2);
        }

        //To build the wall in the given index with the empty square "connectionID"
        private void BuildWall(int direction, int wallId, int connectionID, int w_start, int width, int h_start, int height)
        {
            if (direction == 1)
            {
                for (int i = h_start; i < height; i++)
                {
                    if (connectionID != i && !IsTaken(wallId, i))
                        grid[i, wallId].type = CellType.WALL;
                }
            }
            else if (direction == 0)
            {
                for (int i = w_start; i < width; i++)
                {
                    if (connectionID != i && !IsTaken(i, wallId))
                        grid[wallId, i].type = CellType.WALL;
                }
            }
        }
       
        // To build the maze based on recurive division
        public void RecursiveDivision(int w_start, int h_start, int w_end, int h_end, Random n)
        {

            int width = w_end - w_start;
            int height = h_end - h_start;

            // Recursive Termination
            if (height < 2 || width < 2)
                return;

            // Choose randomly horizontal or vertical
            int direction = ChooseDirection(width, height);

            int wallId;
            int connectionID;
            // Randomly select the position to build the wall
            if (direction == 1)
            {
                wallId = n.Next(w_start, w_end);
                connectionID = n.Next(h_start, h_end);
            }
            else
            {
                wallId = n.Next(h_start, h_end);
                connectionID = n.Next(w_start, w_end);
            }
            BuildWall(direction, wallId, connectionID, w_start, w_end, h_start, h_end);

            // Recursive on sub-areas 
            if (direction == 1)
            {
                RecursiveDivision(w_start, h_start, wallId - 1, h_end, n);
                RecursiveDivision(wallId, h_start, w_end, h_end, n);
            }
            else
            {
                RecursiveDivision(w_start, h_start, w_end, wallId - 1, n);
                RecursiveDivision(w_start, wallId, w_end, h_end, n);
            }
        }

        #endregion

        #region Working with Algorithms 
        private void AssignAlgoValues(bool bBFS, bool bDFS, bool bDIJKSTRA, bool bASTARE, bool bASTARM, bool bSMARTDFS)
        {
            (bfs, dfs, dijkstra, aStarEuclidean, aStarManhattan, smartDFS) = 
                (bBFS, bDFS, bDIJKSTRA, bASTARE, bASTARM, bSMARTDFS);
        }

        public void SetAlgorithm(string algoName)   
        {
            switch (algoName)
            {
                case "BFS":
                    AssignAlgoValues(true, false, false, false, false, false);
                    break;
                case "DFS":
                    AssignAlgoValues(false, true, false, false, false, false);
                    break;
                case "Dijkstra":
                    AssignAlgoValues(false, false, true, false, false, false);
                    break;
                case "AStarEuclidean":
                    AssignAlgoValues(false, false, false, true, false, false);
                    break;
                case "AStarManhattan":
                    AssignAlgoValues(false, false, false, false, true, false);
                    break;
                case "SmartDFS":
                    AssignAlgoValues(false, false, false, false, false, true);
                    break;
                default:
                    AssignAlgoValues(false, false, false, false, false, false);
                    break;
            }
        }

        public bool AnyAlgorithmSet()
        {
            return bfs || dfs || aStarEuclidean || aStarManhattan 
                       || dijkstra || smartDFS;
        }

        private Algorithm SelectedAlgorithm()
        {
            if (bfs) return Algorithm.BFS;
            else if (dfs) return Algorithm.DFS;
            else if (aStarEuclidean) return Algorithm.AStarEuclidian;
            else if (aStarManhattan) return Algorithm.AStarManhattan;
            else if (smartDFS) return Algorithm.SmartDFS;
            else return Algorithm.Dijkstra;
        }

        public (List<Position>, List<Position>) GetSearchPath(Position a, Position b)
        {
            Algorithm searchAlgo = SelectedAlgorithm();

            List<Position> finalPath = new List<Position>();
            List<Position> visitedPositions = new List<Position>();

            switch (searchAlgo)
            {
                case Algorithm.BFS:
                    (finalPath, visitedPositions) = BFS(a, b);
                    break;
                case Algorithm.DFS:
                    (finalPath, visitedPositions) = DFS(a, b);
                    break;
                case Algorithm.Dijkstra:
                    (finalPath, visitedPositions) = Dijkstra(a, b);
                    break;
                case Algorithm.AStarEuclidian:
                    (finalPath, visitedPositions) = AStarEuclideanSearch(a, b);
                    break;
                case Algorithm.AStarManhattan:
                    (finalPath, visitedPositions) = AStarManhattanSearch(a, b);
                    break;
                case Algorithm.SmartDFS:
                    (finalPath, visitedPositions) = SmartDFS(a, b);
                    break;
                default:
                    break;
            }

            if(finalPath.Count != 0)
            {
                if (b == bombPosition)
                {
                    PathToBombFound = true;
                } else
                {
                    PathFound = true;
                }
            }
            else
            {
                if (b == bombPosition)
                {
                    PathToBombFound = false;
                }
                else
                {
                    PathFound = false;
                }
            }
            return (finalPath, visitedPositions);
        }


        private (List<Position>, List<Position>) BFS(Position a, Position b)
        {
            BreadthFirst bfsSearch = new BreadthFirst();
            return bfsSearch.Search(a, b, grid);
        }

        private (List<Position>, List<Position>) DFS(Position a, Position b)
        {
            DepthFirst dfsSearch = new DepthFirst();
            return dfsSearch.Search(a, b, grid);
        }

        private (List<Position>, List<Position>) Dijkstra(Position a, Position b)
        {
            Dijkstra dijkstraAlgorithm = new Dijkstra();
            return dijkstraAlgorithm.Search(a, b, grid);
        }

        private (List<Position>, List<Position>) AStarEuclideanSearch(Position a, Position b)
        {
            AStarEuclideanDistance aStarEuclidean = new AStarEuclideanDistance();
            return aStarEuclidean.Search(a, b, grid);
        }

        private (List<Position>, List<Position>) AStarManhattanSearch(Position a, Position b)
        {
            AStarManhattenDistance aStarManhattan = new AStarManhattenDistance();
            return aStarManhattan.Search(a, b, grid);
        }

        private (List<Position>, List<Position>) SmartDFS(Position a, Position b)
        {
            DepthFirstSmart smartDFS = new DepthFirstSmart();
            return smartDFS.Search(a, b, grid);
        }
        #endregion
    }
}
