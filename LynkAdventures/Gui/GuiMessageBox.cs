using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A message box.
    /// </summary>
    public class GuiMessageBox : Gui
    {
        private string message;
        private float textSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMessageBox"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="textSize">Size of the text. 1.0 for default size, lower values are smaller, larger are bigger.</param>
        public GuiMessageBox(string message, float textSize = 0.7F)
            : base(300, 150, new Point(Game.WIDTH - 300, 0))
        {
            this.message = message;
            this.textSize = textSize;
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.Render(Position.X, Position.Y, GuiResources.RESOURCE_MESSAGE_BOX, Color.White, SpriteEffects.None, layer, 1.0F);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message, new Vector2(Position.X + 15, Position.Y + 10), Color.White, 0.0F, Vector2.Zero, textSize, SpriteEffects.None, layer - 0.0001f);
        }
    
    }
}
