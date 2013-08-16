using LynkAdventures.Entities;
using LynkAdventures.Entities.TileEntities;
using LynkAdventures.MathHelpers;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World.Rooms
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Bomberman's room.
    /// </summary>
    public class RoomBomberman : Room
    {
        private byte TILE_DARK_DIRT, TILE_TRIGGER, TILE_BRICK_RED;
        private readonly Point DOOR1 = new Point(0, 6), DOOR2 = new Point(10, 8);
        private EntityBomberman bomberman;
        private string message1;
        private bool isBombermanKilled = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomBomberman"/>.
        /// </summary>
        /// <param name="xTile">The x position on the level grid.</param>
        /// <param name="yTile">The y position on the level grid.</param>
        /// <param name="tileMap">The tile map texture.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">The manager.</param>
        public RoomBomberman(int xTile, int yTile, Texture2D tileMap, Level level, LevelManager manager)
            : base(xTile, yTile, tileMap, level, manager)
        {
            message1 = "Congratulations!\nYou have completed the\ndemonstration map.\nThanks for playing!";
        }

        /// <summary>
        /// Initialize the room.
        /// </summary>
        public override void InitRoom()
        {
            TILE_DARK_DIRT = new TileDarkDirt(level).ID;
            TILE_BRICK_RED = new TileRedBrick(level).ID;
            TILE_TRIGGER = new TileTrigger(delegate(int xTile, int yTile, Level lvl)
            {
                SetTile(DOOR1.X, DOOR1.Y, TILE_BRICK_RED);
                SetTile(DOOR2.X, DOOR2.Y, TILE_BRICK_RED);
                bomberman = new EntityBomberman(manager);
                level.AddEntity(bomberman, (5 + xTileOffs) * Tile.TILESIZE, (3 + yTileOffs) * Tile.TILESIZE);
            }, false, true, 23, level, typeof(EntityPlayer)).ID;

            LevelBuilder lb = new LevelBuilder(level, tileMap);
            lb.BindTile(ColorMath.ToABGR(0xFFA80000), TILE_BRICK_RED);
            lb.BindTile(ColorMath.ToABGR(0xFFA80001), TILE_BRICK_RED, 1);
            lb.BindTile(ColorMath.ToABGR(0xFF004A7F), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF007C3A), TILE_TRIGGER);
            lb.BuildLevel(xTileOffs, yTileOffs);
        }

        /// <summary>
        /// Sets the winning message and opens the walls.
        /// </summary>
        private void SetWinMessage()
        {
            level.AddEntity(new TileEntitySign(message1, 0.7F, level, manager), (xTileOffs + 5) * Tile.TILESIZE, (yTileOffs + 5) * Tile.TILESIZE);
            SetTile(DOOR1.X, DOOR1.Y, TILE_DARK_DIRT);
            SetTile(DOOR2.X, DOOR2.Y, TILE_DARK_DIRT);
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        public override void Update()
        {
            if (!isBombermanKilled && bomberman != null && bomberman.IsDead)
            {
                SetWinMessage();
                isBombermanKilled = true;
            }
        }
    }
}
