using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.Graphics.Sprites
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A texture that contains smaller textures of the same size.
    /// </summary>
    public class SpriteSheet
    {
        private int spriteWidth, spriteHeight;
        private int sheetWidth, sheetHeight;
        private Texture2D spriteSheetTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteSheet"/>.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet texture.</param>
        /// <param name="spriteWidth">Width of a single sprite.</param>
        /// <param name="spriteHeight">Height of a single sprite.</param>
        public SpriteSheet(Texture2D spriteSheet, int spriteWidth = 32, int spriteHeight = 32)
        {
            this.spriteSheetTexture = spriteSheet;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.sheetWidth = spriteSheet.Width / spriteWidth;
            this.sheetHeight = spriteSheet.Height / spriteHeight;
        }

        /// <summary>
        /// Gets the width of a single sprite.
        /// </summary>
        /// <value>
        /// The width of a single sprite.
        /// </value>
        public int SpriteWidth { get { return spriteWidth; } }

        /// <summary>
        /// Gets the height of a single sprite.
        /// </summary>
        /// <value>
        /// The height of a single sprite.
        /// </value>
        public int SpriteHeight { get { return spriteHeight; } }

        /// <summary>
        /// Gets the amount of sprites in Y plane.
        /// </summary>
        /// <value>
        /// The amount of sprites in Y plane.
        /// </value>
        public int SpriteAmountHeight { get { return sheetHeight; } }

        /// <summary>
        /// Gets the amount of sprites in X plane.
        /// </summary>
        /// <value>
        /// The amount of sprites in X plane.
        /// </value>
        public int SpriteAmountWidth { get { return sheetWidth; } }

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get { return spriteSheetTexture; } }

        /// <summary>
        /// Gets the coordinates and the size of a single sprite in the sprite sheet. Used in rendering.
        /// </summary>
        /// <param name="x">The x in the sprite sheet grid.</param>
        /// <param name="y">The y in the sprite sheet grid.</param>
        /// <returns><see cref="Microsoft.Xna.Framework.Rectangle"/> representation of a sprite for use in rendering.</returns>
        public Rectangle GetSpriteBoundaries(int x, int y)
        {
            return new Rectangle(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight);
        }

        /// <summary>
        /// Gets the coordinates and the size of a single sprite in the sprite sheet. Used in rendering.
        /// </summary>
        /// <param name="spriteID">The sprite ID.</param>
        /// <returns><see cref="Microsoft.Xna.Framework.Rectangle"/> representation of a sprite for use in rendering.</returns>
        public Rectangle GetSpriteBoundaries(int spriteID)
        {
            return new Rectangle((spriteID % sheetWidth) * spriteWidth, (spriteID / sheetWidth) * spriteHeight, spriteWidth, spriteHeight);
        }

        /// <summary>
        /// Gets a copy of a single sprite.
        /// </summary>
        /// <param name="spriteID">The sprite ID.</param>
        /// <returns><see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> representation of a single sprite.</returns>
        public Texture2D GetSpriteData(int spriteID)
        {
            int xSprite = spriteID % sheetWidth;
            int ySprite = spriteID / sheetWidth;
            int xOffs = xSprite * spriteWidth;
            int yOffs = ySprite * spriteHeight;

            uint[] pixels = new uint[spriteWidth * spriteHeight];
            uint[] sheetPixels = new uint[spriteSheetTexture.Width * spriteSheetTexture.Height];
            spriteSheetTexture.GetData(sheetPixels);

            for (int y = 0; y < spriteHeight; y++)
            {
                for (int x = 0; x < spriteWidth; x++)
                {
                    pixels[x + y * spriteWidth] = sheetPixels[(x + xOffs) + (y + yOffs) * spriteSheetTexture.Width];
                }
            }

            Texture2D result = new Texture2D(spriteSheetTexture.GraphicsDevice, spriteWidth, spriteHeight);
            result.SetData(pixels);

            return result;
        }

        /// <summary>
        /// Cuts the into sprites.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="spriteWidth">Width of a single sprite.</param>
        /// <param name="spriteHeight">Height of a single sprite.</param>
        /// <returns>A 2D array of a sprite sheet.</returns>
        public static Texture2D[,] CutIntoSprites(Texture2D texture, int spriteWidth, int spriteHeight)
        {
            int columns = texture.Width / spriteWidth;
            int rows = texture.Height / spriteHeight;
            Texture2D[,] result = new Texture2D[rows, columns];

            uint[] texturePixels = new uint[texture.Width * texture.Height];
            texture.GetData<uint>(texturePixels);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    uint[] pixels = new uint[spriteWidth * spriteHeight];
                    for (int y = 0; y < spriteHeight; y++)
                        for (int x = 0; x < spriteWidth; x++)
                            pixels[x + y * spriteWidth] = texturePixels[(x + column * spriteWidth) + (y + row * spriteHeight) * texture.Width];

                    result[row, column] = new Texture2D(texture.GraphicsDevice, spriteWidth, spriteHeight);
                    result[row, column].SetData(pixels);
                }
            }
            return result;
        }
    }
}
