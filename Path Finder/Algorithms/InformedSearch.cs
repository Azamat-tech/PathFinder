using Path_Finder.Grid;

namespace Path_Finder.Algorithms
{
    abstract class InformedSearch : GraphSearch
    {
        public abstract int EuclideanDistanceHeuristic(Position a, Position b);
        public abstract int ManhattanDistanceHeuristic(Position a, Position b);
    }
}
