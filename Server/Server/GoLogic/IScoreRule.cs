using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic
{
    public interface IScoreRule
    {
        public int CalculateScoreBlack(int blackTerritory);

        public int CalculateScoreWhite(int whiteTerritory);
    }
}
