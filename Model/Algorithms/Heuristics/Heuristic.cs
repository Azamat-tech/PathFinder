using System;

using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms.Heuristics
{
    static class Heuristic
    {
        public static int CalculateManhattanDistanceHeuristic(Position a, Position b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static double CalculateEuclideanDistanceHeuristic(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0));
        }
    }
}
