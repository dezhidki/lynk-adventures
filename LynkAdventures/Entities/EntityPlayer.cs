using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Controls;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LynkAdventures.Entities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// The player.
    /// </summary>
    public class EntityPlayer : EntityLiving
    {
        public const int WEAPON_SWORD = 0;
        public const int WEAPON_SHIELD = 1;
        public const int WEAPON_BOW = 2;
        private const int SPRITE_ID_LYNK_STANDING = 4;
        private AnimationHelper walkAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_LYNK_WALK, 9, 5, true);
        private bool hasMoved = false;
        private Weapon[] weapons;
        private bool[] weaponsInUse;
        private int currentWeaponInUse = -1;
        private bool usedShieldBeforeSword = false;
        private int renderSprite = 0;
        private int arrowAmmo = 0;
        private int rupiesAmount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPlayer"/>.
        /// </summary>
        /// <param name="levelManager">The level manager.</param>
        public EntityPlayer(LevelManager levelManager)
            : base(levelManager)
        {
            hasInfinteHealth = false;
            maxHealth = 10;
            maxGlowTime = 20;
            pushResistancePower = 2;
            weaponsInUse = new bool[3];
            weapons = new Weapon[3];
            xRadius = 22;
            yRadius = 20;
            MoveSpeed = 3;
            isImmortalWhenGlowing = true;
            interactArea = new BoundingBox2D(20, 20, 20, 20);
            interactRadius = new Point(10, 10);
        }

        /// <summary>
        /// Gets or sets the amount of arrows the player has.
        /// </summary>
        /// <value>
        /// The amount of arrows the player has.
        /// </value>
        public int Arrows
        {
            get { return arrowAmmo; }
            set { arrowAmmo = value; }
        }

        /// <summary>
        /// Gets or sets the amount of rupies the player has.
        /// </summary>
        /// <value>
        /// The amount of rupies the player has.
        /// </value>
        public int Rupies
        {
            get { return rupiesAmount; }
            set { rupiesAmount = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the player has any arrows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the player has any arrows; otherwise, <c>false</c>.
        /// </value>
        public bool HasArrows { get { return arrowAmmo > 0; } }

        /// <summary>
        /// Called when entity is dead and is being removed.
        /// </summary>
        public override void OnDeath()
        {
            base.OnDeath();

            Game.GuiManager.LoadAsActiveGui(Game.DeathMenu);
        }

        private int xOffs0 = 9 * Game.SCALE, yOffs0 = 2 * Game.SCALE, xOffs1 = 8 * Game.SCALE, yOffs1 = 10 * Game.SCALE;

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - xOffs0, YCenter + yOffs0, XCenter + xOffs1, YCenter + yOffs1, this);
        }

        /// <summary>
        /// Hits the this entity.
        /// </summary>
        /// <param name="damager">The damager.</param>
        /// <param name="damage">The damage amount.</param>
        public override void Hit(HitSource damager, int damage = 0)
        {
            if (isGlowing && isImmortalWhenGlowing) return;

            bool damageStopped = false;

            if (weapons[WEAPON_SHIELD] != null && weapons[WEAPON_SHIELD].IsInUse)
            {
                WeaponShield shield = (WeaponShield)weapons[WEAPON_SHIELD];
                if (Directions.GetOppositeDir(damager.Direction) == Direction)
                {
                    damage -= shield.DamageProtection;
                    if (damage <= 0)
                    {
                        damage = 0;
                        damageStopped = true;
                    }
                    damager.PushPower -= shield.PushProtection;
                    if (damager.PushPower < 0)
                        damager.PushPower = 0;
                }
            }

            if (damage > 0)
                Sound.Hurt.Play();

            base.Hit(damager, damage);
            isGlowing = !damageStopped;
        }

        /// <summary>
        /// Checks if this entity is solid to the given entity.
        /// </summary>
        /// <param name="ent">The entity to check with.</param>
        /// <returns>
        ///   <c>true</c> if this entity is solid to the given entity; otherwise, <c>false</c>.
        /// </returns>
        public override bool SolidToEntity(Entity ent)
        {
            return true;
        }

        /// <summary>
        /// Gets the array of weapons the player has.
        /// </summary>
        /// <value>
        /// The array of weapons the player has.
        /// </value>
        public Weapon[] Weapons
        {
            get { return weapons; }
        }

        /// <summary>
        /// Adds the weapon to the weapon array.
        /// </summary>
        /// <param name="weapon">The weapon.</param>
        /// <param name="index">The index to which the weapon will be added.</param>
        public void AddWeapon(Weapon weapon, int index)
        {
            weapons[index] = weapon;
        }

        /// <summary>
        /// Gets the weapon currently in use.
        /// </summary>
        /// <returns>The weapon that is currently in use.</returns>
        public Weapon GetWeaponInUse()
        {
            if (currentWeaponInUse == -1)
                return null;
            return weapons[currentWeaponInUse];
        }

        /// <summary>
        /// Updates the weapons.
        /// </summary>
        private void UpdateWeapons()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                Weapon weapon = weapons[i];

                if (weapon != null)
                {
                    weapon.Update();
                }
            }
            if (currentWeaponInUse != -1 && !weapons[currentWeaponInUse].IsInUse)
                currentWeaponInUse = -1;
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="index">The index of the weapon.</param>
        private void UseWeapon(int index)
        {
            currentWeaponInUse = index;
            weaponsInUse[index] = true;
            weapons[index].Use(Direction);
        }

        /// <summary>
        /// Stops using the weapon.
        /// </summary>
        /// <param name="index">The index of the weapon.</param>
        private void StopUsing(int index)
        {
            if (index < 0 || index >= weapons.Length)
                return;

            weapons[index].StopUsing();
            weaponsInUse[index] = false;

            if (currentWeaponInUse == index)
                currentWeaponInUse = -1;
        }

        /// <summary>
        /// Determines whether the weapon is in use.
        /// </summary>
        /// <param name="weaponIndex">Index of the weapon.</param>
        /// <returns>
        ///   <c>true</c> if the weapon is in use; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInUse(int weaponIndex)
        {
            return weaponsInUse[weaponIndex];
        }

        /// <summary>
        /// Checks if the shield was used before the sword and should be used again.
        /// </summary>
        private void CheckSwordUseState()
        {
            if (!weapons[WEAPON_SWORD].IsInUse && IsInUse(WEAPON_SWORD))
            {
                if (usedShieldBeforeSword && KeyboardHandler.IsKeyDown(Keys.LeftControl))
                {
                    StopUsing(WEAPON_SWORD);
                    UseWeapon(WEAPON_SHIELD);
                    usedShieldBeforeSword = false;
                }
                else StopUsing(WEAPON_SWORD);
            }
        }

        /// <summary>
        /// Updates the state of the sword.
        /// </summary>
        private void UpdateSwordState()
        {
            if (weapons[WEAPON_SWORD] == null || IsInUse(WEAPON_BOW)) return;
            CheckSwordUseState();

            if (KeyboardHandler.IsKeyPressed(Keys.Space))
            {
                if (IsInUse(WEAPON_SHIELD))
                {
                    usedShieldBeforeSword = true;
                    StopUsing(WEAPON_SHIELD);
                }
                UseWeapon(WEAPON_SWORD);
            }
        }

        /// <summary>
        /// Updates the state of the bow.
        /// </summary>
        private void UpdateBowState()
        {
            if (weapons[WEAPON_BOW] == null || IsInUse(WEAPON_SWORD) || IsInUse(WEAPON_SHIELD)) return;
            WeaponBow bow = ((WeaponBow)weapons[WEAPON_BOW]);
            if (!HasArrows)
                StopUsing(WEAPON_BOW);

            if (KeyboardHandler.IsKeyPressed(Keys.LeftShift) && HasArrows)
                UseWeapon(WEAPON_BOW);
            else if (!KeyboardHandler.IsKeyDown(Keys.LeftShift))
                StopUsing(WEAPON_BOW);

            if (KeyboardHandler.IsKeyPressed(Keys.Space) && bow.IsLoaded)
                bow.Shoot();
        }

        /// <summary>
        /// Updates the state of the shield.
        /// </summary>
        private void UpdateShieldState()
        {
            if (weapons[WEAPON_SHIELD] == null || IsInUse(WEAPON_SWORD) || IsInUse(WEAPON_BOW)) return;

            if (KeyboardHandler.IsKeyPressed(Keys.LeftControl))
                UseWeapon(WEAPON_SHIELD);
            else if (!KeyboardHandler.IsKeyDown(Keys.LeftControl))
                StopUsing(WEAPON_SHIELD);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();
            UpdateWeapons();
            hasMoved = false;

            UpdateBowState();
            UpdateShieldState();
            UpdateSwordState();

            if (KeyboardHandler.IsKeyPressed(Keys.E))
                Interact(Direction, true);

            Weapon usedWeapon = GetWeaponInUse();
            int speed = usedWeapon == null ? MoveSpeed : usedWeapon.GetMoveSpeed();

            if (usedWeapon == null || !usedWeapon.BlocksMovementWhenUsed)
            {
                if (KeyboardHandler.IsKeyDown(Keys.A))
                {
                    Direction = Directions.LEFT;
                    hasMoved |= Move(-speed, 0);
                }
                if (KeyboardHandler.IsKeyDown(Keys.D))
                {
                    Direction = Directions.RIGHT;
                    hasMoved |= Move(speed, 0);
                }
                if (KeyboardHandler.IsKeyDown(Keys.W))
                {
                    Direction = Directions.UP;
                    hasMoved |= Move(0, -speed);
                }
                if (KeyboardHandler.IsKeyDown(Keys.S))
                {
                    Direction = Directions.DOWN;
                    hasMoved |= Move(0, speed);
                }
            }

            if (hasMoved)
            {
                if (usedWeapon != null)
                    usedWeapon.UpdateMovement();
                else
                    walkAnimation.UpdateStep();
            }
            else if (!hasMoved)
            {
                if (usedWeapon != null)
                    usedWeapon.OnMovementStopped(Direction);
                else
                    walkAnimation.Restart(Direction.ID);
            }

            if (usedWeapon == null)
                renderSprite = hasMoved ? Direction.ID : SPRITE_ID_LYNK_STANDING;
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            Weapon used = GetWeaponInUse();
            if (used != null)
            {
                used.Render(renderer, layer);
            }
            else
                renderer.RenderTile(X, Y, walkAnimation.CurrentSpriteID, renderSprite, walkAnimation.SpriteSheet, currentGlow, layer);
        }
    }
}
