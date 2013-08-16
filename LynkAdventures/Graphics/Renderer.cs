using LynkAdventures.Graphics.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LynkAdventures.Graphics
{
    /// @author Denis Zhidkikh
    /// @version 27.4.2013
    /// <summary>
    /// The renderer.
    /// </summary>
    public class Renderer
    {
        public readonly SpriteSortMode SORTMODE_DEFAULT = SpriteSortMode.BackToFront;
        public readonly BlendState BLENDSTATE_DEFAULT = BlendState.AlphaBlend;
        public readonly SamplerState SAMPLERSTATE_DEFAULT = SamplerState.PointClamp;
        public readonly DepthStencilState DEPTHSTATE_DEFAULT = DepthStencilState.Default;
        public readonly RasterizerState RASTER_DEFAULT = RasterizerState.CullNone;

        private GraphicsDevice device;
        private bool isRendering = false;

        private static List<Effect> shaders = new List<Effect>();
        private static List<SpriteSheet> spriteSheets = new List<SpriteSheet>();

        /// <summary>
        /// Gets or sets the global screen offset which other entities get translated by.
        /// </summary>
        /// <value>
        /// The global screen offset which other entities get translated by.
        /// </value>
        public Point ScreenOffset { get; set; }

        /// <summary>
        /// Gets the list of all registered sprite sheets.
        /// </summary>
        /// <value>
        /// The list of all registered sprite sheets.
        /// </value>
        public static List<SpriteSheet> RegisteredSpriteSheets { get { return spriteSheets; } }

        /// <summary>
        /// Gets the list of registered shaders.
        /// </summary>
        /// <value>
        /// The list of registered shaders.
        /// </value>
        public static List<Effect> RegisteredShaders { get { return shaders; } }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        /// <value>
        /// The sprite batch.
        /// </value>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/>.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="device">The graphics device.</param>
        public Renderer(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            ScreenOffset = Point.Zero;
            this.SpriteBatch = spriteBatch;
            this.device = device;
        }

        /// <summary>
        /// Registers the sprite sheet.
        /// </summary>
        /// <param name="sheet">The sprite sheet.</param>
        /// <returns>The ID of this sprite sheet.</returns>
        public int RegisterSpriteSheet(SpriteSheet sheet)
        {
            spriteSheets.Add(sheet);
            return spriteSheets.IndexOf(sheet);
        }

        /// <summary>
        /// Registers the shader.
        /// </summary>
        /// <param name="effect">The sahder.</param>
        /// <returns>The ID of this shader.</returns>
        public int RegisterShader(Effect effect)
        {
            shaders.Add(effect);
            return shaders.IndexOf(effect);
        }

        /// <summary>
        /// Gets the texture from sprite sheet.
        /// </summary>
        /// <param name="spriteID">The sprite ID.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <returns>A copy of the sprite in <see cref="Microsoft.Xna.Framework.Graphics.Tecture2D"/> representation.</returns>
        public static Texture2D GetTextureFromSpriteSheet(int spriteID, int spriteSheetID)
        {
            return RegisteredSpriteSheets[spriteSheetID].GetSpriteData(spriteID);
        }

        /// <summary>
        /// Begins rendering.
        /// </summary>
        public void BeginRender()
        {
            if (isRendering)
                return;

            SpriteBatch.Begin(SORTMODE_DEFAULT, BLENDSTATE_DEFAULT, SAMPLERSTATE_DEFAULT, DEPTHSTATE_DEFAULT, RASTER_DEFAULT);
            isRendering = true;
        }

        /// <summary>
        /// Renders the texture.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="tint">The color to tint with.</param>
        /// <param name="flip">Texture flipping options.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="scale">Scale of the texture. 1.0 is the default size of the texture.</param>
        /// <param name="angle">The rotation angle in radians.</param>
        public void Render(float xPos, float yPos, Texture2D texture, Color tint, SpriteEffects flip, float layer = 1.0F, float scale = Game.SCALE, float angle = 0.0F)
        {
            SpriteBatch.Draw(texture, new Vector2(xPos, yPos), null, tint, angle, Vector2.Zero, scale, flip, layer);
        }

        /// <summary>
        /// Renders the image in the given rectangle defined by coordinates, width and height.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="width">The width of the final texture.</param>
        /// <param name="height">The height of the final texture.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        public void RenderRect(int xPos, int yPos, int width, int height, Texture2D texture, Color tint, float layer = 1.0F)
        {
            SpriteBatch.Draw(texture, new Rectangle(xPos, yPos, width, height), null, tint, 0.0F, Vector2.Zero, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Renders a texture from its sprite sheet.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="spritePosX">The sprite's X position on sprite sheet grid.</param>
        /// <param name="spritePosY">The sprite's Y position on sprite sheet grid.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="scale">The scale of the texture.</param>
        public void RenderTile(float xPos, float yPos, int spritePosX, int spritePosY, int spriteSheetID, Color tint, float layer = 1.0F, float scale = Game.SCALE)
        {
            xPos -= ScreenOffset.X;
            yPos -= ScreenOffset.Y;

            SpriteSheet sheet = spriteSheets[spriteSheetID];

            SpriteBatch.Draw(sheet.Texture, new Vector2(xPos, yPos), sheet.GetSpriteBoundaries(spritePosX, spritePosY), tint, 0.0F, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Renders a texture from its sprite sheet.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="spriteID">The sprite ID.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="angle">The rotation amount in radians.</param>
        /// <param name="scale">The scale of the texture.</param>
        public void RenderTile(float xPos, float yPos, int spriteID, int spriteSheetID, Color tint, float layer = 1.0F, float angle = 0.0F, float scale = Game.SCALE )
        {
            xPos -= ScreenOffset.X;
            yPos -= ScreenOffset.Y;

            SpriteSheet sheet = spriteSheets[spriteSheetID];

            SpriteBatch.Draw(sheet.Texture, new Vector2(xPos, yPos), sheet.GetSpriteBoundaries(spriteID), tint, angle, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Renders a texture from its sprite sheet.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="sheet">The sprite sheet.</param>
        /// <param name="spriteX">The sprite's X position on sprite sheet grid.</param>
        /// <param name="spriteY">The sprite's Y position on sprite sheet grid.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="angle">The rotation amount in radians.</param>
        /// <param name="scale">The scale of the texture.</param>
        public void RenderTile(float xPos, float yPos, SpriteSheet sheet, int spriteX, int spriteY, Color tint, float layer = 1.0F, float scale = Game.SCALE, float angle = 0.0F)
        {
            xPos -= ScreenOffset.X;
            yPos -= ScreenOffset.Y;

            SpriteBatch.Draw(sheet.Texture, new Vector2(xPos, yPos), sheet.GetSpriteBoundaries(spriteX, spriteY), tint, angle, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Renders the modal rectangle.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="xOffs">The x offset on the texture.</param>
        /// <param name="yOffs">The y offset on the texture.</param>
        /// <param name="imgWith">Width of the final texture.</param>
        /// <param name="imgHeight">Height of the final texture.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="scale">The scale of the texture.</param>
        public void RenderModalRect(int xPos, int yPos, int xOffs, int yOffs, int imgWith, int imgHeight, Texture2D texture, Color tint, float layer = 1.0F, float scale = Game.SCALE)
        {
            xPos -= ScreenOffset.X;
            yPos -= ScreenOffset.Y;
            SpriteBatch.Draw(texture, new Vector2(xPos, yPos), new Rectangle(xOffs, yOffs, imgWith, imgHeight), tint, 0.0F, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Renders the modal rectangle.
        /// </summary>
        /// <param name="xPos">The x pos.</param>
        /// <param name="yPos">The y pos.</param>
        /// <param name="xOffs">The x offset on the texture.</param>
        /// <param name="yOffs">The y offset on the texture.</param>
        /// <param name="imgWith">Width of the final texture.</param>
        /// <param name="imgHeight">Height of the final texture.</param>
        /// <param name="spriteID">The sprite ID.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="tint">The color to tint texture with.</param>
        /// <param name="layer">The layer to render to.</param>
        /// <param name="scale">The scale of the texture.</param>
        public void RenderModalRect(int xPos, int yPos, int xOffs, int yOffs, int imgWidth, int imgHeight, int spriteID, int spriteSheetID, Color tint, float layer = 1.0F, float scale = Game.SCALE)
        {
            xPos -= ScreenOffset.X;
            yPos -= ScreenOffset.Y;
            SpriteSheet sheet = RegisteredSpriteSheets[spriteSheetID];
            Rectangle sprite = sheet.GetSpriteBoundaries(spriteID);
            sprite.X += xOffs;
            sprite.Y += yOffs;
            sprite.Width = imgWidth;
            sprite.Height = imgHeight;
            SpriteBatch.Draw(sheet.Texture, new Vector2(xPos, yPos), sprite, tint, 0.0F, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        /// <summary>
        /// Ends rendering.
        /// </summary>
        public void EndRender()
        {
            if (!isRendering)
                return;

            SpriteBatch.End();
            isRendering = false;
        }
    }
}
