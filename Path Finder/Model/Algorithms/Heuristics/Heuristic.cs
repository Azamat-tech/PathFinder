using System;

using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms.Heuristics
{
    static class Heuristic
    {
        public static int ManhattanDistanceHeuristic(Position a, Position b)
        {
            return Math.Abs(Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y));
        }

        public static double EuclideanDistanceHeuristic(Position a, Position b)
        {
            return Math.Round(Math.Sqrt(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0)), 1);
        }
    }
}
