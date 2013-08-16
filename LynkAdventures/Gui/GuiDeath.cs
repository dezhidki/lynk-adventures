using LynkAdventures.Controls;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A death message.
    /// </summary>
    public class GuiDeath : Gui
    {
        private string message1;
        private Game frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiDeath"/>.
        /// </summary>
        /// <param name="frame">The game instance.</param>
        public GuiDeath(Game frame)
            : base(300, 150, new Point(Game.WIDTH / 2 - 150, Game.HEIGHT / 2 - 75))
        {
            this.frame = frame;
            message1 = "Oh noes! You are dead!\n\n\n\n<ENTER> Restart\n<ESC> Main menu";
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            if (KeyboardHandler.IsKeyPressed(Keys.Enter))
            {
                frame.StartGame();
                Close();
            }
            else if (KeyboardHandler.IsKeyPressed(Keys.Escape))
            {
                Close();
                frame.StartMainMenu();
            }
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.Render(Position.X, Position.Y, GuiResources.RESOURCE_MESSAGE_BOX, Color.White, SpriteEffects.None, layer + 0.0001F, 1.0F);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message1, new Vector2(Position.X + 10, Position.Y + 10), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, layer);
        }
    }
}
