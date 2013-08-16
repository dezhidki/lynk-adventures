using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.Sounds;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities.Weapons
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A sword.
    /// </summary>
    public class WeaponSword : WeaponMelee
    {
        /// <summary>
        /// Sprite offset, because the sprite's size is bigger for sword sprite sheet.
        /// </summary>
        private const int SPRITEOFFSET = 8 * Game.SCALE;

        private int rawSpeed = 0;
        private AnimationHelper entityAnimation;
        private Direction facingDir = Directions.UP;
        private int spriteSheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponSword"/>.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="pushPower">The push power.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hitRadius">The hit radius to adjust the main area box.</param>
        /// <param name="hitAreaBox">The hit area box.</param>
        /// <param name="spriteSheet">The sprite sheet ID of the weapon.</param>
        public WeaponSword(Entity owner, int damage, int pushPower, int speed, Point hitRadius, BoundingBox2D hitAreaBox, int spriteSheet)
            : base(owner)
        {
            BlocksMovementWhenUsed = true;
            this.spriteSheet = spriteSheet;
            this.damage = damage;
            this.power = pushPower;
            this.hitAreaRadiusPoint = hitRadius;
            this.hitRadiusBox = hitAreaBox;
            this.speed = (int)MathHelper.Clamp(5 - speed, 1, 4);
            rawSpeed = speed;
            weaponType = WeaponType.MELEE;
            entityAnimation = new AnimationHelper(spriteSheet, 7, speed, false);
            entityAnimation.AnimationReady += ResetAnimation;
            weaponLength = 16;
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="dir">The direction in which the weapon is used.</param>
        public override void Use(Direction dir)
        {
            if (IsInUse) return;
            facingDir = dir;
            IsInUse = true;

            List<Entity> entitiesToHit = GetEntitiesToInteractWith(dir);
            entitiesToHit.RemoveAll(ent => ent == owner || !ent.CanBePushed);

            HitSource hitSource = new HitSource(dir, power, this, HitSource.OwnerType.WEAPON);
            if (entitiesToHit.Count > 0)
                Sound.Hit.Play();
            foreach (Entity ent in entitiesToHit)
            {
                if (ent != owner)
                    ent.Hit(hitSource, damage);
            }
        }

        /// <summary>
        /// Resets the animation.
        /// </summary>
        private void ResetAnimation()
        {
            IsInUse = false;
            entityAnimation.Restart();
        }

        /// <summary>
        /// Updates the weapon.
        /// </summary>
        public override void Update()
        {
            if (IsInUse)
                entityAnimation.UpdateStep();
        }

        /// <summary>
        /// Renders the owner and the weapon.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer to render to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(owner.X - SPRITEOFFSET, owner.Y - SPRITEOFFSET, entityAnimation.CurrentSpriteID, facingDir.ID, entityAnimation.SpriteSheet, owner.CurrentGlow, RenderLayer.LAYER_WEAPON);
        }

        /// <summary>
        /// Converts this weapon to <see cref="EntityItem" /> that can be placed on a level.
        /// </summary>
        /// <returns>
        /// An <see cref="EntityItem" /> representation of this weapon.
        /// </returns>
        public override EntityItem ToItem()
        {
            return new EntityItemSword(owner.LevelManager, damage, power, rawSpeed, hitAreaRadiusPoint, hitRadiusBox);
        }
    }
}
