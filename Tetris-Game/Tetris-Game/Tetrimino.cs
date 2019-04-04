using System.Drawing;
using System.Linq;

namespace Tetris_Game
{
    class Tetrimino
    {
        protected Point center;
        protected Image tetriminoBlock;
        private static Image ghostBlock = Tetris_Game.Properties.Resources.ghost_block;
        protected byte rotationPosition;
        protected Point[] points = new Point[4];

        protected char name;

        public char getName()
        {
            return name;
        }

        public object Clone ()
        {
            //A cloned instance of a tetromino
            Tetrimino _out = new Tetrimino();
            _out.center.X = center.X;
            _out.center.Y = center.Y;
            _out.tetriminoBlock = tetriminoBlock.Clone() as Image;
            _out.rotationPosition = rotationPosition;
            for (byte i = 0; i < 4; ++i)
                _out.points[i] = points[i];
            return _out;
        }

        public byte getRotationPosition () { return rotationPosition; }

        //Get-set to define the center
        public Point Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public Point getCenter () { return center; } //Gets the center

        public Point[] getPoints() //Gets the current points
        {
            return points;
        }

        public bool CanGoDown () {
            bool moveDown = true;

            //If above the board, try and catch triggered to stop outside index error
            try
            {
                //Check if down can be done
                for (int i = 0; i < points.Length; i++)
                {
                    //Checks the position below each block in the Field array and checks if null
                    char position = Program.Game.Field[points[i].X / 24, 40 - (((points[i].Y / 24) + 1) + 20) - 1];

                    if (position != ' ') // If not null, position is occupied and block cannot move down
                        moveDown = false;
                }
            }
            catch { }

            //Checks if maximum y point beyond end of board
            int maxY = points.Max(p => p.Y);
            return moveDown && ((maxY + 24) < 480);
        }

        public void setPointDown()
        {
            if (CanGoDown()) //Checks that it can move down and that it is above the floor
            {
                if (FormTetrisGame.lockStart == true) //If in lock position, moves made increases until cap hit
                    FormTetrisGame.movesMade++;

                for (int i = 0; i < points.Length; i++) //Increases the y by one spot
                    points[i].Y += 24;

                center.Y += 24; //Moves the center accordingly
            }
        }

        public void setPointRight()
        {
            //Find the maximum of x in array to find limit
            int maxX = points.Max(p => p.X);

            bool moveRight = true;

            //If above the board, try and catch triggered to stop outside index error
            try
            {
                //Check if down can be done
                for (int i = 0; i < points.Length; i++)
                {
                    //Checks the position below each block in the Field array and checks if null
                    char position = Program.Game.Field[(points[i].X / 24) + 1, 40-((points[i].Y / 24)+20)-1];

                    if (position != ' ') // If not null, position is occupied and block cannot move down
                        moveRight = false;
                }
            }
            catch { }

            if ((maxX + 24) < 240 && moveRight == true) //If within game board bounds and can move right
            {
                if (FormTetrisGame.lockStart == true)//If in lock position, moves made increases until cap hit
                    FormTetrisGame.movesMade++;

                for (int i = 0; i < points.Length; i++) // Moves all points one spot right
                    points[i].X += 24;

                center.X += 24;

                Program.lastMove = 'T';
            }
        }

        public void setPointLeft()
        {
            bool moveLeft = true;

            //Find the minimum of x in array to find limit
            int minX = points.Min(p => p.X);

            //If above the board, try and catch triggered to avoid outside index error
            try
            {
                //Check if down can be done
                for (int i = 0; i < points.Length; i++)
                {
                    //Checks the position below each block in the Field array and checks if null
                    char position = Program.Game.Field[(points[i].X / 24) - 1, 40 - ((points[i].Y / 24) + 20) - 1];

                    if (position != ' ') // If not null, position is occupied and block cannot move down
                        moveLeft = false;
                }
            }
            catch { }
            
            
            if (minX > 0 && moveLeft == true) //If within bounds and can move left, moves block
            {
                if (FormTetrisGame.lockStart == true)//If in lock position, moves made increases until cap hit
                    FormTetrisGame.movesMade++;

                for (int i = 0; i < points.Length; i++) //Moves all points left one spot and center accordingly
                    points[i].X -= 24;

                center.X -= 24;

                Program.lastMove = 'T';
            }   
        }

