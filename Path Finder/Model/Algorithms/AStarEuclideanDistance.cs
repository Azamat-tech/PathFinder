using Path_Finder.Grid;
using Path_Finder.Constants;
using Path_Finder.Model.Algorithms.Heuristics;

namespace Path_Finder.Model.Algorithms
{
    /// <summary>
    /// The implementation of a AStar algorithm that inherits from the 
    /// InformedUniformSearch that uses priority queue in selecting the 
    /// most optimal position.
    /// </summary>
    class AStarEuclideanDistance : InformedSearch
    {
        /// <summary>
        /// AStar algorithm is a part of a informed search that utilizes the function f 
        /// that returns the optimal path from the start to a goal node. 
        /// f(n) = h(n) + g(n) where g(n) is the shortest path from start to node n and
        /// h(n) is Euclidian path from n to the end node.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="grid"></param>
        public sealed override void NeighbourTraversal(Position current, ref Cell[,] grid)
        {
            for(int i = 0; i < 8; i++)
            {
                Position neighbour = new Position
                   (
                       current.x + directionD1[i],
                       current.y + directionD2[i]
                   );

                // Checking the bounds of the grid
                if (neighbour.y < 0 || neighbour.x < 0 ||
                    neighbour.y >= BoardConstants.ROWSIZE || 
                    neighbour.x >= BoardConstants.COLUMNSIZE)
                {
                    continue;
                }
                // Checkign if the Position is visited or it is a wall
                if ((grid[neighbour.y, neighbour.x].visited == true) ||
                    (grid[neighbour.y, neighbour.x].type == CellType.WALL))
                {
                    continue;
                }

                if (i < 4)
                {
                    neighbour.Gcost = current.Gcost + 1;
                }
                else
                {
                    neighbour.Gcost = current.Gcost + 1.4;
                }

                neighbour.Hcost = Heuristic.CalculateEuclideanDistanceHeuristic(neighbour, endPosition);

                grid[neighbour.y, neighbour.x].visited = true;
                priorityQueue.Insert(neighbour, neighbour.Fcost);
                allVisistedPositions.Add(neighbour);

                // Set the parent Position 
                grid[neighbour.y, neighbour.x].parent = current;
            }
        }
    }
}
