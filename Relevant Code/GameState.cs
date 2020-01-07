using System;
using System.Runtime.Serialization;

namespace Tetris_Game
{
    [Serializable]
    public class GameState : ISerializable, IComparable<GameState>
    {
        public char[,] Field;
        public ulong Score;
        public ulong Lines;
        public ulong B2B; // The number of back to back high-value scorings
        public ulong ComboCount; // Number of moves in a row

        /// <summary>
        /// Gives the fall speed in seconds per line dropped
        /// </summary>
        public double Fall_Speed => Math.Pow(0.8 - ((Level - 1) * 0.007), Level - 1) * 1000;

        /// <summary>
        /// Gives the current level
        /// </summary>
        public ulong Level => (ulong)Math.Floor(Math.Sqrt((Lines + 0.625) / 2.5) - 0.5) + 1;

        public GameState()
        {
            Score = 0;
            Lines = 0;
            B2B = 0;
            ComboCount = 0;

            // Initialize the field
            Field = new char[10,40];
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 40; ++j)
                    Field[i, j] = ' ';
        }

        public GameState(SerializationInfo info, StreamingContext context)
        {
            Score = (ulong)info.GetValue("Score", typeof(ulong));
            Lines = (ulong)info.GetValue("Lines", typeof(ulong));
            B2B = 0;
            ComboCount = 1;

            Field = new char[10, 40];
            for (int i = 0; i < 10; ++i)
                for (int j = 0; j < 40; ++j)
                    Field[i, j] = ' ';
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Score", Score, typeof(ulong));
            info.AddValue("Level", Level, typeof(ulong));
            info.AddValue("Lines", Lines, typeof(ulong));
        }

        /// <summary>
        /// Compares two GameStates
        /// </summary>
        /// <param name="other">The game state to compare to</param>
        /// <returns>1 if greater, 0 if equal, -1 if less</returns>
        public int CompareTo(GameState other)
        {
            return Score.CompareTo(other.Score);
        }
    }
}
