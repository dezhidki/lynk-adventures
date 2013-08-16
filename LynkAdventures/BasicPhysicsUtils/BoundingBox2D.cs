using Microsoft.Xna.Framework;

namespace LynkAdventures.BasicPhysicsUtils
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A bounding box for an object in the world.
    /// </summary>
    public class BoundingBox2D
    {
        /// <summary>
        /// A bounding box with all parameters set to 0.
        /// </summary>
        public static BoundingBox2D Zero = new BoundingBox2D(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the left side of the bounding box.
        /// </summary>
        /// <value>
        /// The left side of the bounding box.
        /// </value>
        public int XLeft { get; set; }

        /// <summary>
        /// Gets or sets the right side of the bounding box.
        /// </summary>
        /// <value>
        /// The right side of the bounding box.
        /// </value>
        public int XRight { get; set; }

        /// <summary>
        /// Gets or sets the top of the bounding box.
        /// </summary>
        /// <value>
        /// The top of the bounding box.
        /// </value>
        public int YTop { get; set; }

        /// <summary>
        /// Gets or sets the bottom of the bounding box.
        /// </summary>
        /// <value>
        /// The bottom of the bounding box.
        /// </value>
        public int YBottom { get; set; }

        /// <summary>
        /// Gets the owner of this bounding box.
        /// </summary>
        /// <value>
        /// The owner of this bounding box.
        /// </value>
        public BoundingBoxOwner Owner { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox2D"/>.
        /// </summary>
        /// <param name="x0">The left side (X).</param>
        /// <param name="y0">The top (Y).</param>
        /// <param name="x1">The right side (X).</param>
        /// <param name="y1">The bottom (Y).</param>
        /// <param name="owner">The owner. If null, then the bounding box has no owner.</param>
        public BoundingBox2D(int x0, int y0, int x1, int y1, BoundingBoxOwner owner = null)
        {
            XLeft = x0;
            YTop = y0;
            XRight = x1;
            YBottom = y1;
            Owner = owner;
        }

        /// <summary>
        /// Checks if the bounding box intersects with given box.
        /// </summary>
        /// <param name="x0">The left side of the box (X).</param>
        /// <param name="y0">The top of the box (Y).</param>
        /// <param name="x1">The right side of the box (X).</param>
        /// <param name="y1">The bottom of the box (Y).</param>
        /// <returns><c>true</c>, if there is an intersection; otherwise <c>false</c>.</returns>
        public bool Intersects(int x0, int y0, int x1, int y1)
        {
            return !(XRight < x0 || YBottom < y0 || XLeft > x1 || YTop > y1);
        }

        /// <summary>
        /// Checks if the bounding box intersects with another bounding box.
        /// </summary>
        /// <param name="bb">Bounding box to check.</param>
        /// <returns><c>true</c>, if there is an intersection; otherwise <c>false</c>.</returns>
        public bool Intersects(BoundingBox2D bb)
        {
            return Intersects(bb.XLeft, bb.YTop, bb.XRight, bb.YBottom);
        }

        /// <summary>
        /// Moves the bounding box by given amount.
        /// </summary>
        /// <param name="bb">Bounding box.</param>
        /// <param name="point">The offset to move by.</param>
        /// <returns>New bounding box which is offsetted by the given amount.</returns>
        public static BoundingBox2D operator +(BoundingBox2D bb, Point point)
        {
            return new BoundingBox2D(bb.XLeft + point.X, bb.YTop + point.Y, bb.XRight + point.X, bb.YBottom + point.Y);
        }

        /// <summary>
        /// Adds two bounding boxes together giving a new bounding box.
        /// </summary>
        /// <param name="left">Bounding box 1.</param>
        /// <param name="right">Bounding box 2.</param>
        /// <returns>A new bounding box which size and offset is the sum of two given bounding boxes.</returns>
        public static BoundingBox2D operator +(BoundingBox2D left, BoundingBox2D right)
        {
            return new BoundingBox2D(left.XLeft + right.XLeft, left.YTop + right.YTop, left.XRight + right.XRight, left.YBottom + right.YBottom);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "BB2D{" + Owner.ToString() + "} (Left:" + XLeft + " Top: " + YTop + " Right: " + XRight + " Bottom: " + YBottom + ")";
        }
    }
}
