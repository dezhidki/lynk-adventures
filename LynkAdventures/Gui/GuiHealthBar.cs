using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A health bar.
    /// </summary>
    public class GuiHealthBar : Gui
    {
        private EntityLiving owner;
        private int renderWidth = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiHealthBar"/>.
        /// </summary>
        /// <param name="owner">Owner of the health bar.</param>
        public GuiHealthBar(EntityLiving owner)
            : base(30, 4, owner.Position)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Initializes the GUI.
        /// </summary>
        public override void Init()
        {
            pos = owner.Position;
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            renderWidth = (int)((owner.Health / (double)owner.MaxHealth) * Width);
            pos.X = owner.X;
            pos.Y = owner.Y;
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.ScreenOffset = Game.Camera.LeftTop;
            if (renderWidth > 0)
                renderer.RenderModalRect(Position.X, Position.Y, 0, 0, renderWidth, Height, GuiResources.RESOURCE_HEALTHBAR, Color.White, RenderLayer.LAYER_GUI + 0.25f);
            if (renderWidth < Width)
                renderer.RenderModalRect(Position.X + renderWidth * Game.SCALE, Position.Y, renderWidth, 4, Width - renderWidth, 4, GuiResources.RESOURCE_HEALTHBAR, Color.White, RenderLayer.LAYER_GUI + 0.25f);
            renderer.ScreenOffset = Point.Zero;
        }
    }
}
