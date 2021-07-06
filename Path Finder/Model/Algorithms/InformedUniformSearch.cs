using System;

using Path_Finder.Grid;

namespace Path_Finder.Algorithms
{
    /// <summary>
    /// This abstract class represents both Informed and Uniform Search
    /// because they both share same functions and fields. As both need
    /// to able to work with Heuristics to caclulate the distance.
    /// </summary>
    abstract class InformedUniformSearch : GraphSearch
    {
        protected int[] directionD1 = { -1, 1, 0, 0, 1, -1, 1, -1 };
        protected int[] directionD2 = { 0, 0, 1, -1, -1, 1, 1, -1 };

        public double EuclideanDistanceHeuristic(Position a, Position b)
        {
            return Math.Round(Math.Sqrt(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0)), 1);
        }

        public int ManhattanDistanceHeuristic(Position a, Position b)
        {
            return Math.Abs(Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y));
        }
    }
}
