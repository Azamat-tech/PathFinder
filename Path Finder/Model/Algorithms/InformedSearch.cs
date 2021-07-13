using System.Collections.Generic;

using Path_Finder.Grid;
using Path_Finder.Model.Algorithms.PriorityQ;

namespace Path_Finder.Model.Algorithms
{
    abstract class InformedSearch : GraphSearch
    {
        // protected Position startPosition;
        protected int distance;
        protected Position endPosition;

        protected int[] directionD1 = { -1, 1, 0, 0, 1, -1, 1, -1 };
        protected int[] directionD2 = { 0, 0, 1, -1, -1, 1, 1, -1 };

        protected PriorityQueue<Position> priorityQueue = new PriorityQueue<Position>();
        protected bool IsPositionDiagonal(int x, int y) => x != 0 && y != 0;
        public sealed override (List<Position>, List<Position>) Search(Position start, Position end, Cell[,] grid)
        {
            // startPosition = start;
            distance = 0;
            endPosition = end;

            priorityQueue.Insert(start, 0);
            grid[start.y, start.x].visited = true;
            grid[start.y, start.x].parent = start;

            while (priorityQueue.Count != 0)
            {
                Position current = priorityQueue.Extract();
                if (grid[current.y, current.x].type == CellType.END)
                {
                    reached = true;
                    break;
                }
                NeighbourTraversal(current, ref grid);
                distance += 1;
            }
            if (reached)
            {
                GetPath(start, end, grid);
            }
            return (path, allVisistedPositions);
        }
    }
}
