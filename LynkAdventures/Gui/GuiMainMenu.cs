using LynkAdventures.Controls;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace LynkAdventures.Gui
{

    /// <summary>
    /// Main menu.
    /// </summary>
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    public class GuiMainMenu : Gui
    {
        private int choise = 0;
        private Game frame;
        private Texture2D[] choises = new Texture2D[3];
        private string message1, message2, message3, message4;
        private GuiAbout aboutMenu;
        private bool wasOpened = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiMainMenu"/>.
        /// </summary>
        /// <param name="frame">The game instance.</param>
        public GuiMainMenu(Game frame)
            : base(Game.WIDTH, Game.HEIGHT, Point.Zero)
        {
            this.frame = frame;
            aboutMenu = new GuiAbout(this);
            message1 = "<UP, DOWN> Scroll";
            message2 = "<ENTER> Choose";
            message3 = new StringBuilder(Game.TITLE).Append(" ").Append(Game.VERSION).ToString();
            message4 = "By: Denis Zhidkikh";
        }

        /// <summary>
        /// Called when the space button is pressed.
        /// </summary>
        /// <param name="choiseID">The choise ID.</param>
        private void OnChoise(int choiseID)
        {
            if (choiseID == 0)
            {
                if (!frame.IsGameLoaded)
                    frame.StartGame();
                Close();
            }
            else if (choiseID == 1)
            {
                if (frame.IsGameLoaded)
                    frame.StartMainMenu();
                else
                {
                    Close();
                    Game.GuiManager.LoadAsActiveGui(aboutMenu);
                }
            }
            else if (choiseID == 2)
                frame.Exit();
        }

        /// <summary>
        /// Initializes the GUI.
        /// </summary>
        public override void Init()
        {
            choises[0] = frame.IsGameLoaded ? GuiResources.RESOURCE_BUTTON_CONTINUE : GuiResources.RESOURCE_BUTTON_PLAY;
            choises[1] = frame.IsGameLoaded ? GuiResources.RESOURCE_BUTTON_MAIN_MENU : GuiResources.RESOURCE_BUTTON_ABOUT;
            choises[2] = GuiResources.RESOURCE_BUTTON_EXIT;
            base.Init();
        }


        /// <summary>
        /// Called when this GUI is activated.
        /// </summary>
        protected override void OnActivated()
        {
            choises[0] = frame.IsGameLoaded ? GuiResources.RESOURCE_BUTTON_CONTINUE : GuiResources.RESOURCE_BUTTON_PLAY;
            choises[1] = frame.IsGameLoaded ? GuiResources.RESOURCE_BUTTON_MAIN_MENU : GuiResources.RESOURCE_BUTTON_ABOUT;
            if (frame.IsGameLoaded)
                wasOpened = true;
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            if (KeyboardHandler.IsKeyPressed(Keys.Up))
                choise--;
            if (KeyboardHandler.IsKeyPressed(Keys.Down))
                choise++;
            if (KeyboardHandler.IsKeyPressed(Keys.Escape))
            {
                if (wasOpened)
                {
                    wasOpened = false;
                    return;
                }
                if (frame.IsGameLoaded)
                    Close();
                else
                    frame.Exit();
            }

            if (choise < 0) choise = choises.Length - 1;
            else if (choise >= choises.Length) choise = 0;

            if (KeyboardHandler.IsKeyPressed(Keys.Enter))
                OnChoise(choise);
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderRect(Position.X, Position.Y, Game.WIDTH, Game.HEIGHT, frame.IsGameLoaded ? GuiResources.RESOURCE_MAIN_BACKGROUND_INGAME : GuiResources.RESOURCE_MAIN_BACKGROUND, Color.White, RenderLayer.LAYER_GUI + 0.00001f);
            renderer.Render(Game.WIDTH / 2 - 150, 10, GuiResources.RESOURCE_LOGO, Color.White, SpriteEffects.None, RenderLayer.LAYER_GUI, 1.0F);

            for (int i = 0; i < choises.Length; i++)
            {
                renderer.Render(50, 280 + i * 45, choises[i], Color.White, SpriteEffects.None, RenderLayer.LAYER_GUI, 1.0F);
                if (choise == i)
                    renderer.Render(10, 280 + i * 45, GuiResources.RESOURCE_POINTER, Color.White, SpriteEffects.None, RenderLayer.LAYER_GUI, 1.0F);
            }

            renderer.SpriteBatch.DrawString(GuiResources.FONT, message1, new Vector2(20, Game.HEIGHT - 60), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, RenderLayer.LAYER_GUI);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message2, new Vector2(20, Game.HEIGHT - 35), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, RenderLayer.LAYER_GUI);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message3, new Vector2(Game.WIDTH / 2, Game.HEIGHT - 60), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, RenderLayer.LAYER_GUI);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, message4, new Vector2(Game.WIDTH / 2, Game.HEIGHT - 35), Color.White, 0.0F, Vector2.Zero, 0.7F, SpriteEffects.None, RenderLayer.LAYER_GUI);
        }
    }
}
