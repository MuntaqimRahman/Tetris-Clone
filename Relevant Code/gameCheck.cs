using System.Collections.Generic;
using System.Drawing;
using System.Media;

namespace Tetris_Game
{
    public static class gameCheck
    {
        // Lines Scoring Types
        private enum ScoringType : byte { Single, Double, Triple, Tetris, None }

        static SoundPlayer LineClearSound = new SoundPlayer(Tetris_Game.Properties.Resources.LineClear); // Sound for line clears
        static SoundPlayer LevelUpSound = new SoundPlayer(Tetris_Game.Properties.Resources.LevelUp); // Sound for levelup

        /// <summary>
        /// Detects T-Spins
        /// </summary>
        /// <param name="currentPiece">The current type of Tetrimino</param>
        /// <param name="rot_position">The Rotation position of the block (0-3)</param>
        /// <param name="p">The center of the Tetrimino on the game field</param>
        /// <returns>' ' for No T-Spin, 'M' for Mini T-Spin, 'T' for T-Spin</returns>
        private static char Detect_T_Spin(char currentPiece, byte rot_position, Point p)
        {
            if (currentPiece == 't' && Program.lastMove == 'R') // Check for possiblility of a T-Spin
            {
                // Get the occupation of the corners
                byte[] Corners = new byte[4];
                Corners[0] = (byte)((p.X + 1 > 9) ? 1 : (Program.Game.Field[p.X + 1, p.Y + 1] != ' ') ? 1 : 0); // Top Right
                Corners[1] = (byte)((p.X - 1 < 0) ? 1 : (Program.Game.Field[p.X - 1, p.Y + 1] != ' ') ? 1 : 0); // Top Left
                Corners[2] = (byte)((p.X + 1 > 9) ? 1 : (p.Y - 1 < 0) ? 1 : (Program.Game.Field[p.X + 1, p.Y - 1] != ' ') ? 1 : 0); // Bottom Right
                Corners[3] = (byte)((p.X - 1 > 9) ? 1 : (p.Y - 1 < 0) ? 1 : (Program.Game.Field[p.X - 1, p.Y - 1] != ' ') ? 1 : 0); // Bottom Left

                // Count the number of filled edges
                byte filledEdges = 0;
                filledEdges += (byte)((p.X + 1 > 9) ? 1 : (Program.Game.Field[p.X + 1, p.Y] != ' ') ? 1 : 0);
                filledEdges += (byte)((p.X - 1 < 0) ? 1 : (Program.Game.Field[p.X - 1, p.Y] != ' ') ? 1 : 0);
                filledEdges += (byte)((p.Y - 1 < 0) ? 1 : (Program.Game.Field[p.X, p.Y - 1] != ' ') ? 1 : 0);
                filledEdges += (byte)((Program.Game.Field[p.X, p.Y + 1] != ' ') ? 1 : 0);

                // Count the number of filled corners
                byte filledCorners = 0;
                foreach (byte b in Corners)
                    filledCorners += b;

                if (filledCorners >= 3 && Program.lastSuccesfulRotationTest == 5) // Rotation test 5 is always T-Spin
                    return 'T';

                switch (rot_position) // Rotate which corners are checked based on where the block is facing
                {
                    case 0:
                        if (Corners[0] == 1 && Corners[1] == 1 && (Corners[2] == 1 || Corners[3] == 1))
                            return 'T';
                        if (Corners[2] == 1 && Corners[3] == 1 && (Corners[1] == 1 || Corners[0] == 1))
                            return 'M';
                        break;
                    case 1:
                        if (Corners[0] == 1 && Corners[2] == 1 && (Corners[1] == 1 || Corners[3] == 1))
                            return 'T';
                        if (Corners[1] == 1 && Corners[3] == 1 && (Corners[2] == 1 || Corners[0] == 1))
                            return 'M';
                        break;
                    case 2:
                        if (Corners[2] == 1 && Corners[3] == 1 && (Corners[0] == 1 || Corners[1] == 1))
                            return 'T';
                        if (Corners[1] == 1 && Corners[0] == 1 && (Corners[2] == 1 || Corners[3] == 1))
                            return 'M';
                        break;
                    case 3:
                        if (Corners[3] == 1 && Corners[1] == 1 && (Corners[2] == 1 || Corners[0] == 1))
                            return 'T';
                        if (Corners[2] == 1 && Corners[0] == 1 && (Corners[1] == 1 || Corners[3] == 1))
                            return 'M';
                        break;
                }
            }
            return ' ';
        }

