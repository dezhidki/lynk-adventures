using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities.Weapons
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A bow.
    /// </summary>
    public class WeaponBow : Weapon
    {
        private readonly Point[] AIMOFFSETS = { new Point(-28, -64), new Point(-18, 28), new Point(-64, -18), new Point(28, -18) };
        private AnimationHelper walkingAnim, loadingAnim, shootAnim;
        private Direction currentDir;
        private int arrowFlyDistance;
        private bool isLoading = false, isShooting = false;
        private Point renderSpritePos = Point.Zero;
        private int spriteSheet;
        private EntityPlayer player;

        /// <summary>
        /// Gets a value indicating whether the arrow is loaded in the bow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bow is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponBow"/>.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="damage">The damage.</param>
        /// <param name="arrowSpeed">The arrow speed.</param>
        /// <param name="arrowFlyDistance">The arrow fly distance.</param>
        /// <param name="arrowPower">The arrow power.</param>
        /// <param name="spriteSheet">The sprite sheet ID for this weapon.</param>
        public WeaponBow(EntityPlayer owner, int damage, int arrowSpeed, int arrowFlyDistance, int arrowPower, int spriteSheet)
            : base(owner)
        {
            player = owner;
            this.spriteSheet = spriteSheet;
            walkingAnim = new AnimationHelper(spriteSheet, 7, 5, true);
            loadingAnim = new AnimationHelper(spriteSheet, 3, 5);
            shootAnim = new AnimationHelper(spriteSheet, 3, 5);
            IsLoaded = false;
            weaponType = WeaponType.SHOOTABLE;
            BlocksMovementWhenUsed = false;
            loadingAnim.AnimationReady += OnBowLoaded;
            shootAnim.AnimationReady += OnShootAnimationEnded;
            this.damage = damage;
            this.power = arrowPower;
            this.speed = arrowSpeed;
            this.arrowFlyDistance = arrowFlyDistance;
        }

        /// <summary>
        /// Called when the arrow is loaded into the bow.
        /// </summary>
        private void OnBowLoaded()
        {
            BlocksMovementWhenUsed = false;
            isLoading = false;
            loadingAnim.Restart();
            IsLoaded = true;
        }

        /// <summary>
        /// Called after the shooting animation is played.
        /// </summary>
        private void OnShootAnimationEnded()
        {
            player.Arrows--;
            isShooting = false;
            shootAnim.Restart();
            isLoading = true;
            Level level = owner.LevelManager.CurrentLevel;
            owner.LevelManager.CurrentLevel.AddEntity(new EntityArrow(owner.LevelManager, owner.Direction, owner, damage, speed, power, arrowFlyDistance, 2 * 60), owner.XCenter + AIMOFFSETS[owner.Direction.ID].X, owner.YCenter + AIMOFFSETS[owner.Direction.ID].Y);
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="dir">The direction in which the weapon is used.</param>
        public override void Use(Direction dir)
        {
            if (IsInUse)
                return;

            IsInUse = true;
            currentDir = dir;
            isLoading = true;
            BlocksMovementWhenUsed = true;
            renderSpritePos = new Point(GetCurrentSpriteIDX(), GetCurrentSpriteIDY());
        }

        /// <summary>
        /// Stops using the weapon.
        /// </summary>
        public override void StopUsing()
        {
            IsInUse = false;
            isLoading = false;
            isShooting = false;
            BlocksMovementWhenUsed = false;
            walkingAnim.Restart();
            loadingAnim.Restart();
            shootAnim.Restart();
            IsLoaded = false;
        }

        /// <summary>
        /// Shoots the arrow in the bow.
        /// </summary>
        public void Shoot()
        {
            if (isShooting)
                return;
            isShooting = true;
            BlocksMovementWhenUsed = true;
        }

        /// <summary>
        /// Updates the weapon when owner has moved.
        /// </summary>
        public override void UpdateMovement()
        {
            walkingAnim.UpdateStep();
        }

        /// <summary>
        /// Called when the owner stops moving.
        /// </summary>
        /// <param name="dir">Current direction of the owner.</param>
        public override void OnMovementStopped(Direction dir)
        {
            currentDir = dir;
            walkingAnim.Reset();
        }

        /// <summary>
        /// Updates the weapon.
        /// </summary>
        public override void Update()
        {
            if (IsInUse)
            {
                if (isLoading)
                {
                    loadingAnim.UpdateStep();
                }
                else if (isShooting)
                {
                    shootAnim.UpdateStep();
                }
                renderSpritePos = new Point(GetCurrentSpriteIDX(), GetCurrentSpriteIDY());
            }
        }

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
        /// Gets the current sprite's position in sprite sheet (X).
        /// </summary>
        /// <returns>The current sprite's position in sprite sheet (X).</returns>
        private int GetCurrentSpriteIDX()
        {
            if (isLoading)
                return loadingAnim.CurrentSpriteID;
            else if (isShooting)
                return shootAnim.CurrentSpriteID + 3;
            return walkingAnim.CurrentSpriteID;
        }

        /// <summary>
        /// Gets the current sprite's position in sprite sheet (Y).
        /// </summary>
        /// <returns>the current sprite's position in sprite sheet (Y).</returns>
        private int GetCurrentSpriteIDY()
        {
            if (isLoading || isShooting)
                return 4 + owner.Direction.ID;
            return owner.Direction.ID;
        }

        /// <summary>
        /// Renders the owner and the weapon.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer to render to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(owner.X, owner.Y, renderSpritePos.X, renderSpritePos.Y, spriteSheet, owner.CurrentGlow, layer);
        }

        /// <summary>
        /// Converts this weapon to <see cref="EntityItem" /> that can be placed on a level.
        /// </summary>
        /// <returns>
        /// An <see cref="EntityItem" /> representation of this weapon.
        /// </returns>
        public override EntityItem ToItem()
        {
            return new EntityItemBow(owner.LevelManager, damage, speed, power, arrowFlyDistance);
        }
    }
}
