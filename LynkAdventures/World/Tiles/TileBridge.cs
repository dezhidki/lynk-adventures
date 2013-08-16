using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Bridge tile.
    /// </summary>
    public class TileBridge : Tile
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TileBridge"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public TileBridge(Level level)
            : base(level)
        {
            spriteID = 4;
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
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, data, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE + 0.05F);
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE);
        }
    }
}
