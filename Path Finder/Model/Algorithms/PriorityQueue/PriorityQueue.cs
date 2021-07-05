using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder.Model.Algorithms.PriorityQueue
{
    interface PriorityQueue<Position>
    {
        int Count { get; }
        void Insert(Position pos, int priority);
        Position Extract();
        void HeapMin(int i);
        void MinHeapify(int i);
    }
}
