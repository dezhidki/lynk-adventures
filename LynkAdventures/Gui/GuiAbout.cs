using LynkAdventures.Controls;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace LynkAdventures.Gui
{
    /// @author Denis Zhidkikh
    /// @version 27.4.2013
    /// <summary>
    /// About the game.
    /// </summary>
    public class GuiAbout : Gui
    {
        private GuiMainMenu menu;
        private string message, message1;
        private string[] messageLines = { "                          About the game:" ,
                                          "The game was made by Denis Zhidkikh in two months",
                                          "for the OHJ1 course in the University of Jyväskylä.",
                                          "The original idea was to create a game using Jypeli",
                                          "XNA library, but the complexity of the idea made me",
                                          "use clean XNA instead of Jypeli.",
                                          "The game itself is a parody of Zelda games with some",
                                          "ideas of mine."};

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiAbout"/>.
        /// </summary>
        /// <param name="menu">The menu GUI.</param>
        public GuiAbout(GuiMainMenu menu)
            : base(Game.WIDTH, Game.HEIGHT, Point.Zero)
        {
            this.menu = menu;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < messageLines.Length; i++)
            {
                sb.Append(messageLines[i]).Append("\n");
            }

            message = sb.ToString();
            message1 = "<ESC> Main Menu";
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            if (KeyboardHandler.IsKeyPressed(Keys.Escape))
            {
                Close();
                Game.GuiManager.LoadAsActiveGui(menu);
            }
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.Render(0, 0, GuiResources.RESOURCE_MAIN_BACKGROUND, Color.White, SpriteEffects.None, RenderLayer.LAYER_GUI + 0.01F, 1.0F);
            renderer.Render(Game.WIDTH / 2 - 150, 10, GuiResources.RESOURCE_LOGO, Color.White, SpriteEffects.None, RenderLayer.LAYER_GUI, 1.0F);

            renderer.SpriteBatch.DrawString(GuiResources.FONT, message, new Vector2(50, 200), Color.White, 0.0F, Vector2.Zero, 0.9F, SpriteEffects.None, RenderLayer.LAYER_GUI);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message1, new Vector2(20, Game.HEIGHT - 60), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, RenderLayer.LAYER_GUI);
        }
    }
}
