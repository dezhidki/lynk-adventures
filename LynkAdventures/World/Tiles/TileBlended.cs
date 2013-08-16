using LynkAdventures.BasicPhysicsUtils;
using Microsoft.Xna.Framework;

namespace LynkAdventures.World.Tiles
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A type of blended tiles.
    /// </summary>
    public class TileBlended : Tile
    {
        protected readonly Point[] OFFSETS = { Point.Zero, new Point(1, 1), new Point(1, 0), new Point(0, 1) };
        protected Direction dir;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileBlended"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="dir">The blending direction.</param>
        public TileBlended(Level level, Direction dir)
            : base(level)
        {
            this.dir = dir;
        }
    }
}
