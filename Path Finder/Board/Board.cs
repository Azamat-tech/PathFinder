using static Grid.Enums;

namespace Grid
{
    class Board
    {
        public const int WIDTH = 1510;
        public const int HEIGHT = 710;

        public const int MARGIN = 5;

        public const int TOOLBOXWIDTH = 1500;
        public const int TOOLBOXHEIGHT = 70;

        public const int SQUARE = 20;
        public const int COLUMNSIZE = 75;
        public const int ROWSIZE = 35;

        private const int STARTXSQUARE = 15;
        private const int YSQUARE = 18;
        private const int ENDXSQUARE = 60;

        private const int BOMBXSQUARE = (STARTXSQUARE + ENDXSQUARE) / 2;
        private const int BOMBQSQUARE = YSQUARE / 2;

        private readonly Cell[,] grid;

        private bool bombSet;

        private Position previousPosition = new Position(0, 0);

        private Position startPosition = new Position(STARTXSQUARE, YSQUARE);
        private Position endPosition = new Position(ENDXSQUARE, YSQUARE);
        private Position bombPosition = new Position(BOMBXSQUARE, BOMBQSQUARE);

        public Board()
        {
            grid = new Cell[ROWSIZE, COLUMNSIZE];
            for (int i = 0; i < ROWSIZE; i++)
            {
                for(int j = 0; j < COLUMNSIZE; j++)
                {
                    SetCell(i, j, CellType.EMPTY);
                }
            }
            SetStartPosition(STARTXSQUARE, YSQUARE);
            SetEndPosition(ENDXSQUARE, YSQUARE);
        } 

        public Cell[,] GetGrid() => grid;

        public Position GetStartingPosition() => startPosition;

        public Position GetEndPosition() => endPosition;

        public Position GetBombPosition() => bombPosition;

        public bool InsideTheBoard(int posX, int posY)
        {
            if(posX >= MARGIN && posX <= WIDTH - MARGIN && posY >= 85 && posY <= HEIGHT - MARGIN)
            {
                return true;
            }
            return false;
        }


        // Return true if the position is take by the start or end position
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
            for(int i = 0; i < ROWSIZE; i++)
            {
                for(int j = 0; j < COLUMNSIZE; j++)
                {
                    if(grid[i, j].type != CellType.EMPTY)
                    {
                        grid[i, j].type = CellType.EMPTY;
                    }
                }
            }

            // Set the Start position back to an inital place
            SetStartPosition(STARTXSQUARE, YSQUARE);
            // Set the End position back to an inital place
            SetEndPosition(ENDXSQUARE, YSQUARE);
            // Remove the bomb from the Grid
            RemoveBomb();
        }

        public bool IsBombSet()
        {
            if (bombSet)
            {
                return true;
            }
            return false;
        }

        public void RemoveBomb()
        {
            grid[bombPosition.y, bombPosition.x].type = CellType.EMPTY;
            // Set the bomb to its original spot
            bombPosition = new Position((STARTXSQUARE + ENDXSQUARE) / 2, YSQUARE / 2);
            bombSet = false;
        }

        public void AddBomb()
        {
            if(!IsTaken(bombPosition.x, bombPosition.y))
            {
                grid[bombPosition.y, bombPosition.x].type = CellType.BOMB;
                bombSet = true;
            }
        }

    }
}
