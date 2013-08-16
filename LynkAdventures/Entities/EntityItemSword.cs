using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using LynkAdventures.Gui;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities
{
    /// <summary>
    /// A sword item.
    /// </summary>
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    public class EntityItemSword : EntityItem
    {
        private int damage, push, speed;
        private Point hitRadius = Point.Zero;
        private BoundingBox2D hitArea = BoundingBox2D.Zero;
        private bool isTryingPickingUp = false;
        private WeaponSword weaponToGive;
        private GuiSwapWeapons weaponSwap;
        private bool isGuiLoaded = false;
        private EntityPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemSword"/>.
        /// </summary>
        /// <param name="manager">Level manager</param>
        /// <param name="damage">The damage the sword inflicts.</param>
        /// <param name="push">The push amount of the sword.</param>
        /// <param name="speed">The sword's hit speed.</param>
        /// <param name="hitRadius">The radius of the area sword hits.</param>
        /// <param name="hitArea">The area sword hits (excluding radius).</param>
        public EntityItemSword(LevelManager manager, int damage, int push, int speed, Point hitRadius, BoundingBox2D hitArea)
            : base(manager)
        {
            this.damage = damage;
            this.push = push;
            this.hitRadius = hitRadius;
            this.hitArea = hitArea;
            this.speed = speed;
            itemAmount = 1;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_SWORD, 7, 5, true);
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
        /// Initializes the weapon's and GUI's instance.
        /// </summary>
        private void InitWeaponAndGui()
        {
            weaponToGive = new WeaponSword(player, damage, push, speed, hitRadius, hitArea, GameSpriteSheets.SPRITESHEET_LYNK_SWORD);
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
            if (player.Weapons[EntityPlayer.WEAPON_SWORD] != null)
            {
                WeaponSword sword = (WeaponSword)player.Weapons[EntityPlayer.WEAPON_SWORD];
                LevelManager.CurrentLevel.AddEntity(sword.ToItem(), ent.X, ent.Y);
            }

            player.AddWeapon(weaponToGive, EntityPlayer.WEAPON_SWORD);
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
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - 5, YCenter + 10, XCenter + 4, YCenter + 20, this);
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, 0, GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_SHADOWS, Color.White, RenderLayer.LAYER_DETAIL);
            renderer.RenderTile(X, Y, itemAnimation.CurrentSpriteID, itemAnimation.SpriteSheet, Color.White, layer);
        }
    }
}
