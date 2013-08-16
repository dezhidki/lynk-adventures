using LynkAdventures.Entities;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Graphics
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A camera.
    /// </summary>
    public class Camera
    {
        private Point pos;
        private Game game;
        private Entity entToFollow;
        private Point stopBorderTopLeft = Point.Zero;
        private Point stopBorderBottomRight = Point.Zero;

        /// <summary>
        /// Gets or sets a value indicating whether the camera has borders.
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera has borders; otherwise, <c>false</c>.
        /// </value>
        public bool HasBorders { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/>.
        /// </summary>
        /// <param name="xPos">The x position to start from.</param>
        /// <param name="yPos">The y position to start from.</param>
        /// <param name="game">The game instance.</param>
        public Camera(int xPos, int yPos, Game game)
        {
            this.game = game;
            pos = new Point(xPos, yPos);
            HasBorders = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/>.
        /// </summary>
        /// <param name="xPos">The x position to start from.</param>
        /// <param name="yPos">The y position to start from.</param>
        /// <param name="game">The game instance.</param>
        /// <param name="cameraBorderTopLeft">Camera's top left border's position.</param>
        /// <param name="cameraBorderBottomRight">Camera's bottom right border's position.</param>
        public Camera(int xPos, int yPos, Game game, Point cameraBorderTopLeft, Point cameraBorderBottomRight)
        {
            this.game = game;
            pos = new Point(xPos, yPos);
            stopBorderBottomRight = cameraBorderBottomRight;
            stopBorderTopLeft = cameraBorderTopLeft;
            HasBorders = true;
        }

        /// <summary>
        /// Gets or sets the left side of the camera.
        /// </summary>
        /// <value>
        /// The left side of the camera.
        /// </value>
        public int Left
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        /// <summary>
        /// Gets or sets the top of the camera.
        /// </summary>
        /// <value>
        /// The top of the camera.
        /// </value>
        public int Top
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        /// <summary>
        /// Gets or sets the right side of the camera.
        /// </summary>
        /// <value>
        /// The right side of the camera.
        /// </value>
        public int Right
        {
            get { return pos.X + Game.WIDTH; }
            set { pos.X = value - Game.WIDTH; }
        }

        /// <summary>
        /// Gets or sets the bottom of the camera.
        /// </summary>
        /// <value>
        /// The bottom of the camera.
        /// </value>
        public int Bottom
        {
            get { return pos.Y + Game.HEIGHT; }
            set { pos.Y = value - Game.HEIGHT; }
        }

        /// <summary>
        /// Gets or sets the top left corner of the camera.
        /// </summary>
        /// <value>
        /// The top left corner of the camera
        /// </value>
        public Point LeftTop
        {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// Gets or sets the bottom right corner of the camera.
        /// </summary>
        /// <value>
        /// The bottom right corner of the camera.
        /// </value>
        public Point RightBottom
        {
            get { return new Point(pos.X + Game.WIDTH, pos.Y + Game.HEIGHT); }
            set
            {
                pos.X = value.X - Game.WIDTH;
                pos.Y = value.Y - Game.HEIGHT;
            }
        }

        /// <summary>
        /// Gets or sets the top left border of the camera.
        /// </summary>
        /// <value>
        /// The left top left border of the camera.
        /// </value>
        public Point LeftTopBorder
        {
            get { return stopBorderTopLeft; }
            set { stopBorderTopLeft = value; }
        }

        /// <summary>
        /// Gets or sets the bottom right border of the camera.
        /// </summary>
        /// <value>
        /// The bottom right border of the camera.
        /// </value>
        public Point RightBottomBorder
        {
            get { return stopBorderBottomRight; }
            set { stopBorderBottomRight = value; }
        }

        /// <summary>
        /// Follows the entity.
        /// </summary>
        /// <param name="ent">Entity to follow.</param>
        public void FollowEntity(Entity ent)
        {
            entToFollow = ent;
        }

        /// <summary>
        /// Stops following the entity.
        /// </summary>
        public void StopFollowing()
        {
            entToFollow = null;
        }

        /// <summary>
        /// Updates the camera.
        /// </summary>
        public void Update()
        {
            if (entToFollow != null)
            {
                Left = entToFollow.XCenter - Game.WIDTH / 2;
                Top = entToFollow.YCenter - Game.HEIGHT / 2;
            }

            if (HasBorders)
            {
                if (Left < stopBorderTopLeft.X)
                    Left = stopBorderTopLeft.X;
                else if (Right > stopBorderBottomRight.X)
                    Right = stopBorderBottomRight.X;

                if (Top < stopBorderTopLeft.Y)
                    Top = stopBorderTopLeft.Y;
                else if (Bottom > stopBorderBottomRight.Y)
                    Bottom = stopBorderBottomRight.Y;
            }
        }
    }
}
