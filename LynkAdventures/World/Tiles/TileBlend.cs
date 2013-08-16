using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.Graphics.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A tile used to blend two different textures (TEST TILE!).
    /// </summary>
    public class TileBlend : Tile
    {
        private int spriteIDBlend, spriteIDBottom, spriteSheetID;
        private Direction blendDir;
        private bool isSolid = false;
        private Texture2D[,] masks;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileBlend"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="spriteIDBlend">The sprite ID to blend.</param>
        /// <param name="spriteIDBottom">The sprite ID of tile's bottom.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="blendDir">The blending direction.</param>
        /// <param name="isSolid">if set to <c>true</c> the tile is solid.</param>
        public TileBlend(Level level, int spriteIDBlend, int spriteIDBottom, int spriteSheetID, Direction blendDir, bool isSolid = false)
            : base(level)
        {
            this.spriteIDBlend = spriteIDBlend;
            this.spriteIDBottom = spriteIDBottom;
            this.spriteSheetID = spriteSheetID;
            this.blendDir = blendDir;
            this.isSolid = isSolid;
            masks = SpriteSheet.CutIntoSprites(Renderer.RegisteredSpriteSheets[GameSpriteSheets.SPRITESHEET_TILE_ALPHA_MASKS1].Texture, 32, 32);
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

        private Vector2 ToHomogeneousPos(Vector2 pos)
        {
            return new Vector2(pos.X / 256, pos.Y / 256);
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
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteIDBottom, spriteSheetID, Color.White, RenderLayer.LAYER_TILE + 0.05F);

            Rectangle rect = Renderer.RegisteredSpriteSheets[spriteSheetID].GetSpriteBoundaries(spriteIDBlend);
            Texture2D alphaMask = masks[0, blendDir.ID];
            Effect shader = Renderer.RegisteredShaders[Shaders.AlphaMasking];
            shader.Parameters["AlphaMask"].SetValue(alphaMask);

            renderer.SpriteBatch.End();

            renderer.SpriteBatch.Begin(renderer.SORTMODE_DEFAULT, BlendState.NonPremultiplied, renderer.SAMPLERSTATE_DEFAULT, renderer.DEPTHSTATE_DEFAULT, renderer.RASTER_DEFAULT, shader);
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteIDBlend, spriteSheetID, Color.White, RenderLayer.LAYER_TILE);
            renderer.SpriteBatch.End();

            renderer.SpriteBatch.Begin(renderer.SORTMODE_DEFAULT, renderer.BLENDSTATE_DEFAULT, renderer.SAMPLERSTATE_DEFAULT, renderer.DEPTHSTATE_DEFAULT, renderer.RASTER_DEFAULT);
        }
    }
}
