using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.World;

namespace LynkAdventures.Entities.TileEntities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A tile in the world, that has the properties of an entity.
    /// </summary>
    public class TileEntity : Entity
    {
        protected Level level;
        protected bool shouldUpdate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntity"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="manager">Level manager.</param>
        public TileEntity(Level level, LevelManager manager)
            : base(manager)
        {
            this.level = level;
            canBePushed = false;
            CanGlowOnHit = false;
        }

        /// <summary>
        /// Gets the tile coordinate on the level grid (X).
        /// </summary>
        /// <value>
        /// The coordinate on the level grid (X).
        /// </value>
        public int XTile { get { return X >> Level.TILESHIFT; } }

        /// <summary>
        /// Gets the tile coordinate on the level grid (Y).
        /// </summary>
        /// <value>
        /// The coordinate on the level grid (Y).
        /// </value>
        public int YTile { get { return Y >> Level.TILESHIFT; } }

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(X, Y, X + Width, Y + Height, this);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            if (!shouldUpdate)
                return;
        }
    }
}
