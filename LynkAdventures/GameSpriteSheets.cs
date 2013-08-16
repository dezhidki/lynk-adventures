using LynkAdventures.Graphics;
using LynkAdventures.Graphics.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Main spritesheets and resources.
    /// </summary>
    public static class GameSpriteSheets
    {
        public static int SPRITESHEET_TILES { get; private set; }
        public static int SPRITESHEET_ITEM_RUBY { get; private set; }
        public static int SPRITESHEET_LYNK_WALK { get; private set; }
        public static int SPRITESHEET_LYNK_SWORD { get; private set; }
        public static int SPRITESHEET_LYNK_SHIELD { get; private set; }
        public static int SPRITESHEET_TILE_EDGES { get; private set; }
        public static int SPRITESHEET_LYNK_BOW { get; private set; }
        public static int SPRITESHEET_ARROW { get; private set; }
        public static int SPRITESHEET_ITEM_ARROW { get; private set; }
        public static int SPRITESHEET_ITEM_ARROW_SHADOWS { get; private set; }
        public static int SPRITESHEET_ITEM_SHIELD { get; private set; }
        public static int SPRITESHEET_ITEM_SWORD { get; private set; }
        public static int SPRITESHEET_ITEM_BOW { get; private set; }
        public static int SPRITESHEET_ITEM_PLACEHOLDER { get; private set; }
        public static int SPRITESHEET_ITEM_WEAPON_SHADOWS { get; private set; }
        public static int SPRITESHEET_ITEM_WEAPON_ICONS { get; private set; }
        public static int SPRITESHEET_ITEM_HEALTH { get; private set; }
        public static int SPRITESHEET_MOB_SLIME { get; private set; }
        public static int SPRITESHEET_MOB_BAT { get; private set; }
        public static int SPRITESHEET_MOB_SPIDER { get; private set; }
        public static int SPRITESHEET_MOB_BOMBERMAN { get; private set; }
        public static int SPRITESHEET_OBJECT_FIRE { get; private set; }
        public static int SPRITESHEET_OBJECT_BOMB { get; private set; }
        public static int SPRITESHEET_OBJECT_EXPLOSION { get; private set; }
        public static int SPRITESHEET_TILE_ALPHA_MASKS1 { get; private set; }

        /// <summary>
        /// Loads the sprite sheets.
        /// </summary>
        /// <param name="content">The content manager.</param>
        /// <param name="renderer">The renderer.</param>
        public static void LoadSpriteSheets(ContentManager content, Renderer renderer)
        {
            GameSpriteSheets.SPRITESHEET_TILES = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Tiles/testSheet")));
            GameSpriteSheets.SPRITESHEET_ITEM_RUBY = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/ruby")));
            GameSpriteSheets.SPRITESHEET_LYNK_WALK = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/lynk_walk")));
            GameSpriteSheets.SPRITESHEET_LYNK_SWORD = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/lynk_sword"), 48, 48));
            GameSpriteSheets.SPRITESHEET_LYNK_SHIELD = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/lynk_shield")));
            GameSpriteSheets.SPRITESHEET_TILE_EDGES = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Tiles/edges")));
            GameSpriteSheets.SPRITESHEET_TILE_ALPHA_MASKS1 = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Other/tileAlphaMasks")));
            GameSpriteSheets.SPRITESHEET_LYNK_BOW = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/lynk_bow")));
            GameSpriteSheets.SPRITESHEET_ARROW = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("WorldObjects/arrow"), 24, 24));
            GameSpriteSheets.SPRITESHEET_ITEM_ARROW = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_arrow")));
            GameSpriteSheets.SPRITESHEET_ITEM_ARROW_SHADOWS = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_arrow_shadows")));
            GameSpriteSheets.SPRITESHEET_ITEM_SHIELD = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_shield")));
            GameSpriteSheets.SPRITESHEET_ITEM_SWORD = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_sword")));
            GameSpriteSheets.SPRITESHEET_ITEM_PLACEHOLDER = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_placeholder")));
            GameSpriteSheets.SPRITESHEET_ITEM_BOW = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_bow")));
            GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_SHADOWS = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/item_weaponshadows")));
            GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_ICONS = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/weapon_icons_lowres")));
            GameSpriteSheets.SPRITESHEET_MOB_SLIME = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/slime")));
            GameSpriteSheets.SPRITESHEET_MOB_BAT = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/bat")));
            GameSpriteSheets.SPRITESHEET_MOB_SPIDER = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/spider")));
            GameSpriteSheets.SPRITESHEET_MOB_BOMBERMAN = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Mobs/bomberman")));
            GameSpriteSheets.SPRITESHEET_OBJECT_FIRE = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("WorldObjects/fire")));
            GameSpriteSheets.SPRITESHEET_OBJECT_BOMB = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("WorldObjects/bomb")));
            GameSpriteSheets.SPRITESHEET_OBJECT_EXPLOSION = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("WorldObjects/explosion")));
            GameSpriteSheets.SPRITESHEET_ITEM_HEALTH = renderer.RegisterSpriteSheet(new SpriteSheet(content.Load<Texture2D>("Items/health")));
        }
    }
}
