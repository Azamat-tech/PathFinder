using System.Collections.Generic;

using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms
{
    abstract class InformedSearch : ProblemSpecificSearch
    {
        protected Position startPosition;
        protected Position endPosition;
        public sealed override (List<Position>, List<Position>) Search(Position start, Position end, Cell[,] grid)
        {
            startPosition = start;
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
            }
            if (reached)
            {
                GetPath(start, end, grid);
            }
            return (path, allVisistedPositions);
        }
    }
}
