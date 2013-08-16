using LynkAdventures.Entities;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Ingame GUI.
    /// </summary>
    public class GuiIngame : Gui
    {
        private readonly Color ammoTextColor = new Color(255, 190, 0);
        private EntityPlayer player;
        private int playerArrowAmmo = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiIngame"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        public GuiIngame(EntityPlayer player)
            : base(800, 100, Point.Zero)
        {
            this.player = player;
            pos = new Point(0, Game.HEIGHT - Height);
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            if (player.Weapons[EntityPlayer.WEAPON_BOW] != null)
                playerArrowAmmo = player.Arrows;
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderRect(Position.X, Position.Y, Game.WIDTH, GuiResources.RESOURCE_PLAYER_BOTTOMBAR.Height, GuiResources.RESOURCE_PLAYER_BOTTOMBAR, Color.White, layer + 0.002f);

            // Render weapons

            for (int i = 0; i < 3; i++)
            {
                renderer.Render((i * 45), Position.Y + 20, GuiResources.RESOURCE_PLAYER_WEAPONSLOT, Color.Yellow, SpriteEffects.None, layer + 0.001f, 1.0f);
                Weapon weapon = player.Weapons[i];
                if (weapon != null)
                    renderer.RenderTile((i * 45) + 4, Position.Y + 25, i, GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_ICONS, Color.White, layer, 0.0F, 1.0F);
            }

            // Render icons

            renderer.RenderTile(-12, Game.HEIGHT - 40, 0, 2, GameSpriteSheets.SPRITESHEET_ITEM_ARROW, Color.White, layer + 0.001f, 2.0F);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, playerArrowAmmo.ToString(), new Vector2(40, Game.HEIGHT - 30), ammoTextColor, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, layer + 0.001f);

            renderer.Render(2 * 45 - 5, Game.HEIGHT - 35, GuiResources.RESOURCE_RUBY_ICON, Color.White, SpriteEffects.None, layer + 0.001f, 2.0f);
            renderer.SpriteBatch.DrawString(GuiResources.FONT, player.Rupies.ToString(), new Vector2(2 * 45 + 30, Game.HEIGHT - 30), ammoTextColor, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, layer + 0.001f);

            // Render hearts

            int heartsToRender = player.MaxHealth / 2 + player.MaxHealth % 2;
            int playerHearts = player.Health / 2 + player.Health % 2;
            int renderedHearts = 0;

            for (int i = 0; i < heartsToRender; i++)
            {
                renderer.RenderTile(4 * 40 + i * 38, Position.Y + 20, GuiResources.RESOURCE_PLAYER_HEARTS, 2, 0, Color.White, layer + 0.0015f, 3.0F);
                if (i < playerHearts)
                {
                    renderer.RenderTile(4 * 40 + i * 38, Position.Y + 20, GuiResources.RESOURCE_PLAYER_HEARTS, player.Health - renderedHearts == 1 ? 1 : 0, 0, Color.White, layer + 0.0015f, 3.0F);
                    renderedHearts += 2;
                }
            }
        }
    }
}