        public bool canRotate() //Checks if transformation will result in points outside game bounds
        {
            //All of the below calculations only apply to rotating clockwise, slightly different calculations if it's counter clockwise in calculating newY,XR,XL

            //Find the maximum of x in array to find limit
            int maxXd = (points.Max(p => p.X) + 24) - center.X; //Change between the maximum x and centre x
            int newY = center.Y + maxXd;

            int maxYd = (points.Max(p => p.Y) + 24) - center.Y;  //Change between the maximum y and centre y
            int newXL = center.X - maxYd;

            int minYd = center.Y - points.Min(p => p.Y); //Change between the centre x and minimum x
            int newXR = center.X + minYd;

            if (newY <= 480 && newXL >= 0 && newXR <= 240) //Checks if all limits within bounds of the game board
                return true;
            else
                return false;
        }

        public void Draw(Graphics g)
        {
            //Draws the 4 blocks of the tetris piece
            for (int i =0; i < points.Length; i++)
                g.DrawImage(tetriminoBlock,points[i]);
        }

        public void GhostDraw(Graphics g, Point[] nonGhostPoints)
        {
            //Draws the 4 blocks of the ghost piece
            for (int i = 0; i < points.Length; i++)
                if (!nonGhostPoints.Contains(points[i]))
                    g.DrawImage(ghostBlock, points[i].X, points[i].Y, 24, 24);
        }

        private bool transformCheck(Point[] newPoints) //Checks if transformations will transform into space already occupied
        {
            try //Try and check for transformation above the board which is out of index
            {
                for (int i = 0; i < newPoints.Length; i++) //Checks if the points after being transformed are occupied
                    if (Program.Game.Field[(newPoints[i].X / 24), 40-((newPoints[i].Y / 24)+20)-1] != ' ')
                        return false;

                return true;
            }
            catch
            {
                return true;
            }
        }
        
        public void wallkickCheck(Point[] rotatedPoints, bool clockwise)
        {
            Point[] gamePoints = new Point[4];
            Point[] transformedPoints = new Point[4];
            
            //Convert points to gamefield points with different origin location
            for (int i = 0; i < gamePoints.Length; i++)
                gamePoints[i] = new Point(rotatedPoints[i].X / 24, 40 - ((rotatedPoints[i].Y / 24) + 20)-1);

            for (int i = 0; i < 5; i++) //Traversing through the offsets
                for (int j = 0; j < 5; j++) //Transversing through each point 
                {
                    int correctOutputs = 0;

                    int xOff = 0, yOff = 0;//Offset that applies

                    for (int k = 0; k < 4; k++)//Applies to each point
                    {
                        int newX = 0, newY = 0;
                        

                        if (name != 'i') // I is atypical and needs special case
                        {
                            //Goes to defined row in offset table and applies the offset
                            if (clockwise == true)
                            {
                                xOff = offsets[rotationPosition, j].X;
                                yOff = offsets[rotationPosition, j].Y;
                            }
                            else
                            {
                                int offsetPosition = ((((2 - rotationPosition) % 4) + 4) % 4); //Proper mod function with negative inputs (c# mod only finds remainder)

                                xOff = offsets[offsetPosition, j].X;
                                yOff = offsets[offsetPosition, j].Y;
                            }

                            
                        }else
                        {
                            //Goes to defined row in offset table and applies the offset
                            if (clockwise == true)
                            {
                                xOff = I_offsets[rotationPosition, j].X;
                                yOff = I_offsets[rotationPosition, j].Y;
                                
                            }
                            else
                            {
                                xOff = I_offsets[(rotationPosition + 1) % 4, j].X;
                                yOff = I_offsets[(rotationPosition + 1) % 4, j].Y;
                            }
                        }

                        //Applies the offsets to new x and y
                        newX = gamePoints[k].X + xOff; 
                        newY = gamePoints[k].Y + yOff;

                        transformedPoints[k] = new Point(newX, newY); //Puts new point in the array

                        if ((newX < 0 || newX > 9) || ((newY > 39) || (newY < 0))) // Checks that newX and Y are within the bounds of the array
                            break;

                        char position = Program.Game.Field[transformedPoints[k].X, transformedPoints[k].Y]; //Gets the position of the transformed point

                        if (position != ' ') //If position occupied, loop breaks and 
                            break;

                        correctOutputs++;
                    }

                    if (correctOutputs == 4) //If all points within bounds and not in an occupied space, points set to transformed points
                    {
                        if (FormTetrisGame.lockStart == true) //If in locked state, moves made added to
                            FormTetrisGame.movesMade++;

                            //Center moved accordinly
                            center.X += xOff * 24;
                            center.Y -= yOff * 24;
                        

                        for (int k = 0; k < 4; k++) //Sets points to transformed points and converts to draw origin base
                        {
                            points[k] = new Point(transformedPoints[k].X*24, 480 - ((transformedPoints[k].Y +1)* 24));
                        }

                        //Changes the rotation position accordingly
                        if (clockwise == true) 
                        {
                            rotationPosition = (byte)((rotationPosition + 1) % 4);
                        } else
                        {
                            rotationPosition = (byte)((rotationPosition + 3) % 4);
                        }

                        //Tells program last move was a rotation
                        Program.lastMove = 'R';
                        Program.lastSuccesfulRotationTest = (byte)j;
                        return;
                    }
                }
        }

