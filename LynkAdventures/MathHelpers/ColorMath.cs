using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace LynkAdventures.MathHelpers
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// Helper class for color related methods.
    /// </summary>
    public class ColorMath
    {
        /// <summary>
        /// Converts ABGR color to ARGB.
        /// </summary>
        /// <param name="abgr">The ABGR color.</param>
        /// <returns>ARGB color.</returns>
        public static uint ToARGB(uint abgr)
        {
            uint alphaAndGreen = abgr & 0xFF00FF00;
            uint blue = (abgr & 0x00FF0000) >> 16;
            uint red = (abgr & 0x000000FF) << 16;

            return alphaAndGreen | blue | red;
        }

        /// <summary>
        /// Converts ARGB color to ABGR.
        /// </summary>
        /// <param name="argb">The ARGB color.</param>
        /// <returns>ABGR color.</returns>
        public static uint ToABGR(uint argb)
        {
            uint alphaAndGreen = argb & 0xFF00FF00;
            uint red = (argb & 0x00FF0000) >> 16;
            uint blue = (argb & 0x000000FF) << 16;

            return alphaAndGreen | red | blue;
        }

        /// <summary>
        /// Converts ABGR color to <see cref="Microsoft.Xna.Framework.Color"/>.
        /// </summary>
        /// <param name="abgr">The ABGR color.</param>
        /// <returns><see cref="Microsoft.Xna.Framework.Color"/> represenation of the color.</returns>
        public static Color ToColor(uint abgr)
        {
            uint alpha = (abgr & 0xFF000000) >> 24;
            uint blue = (abgr & 0x00FF0000) >> 16;
            uint green = (abgr & 0x0000FF00) >> 8;
            uint red = abgr & 0x000000FF;

            return new Color((int)red, (int)green, (int)blue, (int)alpha);
        }

        /// <summary>
        /// Gets the most used color in the given texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <returns>The most used color in the used texture.</returns>
        public static Color GetMostUsedColor(Texture2D texture)
        {
            uint[] pixels = new uint[texture.Width * texture.Height];
            texture.GetData(pixels);
            return ToColor(pixels.ToList().GroupBy(col => col).OrderBy(g => g.Count()).ElementAt(0).Key);
        }
    }
}
