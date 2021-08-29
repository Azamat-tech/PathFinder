using System.Collections.Generic;

using Path_Finder.Grid;
using Path_Finder.Model.Algorithms.PriorityQ;
using Path_Finder.Model.Algorithms.Heuristics;

namespace Path_Finder.Model.Algorithms
{
    abstract class InformedSearch : GraphSearch
    {
        protected Position endPosition;

        protected PriorityQueue<Position> priorityQueue = new PriorityQueue<Position>();
        public sealed override (List<Position>, List<Position>) Search(Position start, Position end, Cell[,] grid)
        {
            endPosition = end;

            start.Gcost = 0;
            start.Hcost = Heuristic.CalculateManhattanDistanceHeuristic(start, end);

            priorityQueue.Insert(start, start.Hcost);

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
