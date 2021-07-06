using System.Collections.Generic;

using Path_Finder.Grid;

namespace Path_Finder.Algorithms
{
    public enum Algorithm
    {
        BFS, DFS, AStar, Dijkstra
    }
    abstract class GraphSearch
    {
        public List<Position> path = new List<Position>();
        public List<Position> allVisistedPositions = new List<Position>();

        protected bool reached;


        /// <summary>
        /// Search function returns the List of the positions from the start to 
        /// end position using different graph search algorithms. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public abstract (List<Position>, List<Position>) Search(Position start, Position end, Cell[,] grid);

        /// <summary>
        /// NeighbourTraversal explores the neighbours of the node (position)
        /// and checks if they are within the range of the grid and if they 
        /// were visited or not the walls. Then they are added to the queue
        /// changed on the grid as visited, added to allVisistedPositions and
        /// assigned parent nodes.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="grid"></param>
        public abstract void NeighbourTraversal(Position current, ref Cell[,] grid);

        /// <summary>
        /// GetPath addes to the list the path from the end to start position.
        /// It usese the parent property of the Cell and generates the list of
        /// the positions. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="grid"></param>
        public void GetPath(Position start, Position end, Cell[,] grid)
        {
            Position previousPosition;
            path.Add(end);
            while (true)
            {
                previousPosition = grid[end.y, end.x].parent;
                path.Insert(0, previousPosition);
                end = previousPosition;

                if (previousPosition == start)
                {
                    break;
                }
            }
        }
    }
}
