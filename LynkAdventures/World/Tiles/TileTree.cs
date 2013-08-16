using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013 
    /// <summary>
    /// Tree.
    /// </summary>
    public class TileTree : Tile
    {
        private const int SPRITE_ID_TREE_TOP = 7;
        private const int BUSHOFFSET = 20 * Game.SCALE;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileTree"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public TileTree(Level level)
            : base(level)
        {
            spriteID = 6;
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
                return new BoundingBox2D((xTile * TILESIZE) + 8, (yTile * TILESIZE) + 20, ((xTile + 1) * TILESIZE) - 10, ((yTile + 1) * TILESIZE) - 4, this);
            return new BoundingBox2D((xTile * TILESIZE) + 8, (yTile * TILESIZE) + 20, ((xTile + 1) * TILESIZE) - 10, ((yTile + 1) * TILESIZE), this);
        }

        /// <summary>
        /// Renders the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public override void Render(int xTile, int yTile, Renderer renderer, Level level, byte data)
        {
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, data, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE);
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_DETAIL);
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE - BUSHOFFSET, SPRITE_ID_TREE_TOP, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_WEAPON - 0.05F);
        }
    }
}
