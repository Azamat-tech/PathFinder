using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder.Algorithms
{
    abstract class UninformedSearch : GraphSearch
    {
        protected int[] directionR = { -1, 1, 0, 0 };
        protected int[] directionC = { 0, 0, 1, -1 };
    }
}
