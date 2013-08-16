using LynkAdventures.Graphics;
using LynkAdventures.MathHelpers;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Grass,
    /// </summary>
    public class TileGrass : Tile
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TileGrass"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public TileGrass(Level level)
            : base(level)
        {
            spriteID = 0;
            TileColor = ColorMath.GetMostUsedColor(Renderer.GetTextureFromSpriteSheet(spriteID, GameSpriteSheets.SPRITESHEET_TILES));
        }

        /// <summary>
        /// Renders the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public override void Render(int xTile, int yTile, Renderer renderer, Level level, byte data = 0)
        {
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID + data, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE);
        }
    }
}
