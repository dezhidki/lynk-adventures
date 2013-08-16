using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LynkAdventures.World
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A comparer which is used in sorting the draw order for entities-
    /// </summary>
    class EntityComparer : Comparer<Entity>
    {
        /// <summary>
        /// Compares two entities together.
        /// </summary>
        /// <param name="e1">The e1.</param>
        /// <param name="e2">The e2.</param>
        /// <returns>+1 if to draw e1 before e2, -1 to draw e1 after e2.</returns>
        public override int Compare(Entity e1, Entity e2)
        {
            if (e1.YCenter > e2.YCenter)
                return +1;
            if (e1.YCenter < e2.YCenter)
                return -1;
            return 0;
        }
    }

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A game level.
    /// </summary>
    public abstract class Level
    {
        public const int TILESHIFT = 6;
        public readonly int UPDATEDIVIDER;

        private List<Tile> levelTiles;
        protected int width, height;
        protected byte[] tiles;
        protected byte[] data;
        protected List<Entity>[] entitiesInTiles;
        protected List<Entity> entities;
        protected Random random;
        protected LevelManager manager;
        protected Texture2D levelColorMap;
        private EntityComparer entityComparer;

        /// <summary>
        /// Gets the registered level tiles.
        /// </summary>
        /// <value>
        /// The registered level tiles.
        /// </value>
        public List<Tile> RegisteredLevelTiles { get { return levelTiles; } }

        /// <summary>
        /// Gets the entities in the level.
        /// </summary>
        /// <value>
        /// The entities in the level.
        /// </value>
        public List<Entity> Entities { get { return entities; } }

        /// <summary>
        /// Gets or sets the spawn point.
        /// </summary>
        /// <value>
        /// The spawn point.
        /// </value>
        public Point SpawnPoint { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public EntityPlayer Player { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/>.
        /// </summary>
        /// <param name="colorMap">A <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that has information about tiles in the level.</param>
        /// <param name="manager">The level manager.</param>
        public Level(Texture2D colorMap, LevelManager manager)
        {
            levelTiles = new List<Tile>();
            random = new Random();
            entityComparer = new EntityComparer();
            levelColorMap = colorMap;
            this.manager = manager;
            this.width = colorMap.Width;
            this.height = colorMap.Height;
            UPDATEDIVIDER = height / (width / 10);
            InitTiles();

            tiles = new byte[width * height];
            data = new byte[width * height];

            entitiesInTiles = new List<Entity>[width * height];
            for (int i = 0; i < entitiesInTiles.Length; i++)
            {
                entitiesInTiles[i] = new List<Entity>();
            }

            entities = new List<Entity>();
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get { return width; } }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get { return height; } }

        /// <summary>
        /// Initializes the tiles.
        /// </summary>
        protected abstract void InitTiles();

        /// <summary>
        /// Initializes the level.
        /// </summary>
        /// <exception cref="System.Exception">Level's color map is null.</exception>
        public virtual void InitLevel()
        {
            if (levelColorMap == null)
                throw new Exception("Level's color map is null!");
        }

        /// <summary>
        /// Called when the level changes.
        /// </summary>
        public virtual void OnLevelChange()
        {
        }

        /// <summary>
        /// Called when the level manager changes to this level.
        /// </summary>
        public virtual void OnLevelEntered()
        {
        }

        /// <summary>
        /// Renders the background.
        /// </summary>
        /// <param name="screenOffset">The screen offset.</param>
        /// <param name="renderer">The renderer.</param>
        public void RenderBackground(Point screenOffset, Renderer renderer)
        {
            renderer.ScreenOffset = screenOffset;

            int xt0 = (screenOffset.X) >> TILESHIFT;
            int yt0 = (screenOffset.Y) >> TILESHIFT;
            xt0--; yt0--;

            int xt1 = (Game.WIDTH + screenOffset.X) >> TILESHIFT;
            int yt1 = (Game.HEIGHT + screenOffset.Y) >> TILESHIFT;
            xt1++; yt1++;

            for (int yTile = yt0; yTile <= yt1; yTile++)
            {
                for (int xTile = xt0; xTile <= xt1; xTile++)
                {
                    Tile tile = GetTile(xTile, yTile);

                    if (tile != null)
                        tile.Render(xTile, yTile, renderer, this, GetData(xTile, yTile));
                }
            }

            renderer.ScreenOffset = Point.Zero;
        }

        /// <summary>
        /// Renders the entities.
        /// </summary>
        /// <param name="screenOffset">The screen offset.</param>
        /// <param name="renderer">The renderer.</param>
        public void RenderEntities(Point screenOffset, Renderer renderer)
        {
            renderer.ScreenOffset = screenOffset;
            int xt0 = (screenOffset.X) >> TILESHIFT;
            int yt0 = (screenOffset.Y) >> TILESHIFT;
            xt0--; yt0--;

            int xt1 = (Game.WIDTH + screenOffset.X) >> TILESHIFT;
            int yt1 = (Game.HEIGHT + screenOffset.Y) >> TILESHIFT;
            xt1++; yt1++;

            List<Entity> entitiesToRender = new List<Entity>();

            for (int yTile = yt0; yTile <= yt1; yTile++)
            {
                for (int xTile = xt0; xTile <= xt1; xTile++)
                {
                    entitiesToRender = Enumerable.Union<Entity>(entitiesToRender, GetEntitiesFromTile(xTile, yTile)).ToList<Entity>();
                }
            }

            if (entitiesToRender.Count > 0)
            {
                SortAndRenderEntities(renderer, entitiesToRender);
            }
            entitiesToRender.Clear();

            renderer.ScreenOffset = Point.Zero;
        }

        /// <summary>
        /// Sorts the and renders entities.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="entityList">The entity list.</param>
        public void SortAndRenderEntities(Renderer renderer, List<Entity> entityList)
        {
            entityList = entityList.OrderBy(ent => ent, entityComparer).ToList<Entity>();
            float[] depthValues = BuildDepthValues(entityList);
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i].Render(renderer, depthValues[i]);
            }
        }

        /// <summary>
        /// Builds the depth values for entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>An array containing drawing depth value for each entity.</returns>
        protected float[] BuildDepthValues(List<Entity> entities)
        {
            List<float> result = new List<float>();
            float farthestDepth = RenderLayer.LAYER_ENTITY;
            float nearestDepth = farthestDepth - 0.1F;
            float step = (farthestDepth - nearestDepth) / entities.Count;
            float currentStep = farthestDepth;
            foreach (Entity ent in entities)
            {
                result.Add(currentStep);
                currentStep -= step;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        /// <exception cref="System.Exception">
        /// The entity to remove is on a non-existent tile.
        /// or
        /// The entity is put on a non-existent tile.
        /// </exception>
        public virtual void Update()
        {
            for (int i = 0; i < width * height / UPDATEDIVIDER; i++)
            {
                int xTile = random.Next(width);
                int yTile = random.Next(height);
                GetTile(xTile, yTile).Update(xTile, yTile, this, GetData(xTile, yTile));
            }

            for (int i = 0; i < entities.Count; i++)
            {
                Entity ent = entities[i];
                int xTile_old = ent.X >> TILESHIFT;
                int yTile_old = ent.Y >> TILESHIFT;

                ent.Update();

                if (ent.IsDead)
                {
                    ent.OnDeath();
                    RemoveEntity(ent);
                    RemoveEntityFromTileList(ent, xTile_old, yTile_old);
                    continue;
                }

                int xTile_new = ent.X >> TILESHIFT;
                int yTile_new = ent.Y >> TILESHIFT;

                if (xTile_old != xTile_new || yTile_old != yTile_new)
                {
                    if (!RemoveEntityFromTileList(ent, xTile_old, yTile_old))
                        throw new Exception("Couldn't find the tile: " + xTile_old + ", " + yTile_old + " !");
                    if (!InsertEntityIntoTiles(ent, xTile_new, yTile_new))
                        throw new Exception("Couldn't put the entity into tile: " + xTile_new + ", " + yTile_new + " !");
                }
            }
        }

        /// <summary>
        /// Gets the data value for the tile.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns>The data value assiociated with the tile.</returns>
        public byte GetData(int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return 0;

            return data[xTile + yTile * width];
        }

        /// <summary>
        /// Sets the data value.
        /// </summary>
        /// <param name="dataVal">The data value.</param>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        public void SetData(byte dataVal, int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return;
            data[xTile + yTile * width] = dataVal;
        }

        /// <summary>
        /// Gets the tile.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns>The <see cref="Tiles.Tile"/> representation of the tile.</returns>
        public Tile GetTile(int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return null;

            return RegisteredLevelTiles[tiles[xTile + yTile * width]];
        }

        /// <summary>
        /// Sets the tile.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>false</c> if the given position is invalid.</returns>
        public bool SetTile(int xTile, int yTile, Tile tile)
        {
            if (!IsValidPosition(xTile, yTile))
                return false;

            tiles[xTile + yTile * width] = tile.ID;
            return true;
        }

        /// <summary>
        /// Sets the tile.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <param name="tileID">The tile ID.</param>
        /// <returns><c>false</c> if the position is invalid.</returns>
        public bool SetTile(int xTile, int yTile, byte tileID)
        {
            if (!IsValidPosition(xTile, yTile))
                return false;

            tiles[xTile + yTile * width] = tileID;
            return true;
        }

        /// <summary>
        /// Sets the entity position.
        /// </summary>
        /// <param name="ent">The ent.</param>
        /// <param name="xPos">The x position.</param>
        /// <param name="yPos">The y position.</param>
        /// <returns><c>false</c> if the position is invalid.</returns>
        public bool SetEntityPosition(Entity ent, int xPos, int yPos)
        {
            if (!entities.Contains(ent) || !IsValidPosition(xPos >> TILESHIFT, yPos >> TILESHIFT)) return false;

            if (!RemoveEntityFromTileList(ent, ent.X >> TILESHIFT, ent.Y >> TILESHIFT)) return false;
            ent.Position = new Point(xPos, yPos);
            if (!InsertEntityIntoTiles(ent, xPos >> TILESHIFT, yPos >> TILESHIFT)) return false;

            return true;
        }

        /// <summary>
        /// Adds the entity on the level.
        /// </summary>
        /// <param name="ent">The ent.</param>
        /// <param name="xPos">The x position of the entity.</param>
        /// <param name="yPos">The y position of the entity.</param>
        public void AddEntity(Entity ent, int xPos, int yPos)
        {
            ent.Position = new Point(xPos, yPos);
            if (ent is EntityPlayer)
            {
                Player = (EntityPlayer)ent;
                Player.Position = SpawnPoint;
                xPos = SpawnPoint.X;
                yPos = SpawnPoint.Y;
            }
            ent.Init();
            entities.Add(ent);
            if (!InsertEntityIntoTiles(ent, xPos >> TILESHIFT, yPos >> TILESHIFT))
                ent.Die();
        }

        /// <summary>
        /// Caches entity's position into the tile.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns><c>false</c> if the position is invalid.</returns>
        protected bool InsertEntityIntoTiles(Entity ent, int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return false;

            entitiesInTiles[xTile + yTile * width].Add(ent);
            return true;
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <param name="ent">The entity.</param>
        public void RemoveEntity(Entity ent)
        {
            entities.Remove(ent);
            RemoveEntityFromTileList(ent, ent.X >> TILESHIFT, ent.Y >> TILESHIFT);
        }

        /// <summary>
        /// Removes the entity from the tile cache.
        /// </summary>
        /// <param name="ent">The ent.</param>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns><c>false</c> if the position is invalid.</returns>
        protected bool RemoveEntityFromTileList(Entity ent, int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return false;
            List<Entity> entities = entitiesInTiles[xTile + yTile * width];

            entitiesInTiles[xTile + yTile * width].Remove(ent);

            entities = entitiesInTiles[xTile + yTile * width];

            return true;
        }

        /// <summary>
        /// Gets all the entities within the tile.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns>A list of entities within the given tile.</returns>
        public List<Entity> GetEntitiesFromTile(int xTile, int yTile)
        {
            if (!IsValidPosition(xTile, yTile))
                return new List<Entity>();

            return entitiesInTiles[xTile + yTile * width];
        }

        /// <summary>
        /// Gets the entities within the specified bounding box..
        /// </summary>
        /// <param name="bb">The bounding box.</param>
        /// <returns>A list of entities within the given bounding box.</returns>
        public List<Entity> GetEntities(BoundingBox2D bb)
        {
            List<Entity> result = new List<Entity>();

            int xt0 = (bb.XLeft >> TILESHIFT) - 1;
            int yt0 = (bb.YTop >> TILESHIFT) - 1;
            int xt1 = (bb.XRight >> TILESHIFT) + 1;
            int yt1 = (bb.YBottom >> TILESHIFT) + 1;

            for (int yTile = yt0; yTile <= yt1; yTile++)
            {
                for (int xTile = xt0; xTile <= xt1; xTile++)
                {
                    if (!IsValidPosition(xTile, yTile))
                        continue;
                    List<Entity> entities = entitiesInTiles[xTile + yTile * width];
                    foreach (Entity ent in entities)
                    {
                        if (ent.GetBoundingBox().Intersects(bb))
                            result.Add(ent);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the bounding boxes that are colliding with the specified bounding box.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="boundingBox">The bounding box. If null, will use entity's bounding box.</param>
        /// <returns>A list of colliding bounding boxes.</returns>
        public List<BoundingBox2D> GetBoundingBoxesForEntity(Entity entity, BoundingBox2D boundingBox = null)
        {
            List<BoundingBox2D> result = new List<BoundingBox2D>();

            BoundingBox2D BBToCheck = boundingBox == null ? entity.GetBoundingBox() : boundingBox;

            int xt0 = (BBToCheck.XLeft >> TILESHIFT) - 1;
            int yt0 = (BBToCheck.YTop >> TILESHIFT) - 1;
            int xt1 = (BBToCheck.XRight >> TILESHIFT) + 1;
            int yt1 = (BBToCheck.YBottom >> TILESHIFT) + 1;

            for (int yTile = yt0; yTile <= yt1; yTile++)
            {
                for (int xTile = xt0; xTile <= xt1; xTile++)
                {
                    if (!IsValidPosition(xTile, yTile))
                        continue;
                    List<Entity> entities = entitiesInTiles[xTile + yTile * width];
                    foreach (Entity ent in entities)
                    {
                        BoundingBox2D entBB = ent.GetBoundingBox();
                        if (entBB.Intersects(BBToCheck))
                            ent.AddBoundingBox(entity, ref result);
                    }

                    Tile tile = GetTile(xTile, yTile);
                    if (tile.GetBoundingBox(xTile, yTile, entity).Intersects(BBToCheck))
                        GetTile(xTile, yTile).AddBoundingBox(xTile, yTile, entity, ref result);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the given position is valid.
        /// </summary>
        /// <param name="xTile">The x position of the tile.</param>
        /// <param name="yTile">The y position of the tile.</param>
        /// <returns>
        ///   <c>true</c> if the given position is valid; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsValidPosition(int xTile, int yTile)
        {
            return !(xTile < 0 || xTile >= width || yTile < 0 || yTile >= height);
        }
    }
}
