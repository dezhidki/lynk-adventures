using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;

namespace LynkAdventures.Entities.Weapons
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// Weapon's type.
    /// </summary>
    public enum WeaponType
    {
        GENERIC, MELEE, SHIELD, SHOOTABLE
    }

    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// Weapon.
    /// </summary>
    public class Weapon
    {
        protected Entity owner;
        protected int spriteSheetID = 0;
        protected int damage = 0;
        protected int power = 0;
        protected int speed = 0;
        protected WeaponType weaponType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/>.
        /// </summary>
        /// <param name="owner">Owner of the weapon.</param>
        public Weapon(Entity owner)
        {
            BlocksMovementWhenUsed = false;
            this.owner = owner;
            weaponType = WeaponType.GENERIC;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the weapon blocks any owner's movement while used.
        /// </summary>
        /// <value>
        /// <c>true</c> if the weapon blocks any owner's movement while used; otherwise, <c>false</c>.
        /// </value>
        public bool BlocksMovementWhenUsed { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the weapon is currently in use.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the weapon is currently in use; otherwise, <c>false</c>.
        /// </value>
        public bool IsInUse { get; protected set; }

        /// <summary>
        /// Gets the damage this weapon inflicts.
        /// </summary>
        /// <value>
        /// The damage this weapon inflicts.
        /// </value>
        public int Damage { get { return damage; } }

        /// <summary>
        /// Gets the push power of the weapon.
        /// </summary>
        /// <value>
        /// The push power of the weapon.
        /// </value>
        public int PushPower { get { return power; } }

        /// <summary>
        /// Gets the speed rate of the weapon.
        /// </summary>
        /// <value>
        /// The speed rate of the weapon.
        /// </value>
        public int Speed { get { return speed; } }

        /// <summary>
        /// Initializes the weapon.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Uses the weapon.
        /// </summary>
        /// <param name="dir">The direction in which the weapon is used.</param>
        public virtual void Use(Direction dir)
        {
        }

        /// <summary>
        /// Gets the owner's move speed.
        /// </summary>
        /// <returns>Owner's move speed.</returns>
        public virtual int GetMoveSpeed()
        {
            return 0;
        }

        /// <summary>
        /// Stops using the weapon.
        /// </summary>
        public virtual void StopUsing()
        {
        }

        /// <summary>
        /// Updates the weapon when owner has moved.
        /// </summary>
        public virtual void UpdateMovement()
        {
        }

        /// <summary>
        /// Called when the owner stops moving.
        /// </summary>
        /// <param name="dir">Current direction of the owner.</param>
        public virtual void OnMovementStopped(Direction dir)
        {
        }

        /// <summary>
        /// Updates the weapon.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Renders the owner and the weapon.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer to render to.</param>
        public virtual void Render(Renderer renderer, float layer)
        {
        }

        /// <summary>
        /// Converts this weapon to <see cref="EntityItem"/> that can be placed on a level.
        /// </summary>
        /// <returns>An <see cref="EntityItem"/> representation of this weapon.</returns>
        public virtual EntityItem ToItem()
        {
            return new EntityItem(owner.LevelManager);
        }
    }
}
