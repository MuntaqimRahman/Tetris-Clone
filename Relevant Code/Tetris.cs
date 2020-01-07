using MoreLinq;
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Tetris_Game
{
    public partial class FormTetrisGame : Form
    {
        ManualResetEvent man = new ManualResetEvent(true); // Blocks thread for soft droping

        public System.Timers.Timer timeInterval; //Ticks every interval as defined by the level
        public bool switched = false; //Whether a block is switched - stops infinite switiching
        int blockNumber = 0; //The number of the block
        SoundPlayer LockingSound = new SoundPlayer(Tetris_Game.Properties.Resources.Land); // Sound effect for landing
        public char[] blockNames = new char[7] { 't', 'i', 'j', 's', 'z', 'o', 'l' }; // Names of all the blocks
        public char[] nextBlocks = new char[7];
        bool backgroundmusicactive = false; // Flag for background music
        WaveOutEvent BackgroundAudio;
        Random rnd;

        //General object and type to be defined by instance method
        public static object blockInstance;
        public static Type generalType;

        public static bool lockStart; // Flag for locking process
        public static int movesMade; // Number of moves made since flag was set

        //Block stored
        char storedBlock = ' ';

        /// <summary>
        /// Default Constructor for FormTetrisGame
        /// </summary>
        public FormTetrisGame ()
        {
            InitializeComponent();
            setTimer(); // Initialize the properties of the timer
            StartBackgroundMusic(); // Start playing the background music
            //Initial shuffle of the primary array
            rnd = new Random();
            //Order the array randomly to shuffle the array
            blockNames = blockNames.OrderBy(x => rnd.Next()).ToArray();

            //Shuffles the next blocks
            shuffleBlocks();
            createInstance(); //Creates the instance of the specific type of block
            updateNext(); //Updates panel to show the next 3 blocks
            
            try
            {
                Program.GetHighscore(); // Try and get the highscore from the file
            }
            catch { }
        }

        /// <summary>
        /// Handles the loading of the form
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments for the event</param>
        private void FormTetrisGame_Load(object sender, EventArgs e)
        {
            timeInterval.Start(); // Begin the timer
        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        private void Pause()
        {
            if (timeInterval.Enabled) // Switch timer activity
                timeInterval.Stop();
            else
                timeInterval.Start();
        }

        /// <summary>
        /// Initialize and Start Backround Music
        /// </summary>
        private void StartBackgroundMusic()
        {
            UnmanagedMemoryStream sound = Tetris_Game.Properties.Resources.Background;

            MemoryStream ms = new MemoryStream(StreamToBytes(sound));

            LoopStream ws = new LoopStream(new WaveFileReader(ms));

            BackgroundAudio = new WaveOutEvent();
            BackgroundAudio.Volume = 0.5f;
            BackgroundAudio.Init(ws);
            BackgroundAudio.Play();
            
            backgroundmusicactive = true;
        }

        public static byte[] StreamToBytes(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// Initializes the timer
        /// </summary>
        private void setTimer()
        {
            //The interval between each tick
            timeInterval = new System.Timers.Timer(Program.Game.Fall_Speed);

            //Elapsed timer connected to onTimedEvent
            timeInterval.Elapsed += OnTimedEvent;

            //AutoReset set to true
            timeInterval.AutoReset = true;
        }

        /// <summary>
        /// Handles moving the pieces down
        /// </summary>
        /// <param name="sender">Object that sent the event</param>
        /// <param name="e">Arguments for the event</param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try //Try and catch for weird threading and dll errors
            {
                //Checks if the block can go down
                MethodInfo downCheck = generalType.GetMethod("CanGoDown");
                bool canPass = (bool)downCheck.Invoke(blockInstance, null);

                if (!man.WaitOne(0) && canPass) // Soft drop scoring
                    Program.Game.Score += 1;
                if (canPass)
                    Program.lastMove = 'F'; // Update the last move

                //Invoking the down method to move block down
                MethodInfo downMethod = generalType.GetMethod("setPointDown");
                downMethod.Invoke(blockInstance, null);

                canPass = (bool)downCheck.Invoke(blockInstance, null); // Update whether the piece can still go down


                if (!canPass) //If can't pass, lock process begins
                {
                    lockProcess();
                    lockStart = true;
                }

                //If more than 15 moves made since lock (approximate due to time checks) and cannot pass, locks immediately
                if (movesMade > 15 && canPass == false)
                    initiateNextPiece(); //Starts the next piece

                pnlGameBoard.Invalidate(); //Draws again
            }
            catch { }
        }

        /// <summary>
        /// Updates the block preview
        /// </summary>
        private void updateNext()
        {
            int arrayPosition = blockNumber % 7;//The position in the array

            //The filepath to the next blocks
            string next1 = "";
            string next2 = "";
            string next3 = "";


            //Tries to get the next few blocks in the array, if it's out of the bounds goes to the next set of blocks
            try
            {
                next1 += blockNames[arrayPosition + 1];
            }
            catch
            {
                next1 += nextBlocks[arrayPosition - 6];
            }

            try
            {
                next2 += blockNames[arrayPosition + 2];
            }
            catch
            {
                next2 += nextBlocks[arrayPosition - 5];
            }

            try
            {
                next3 += blockNames[arrayPosition + 3];
            }
            catch
            {
                next3 += nextBlocks[arrayPosition - 4];
            }




            //Sets the pbox image to the next blocks
            pboxBlock1.Image = Program.ImageFromName(next1);
            pboxBlock2.Image = Program.ImageFromName(next2);
            pboxBlock3.Image = Program.ImageFromName(next3);
        }

        /// <summary>
        /// Shuffle the positions in nextBlocks
        /// </summary>
        private void shuffleBlocks()
        {
            //Shuffle the nextblocks based off current
            nextBlocks = blockNames.OrderBy(x => rnd.Next()).ToArray();
        }

        /// <summary>
        /// Specify the type of the general object of a tetromino
        /// </summary>
        private void createInstance()
        {
            lockStart = false; // Reset the lock flag

            // Get Class name for new block
            char upperBlock = char.ToUpper(blockNames[blockNumber % 7]);
            string assemblyType = "Tetris_Game." + upperBlock + "_Block";

            // Assign Type
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            generalType = executingAssembly.GetType(assemblyType);

            //Creates the instance once the type is defined
            blockInstance = Activator.CreateInstance(generalType);
        }

        /// <summary>
        /// Sets the next piece after a piece has been locked
        /// </summary>
        public void initiateNextPiece()
        {
            if(checkIfLockOut()) //Checks if the piece locked outside the bounds of the board (a lose case)
            {
                if (timeInterval.Enabled) // Prevent multiple instances
                    loseCase();
                return;
            }

            assignArray(); //Assigns the gameboard with the piece that is locked
            LockingSound.Play();

            //Gets the rotation position and center of the current piece to update score
            MethodInfo getRotNum = generalType.GetMethod("getRotationPosition");
            MethodInfo getCenter = generalType.GetMethod("getCenter");
            Point center = (Point)getCenter.Invoke(blockInstance, null);
            gameCheck.UpdateGameState((byte)getRotNum.Invoke(blockInstance, null), new Point(center.X / 24, 40 - ((center.Y / 24) + 20) - 1), blockNames[blockNumber % 7]);

            //Resets the time interval if level is increased
            timeInterval.Interval = Program.Game.Fall_Speed;
            blockNumber++;//Sets the current piece to the next piece
            createInstance(); //Creates the instance of the new block

            if (checkIfOccupied()) //Checks if the new block spawns where a current blocks exists (a lose case)
            {
                if (timeInterval.Enabled) // Prevent multiple instances
                    loseCase();
                return;
            }

            

            if ((blockNumber%7) == 0) //If at the end of the array
            {
                blockNames = nextBlocks; //Sets the current block name to the next set
                createInstance(); //Creates an instance based off the new array
                shuffleBlocks(); //Shuffles the next blocks
            }

            updateNext(); //Updates the panel

            switched = false; //If switch occurs, now set to false
            
            pnlGameBoard.Invalidate();

        }

        /// <summary>
        /// Begin the locking of the current piece
        /// </summary>
        public async void lockProcess()
        {
            try //For weird dll errors
            {

                int movesMades = 0; //Resets the moves made
                
                //Method info to get points and check if it can go down
                MethodInfo getPointMethod = generalType.GetMethod("getPoints");
                MethodInfo downCheck = generalType.GetMethod("CanGoDown");



                //Gets the points of the block
                Point[] blockPoints = (Point[])getPointMethod.Invoke(blockInstance, null);

                await Task.Delay(500);//Waits half a second


                //Checks if the piece can go down, stops locking if it can
                bool canPass = (bool)downCheck.Invoke(blockInstance, null);
                if (canPass == true)
                    return;


                bool positionMoved = false; //Boolean to see if position moved between lock starting and now

                //The points after 
                Point[] newPoints = (Point[])getPointMethod.Invoke(blockInstance, null);


                for (int i = 0; i < 4; i++) //Checks if the position of the block has moved
                {
                    if (newPoints[i] != blockPoints[i]) //Goes through each point and checks if they are different
                    {
                        positionMoved = true; //Position moved = true
                        movesMades++; //Increase the moves made since lock
                        break;
                    }
                }



                if (!positionMoved) //If the position not moved, locks and sets next piece
                {
                    initiateNextPiece();
                    return;
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// Checks if the current piece locked completely out of the play field
        /// </summary>
        /// <returns>True if the piece locked out, false otherwise</returns>
        private bool checkIfLockOut()
        {
            //Gets the points of the block
            MethodInfo getPointMethod = generalType.GetMethod("getPoints");
            Point[] points = (Point[])getPointMethod.Invoke(blockInstance, null);

            if (points.Max(p => p.Y) < 0) // Return true if above the visible field
                return true;

            return false; //Returns false if not locked outside y bound
        }

        /// <summary>
        /// Checks if the space a new piece would spawn is obstructed
        /// </summary>
        /// <returns>True if obstructed, false otherwise</returns>
        private bool checkIfOccupied()
        {
            //Gets the points of the block
            MethodInfo getPointMethod = generalType.GetMethod("getPoints");
            Point[] points = (Point[])getPointMethod.Invoke(blockInstance, null);

            for (int i = 0; i < 4; i++) //Checks if any blocks in a space that is occupied
            {
                //Converts the position of the point based with origin in top left to the position of the point with origin in bottom left (as game board array is defined)
                char position = Program.Game.Field[(points[i].X / 24), 40 - ((points[i].Y / 24) + 20) - 1];

                if (position != ' ') //If the position not empty, then space occupied
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Finds the position of the blocks relative to the gameboard
        /// </summary>
        private void assignArray()
        {
            //Gets the points of the block
            MethodInfo getPointMethod = generalType.GetMethod("getPoints");
            Point[] blockPoints = (Point[])getPointMethod.Invoke(blockInstance, null);

            //Gets the name of the block
            MethodInfo getCharMethod = generalType.GetMethod("getName");
            char filledCharacter = (char)getCharMethod.Invoke(blockInstance, null);

            //Array has origin based in bottom left instead of top left, so points divided by 24 and translated across the array with the character
            for (int i = 0; i < blockPoints.Length; i++)
                Program.Game.Field[blockPoints[i].X / 24, 40 - ((blockPoints[i].Y / 24) + 20) - 1] = filledCharacter;
        }

        /// <summary>
        /// Handles a lose of the game
        /// </summary>
        private void loseCase()
        {
            //Game paused and all inputs disallowed
            Pause();
            //Stops the background music
            BackgroundAudio.Stop();

            //Allows user to play again or exit
            DialogResult result = MessageBox.Show(String.Format("Game Over!\nYour score was: {0}.\nDo you want to try again?", Program.Game.Score), ":(", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            try
            {
                Program.SaveIfHighscore(); // Try to save the high score
            }
            catch { MessageBox.Show("An error has occured while saving highscores", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            if (result == DialogResult.No) // Player does not want to continue, close the game
                Environment.Exit(0);
            else // Player wants to continue, create a new game
            {
                Program.Game = new GameState(); // Reset the game
                setTimer(); // Reset the timer
                BackgroundAudio.Play();
                
                //Order the array randomly to shuffle the array
                blockNames = blockNames.OrderBy(x => rnd.Next()).ToArray();

                shuffleBlocks(); // Randomize nextblocks
                createInstance(); // Create the initial block
                updateNext(); // Show the preview blocks
                Pause(); // Let the game re-commence
            }
            pnlGameBoard.Invalidate();
        }

        /// <summary>
        /// Paint the Gameboard
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments for the event</param>
        private void pnlGameBoard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Graphics[] parameter = new Graphics[] {g}; //Graphics parameter to late bind draw event

            //Draw Method Info
            MethodInfo drawMethod = generalType.GetMethod("Draw");

            drawMethod.Invoke(blockInstance, parameter); //Draws the current block

            drawPieces(g); // Draw the game field
            DrawGhost(g); // Draw the ghost block

            // Update the scoring boxes
            lblHighScore.Text = Program.HighScore.Score.ToString();
            lblLines.Text = Program.Game.Lines.ToString();
            lblScore.Text = Program.Game.Score.ToString();
            lblLevel.Text = Program.Game.Level.ToString();
        }

        /// <summary>
        /// Draws all of the pieces in the Game Field
        /// </summary>
        /// <param name="g">Graphics object to be drawn upon</param>
        private void drawPieces(Graphics g)
        {
            //Goes through the gameboard
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 20; j++)
                    if (Program.Game.Field[i, j] != ' ') // If the position isn't null, draw the block in that space
                    {
                        //Gets the image in that block
                        Image tetrominoBlock = Program.ImageFromName(Program.Game.Field[i, j] + "-block");

                        //Gets the point defined from origin in top left
                        Point boardPosition = new Point(i * 24, 480 - ((j + 1) * 24));
                        //Draws the image of that block
                        g.DrawImage(tetrominoBlock, boardPosition);
                    }
        }

        /// <summary>
        /// Draws the ghost piece
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawGhost (Graphics g)
        {
            // Clone the current block
            MethodInfo cloneMethod = generalType.GetMethod("Clone");
            object GhostBlock = cloneMethod.Invoke(blockInstance, null);

            MethodInfo downTestMethod = generalType.GetMethod("CanGoDown"); // Get the method to check if a piece can descend
            
            //If can go down true
            if ((bool)downTestMethod.Invoke(GhostBlock, null))
            {
                // Move the block down as far as possible
                MethodInfo downMethod = generalType.GetMethod("setPointDown");
                do
                {
                    downMethod.Invoke(GhostBlock, null);
                } while ((bool)downTestMethod.Invoke(GhostBlock, null) == true);

                // Get the points of the current block
                MethodInfo getPointsMethod = generalType.GetMethod("getPoints");
                object[] parameter = new object[] { g, (Point[])getPointsMethod.Invoke(blockInstance, null)};

                // Draw the ghost block
                MethodInfo drawMethod = generalType.GetMethod("GhostDraw");
                drawMethod.Invoke(GhostBlock, parameter);
            }
        }
        
        /// <summary>
        /// Process the key inputs
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData">Keys that were pressed</param>
        /// <returns>True if the key was handled, false otherwise</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Detect keys that are active during a paused game
            switch (keyData)
            {
                case Keys.Escape: // Pause
                case Keys.F1:
                    Pause();
                    break;
                case Keys.Add: // Increase Music Volume
                    if (BackgroundAudio.Volume < 0.9) BackgroundAudio.Volume += (float)0.1; // Change the volume of the Windows Media Player Instance
                    else BackgroundAudio.Volume = (float)1.0;
                    break;
                case Keys.Subtract: // Decrease Music Volume
                    if (BackgroundAudio.Volume > 0.1) BackgroundAudio.Volume -= (float)0.1; // Change the volume of the Windows Media Player Instance
                    else BackgroundAudio.Volume = (float)0.0;
                    break;
                case Keys.MediaPlayPause: // Start or stop music
                    if (backgroundmusicactive)
                    {
                        BackgroundAudio.Pause();
                        backgroundmusicactive = false;
                    }
                    else
                    {
                        BackgroundAudio.Play();
                        backgroundmusicactive = true;
                    }
                    break;
            }
            if (!timeInterval.Enabled) // If the game isn't active don't handle keys
                return false;
            // Detect keys that are active only during game play
            switch (keyData)
            {
                case Keys.Right: // Move right
                case Keys.NumPad6:
                    MethodInfo rightMethod = generalType.GetMethod("setPointRight");
                    rightMethod.Invoke(blockInstance, null);
                    break;
                case Keys.Left: // Move left
                case Keys.NumPad4:
                    MethodInfo leftMethod = generalType.GetMethod("setPointLeft");
                    leftMethod.Invoke(blockInstance, null);
                    break;
                case Keys.Down: // Soft Drop
                case Keys.NumPad2:
                    if (man.WaitOne(0)) // Make sure that the piece isn't already being soft dropped
                        SoftDrop();
                    break;
                case Keys.X: // Rotate Clockwise
                case Keys.Up:
                case Keys.NumPad1:
                case Keys.NumPad5:
                case Keys.NumPad9:
                    MethodInfo transformMethod = generalType.GetMethod("transformPointsClockwise");
                    transformMethod.Invoke(blockInstance, null);
                    break;
                case Keys.Control | Keys.ControlKey: // Rotate Counter Clockwise
                case Keys.Z:
                case Keys.NumPad3:
                case Keys.NumPad7:
                    MethodInfo ctransformMethod = generalType.GetMethod("transformPointsCounterClockwise");
                    ctransformMethod.Invoke(blockInstance, null);
                    break;
                case Keys.Shift | Keys.ShiftKey: // Hold a piece
                case Keys.C:
                case Keys.NumPad0:
                    Hold();
                    break;
                case Keys.Space: // Hard Drop
                case Keys.NumPad8:
                    HardDrop();
                    break;
            }

            //Invalidate so movement is immediate and smooth
            pnlGameBoard.Invalidate();
            return true;
        }

        /// <summary>
        /// Resets man event for ending soft drop
        /// </summary>
        /// <param name="sender">Object that raised event</param>
        /// <param name="e">Arguments for event</param>
        private void FormTetrisGame_KeyUp(object sender, KeyEventArgs e)
        {
            man.Set(); //Sends signal to soft drop to unblock
        }

        /// <summary>
        /// Holds the current piece
        /// </summary>
        private void Hold ()
        {
            if (storedBlock == ' ')
            {
                switched = true;

                storedBlock = blockNames[blockNumber % 7];

                //Sets the next piece
                blockNumber++;
                createInstance();
                updateNext();


                pboxHoldPiece.Image = Program.ImageFromName(storedBlock.ToString()); //Adds the image of the stored block to hold panel

            } else if (!switched) //Only allows switch if hasn't already switched
            {
                switched = true;//Switched true so user cannot switch again

                //Redefines type to held block
                char upperBlock = char.ToUpper(storedBlock);
                string assemblyType = "Tetris_Game."+upperBlock+"_Block";
                
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                generalType = executingAssembly.GetType(assemblyType);


                //Creates the instance once the type is defined
                blockInstance = Activator.CreateInstance(generalType);

                //Stored block set to current block and image changed to stored block
                storedBlock = blockNames[blockNumber % 7];
                pboxHoldPiece.Image = Program.ImageFromName(storedBlock.ToString());


            }

            
        }

        /// <summary>
        /// Performs a hard drop
        /// </summary>
        private void HardDrop()
        {
            MethodInfo downTestMethod = generalType.GetMethod("CanGoDown");

            if ((bool)downTestMethod.Invoke(blockInstance, null))
            {
                //Keeps moving points down until it no longer can
                MethodInfo downMethod = generalType.GetMethod("setPointDown");
                do
                {
                    downMethod.Invoke(blockInstance, null);
                    Program.Game.Score += 2;
                } while ((bool)downTestMethod.Invoke(blockInstance, null) == true);
            }
            Program.lastMove = 'H'; //Last move set to H for hard drop
            pnlGameBoard.Invalidate(); //Redrawn

            initiateNextPiece(); //Next piece immeidiately called - no lock time given
        }

        /// <summary>
        /// Performs a soft drop
        /// </summary>
        private async void SoftDrop()
        {
            man.Reset(); //Changes boolean to false so threads blocked
            timeInterval.Interval = Program.Game.Fall_Speed / 20; //Gravity changed to quicker time
            await WaitForLift(); //Waits for user to lift
            timeInterval.Interval = Program.Game.Fall_Speed; // Resets the fall speed
        }

        /// <summary>
        /// Blocks soft drop until key up
        /// </summary>
        /// <returns></returns>
        private Task WaitForLift()
        {
            return Task.Factory.StartNew(() =>
            {
                man.WaitOne();
            }
             );
        }

        private void FormTetrisGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete("background.wav");
        }
    }
}
