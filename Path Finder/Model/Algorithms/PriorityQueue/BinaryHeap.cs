using System;
using System.Collections.Generic;

using Path_Finder.Grid;

namespace Path_Finder.Model.Algorithms.PriorityQueue
{
    /// <summary>
    /// Implementation of priority queue using the Binary Heap data structure
    /// Every item in the priority queue is an element of type Position with 
    /// a corresponding priority of type Int.
    /// </summary>
    class BinaryHeap : IPriorityQueue<Position>
    {
        class Node
        {
            public Position position;
            public double priority;
            public Node (Position pos, double prio)
            {
                position = pos;
                priority = prio;
            }  
        }

        private List<Node> buffer = new List<Node>();
        private int heapSize = -1;
        private static int parent(int i) => (i - 1) / 2;
        private static int leftChild(int i) => 2 * i + 1;
        private static int rightChild(int i) => 2 * i + 2;

        public int Count => buffer.Count;

        private void swap(int parentN, int childN)
        {
            Node temp = buffer[parentN];
            buffer[parentN] = buffer[childN];
            buffer[childN] = temp;
        }

        public void Insert(Position position, double priority)
        {
            Node node = new Node(position, priority);
            buffer.Add(node);
            heapSize++;
            HeapMin(heapSize);
        }

        public void HeapMin(int heapSize)
        {
            while (heapSize >= 0 && buffer[parent(heapSize)].priority > buffer[heapSize].priority)
            {
                swap(heapSize, parent(heapSize));
                heapSize = parent(heapSize);
            }
        }

        public Position Extract()
        {
            Position output = buffer[0].position;
            buffer[0] = buffer[heapSize];
            buffer.RemoveAt(heapSize);
            heapSize--;
            MinHeapify(0);
            return output;
        }

        public void MinHeapify(int i)
        {
            int pos = i;
            int left = leftChild(i);
            int right = rightChild(i);
            if (left <= heapSize && buffer[pos].priority > buffer[left].priority)
                pos = left;
            if (right <= heapSize && buffer[pos].priority > buffer[right].priority)
                pos = right;
            if (pos != i)
            {
                swap(pos, i);
                MinHeapify(pos);
            }
        }
    }
}
