using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.MathHelpers;
using Microsoft.Xna.Framework;
using System;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Water,
    /// </summary>
    public class TileWater : Tile
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TileWater"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public TileWater(Level level)
            : base(level)
        {
            spriteID = 16;
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
            return !(ent is EntityArrow);
        }

        /// <summary>
        /// Updates the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public override void Update(int xTile, int yTile, Level level, byte data = 0)
        {
            if (DateTime.Now.Ticks % 2 == 0)
            {
                Random rand = new Random();
                level.SetData((byte)rand.Next(0, 4), xTile, yTile);
            }
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
            DecorateTileEdges(xTile, yTile, 2, 2, 0, level, renderer);
        }
    }
}
