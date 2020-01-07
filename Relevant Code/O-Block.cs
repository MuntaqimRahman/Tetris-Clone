using System.Drawing;

namespace Tetris_Game
{
    class O_Block: Tetrimino
    {
        //Constructor setting block to its image, default center that is within grid intervals, finds new points and sets name to block name
        public O_Block()
        {
            tetriminoBlock = Tetris_Game.Properties.Resources.o_block;
            center = new Point(120, -24);
            FormTetrisGame.movesMade = 0;
            findPoints();

            name = 'o';
        }

        public void findPoints()//Defines all other points based off center
        {
            points[0] = new Point(center.X - 24, center.Y - 24);
            points[1] = new Point(center.X , center.Y - 24);
            points[2] = new Point(center.X - 24, center.Y);
            points[3] = new Point(center.X , center.Y);
        }
    }
}
