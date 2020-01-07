﻿using System.Drawing;

namespace Tetris_Game
{
    class S_Block : Tetrimino
    {
        //Constructor setting block to its image, default center that is within grid intervals, finds new points and sets name to block name
        public S_Block()
        {
            tetriminoBlock = Tetris_Game.Properties.Resources.s_block;
            center = new Point(132, -12);
            FormTetrisGame.movesMade = 0;
            findPoints();

            name = 's';
        }

        public void findPoints() //Defines all other points based off center
        {

            points[0] = new Point(center.X - 36, center.Y - 12);
            points[1] = new Point(center.X - 12, center.Y - 12);
            points[2] = new Point(center.X - 12, center.Y - 36);
            points[3] = new Point(center.X + 12, center.Y - 36);
            

        }
    }
}
