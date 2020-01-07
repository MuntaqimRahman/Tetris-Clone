using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Game
{
    class pnlGameboardDoubleBuffer:Panel
    {
        public pnlGameboardDoubleBuffer()
        {
            //Sets double buffer to true so no flickering occurs, changed in designer
            this.DoubleBuffered = true;
        }

    }
}
