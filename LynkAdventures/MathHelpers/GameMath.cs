using Microsoft.Xna.Framework;
using System;

namespace LynkAdventures.MathHelpers
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// Math helper for game math.
    /// </summary>
    public class GameMath
    {
        public const int CHAR_BITS = 8;
        public const int SIGN_MATH_HELPER_INT = 31;

        /// <summary>
        /// Determines whether the two values are almost the same.
        /// </summary>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="epsilon">The largest difference between the numbers to be considered as almost same. Default is 0.</param>
        /// <returns><c>true</c>, if both numbers are almost the same; otherwise <c>false</c>.</returns>
        public static bool AlmostSame(int val1, int val2, int epsilon = 0)
        {
            return Math.Abs(val1 - val2) <= epsilon;
        }

        /// <summary>
        /// Gets the sign of the number.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns>-1, if the number is negative, 1, if number is 0 or larger.</returns>
        public static int GetSign(int num)
        {
            return 1 | (num >> SIGN_MATH_HELPER_INT);
        }
    }
}
