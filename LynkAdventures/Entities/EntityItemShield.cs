using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using LynkAdventures.Gui;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 24.3.2013
    /// <summary>
    /// A shield item.
    /// </summary>
    public class EntityItemShield : EntityItem
    {
        private int damageProtect, pushProtect;
        private bool isTryingPickingUp = false;
        private WeaponShield weaponToGive;
        private GuiSwapWeapons weaponSwap;
        private bool isGuiLoaded = false;
        private EntityPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemShield"/>.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="damageProtect">The damage protect.</param>
        /// <param name="pushProtect">The push protect.</param>
        public EntityItemShield(LevelManager manager, int damageProtect, int pushProtect)
            : base(manager)
        {
            this.damageProtect = damageProtect;
            this.pushProtect = pushProtect;
            itemAmount = 0;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_SHIELD, 7, 5, true);
        }

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - 6, YCenter + 8, XCenter + 5, YCenter + 20, this);
        }

        /// <summary>
        /// Called when the level is changed.
        /// </summary>
        /// <param name="newLevel">The level <see cref="LevelManager" /> is changing to.</param>
        public override void OnLevelChange(Level newLevel)
        {
            if (weaponSwap != null)
            {
                weaponSwap.Close(true);
                player = null;
                isGuiLoaded = false;
            }
        }

        /// <summary>
        /// Inits the weapon and GUI.
        /// </summary>
        private void InitWeaponAndGui()
        {
            weaponToGive = new WeaponShield(player, damageProtect, pushProtect, GameSpriteSheets.SPRITESHEET_LYNK_SHIELD);
            weaponSwap = new GuiSwapWeapons(player, weaponToGive);
        }

        /// <summary>
        /// Called when this entity gets interacted by another one.
        /// </summary>
        /// <param name="ent">The entity that interacted with this one.</param>
        public override void OnInteractBy(Entity ent)
        {
            if (ent is EntityPlayer && player == null)
            {
                player = (EntityPlayer)ent;
                InitWeaponAndGui();
            }
            if (player.Weapons[EntityPlayer.WEAPON_SHIELD] != null)
            {
                WeaponShield shield = (WeaponShield)player.Weapons[EntityPlayer.WEAPON_SHIELD];
                LevelManager.CurrentLevel.AddEntity(shield.ToItem(), ent.X, ent.Y);
            }

            player.AddWeapon(weaponToGive, EntityPlayer.WEAPON_SHIELD);
            weaponSwap.Close(true);
            base.PickUp(ent);
        }

        /// <summary>
        /// Picks up this item.
        /// </summary>
        /// <param name="ent">The entity which picks this item up.</param>
        public override void PickUp(Entity ent)
        {
            if (IsDead) return;

            isTryingPickingUp = true;
            if (!isGuiLoaded)
            {
                player = (EntityPlayer)ent;
                InitWeaponAndGui();
                Game.GuiManager.LoadGui(weaponSwap, false);
                isGuiLoaded = true;
            }
            weaponSwap.Activate();
        }

        /// <summary>
        /// Determines whether the entity can pick up this item.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity can pick up this item; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanPickUp(Entity ent)
        {
            return (ent is EntityPlayer);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (isTryingPickingUp)
            {
                List<Entity> entities = LevelManager.CurrentLevel.GetEntities(GetBoundingBox());
                if (!entities.Contains(player))
                {
                    isTryingPickingUp = false;
                    weaponSwap.Close();
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
            renderer.RenderTile(X, Y, 1, GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_SHADOWS, Color.White, RenderLayer.LAYER_DETAIL);
            renderer.RenderTile(X, Y, itemAnimation.CurrentSpriteID, itemAnimation.SpriteSheet, Color.White, layer);
        }
    }
}
