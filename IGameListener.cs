using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    public interface IGameListener
    {
        void GameOver(int winner);
    }
}
