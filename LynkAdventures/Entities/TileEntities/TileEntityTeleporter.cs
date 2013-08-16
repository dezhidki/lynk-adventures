using LynkAdventures.Graphics;
using LynkAdventures.World;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities.TileEntities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A teleporter, which allows to transfer between places and levels.
    /// </summary>
    public class TileEntityTeleporter : TileEntity
    {
        private Point teleportTarget;
        private int levelID;
        private int tileSpriteID;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntityTeleporter"/>.
        /// </summary>
        /// <param name="xTileTarget">The teleport target in the level grid (X).</param>
        /// <param name="yTileTarget">The teleport target in the level grid (Y).</param>
        /// <param name="levelID">Level's ID to teleport to.</param>
        /// <param name="tileSpriteID">ID of this tile's sprite.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">The manager.</param>
        public TileEntityTeleporter(int xTileTarget, int yTileTarget, int levelID, int tileSpriteID, Level level, LevelManager manager)
            : base(level, manager)
        {
            teleportTarget = new Point(xTileTarget * Tile.TILESIZE, yTileTarget * Tile.TILESIZE);
            this.levelID = levelID;
            this.tileSpriteID = tileSpriteID;
        }

        /// <summary>
        /// Called when this entity gets touched by another.
        /// </summary>
        /// <param name="ent">The entity that touched.</param>
        public override void OnTouch(Entity ent)
        {
            if (levelID != levelManager.CurrentLevelID)
                levelManager.ChangeLevel(levelID);

            levelManager.CurrentLevel.SetEntityPosition(levelManager.CurrentLevel.Player, teleportTarget.X, teleportTarget.Y);
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, tileSpriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE - 0.01f);
        }
    }
}
