using System;
using System.Collections;
using System.Collections.Generic;

namespace Path_Finder.Algorithms
{
    /// <summary>
    /// The implementation of a Dijkstras algorithm that inherits 
    /// from the UniformSearch using the priority queue in picking 
    /// the nodes from the frontier. 
    /// </summary>
    class Dijkstra : UniformSearch
    {
        private PriorityQueue<Position, int> pQueue = new PriorityQueue<Position, int>(); 
    }
}
