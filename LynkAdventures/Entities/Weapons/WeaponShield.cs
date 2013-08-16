using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;

namespace LynkAdventures.Entities.Weapons
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A shield.
    /// </summary>
    public class WeaponShield : WeaponMelee
    {
        private AnimationHelper wieldAnimation, walkAnimation;
        private Direction currentDir = Directions.UP;
        private int damageProtection, pushProtection;
        private bool isWielding = false;
        private int spritePosX = 0, spritePosY = 0;
        private int spriteSheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponShield"/>.
        /// </summary>
        /// <param name="ent">The owner.</param>
        /// <param name="damageProtection">The amount of damage reflected on every hit recieved.</param>
        /// <param name="pushProtection">The amount of push power negated on every hit recieved.</param>
        /// <param name="spriteSheet">The sprite sheet ID for this weapon.</param>
        public WeaponShield(Entity ent, int damageProtection, int pushProtection, int spriteSheet)
            : base(ent)
        {
            this.spriteSheet = spriteSheet;
            wieldAnimation = new AnimationHelper(spriteSheet, 3, 3);
            walkAnimation = new AnimationHelper(spriteSheet, 7, 5, true);
            this.damageProtection = damageProtection;
            this.pushProtection = pushProtection;
            this.damage = damageProtection;
            this.power = pushProtection;
            weaponType = WeaponType.SHIELD;
            BlocksMovementWhenUsed = false;
            wieldAnimation.AnimationReady += OnShieldWielded;
        }

        /// <summary>
        /// Gets the amount of damage the shield protects from.
        /// </summary>
        /// <value>
        /// The amount of damage shield protects from.
        /// </value>
        public int DamageProtection { get { return damageProtection; } }

        /// <summary>
        /// Gets the amount of push power the shield negates.
        /// </summary>
        /// <value>
        /// The amount of push power the shield negates.
        /// </value>
        public int PushProtection { get { return pushProtection; } }

        /// <summary>
        /// Gets the owner's move speed.
        /// </summary>
        /// <returns>
        /// Owner's move speed.
        /// </returns>
        public override int GetMoveSpeed()
        {
            return 1;
        }

        /// <summary>
        /// Called after the shield's wielding animation.
        /// </summary>
        private void OnShieldWielded() 
        {
            BlocksMovementWhenUsed = false;
            isWielding = false;
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="dir">The direction in which the weapon is used.</param>
        public override void Use(Direction dir)
        {
            if (IsInUse) return;
            IsInUse = true;
            currentDir = dir;

            isWielding = true;
            spritePosY = owner.Direction.ID + 4;
            BlocksMovementWhenUsed = true;
            wieldAnimation.StartAnimation();
        }

        /// <summary>
        /// Stops using the weapon.
        /// </summary>
        public override void StopUsing()
        {
            IsInUse = false;
            isWielding = false;
            spritePosX = 0;
            BlocksMovementWhenUsed = false;
            wieldAnimation.StopAnimation();
            walkAnimation.Reset();
        }

        /// <summary>
        /// Updates the weapon when owner has moved.
        /// </summary>
        public override void UpdateMovement()
        {
            walkAnimation.UpdateStep();
        }

        /// <summary>
        /// Called when the owner stops moving.
        /// </summary>
        /// <param name="dir">Current direction of the owner.</param>
        public override void OnMovementStopped(Direction dir)
        {
            walkAnimation.Reset(dir.ID);
        }

        /// <summary>
        /// Updates the weapon.
        /// </summary>
        public override void Update()
        {
            if (IsInUse)
            {
                wieldAnimation.UpdateStep();
                spritePosX = isWielding ? wieldAnimation.CurrentSpriteID : walkAnimation.CurrentSpriteID;
                spritePosY = isWielding ? owner.Direction.ID + 4 : owner.Direction.ID;
            }
        }

        /// <summary>
        /// Renders the owner and the weapon.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer to render to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(owner.X, owner.Y, spritePosX, spritePosY, walkAnimation.SpriteSheet, owner.CurrentGlow, layer);
        }

        /// <summary>
        /// Converts this weapon to <see cref="EntityItem" /> that can be placed on a level.
        /// </summary>
        /// <returns>
        /// An <see cref="EntityItem" /> representation of this weapon.
        /// </returns>
        public override EntityItem ToItem()
        {
            return new EntityItemShield(owner.LevelManager, damageProtection, pushProtection);
        }
    }
}
