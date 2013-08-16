using System;

namespace LynkAdventures.BasicPhysicsUtils
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// Kuvaa jotakin voimalähdettä, joka voi työntää jotakin pelimaailmassa olevaa.
    /// </summary>
    public struct HitSource
    {

        /// @author Denis Zhidkikh
        /// @version 23.3.2013
        /// <summary>
        /// Type of the <see cref="HitSource"/>'s owner.
        /// </summary>
        public enum OwnerType
        {
            ENTITY, WEAPON, WORLD, OTHER, PROJECTILE
        }

        /// <summary>
        /// The amount of push power.
        /// </summary>
        public int PushPower;
        
        /// <summary>
        /// <see cref="Direction"/> where to push.
        /// </summary>
        public readonly Direction Direction;

        /// <summary>
        /// The creator of this <see cref="HitSource"/>.
        /// </summary>
        public readonly object Creator;

        /// <summary>
        /// The <see cref="OwnerType"/> of this <see cref="HitSource"/>.
        /// </summary>
        public readonly OwnerType CreatorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="HitSource"/>.
        /// </summary>
        /// <param name="dir">Direction in which object is pushed.</param>
        /// <param name="power">The power.</param>
        /// <param name="owner">The owner of this instance.</param>
        /// <param name="creatorType">The <see cref="OwnerType"/> of the owner.</param>
        public HitSource(Direction dir, int power, Object owner, OwnerType creatorType)
        {
            PushPower = power;
            Creator = owner;
            Direction = dir;
            CreatorType = creatorType;
        }
    }
}
