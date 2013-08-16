using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Gui;
using LynkAdventures.World;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// An entity which has health.
    /// </summary>
    public class EntityLiving : Entity
    {
        protected int health = 0, maxHealth = 10;
        protected bool hasInfinteHealth = false;
        protected bool isImmortalWhenGlowing = false;
        protected bool hasHealthBar = false;
        private bool isHealthInitialized = false;
        private GuiHealthBar healthBar;
        private int maxBarDisplayTime = 100, currentBarDisplayTime = 0;
        private bool showHealthBar = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityLiving"/> class.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntityLiving(LevelManager manager)
            : base(manager)
        {
            healthBar = new GuiHealthBar(this);
        }

        /// <summary>
        /// Gets or sets the health of the entity.
        /// </summary>
        /// <value>
        /// The health.
        /// </value>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Gets the max health this entity can have.
        /// </summary>
        /// <value>
        /// The max health this entity can have.
        /// </value>
        public int MaxHealth { get { return maxHealth; } }

        /// <summary>
        /// Gets a value indicating whether this entity is immortal. Can still be removed using <see cref="Die"/> method.
        /// </summary>
        /// <value>
        /// <c>true</c> if this entity is immortal; otherwise, <c>false</c>.
        /// </value>
        public bool IsImmortal { get { return hasInfinteHealth; } }

        /// <summary>
        /// Initializes the entity right before adding to the <see cref="Level" />.
        /// </summary>
        public override void Init()
        {
            base.Init();
            if (!hasInfinteHealth && !isHealthInitialized)
            {
                health = maxHealth;
                isHealthInitialized = true;
            }
            if (hasHealthBar)
            {
                Game.GuiManager.LoadGui(healthBar, false);
            }
        }

        /// <summary>
        /// Hits the this entity.
        /// </summary>
        /// <param name="damager">The damager.</param>
        /// <param name="damage">The damage amount.</param>
        public override void Hit(HitSource damager, int damage = 0)
        {
            if (isGlowing && isImmortalWhenGlowing) return;

            if (!hasInfinteHealth)
                health -= damage;

            if (health <= 0)
            {
                OnKilledBy(damager);
                Die();
                return;
            }

            if (!showHealthBar)
            {
                currentBarDisplayTime = 0;
                showHealthBar = true;
                healthBar.Activate();
            }
            else
                currentBarDisplayTime = 0;

            base.Hit(damager, damage);
        }

        /// <summary>
        /// Called when entity is dead and is being removed.
        /// </summary>
        public override void OnDeath()
        {
            if (hasHealthBar)
                healthBar.Close(true);
        }

        /// <summary>
        /// Called when the level is changed.
        /// </summary>
        /// <param name="newLevel">The level <see cref="LevelManager" /> is changing to.</param>
        public override void OnLevelChange(Level newLevel)
        {
            if (hasHealthBar)
                healthBar.Close();
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (showHealthBar && hasHealthBar)
            {
                currentBarDisplayTime++;
                if (currentBarDisplayTime >= maxBarDisplayTime)
                {
                    showHealthBar = false;
                    healthBar.Close();
                }
            }
        }
    }
}
