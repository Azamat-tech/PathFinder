using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Path_Finder
{
    class Position
    {
        public int x, y;
        public Position(int posX, int posY)
        {
            x = posX;
            y = posY;
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return (pos1.x == pos2.x && pos1.y == pos2.y);
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return (pos1.x != pos2.x && pos1.y != pos2.y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
