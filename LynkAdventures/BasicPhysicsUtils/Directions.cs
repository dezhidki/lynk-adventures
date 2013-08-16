using Microsoft.Xna.Framework;

namespace LynkAdventures.BasicPhysicsUtils
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A structure for a generic direction.
    /// </summary>
    public struct Direction
    {
        /// <summary>
        /// The ID of the direction.
        /// </summary>
        public int ID;

        /// <summary>
        /// A <see cref="Microsoft.Xna.Framework.Point"/> that represents the direction.
        /// </summary>
        public Point DirPoint;

        /// <summary>
        /// The angle of the direction.
        /// </summary>
        public float Angle;

        /// <summary>
        /// The <see cref="String"/> representation of the direction.
        /// </summary>
        public string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direction"/>.
        /// </summary>
        /// <param name="id">The ID of the direction.</param>
        /// <param name="point">The vector representation of the direction.</param>
        /// <param name="angle">The angle of the direction.</param>
        /// <param name="name">The <see cref="String"/> representation of the direction.</param>
        public Direction(int id, Point point, float angle, string name)
        {
            ID = id;
            DirPoint = point;
            Angle = angle;
            Name = name;
        }

        /// <summary>
        /// Checks if both directions are equal.
        /// </summary>
        /// <param name="left">Direction 1.</param>
        /// <param name="right">Direction 2.</param>
        /// <returns><c>true</c>, if both directions are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Direction left, Direction right)
        {
            return left.ID == right.ID;
        }

        /// <summary>
        /// Checks if both directions are not equal.
        /// </summary>
        /// <param name="left">Direction 1.</param>
        /// <param name="right">Direction 2.</param>
        /// <returns><c>true</c>, if both directions are not equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Direction left, Direction right)
        {
            return left.ID != right.ID;
        }

        /// <summary>
        /// Checks if the given number is the same as the given direction's ID.
        /// </summary>
        /// <param name="dirID">Direction ID.</param>
        /// <param name="right">Direction 2.</param>
        /// <returns><c>true</c>, if given number is the ID of the direction; otherwise <c>false</c>.</returns>
        public static bool operator ==(int dirID, Direction right)
        {
            return dirID == right.ID;
        }

        /// <summary>
        /// Checks if the given number is different from the given direction's ID.
        /// </summary>
        /// <param name="dirID">Direction ID.</param>
        /// <param name="right">Direction 2.</param>
        /// <returns><c>true</c>, if given number isn't direction's ID; otherwise <c>false</c>.</returns>
        public static bool operator !=(int dirID, Direction right)
        {
            return dirID != right.ID;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + ID.GetHashCode();
            hash = (hash * 7) + DirPoint.GetHashCode();
            hash = (hash * 7) + Angle.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            return hash;
        }
    }

    /// @author Denis Zhidkikh
    /// <summary>
    /// Class that contains the most used directions in the game.
    /// </summary>
    public static class Directions
    {
        /// <summary>
        /// Up direction.
        /// </summary>
        public static readonly Direction UP = new Direction(0, new Point(0, -1), 0, "NORTH");

        /// <summary>
        /// Down direction.
        /// </summary>
        public static readonly Direction DOWN = new Direction(1, new Point(0, 1), MathHelper.ToRadians(180), "SOUTH");

        /// <summary>
        /// Left direction.
        /// </summary>
        public static readonly Direction LEFT = new Direction(2, new Point(-1, 0), MathHelper.ToRadians(90), "WEST");

        /// <summary>
        /// Right direction.
        /// </summary>
        public static readonly Direction RIGHT = new Direction(3, new Point(1, 0), MathHelper.ToRadians(270), "EAST");

        /// <summary>
        /// No direction (null direction).
        /// </summary>
        public static readonly Direction NONE = new Direction(-1, Point.Zero, 0, "NONE");

        /// <summary>
        /// Array with all directions in their natural order (by ID).
        /// </summary>
        public static readonly Direction[] DIRS = { UP, DOWN, LEFT, RIGHT, NONE };


        /// <summary>
        /// Gets the opposite direction of the given one.
        /// </summary>
        /// <param name="dir">Direction.</param>
        /// <returns>Opposite direction of the given one.</returns>
        public static Direction GetOppositeDir(Direction dir)
        {
            Point pointDir = dir.DirPoint;
            return ByDirPoint(new Point(-pointDir.X, -pointDir.Y));
        }

        /// <summary>
        /// Gets the direction by given vector representation.
        /// </summary>
        /// <param name="dirPoint">Vector representation of direction.</param>
        /// <returns><see cref="Direction"/> that has the same vector.</returns>
        public static Direction ByDirPoint(Point dirPoint)
        {
            for (int i = 0; i < DIRS.Length; i++)
            {
                if (DIRS[i].DirPoint == dirPoint)
                    return DIRS[i];
            }
            return NONE;
        }
    }
}
