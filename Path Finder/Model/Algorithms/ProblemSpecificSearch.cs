using Path_Finder.Model.Algorithms.PriorityQ;
using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms
{
    abstract class ProblemSpecificSearch : GraphSearch
    {
        protected int[] directionD1 = { -1, 1, 0, 0, 1, -1, 1, -1 };
        protected int[] directionD2 = { 0, 0, 1, -1, -1, 1, 1, -1 };

        protected PriorityQueue<Position> priorityQueue = new PriorityQueue<Position>();
    }
}
