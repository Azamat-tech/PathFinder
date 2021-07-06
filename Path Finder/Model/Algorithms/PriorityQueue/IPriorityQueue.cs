namespace Path_Finder.Model.Algorithms.PriorityQueue
{
    interface IPriorityQueue<Position>
    {
        int Count { get; }
        Position Extract();
        void Insert(Position pos, double priority);
        void HeapMin(int i);
        void MinHeapify(int i);
    }
}
