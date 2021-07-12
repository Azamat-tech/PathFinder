using System;
using System.Collections.Generic;

namespace Path_Finder.Model.Algorithms.PriorityQ
{
    /// <summary>
    /// Implementation of priority queue using the Min Binary Heap data structure
    /// Every item in the priority queue is an element of type Position with 
    /// a corresponding priority of type Int.
    /// </summary>
    class PriorityQueue<Position> : IPriorityQueue<Position>
    {
        internal class Node : IComparable<Node>
        {
            public Position position;
            public double priority;
            public int CompareTo(Node otherNode)
            {
                return priority.CompareTo(otherNode.priority);
            }
        }
                
        private BinaryMinHeap<Node> minHeap = new BinaryMinHeap<Node>();
        public int Count => minHeap.GetLength;
        public void Insert(Position pos, double prio)
        {
            minHeap.Add(new Node() { position = pos, priority = prio });
        }

        public Position Extract()
        {
            return minHeap.Remove().position;
        }
    }

    class BinaryMinHeap<T> : IBinaryMinHeap<T> where T : IComparable<T>
    {
        private List<T> buffer = new List<T>();

        private int heapSize = -1;
        private static int parent(int i) => (i - 1) / 2;
        private static int leftChild(int i) => 2 * i + 1;
        private static int rightChild(int i) => 2 * i + 2;
        public int GetLength => buffer.Count;
        private void swap(int parentN, int childN)
        {
            T temp = buffer[parentN];
            buffer[parentN] = buffer[childN];
            buffer[childN] = temp;
        }

        public void Add(T element)
        {
            buffer.Add(element);
            heapSize++;
            HeapMin(heapSize);
        }

        public void HeapMin(int heapSize)
        {
            while (heapSize >= 0 && buffer[heapSize].CompareTo(buffer[parent(heapSize)]) == -1)
            {
                swap(heapSize, parent(heapSize));
                heapSize = parent(heapSize);
            }
        }

        public T Remove()
        {
            T output = buffer[0];
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
            if (left <= heapSize && buffer[left].CompareTo(buffer[pos]) == -1)
            {
                pos = left;
            }
            if (right <= heapSize && buffer[right].CompareTo(buffer[pos]) == -1)
            {
                pos = right;
            }
            if (pos != i)
            {
                swap(pos, i);
                MinHeapify(pos);
            }
        }
    }
}