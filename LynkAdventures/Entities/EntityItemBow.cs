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
    /// @version 23.3.2013
    /// <summary>
    /// A bow item.
    /// </summary>
    public class EntityItemBow : EntityItem
    {
        private int damage, arrowSpeed, arrowFlyDistance, pushPower;
        private bool isTryingPickingUp = false;
        private WeaponBow weaponToGive;
        private GuiSwapWeapons weaponSwap;
        private bool isGuiLoaded = false;
        private EntityPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemBow"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        /// <param name="damage">The damage the bow's arrows inflict.</param>
        /// <param name="arrowSpeed">The bow's arrows' speed.</param>
        /// <param name="pushPower">The bow's arrows' push amount.</param>
        /// <param name="arrowFlyDistance">The bow's arrows' max flying distance. See <see cref="EntityArrow"/> for more information.</param>
        public EntityItemBow(LevelManager manager, int damage, int arrowSpeed, int pushPower, int arrowFlyDistance)
            : base(manager)
        {
            this.damage = damage;
            this.arrowSpeed = arrowSpeed;
            this.arrowFlyDistance = arrowFlyDistance;
            this.pushPower = pushPower;
            itemAmount = 1;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_BOW, 9, 5, true);
        }

        /// <summary>
        /// Initializes the weapon's and GUI's instance.
        /// </summary>
        private void InitWeaponAndGui()
        {
            weaponToGive = new WeaponBow(player, damage, arrowSpeed, arrowFlyDistance, pushPower, GameSpriteSheets.SPRITESHEET_LYNK_BOW);
            weaponSwap = new GuiSwapWeapons(player, weaponToGive);
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
            if (player.Weapons[EntityPlayer.WEAPON_BOW] != null)
            {
                WeaponBow bow = (WeaponBow)player.Weapons[EntityPlayer.WEAPON_BOW];
                LevelManager.CurrentLevel.AddEntity(bow.ToItem(), ent.X, ent.Y);
            }

            player.AddWeapon(weaponToGive, EntityPlayer.WEAPON_BOW);
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
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - 8, YCenter + 8, XCenter + 7, YCenter + 10, this);
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
            renderer.RenderTile(X, Y, 2, GameSpriteSheets.SPRITESHEET_ITEM_WEAPON_SHADOWS, Color.White, RenderLayer.LAYER_DETAIL);
            renderer.RenderTile(X, Y, itemAnimation.CurrentSpriteID, itemAnimation.SpriteSheet, Color.White, layer);
        }
    }
}
