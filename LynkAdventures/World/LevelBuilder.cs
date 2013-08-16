using LynkAdventures.Entities;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LynkAdventures.World
{

    /// <summary>
    /// A method that is called on the tile.
    /// </summary>
    /// <param name="xTile">The x tile.</param>
    /// <param name="yTile">The y tile.</param>
    public delegate void TileCreatorMethod(int xTile, int yTile);

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A level builder, that creates levels from a color map.
    /// </summary>
    public class LevelBuilder
    {
        private Texture2D colorMap;
        private Level level;
        private Dictionary<uint, List<TileCreatorMethod>> tileList;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelBuilder"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="colorMap">The color map that contains the information about the game objects and their position.</param>
        public LevelBuilder(Level level, Texture2D colorMap)
        {
            this.level = level;
            this.colorMap = colorMap;
            tileList = new Dictionary<uint, List<TileCreatorMethod>>();
        }

        /// <summary>
        /// Binds the tile to the color in the color map.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="tileID">The tile ID.</param>
        /// <param name="data">The data value.</param>
        public void BindTile(uint color, byte tileID, byte data = 0)
        {
            TileCreatorMethod creator =
                delegate(int xTile, int yTile)
                {
                    level.SetTile(xTile, yTile, tileID);
                    level.SetData(data, xTile, yTile);
                };

            if (!tileList.ContainsKey(color))
                tileList.Add(color, new List<TileCreatorMethod>());
            tileList[color].Add(creator);
        }

        /// <summary>
        /// Binds the entity to the color in the color map.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="entType"><see cref="System.Type"/> of the entity.</param>
        /// <param name="args">Arguments for the entity's class's constructor.</param>
        /// <exception cref="System.ArgumentException">Trying to add an entity, which doesn't inherit Entity in any way.</exception>
        public void BindEntity(uint color, Type entType, params object[] args)
        {
            if (!typeof(Entity).IsAssignableFrom(entType))
                throw new ArgumentException("Trying to add an entity, which doesn't inherit Entity in any way", "entType");

            TileCreatorMethod creator =
                delegate(int xTile, int yTile)
                {
                    level.AddEntity((Entity)Activator.CreateInstance(entType, args), xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                };

            if (!tileList.ContainsKey(color))
                tileList.Add(color, new List<TileCreatorMethod>());
            tileList[color].Add(creator);
        }

        /// <summary>
        /// Binds the custom <see cref="TileCreatorMethod"/> to the color in the color map.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="creator">The method.</param>
        public void BindCustom(uint color, TileCreatorMethod creator)
        {
            if (!tileList.ContainsKey(color))
                tileList.Add(color, new List<TileCreatorMethod>());
            tileList[color].Add(creator);
        }

        /// <summary>
        /// Builds the level.
        /// </summary>
        /// <param name="xOffs">The x offset.</param>
        /// <param name="yOffs">The y offset.</param>
        public void BuildLevel(int xOffs = 0, int yOffs = 0)
        {
            uint[] cols = new uint[colorMap.Width * colorMap.Height];
            colorMap.GetData<uint>(cols);

            for (int yTile = 0; yTile < colorMap.Height; yTile++)
            {
                int tileY = yTile + yOffs;
                if (tileY < 0 || tileY >= level.Height) continue;

                for (int xTile = 0; xTile < colorMap.Width; xTile++)
                {
                    int tileX = xTile + xOffs;
                    if (tileX < 0 || tileX >= level.Width) continue;

                    uint col = cols[xTile + yTile * colorMap.Width];
                    if (tileList.ContainsKey(col))
                    {
                        tileList[col].ForEach(method => method(tileX, tileY));
                    }
                }
            }
        }
    }
}
