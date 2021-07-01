using System.Collections;
using System.Collections.Generic;

using Path_Finder.Grid;
using Path_Finder.Constants;

namespace Path_Finder.Algorithms
{
    abstract class GraphSearch
    {
        public bool reached; 
        public List<Position> path;
        public List<Position> allVisistedPositions;

        protected int[] directionR = { -1, 1, 0, 0 };
        protected int[] directionC = { 0, 0, 1, -1 };

        public abstract List<Position> Search(Position start, Position end, Cell[,] grid);
        public abstract void NeighbourTraversal(Position current, ref Cell[,] grid);
        public abstract void GetPath(Position start, Position end, Cell[,] grid);

    }
}
