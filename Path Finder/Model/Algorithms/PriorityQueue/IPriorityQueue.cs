using System;

namespace Path_Finder.Model.Algorithms.PriorityQ
{
    interface IPriorityQueue<Position>
    {
        int Count { get; }
        Position Extract();
        void Insert(Position pos, double priority);
    }

    interface IBinaryMinHeap<T> 
    {
        int GetLength { get; }
        void HeapMin(int i);
        void MinHeapify(int i);
    }
}
