namespace Path_Finder.Model.Algorithms
{
    abstract class UninformedSearch : GraphSearch
    {
        protected int[] directionR = { -1, 1, 0, 0 };
        protected int[] directionC = { 0, 0, 1, -1 };
    }
}
