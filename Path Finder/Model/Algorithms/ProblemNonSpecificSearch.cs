using Path_Finder.Model.Algorithms.PriorityQ;
using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms
{
    abstract class ProblemNonSpecificSearch : GraphSearch
    {
        protected int[] directionR = { -1, 1, 0, 0 };
        protected int[] directionC = { 0, 0, 1, -1 };
    }
}
