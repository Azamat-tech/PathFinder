using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder.Model.Algorithms.PriorityQueue
{
    interface IPriorityQueue<Position>
    {
        int Count { get; }
        Position Extract();
        void Insert(Position pos, int priority);
        void HeapMin(int i);
        void MinHeapify(int i);
    }
}
