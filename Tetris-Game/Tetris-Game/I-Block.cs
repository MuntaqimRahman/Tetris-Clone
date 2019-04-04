using System.Drawing;

namespace Tetris_Game
{
    class I_Block : Tetrimino
    {
        //Constructor setting block to its image, default center that is within grid intervals, finds new points and sets name to block name
        public I_Block()
        {
            tetriminoBlock = Tetris_Game.Properties.Resources.i_block;
            center = new Point(120,0);
            FormTetrisGame.movesMade = 0;
            findPoints();

            name = 'i';
        }

        public void findPoints()//Defines all other points based off center
        {
            points[0] = new Point(center.X - 48, center.Y - 24);
            points[1] = new Point(center.X - 24, center.Y - 24);
            points[2] = new Point(center.X, center.Y - 24);
            points[3] = new Point(center.X + 24, center.Y - 24);
        }
    }
}
