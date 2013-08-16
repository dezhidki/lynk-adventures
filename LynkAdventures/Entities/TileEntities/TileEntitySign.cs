using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.Gui;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities.TileEntities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A sign, that has text on it.
    /// </summary>
    public class TileEntitySign : TileEntity
    {
        private GuiMessageBox messagaBox;
        private bool isPlayerTouching = false;
        private bool isGuiLoaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntitySign"/>.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="textSizeScale">Font size. 1.0 is original size.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">Level manager.</param>
        public TileEntitySign(string message, float textSizeScale, Level level, LevelManager manager)
            : base(level, manager)
        {
            messagaBox = new GuiMessageBox(message, textSizeScale);
            shouldUpdate = true;
        }

        private int xOffs = 9 * Game.SCALE, yOffs0 = 6 * Game.SCALE, yOffs1 = 10 * Game.SCALE;

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - xOffs, YCenter - yOffs0, XCenter + xOffs, YCenter + yOffs1, this);
        }

        /// <summary>
        /// Called when the level is changed.
        /// </summary>
        /// <param name="newLevel">The level <see cref="LevelManager" /> is changing to.</param>
        public override void OnLevelChange(Level newLevel)
        {
            isGuiLoaded = false;
            isPlayerTouching = false;
            messagaBox.Close(true);
        }

        /// <summary>
        /// Called when this entity gets touched by another.
        /// </summary>
        /// <param name="ent">The entity that touched.</param>
        public override void OnTouch(Entity ent)
        {
            if (ent is EntityPlayer)
            {
                isPlayerTouching = true;
                if (!isGuiLoaded)
                {
                    Game.GuiManager.LoadGui(messagaBox, false);
                    isGuiLoaded = true;
                }
                messagaBox.Activate();
            }
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            if (isPlayerTouching)
            {
                List<Entity> entities = levelManager.CurrentLevel.GetEntities(GetBoundingBox());
                if (!entities.Contains(levelManager.CurrentLevel.Player))
                {
                    isPlayerTouching = false;
                    messagaBox.Close();
                }
            }
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, 31, GameSpriteSheets.SPRITESHEET_TILES, Color.White, layer);
        }
    }
}
