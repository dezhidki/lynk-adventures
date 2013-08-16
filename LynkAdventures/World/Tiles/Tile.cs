using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.World.Tiles
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A tile which has a fixed position on the map.
    /// </summary>
    public class Tile : BoundingBoxOwner
    {
        public const int EXCLUDE_NONE = 0x0;
        public const int EXCLUDE_UP = 0x1;
        public const int EXCLUDE_DOWN = 0x2;
        public const int EXCLUDE_LEFT = 0x4;
        public const int EXCLUDE_RIGHT = 0x8;
        public const int EXCLUDE_ALL = 0xF;

        public const int TILESIZE = 32 * Game.SCALE;

        protected int spriteID;
        protected byte id;

        /// <summary>
        /// Gets or sets the color of the tile.
        /// </summary>
        /// <value>
        /// The color of the tile.
        /// </value>
        public Color TileColor { get; protected set; }

        /// <summary>
        /// Gets the tile ID.
        /// </summary>
        /// <value>
        /// The tile ID.
        /// </value>
        public byte ID { get { return id; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/>.
        /// </summary>
        /// <param name="level">The level.</param>
        public Tile(Level level)
        {
            TileColor = Color.White;
            level.RegisteredLevelTiles.Add(this);
            this.id = (byte)level.RegisteredLevelTiles.IndexOf(this);
        }

        /// <summary>
        /// Determines whether the tile is solid to the specified entity.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the tile is solid to the entity; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsSolid(Entity ent)
        {
            return false;
        }

        /// <summary>
        /// Gets the bounding box of the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="ent">The entity that asks for the bounding box.</param>
        /// <returns>The bounding box of this tile.</returns>
        public virtual BoundingBox2D GetBoundingBox(int xTile, int yTile, Entity ent = null)
        {
            return new BoundingBox2D(xTile * TILESIZE, yTile * TILESIZE, (xTile + 1) * TILESIZE, (yTile + 1) * TILESIZE, this);
        }

        /// <summary>
        /// Adds the bounding box to the list.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="ent">The entity that asked for the bounding box.</param>
        /// <param name="list">The list.</param>
        public virtual void AddBoundingBox(int xTile, int yTile, Entity ent, ref List<BoundingBox2D> list)
        {
            if (IsSolid(ent))
                list.Add(GetBoundingBox(xTile, yTile));
        }

        /// <summary>
        /// An event for collider when it colldes with an entity.
        /// </summary>
        /// <param name="ent">The entity that collided.</param>
        /// <param name="bb">The bounding box of the entity.</param>
        public virtual void OnCollidedWidth(Entity ent, BoundingBox2D bb)
        {
        }

        /// <summary>
        /// An event for collided entity when it collides with an object.
        /// </summary>
        /// <param name="owner">Collider.</param>
        /// <param name="bb">Collider's bounding box.</param>
        public virtual void OnCollidedBy(BoundingBoxOwner owner, BoundingBox2D bb)
        {
        }

        /// <summary>
        /// Called when an entity steps on the tile.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="level">The level.</param>
        public virtual void OnStepped(Entity ent, int xTile, int yTile, Level level)
        {
        }

        /// <summary>
        /// Updates the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public virtual void Update(int xTile, int yTile, Level level, byte data = 0)
        {
        }

        /// <summary>
        /// Renders the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public virtual void Render(int xTile, int yTile, Renderer renderer, Level level, byte data = 0)
        {
        }

        /// <summary>
        /// Decorates the tile edges.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="width">The width of the edge (if drawn on top of the tile).</param>
        /// <param name="height">The height of the edge (if drawn on top of the tile).</param>
        /// <param name="startSpriteID">The first sprite's ID which contains the edges.</param>
        /// <param name="level">The level.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="exclusion">Edges to exclude. See EXLUDE_ variables.</param>
        /// <param name="layer">The layer to render to.</param>
        protected void DecorateTileEdges(int xTile, int yTile, int width, int height, int startSpriteID, Level level, Renderer renderer, int exclusion = EXCLUDE_NONE, float layer = RenderLayer.LAYER_DETAIL)
        {
            Tile tile = level.GetTile(xTile, yTile);

            bool up = !(level.GetTile(xTile, yTile - 1) == this);
            bool down = !(level.GetTile(xTile, yTile + 1) == this);
            bool right = !(level.GetTile(xTile + 1, yTile) == this);
            bool left = !(level.GetTile(xTile - 1, yTile) == this);

            bool excludeUp = (exclusion & EXCLUDE_UP) == EXCLUDE_UP;
            bool excludeDown = (exclusion & EXCLUDE_DOWN) == EXCLUDE_DOWN;
            bool excludeRight = (exclusion & EXCLUDE_RIGHT) == EXCLUDE_RIGHT;
            bool excludeLeft = (exclusion & EXCLUDE_LEFT) == EXCLUDE_LEFT;

            if (up && !excludeUp)
            {
                renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE - (height - 1) * Game.SCALE, startSpriteID, GameSpriteSheets.SPRITESHEET_TILE_EDGES, TileColor, layer);
            }
            if (down && !excludeDown)
            {
                renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE + (height - 1) * Game.SCALE, startSpriteID + 1, GameSpriteSheets.SPRITESHEET_TILE_EDGES, TileColor, layer);
            }
            if (left && !excludeLeft)
            {
                renderer.RenderTile(xTile * TILESIZE - (width - 1) * Game.SCALE, yTile * TILESIZE, startSpriteID + 2, GameSpriteSheets.SPRITESHEET_TILE_EDGES, TileColor, layer);
            }
            if (right && !excludeRight)
            {
                renderer.RenderTile(xTile * TILESIZE + (width - 1) * Game.SCALE, yTile * TILESIZE, startSpriteID + 3, GameSpriteSheets.SPRITESHEET_TILE_EDGES, TileColor, layer);
            }
        }

        /// <summary>
        /// Gives the tile a more depth-like look.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="spriteID">The sprite ID of the tile's texture.</param>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="shadow">if set to <c>true</c> draw shadow too.</param>
        protected void RenderTileDepth(int xTile, int yTile, int spriteID, int spriteSheetID, Renderer renderer, Level level, bool shadow = true)
        {
            if (level.GetTile(xTile, yTile - 1) != this)
                renderer.RenderModalRect(xTile * TILESIZE, yTile * TILESIZE, 0, 0, 32, 16, spriteID, spriteSheetID, Color.White, RenderLayer.LAYER_TILE_SOLID);

            if (shadow && level.GetTile(xTile, yTile + 1) != this)
                renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE + 10, 8, GameSpriteSheets.SPRITESHEET_TILE_EDGES, Color.White, RenderLayer.LAYER_TILE - 0.05F);
        }
    }
}
