using GoLogic.Goban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic
{
    public interface IMoveValidator
    {
        bool IsValidMove(Stone stone);
    }
}
