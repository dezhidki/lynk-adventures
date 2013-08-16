using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World.Rooms
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Room that can be placed on the level.
    /// </summary>
    public class Room
    {
        protected int xTileOffs, yTileOffs;
        protected Level level;
        protected LevelManager manager;
        protected int width, height;
        protected Texture2D tileMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/>.
        /// </summary>
        /// <param name="xTileOffs">The x tile offset (level grid).</param>
        /// <param name="yTileOffs">The y tile offset (level grid).</param>
        /// <param name="tileMap">The tile map texture.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">The manager.</param>
        public Room(int xTileOffs, int yTileOffs, Texture2D tileMap, Level level, LevelManager manager)
        {
            this.xTileOffs = xTileOffs;
            this.yTileOffs = yTileOffs;
            this.level = level;
            this.manager = manager;
            this.tileMap = tileMap;
        }

        /// <summary>
        /// Initialize the room.
        /// </summary>
        public virtual void InitRoom()
        {
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Sets the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="tileID">The tile ID.</param>
        public void SetTile(int xTile, int yTile, byte tileID)
        {
            if (!IsValidPos(xTile, yTile)) return;
            level.SetTile(xTile + xTileOffs, yTile + yTileOffs, tileID);
        }

        /// <summary>
        /// Sets the data value to a tile position.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="dataID">The data value.</param>
        public void SetData(int xTile, int yTile, byte dataID)
        {
            if (!IsValidPos(xTile, yTile)) return;
            level.SetData(dataID, xTile + xTileOffs, yTile + yTileOffs);
        }

        /// <summary>
        /// Determines whether the specified position is valid (inside the room).
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <returns>
        ///   <c>true</c> if the specified position is valid (inside the room); otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidPos(int xTile, int yTile)
        {
            return (xTile < 0 || xTile >= width || yTile < 0 || yTile >= height);
        }
    }
}
