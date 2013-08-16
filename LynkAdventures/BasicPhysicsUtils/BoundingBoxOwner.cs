using LynkAdventures.Entities;

namespace LynkAdventures.BasicPhysicsUtils
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Interface for every object in the game world that owns a <see cref="BoundingBox2D"/>;
    /// </summary>
    public interface BoundingBoxOwner
    {

        /// <summary>
        /// An event for collider when it colldes with an entity.
        /// </summary>
        /// <param name="ent">The entity that collided.</param>
        /// <param name="bb">The bounding box of the entity.</param>
        void OnCollidedWidth(Entity ent, BoundingBox2D bb);

        /// <summary>
        /// An event for collided entity when it collides with an object.
        /// </summary>
        /// <param name="owner">Collider.</param>
        /// <param name="bb">Collider's bounding box.</param>
        void OnCollidedBy(BoundingBoxOwner owner, BoundingBox2D bb);
    }
}
