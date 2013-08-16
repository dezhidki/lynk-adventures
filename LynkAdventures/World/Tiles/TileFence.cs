using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.MathHelpers;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Fence.
    /// </summary>
    public class TileFence : Tile
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TileFence"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public TileFence(Level level)
            : base(level)
        {
            spriteID = 21;
            TileColor = ColorMath.GetMostUsedColor(Renderer.GetTextureFromSpriteSheet(spriteID, GameSpriteSheets.SPRITESHEET_TILES));
        }

        /// <summary>
        /// Determines whether the tile is solid to the specified entity.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the tile is solid to the entity; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSolid(Entity ent)
        {
            return true;
        }

        /// <summary>
        /// Gets the bounding box of the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="ent">The entity that asks for the bounding box.</param>
        /// <returns>
        /// The bounding box of this tile.
        /// </returns>
        public override BoundingBox2D GetBoundingBox(int xTile, int yTile, Entity ent = null)
        {
            if (ent is EntityArrow)
                return new BoundingBox2D(xTile * TILESIZE, yTile * TILESIZE + 6, (xTile + 1) * TILESIZE, (yTile + 1) * TILESIZE - 6, this);

            return new BoundingBox2D(xTile * TILESIZE, (yTile * TILESIZE) + 15, (xTile + 1) * TILESIZE, ((yTile + 1) * TILESIZE) - 6, this);
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
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, data, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE + 0.01f);
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE);
            RenderTileDepth(xTile, yTile, spriteID + 1, GameSpriteSheets.SPRITESHEET_TILES, renderer, level, false);
        }
    }
}
