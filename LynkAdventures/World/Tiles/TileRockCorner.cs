using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Cornerpiece.
    /// </summary>
    public class TileRockCorner : TileBlended
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TileRockCorner"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="dir">The blending direction.</param>
        public TileRockCorner(Level level, Direction dir)
            : base(level, dir)
        {
            spriteID = 14;
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
        /// Renders the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public override void Render(int xTile, int yTile, Renderer renderer, Level level, byte data = 0)
        {
            Point offset = OFFSETS[dir.ID];
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, data, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE + 0.05F);
            renderer.RenderTile((xTile + offset.X) * TILESIZE, (yTile + offset.Y) * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE, dir.Angle);
        }
    }
}