        public void transformPointsClockwise()
        {
            //Set newpoints to the previous points to be transformed later
            Point[] newPoints = new Point[4];
            for (int i = 0; i < newPoints.Length; i++)
                newPoints[i] = points[i];


            for (int i = 0; i < newPoints.Length; i++)
            {
                //Adds 24 to move edge down so point being transformed will form left side (as rect in c# starts at top left)
                newPoints[i].Y += 24;

                //Change of the new point between the center
                int xChange = newPoints[i].X - center.X;
                int yChange = newPoints[i].Y - center.Y;

                //New x and y point with the negative reciprocal from the center
                newPoints[i].X = center.X - yChange;
                newPoints[i].Y = center.Y + xChange;
            }

            if (transformCheck(newPoints) == true && canRotate() == true) //Checks that transformation not outside bounds or overlapped
            {
                //Sets points to transformed points and changes rotation position
                points = newPoints;
                Program.lastMove = 'R';
                rotationPosition = (byte)((rotationPosition + 1) % 4);

                if (FormTetrisGame.lockStart == true) //If in lock state, alters moves made
                    FormTetrisGame.movesMade++;
            }
            else
                wallkickCheck(newPoints,true); //If transformation not allowed, checks if wallkick allowed
        }

        public void transformPointsCounterClockwise()
        {
            
                //Set newpoints to the previous points to be transformed later
                Point[] newPoints = new Point[4];
                for (int i = 0; i < newPoints.Length; i++)
                    newPoints[i] = points[i];

                for (int i = 0; i < newPoints.Length; i++)
                {
                    ///Adds 24 to move edge right so point being transformed will form top side (as rect in c# start in top left)
                    newPoints[i].X += 24;

                    //Change of the new point between the center
                    int xChange = newPoints[i].X - center.X;
                    int yChange = newPoints[i].Y - center.Y;

                    //New x and y point with the negative reciprocal from the center
                    newPoints[i].X = center.X + yChange;
                    newPoints[i].Y = center.Y - xChange;
                }

                if (transformCheck(newPoints) == true && canRotate() == true)//Checks that transformation not outside bounds or overlapped
                {
                    //Sets points to transformed points and changes rotation position
                    points = newPoints;
                    rotationPosition = (byte)((rotationPosition + 3) % 4);

                    Program.lastMove = 'R';

                    if (FormTetrisGame.lockStart == true)//If in lock state, alters moves made
                    {
                        FormTetrisGame.movesMade++;
                    }

                }
            else
            {
                wallkickCheck(newPoints,false);//If transformation not allowed, checks if wallkick allowed
            }
                

        }

        //Offset tables for wallkicks
        public static readonly Offset[,] offsets = new Offset[,] {{new Offset(0,0), new Offset(-1,0), new Offset(-1,1), new Offset(0,-2), new Offset(-1,-2)},  // 0 >> 1, 2 >> 1
													              {new Offset(0,0), new Offset(1,0), new Offset(1,-1), new Offset(0,2), new Offset(1,2)},      // 1 >> 0, 1 >> 2
												                  {new Offset(0,0), new Offset(1,0), new Offset(1,1), new Offset(0,-2), new Offset(1,-2)},     // 2 >> 3, 0 >> 3
													              {new Offset(0,0), new Offset(-1,0), new Offset(-1,-1), new Offset(0,2), new Offset(-1,2)}};  // 3 >> 2, 3 >> 0

        public static readonly Offset[,] I_offsets = new Offset[,] {{new Offset(0,0), new Offset(-2,0), new Offset(1,0), new Offset(-2,-1), new Offset(1,2)},  // 0 >> 1, 3 >> 2
													                {new Offset(0,0), new Offset(-1,0), new Offset(2,0), new Offset(-1,2), new Offset(2,-1)},  // 1 >> 2, 0 >> 3
                                                                    {new Offset(0,0), new Offset(2,0), new Offset(-1,0), new Offset(2,1), new Offset(-1,-2)},  // 1 >> 0, 2 >> 3
													                {new Offset(0,0), new Offset(1,0), new Offset(-2,0), new Offset(1,-2), new Offset(-2,1)}}; // 2 >> 1, 3 >> 0

        public class Offset
        {
            public short X;
            public short Y;

            public Offset(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

    }
}
