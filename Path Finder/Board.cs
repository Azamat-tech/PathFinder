using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Path_Finder.Cell;

namespace Path_Finder
{
    enum Cell
    { 
        empty, wall, start, end
    }
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

        //
        public Cell[,] grid = new Cell[ROWSIZE, COLUMNSIZE];

        // Booleans for the Keys
        public bool startKeyDown = true;
        public bool endKeyDown = true;

        // Start and End Position
        public Position startPosition = new Position(15,18);
        public Position endPosition = new Position(60, 18);

        public Board()
        {
            grid[startPosition.y, startPosition.x] = start;
            grid[endPosition.y, endPosition.x] = end;
        }

        public bool InsideTheBoard(int posX, int posY)
        {
            if(posX >= MARGIN && posX <= WIDTH - MARGIN && posY >= 85 && posY <= HEIGHT - MARGIN)
            {
                return true;
            }
            return false;
        }

        public bool IsValidCell(int posX, int posY)
        {
            if ((startPosition.x == posX && startPosition.y == posY) || (endPosition.x == posX && endPosition.y == posY))
            {
                return false;
            }
            return true;
        }

        public void SetStartPosition(int posX, int posY)
        {
            if(IsValidCell(posX, posY))
            {
                grid[posY, posX] = start;
                grid[startPosition.y, startPosition.x] = empty;
                startPosition = new Position(posX, posY);
            }
        }

        public void SetEndPosition(int posX, int posY)
        {

        }

        public void SetWalls(int posX, int posY)
        {
            if(IsValidCell(posX, posY))
            {
                if (grid[posY, posX] == wall)
                {
                    grid[posY, posX] = empty;
                }
                else
                {
                    grid[posY, posX] = wall;
                }
            } 
        }

        public void DeleteAttribute(int posX, int posY)
        {
            if (grid[posY, posX] == wall) grid[posY, posX] = empty;
        }

    }
}
