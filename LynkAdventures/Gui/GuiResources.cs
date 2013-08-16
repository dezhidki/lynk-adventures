using LynkAdventures.Graphics.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.Gui
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// All the GUI resources.
    /// </summary>
    public static class GuiResources
    {
        public static Texture2D RESOURCE_PLAYER_BOTTOMBAR { get; private set; }
        public static Texture2D RESOURCE_PLAYER_WEAPONSLOT { get; private set; }
        public static Texture2D RESOURCE_HEALTHBAR { get; private set; }
        public static Texture2D RESOURCE_WEAPONSWAPBG { get; private set; }
        public static Texture2D RESOURCE_MESSAGE_BOX { get; private set; }
        public static Texture2D RESOURCE_RUBY_ICON { get; private set; }
        public static Texture2D RESOURCE_MAIN_BACKGROUND { get; private set; }
        public static Texture2D RESOURCE_MAIN_BACKGROUND_INGAME { get; private set; }
        public static Texture2D RESOURCE_BUTTON_PLAY { get; private set; }
        public static Texture2D RESOURCE_BUTTON_EXIT { get; private set; }
        public static Texture2D RESOURCE_BUTTON_CONTINUE { get; private set; }
        public static Texture2D RESOURCE_BUTTON_MAIN_MENU { get; private set; }
        public static Texture2D RESOURCE_BUTTON_ABOUT { get; private set; }
        public static Texture2D RESOURCE_LOGO { get; private set; }
        public static Texture2D RESOURCE_POINTER { get; private set; }
        public static SpriteSheet RESOURCE_PLAYER_HEARTS { get; private set; }
        public static SpriteFont FONT { get; private set; }

        /// <summary>
        /// Initializes the resources.
        /// </summary>
        /// <param name="manager">The content manager.</param>
        public static void InitializeResources(ContentManager manager)
        {
            RESOURCE_PLAYER_BOTTOMBAR = manager.Load<Texture2D>("Gui/bottom_bar");
            RESOURCE_PLAYER_HEARTS = new SpriteSheet(manager.Load<Texture2D>("Gui/heart"), 16, 16);
            RESOURCE_PLAYER_WEAPONSLOT =  manager.Load<Texture2D>("Gui/itemSlot");
            RESOURCE_WEAPONSWAPBG = manager.Load<Texture2D>("Gui/weaponSwapBg");
            RESOURCE_HEALTHBAR = manager.Load<Texture2D>("Gui/entityHealthBar");
            RESOURCE_MESSAGE_BOX = manager.Load<Texture2D>("Gui/messagebox");
            RESOURCE_RUBY_ICON = manager.Load<Texture2D>("Gui/rubyIcon");
            RESOURCE_MAIN_BACKGROUND = manager.Load<Texture2D>("Gui/background");
            RESOURCE_MAIN_BACKGROUND_INGAME = manager.Load<Texture2D>("Gui/background_ingame");
            RESOURCE_BUTTON_PLAY = manager.Load<Texture2D>("Gui/playButton");
            RESOURCE_BUTTON_EXIT = manager.Load<Texture2D>("Gui/exitButton");
            RESOURCE_BUTTON_CONTINUE = manager.Load<Texture2D>("Gui/continueButton");
            RESOURCE_BUTTON_MAIN_MENU = manager.Load<Texture2D>("Gui/mainMenuButton");
            RESOURCE_BUTTON_ABOUT = manager.Load<Texture2D>("Gui/aboutButton");
            RESOURCE_LOGO = manager.Load<Texture2D>("Gui/logo");
            RESOURCE_POINTER = manager.Load<Texture2D>("Gui/pointer");
            FONT = manager.Load<SpriteFont>("Fonts/font1");
            FONT.Spacing = 1.0F;
        }
    }
}
