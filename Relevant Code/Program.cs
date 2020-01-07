using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Tetris_Game
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Game = new GameState(); // Create a new gamestate
            binaryFormat = new BinaryFormatter(); // Initialize the binaryformatter
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormTetrisGame());
        }

        public static GameState HighScore; // Gamestate for highscore
        public static GameState Game; // Current gamestate
        public static BinaryFormatter binaryFormat; // Formatter for getting and saving highscores

        public static char lastMove = ' '; // The last successful move of the active block
        public static byte lastSuccesfulRotationTest = 0; // The last successful rotation test

        /// <summary>
        /// Tries to load the previous highscore
        /// </summary>
        public static void GetHighscore()
        {
            if (File.Exists("score.dat"))
                using (var Stream = new FileStream("score.dat", FileMode.Open))
                    HighScore = binaryFormat.Deserialize(Stream) as GameState;
            else
                HighScore = new GameState();
        }

        /// <summary>
        /// Tries to save the current score if it is the newest highscore
        /// </summary>
        public static void SaveIfHighscore ()
        {
            if (Game.CompareTo(HighScore) == 1)
            {
                using (var Stream = new FileStream("score.dat", FileMode.Create))
                    binaryFormat.Serialize(Stream, Game as object);
                HighScore = Game;
            }
        }

        public static Image ImageFromName (string Name)
        {
            switch (Name)
            {
                case "i":
                    return Tetris_Game.Properties.Resources.i;
                case "j":
                    return Tetris_Game.Properties.Resources.j;
                case "l":
                    return Tetris_Game.Properties.Resources.l;
                case "o":
                    return Tetris_Game.Properties.Resources.o;
                case "s":
                    return Tetris_Game.Properties.Resources.s;
                case "t":
                    return Tetris_Game.Properties.Resources.t;
                case "z":
                    return Tetris_Game.Properties.Resources.z;
                case "i-block":
                    return Tetris_Game.Properties.Resources.i_block;
                case "j-block":
                    return Tetris_Game.Properties.Resources.j_block;
                case "l-block":
                    return Tetris_Game.Properties.Resources.l_block;
                case "o-block":
                    return Tetris_Game.Properties.Resources.o_block;
                case "s-block":
                    return Tetris_Game.Properties.Resources.s_block;
                case "t-block":
                    return Tetris_Game.Properties.Resources.t_block;
                case "z-block":
                    return Tetris_Game.Properties.Resources.z_block;
                default:
                    return null;
            }
        }
    }
}
