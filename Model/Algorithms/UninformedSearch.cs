using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder.Model.Algorithms
{
    abstract class UninformedSearch : GraphSearch
    {
        protected int[] directionD1 = { -1, 1, 0, 0};
        protected int[] directionD2 = { 0, 0, 1, -1};
    }
}
