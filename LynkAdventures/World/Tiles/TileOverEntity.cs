using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A tile which is drawn on top of the entity.
    /// </summary>
    public class TileOverEntity : Tile
    {
        private bool isSolid;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileOverEntity"/>.
        /// </summary>
        /// <param name="spriteID">The sprite ID.</param>
        /// <param name="solid">if set to <c>true</c> the tile is solid.</param>
        /// <param name="level">The level.</param>
        public TileOverEntity(int spriteID, bool solid, Level level)
            : base(level)
        {
            this.spriteID = spriteID;
            isSolid = solid;
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
            return isSolid;
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
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_ENTITY - 0.101f);
        }
    }
}
