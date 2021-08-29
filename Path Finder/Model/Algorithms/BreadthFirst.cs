using System.Collections.Generic;

using Path_Finder.Grid;
using Path_Finder.Constants;

namespace Path_Finder.Model.Algorithms
{
    /// <summary>
    /// The implementation of a Breadth-First Search algorithm that inherits 
    /// from the UninformedSearch using the LIFO method in picking the nodes
    /// from the frontier
    /// </summary>
    class BreadthFirst : UninformedSearch
    {
        private Queue<Position> queue = new Queue<Position>();

        public sealed override void NeighbourTraversal(Position current, ref Cell[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                Position neighbour = new Position
                                    (
                                        current.x + directionD1[i],
                                        current.y + directionD2[i]
                                    );
                // Checking the bounds of the grid
                if (neighbour.y < 0 || neighbour.x < 0 || 
                    neighbour.y >= BoardConstants.ROWSIZE || neighbour.x >= BoardConstants.COLUMNSIZE)
                {
                    continue;
                }
                // Checkign if the Position is visited or it is a wall
                if ((grid[neighbour.y, neighbour.x].visited == true) || 
                    (grid[neighbour.y, neighbour.x].type == CellType.WALL))
                {
                    continue;
                }

                grid[neighbour.y, neighbour.x].visited = true;
                queue.Enqueue(neighbour);
                allVisistedPositions.Add(neighbour);

                // Set the parent position
                grid[neighbour.y, neighbour.x].parent = current;
            } 
        }

        public sealed override (List<Position>, List<Position>) Search(Position start, Position end, Cell[,] grid)
        {
            queue.Enqueue(start);
            grid[start.y, start.x].visited = true;
            grid[start.y, start.x].parent = start;

            while (queue.Count != 0)
            {
                Position current = queue.Dequeue();

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
