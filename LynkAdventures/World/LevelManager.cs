using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LynkAdventures.World
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A level manager responsible for drawing, updating and handling levels.
    /// </summary>
    public class LevelManager
    {
        private List<Level> loadedLevels;
        private int currentLevelID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelManager"/>.
        /// </summary>
        /// <param name="currentLevelID">The current level ID.</param>
        /// <param name="levels">Levels to register on construction.</param>
        public LevelManager(int currentLevelID, params Level[] levels)
        {
            this.currentLevelID = currentLevelID;

            if (levels.Length != 0)
            {
                loadedLevels = levels.ToList();
            }
            else
                loadedLevels = new List<Level>();
        }

        /// <summary>
        /// Gets a value indicating whether are there any levels loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if there are any levels loaded; otherwise, <c>false</c>.
        /// </value>
        public bool AreAnyLevelsLoaded
        {
            get { return loadedLevels.Count != 0; }
        }

        /// <summary>
        /// Gets the current level.
        /// </summary>
        /// <value>
        /// The current level.
        /// </value>
        public Level CurrentLevel
        {
            get { return loadedLevels[currentLevelID]; }
        }

        /// <summary>
        /// Gets the current level ID.
        /// </summary>
        /// <value>
        /// The current level ID.
        /// </value>
        public int CurrentLevelID { get { return currentLevelID; } }

        /// <summary>
        /// Gets the amount of levels.
        /// </summary>
        /// <value>
        /// The amount of levels.
        /// </value>
        public int LevelCount { get { return loadedLevels.Count; } }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <param name="levelID">The level ID.</param>
        /// <returns>Level by its ID.</returns>
        public Level GetLevel(int levelID)
        {
            if (levelID > loadedLevels.Count)
                return null;

            return loadedLevels[levelID];
        }

        /// <summary>
        /// Initializes all the levels.
        /// </summary>
        public void InitAllLevels()
        {
            foreach (Level level in loadedLevels)
            {
                level.InitLevel();
            }
        }

        /// <summary>
        /// Adds the level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>ID of the level.</returns>
        public int AddLevel(Level level)
        {
            loadedLevels.Add(level);

            return loadedLevels.IndexOf(level);
        }

        /// <summary>
        /// Changes the level to another one.
        /// </summary>
        /// <param name="levelID">The level ID to change to.</param>
        public void ChangeLevel(int levelID)
        {
            Level nextLevel = GetLevel(levelID);
            ChangeLevel(levelID, nextLevel.SpawnPoint.X, nextLevel.SpawnPoint.Y);
        }

        /// <summary>
        /// Changes the level.
        /// </summary>
        /// <param name="levelID">The level ID to change to.</param>
        /// <param name="playerPosX">The player positon (X).</param>
        /// <param name="playerPosY">The player poition (Y).</param>
        public void ChangeLevel(int levelID, int playerPosX, int playerPosY)
        {
            EntityPlayer player = CurrentLevel.Player;

            foreach (Entity ent in CurrentLevel.Entities)
            {
                ent.OnLevelChange(GetLevel(levelID));
            }

            CurrentLevel.OnLevelChange();
            CurrentLevel.RemoveEntity(player);
            CurrentLevel.Player = null;

            currentLevelID = levelID;
            CurrentLevel.AddEntity(player, playerPosX, playerPosY);
            CurrentLevel.OnLevelEntered();
        }

        /// <summary>
        /// Renders the current level.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="renderer">The renderer.</param>
        public void RenderLevel(Camera camera, Renderer renderer)
        {
            CurrentLevel.RenderBackground(camera.LeftTop, renderer);
            CurrentLevel.RenderEntities(camera.LeftTop, renderer);
        }
    }
}
