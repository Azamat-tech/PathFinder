using Path_Finder.Grid;
using Path_Finder.Model.Algorithms;

using Path_Finder.Constants;
using Path_Finder.Model.Algorithms.Heuristics;

namespace Path_Finder.Model.Algorithms
{
    /// <summary>
    /// The implementation of smart(informed) Depth-First Search (Greedy Search) which knows
    /// the current and the end position.When it has a choice about which 
    /// way to go, it will prioritize the direction(s) that take it closer to the goal.
    /// For example, if the goal is directly eastward, then its first choice would 
    /// be to go east, its second choice would be to go north or south, and its 
    /// last choice would be to go west.
    /// </summary>
    class DepthFirstSmart : InformedSearch
    {
        public sealed override void NeighbourTraversal(Position current, ref Cell[,] grid)
        {
            int distance;
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

                distance = Heuristic.ManhattanDistanceHeuristic(endPosition, current);

                grid[neighbour.y, neighbour.x].visited = true;
                priorityQueue.Insert(neighbour, distance * 10);
                allVisistedPositions.Add(neighbour);

                // Set the parent Position 
                grid[neighbour.y, neighbour.x].parent = current;
            }
        }
    }
}
