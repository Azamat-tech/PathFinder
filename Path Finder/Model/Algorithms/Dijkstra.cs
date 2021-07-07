using Path_Finder.Constants;
using Path_Finder.Grid;
using Path_Finder.Model.Algorithms.Heuristics;

namespace Path_Finder.Model.Algorithms
{
    /// <summary>
    /// The implementation of a Dijkstras algorithm that inherits 
    /// from the UniformSearch using the priority queue in picking 
    /// the nodes from the frontier. 
    /// 
    /// Search function in Dijkstras algorithm should calculate at every state the 
    /// function g for each neighbouring states. That is done to choose the closest
    /// distance from start to Position n i.e g(n). 
    /// </summary>
    class Dijkstra : UniformSearch
    {

        /// <summary>
        /// NeighbourTraversal is responsible for calculating the g function for neighbours
        /// while iterative over them. Manhattan and Euclidean distance will be used by weight
        /// being 1 from one Position to another.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="grid"></param>
        public sealed override void NeighbourTraversal(Position current, ref Cell[,] grid)
        {
            int distance;
            for(int i = 0; i < 8; i++)
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

                distance = Heuristic.ManhattanDistanceHeuristic(startPosition, neighbour);

                grid[neighbour.y, neighbour.x].visited = true;
                priorityQueue.Insert(neighbour, distance);
                allVisistedPositions.Add(neighbour);

                // Set the parent Position 
                grid[neighbour.y, neighbour.x].parent = current;
            }
        }
    }
}
