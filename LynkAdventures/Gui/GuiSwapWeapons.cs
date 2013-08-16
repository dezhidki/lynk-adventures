using LynkAdventures.Entities;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A weapon swap GUI.
    /// </summary>
    public class GuiSwapWeapons : Gui
    {
        private const int DIFFERENCE_SPEED = 0;
        private const int DIFFERENCE_POWER = 1;
        private const int DIFFERENCE_DAMAGE = 2;

        private Type[] weaponTypes = { typeof(WeaponSword), typeof(WeaponShield), typeof(WeaponBow) };
        private string[] attributes = { "SPD", "POW", "DMG" };
        private int[] weaponAttributes = new int[3];
        private int[] differences = new int[3];
        private Color[] highLightColors = new Color[3];
        private EntityPlayer player;
        private Weapon weapon;
        private int weaponID;
        private int textStartY = 70;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiSwapWeapons"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="weapon">The weapon to swap by.</param>
        public GuiSwapWeapons(EntityPlayer player, Weapon weapon)
            : base(130, 160, Point.Zero)
        {
            this.player = player;
            this.weapon = weapon;
            weaponID = Array.IndexOf(weaponTypes, weapon.GetType());
            weaponAttributes[DIFFERENCE_SPEED] = weapon.Speed;
            weaponAttributes[DIFFERENCE_POWER] = weapon.PushPower;
            weaponAttributes[DIFFERENCE_DAMAGE] = weapon.Damage;
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            if (player.Weapons[weaponID] != null)
            {
                Weapon playerWeapon = player.Weapons[weaponID];
                differences[DIFFERENCE_SPEED] = weapon.Speed - playerWeapon.Speed;
                differences[DIFFERENCE_POWER] = weapon.PushPower - playerWeapon.PushPower;
                differences[DIFFERENCE_DAMAGE] = weapon.Damage - playerWeapon.Damage;
            }
            else
            {
                differences[DIFFERENCE_SPEED] = weapon.Speed;
                differences[DIFFERENCE_POWER] = weapon.PushPower;
                differences[DIFFERENCE_DAMAGE] = weapon.Damage;
            }
            
            for (int i = 0; i < differences.Length; i++)
            {
                highLightColors[i] = differences[i] < 0 ? Color.Red : (differences[i] == 0 ? Color.White : Color.LightGreen);
            }
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.Render(Position.X, Position.Y, GuiResources.RESOURCE_WEAPONSWAPBG, Color.White, SpriteEffects.None, layer, 1.0F);
            renderer.RenderTile(Position.X, Position.Y + 5, weaponID, GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_ICONS, Color.White, layer - 0.0001F, 0.0F, 1.5F);
            for (int i = 0; i < differences.Length; i++)
            {
                renderer.SpriteBatch.DrawString(GuiResources.FONT, attributes[i], new Vector2(pos.X + 5, pos.Y + textStartY + 20 * i), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, layer - 0.0001f);
                renderer.SpriteBatch.DrawString(GuiResources.FONT, weaponAttributes[i].ToString(), new Vector2(pos.X + 60, pos.Y + textStartY + 20 * i), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, layer - 0.0001f);
                renderer.SpriteBatch.DrawString(GuiResources.FONT, differences[i].ToString(), new Vector2(pos.X + 90, pos.Y + textStartY + 20 * i), highLightColors[i], 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, layer - 0.0001f);
            }

            renderer.SpriteBatch.DrawString(GuiResources.FONT, "<E> TAKE", new Vector2(pos.X + 5, pos.Y + Height - 20), Color.White, 0.0f, Vector2.Zero, 0.6f, SpriteEffects.None, layer - 0.0001f);
        }
    }
}