        /// <summary>
        /// Determines the new state of the game after a piece is locked
        /// </summary>
        /// <param name="RotationPosition">(0-3) represents the rotation of the locked piece going clockwise 90(used for T-Spins)</param>
        /// <param name="P">Point representing the origin of the locked piece(used for T-Spins)</param>
        /// <param name="CurrentPiece">The type of Tetrimino that was locked</param>
        /// <param name="LastMove">The last change in the piece(Rotation, Translation, Fall) (Used for T-Spins)</param>
        /// <param name="RotationTestNumber">The first successful test for a rotation (Used for T-Spin)</param>
        public static void UpdateGameState(byte RotationPosition, Point P, char CurrentPiece)
        {
            var lines = GetLineScores(CheckForLines()); // Get the number of lines cleared
            char t_spin = Detect_T_Spin(CurrentPiece, RotationPosition, P); // Detect whether a T-Spin has occurred
            ulong score = 0; // Score change

            if (lines == ScoringType.None && t_spin == ' ') // No Scoring
            {
                Program.Game.ComboCount = 0;
            }
            else if (lines == ScoringType.None && t_spin == 'T') // T Spin no lines
            {
                ++Program.Game.ComboCount;
                score += 400 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.None && t_spin == 'M') // T Spin Mini no lines
            {
                ++Program.Game.ComboCount;
                score += 100 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Single && t_spin == ' ') // Single
            {
                ++Program.Game.ComboCount;
                score += 100 * Program.Game.Level;
                Program.Game.B2B = 0;
            }
            else if (lines == ScoringType.Single && t_spin == 'T') // T Spin Single
            {
                ++Program.Game.ComboCount;
                score += 800 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Single && t_spin == 'M') // T Spin Mini Single
            {
                ++Program.Game.ComboCount;
                score += 200 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Double && t_spin == ' ') // Double
            {
                ++Program.Game.ComboCount;
                score += 300 * Program.Game.Level;
                Program.Game.B2B = 0;
            }
            else if (lines == ScoringType.Double && t_spin == 'T') // T Spin Double
            {
                ++Program.Game.ComboCount;
                score += 1200 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Double && t_spin == 'M') // T Spin Mini Double
            {
                ++Program.Game.ComboCount;
                score += 400 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Triple && t_spin == ' ') // Triple
            {
                ++Program.Game.ComboCount;
                score += 500 * Program.Game.Level;
                Program.Game.B2B = 0;
            }
            else if (lines == ScoringType.Triple && t_spin == 'T') // T Spin Triple
            {
                ++Program.Game.ComboCount;
                score += 1600 * Program.Game.Level;
                ++Program.Game.B2B;
            }
            else if (lines == ScoringType.Tetris) // Tetris
            {
                ++Program.Game.ComboCount;
                score += 800 * Program.Game.Level;
                ++Program.Game.B2B;
            }

            if (Program.Game.B2B > 1) // Back to back scoring
                score = (ulong)(1.5 * score);
            if (Program.Game.ComboCount > 1) // Combo scoring
                score += 50 * Program.Game.ComboCount * Program.Game.Level;
            Program.Game.Score += score; // Update score
        }

        /// <summary>
        /// Finds filled lines
        /// </summary>
        /// <returns>A List<byte> Containing all of the filled lines</returns>
        private static List<byte> CheckForLines()
        {
            List<byte> filledLines = new List<byte>();
            for (byte i = 0; i < 40; ++i) // For all lines
            {
                byte notFilled = 0;
                for (byte j = 0; j < 10; ++j) // Count the number of unfilled boxes
                    if (Program.Game.Field[j, i] == ' ')
                        ++notFilled;
                if (notFilled == 0) // If its full add it to the list
                    filledLines.Add(i);
                else if (notFilled == 10) // If its empty stop searching
                    break;
            }
            return filledLines;
        }

        /// <summary>
        /// Determines the type of scoring for a set of lines
        /// </summary>
        /// <param name="FilledLines">Lines that are filled</param>
        /// <returns>The type of scoring to be applied</returns>
        private static ScoringType GetLineScores(List<byte> FilledLines)
        {
            Gravity(FilledLines); // Remove filled lines and shift everything down
            var prevLevel = Program.Game.Level; // Store prev level
            Program.Game.Lines += (ulong)FilledLines.Count; // Increase the line count number
            if (FilledLines.Count != 0) // Play a sound for lines cleared
                LineClearSound.Play();
            if (prevLevel != Program.Game.Level) // Play a sound for level change
                LevelUpSound.Play();
            switch (FilledLines.Count) // Return a scoring type
            {
                case 0:
                    return ScoringType.None;
                case 1:
                    return ScoringType.Single;
                case 2:
                    return ScoringType.Double;
                case 3:
                    return ScoringType.Triple;
                case 4:
                    return ScoringType.Tetris;
                default:
                    return ScoringType.None;
            }
        }

        /// <summary>
        /// Eliminate filled lines and shift down playfield
        /// </summary>
        /// <param name="FilledLines">Lines to be eliminated</param>
        private static void Gravity(List<byte> FilledLines)
        {
            byte offset = 0;
            for (byte i = 0; i < 40; ++i)
            {
                while (FilledLines.Contains((byte)(i + offset))) // Increment offset until an unfilled line is reacherd
                    ++offset;
                for (int j = 0; j < 10; ++j) // Fill current row
                    Program.Game.Field[j, i] = (i + offset < 40) ? Program.Game.Field[j, i + offset] : ' ';
            }
        }
    }
}
