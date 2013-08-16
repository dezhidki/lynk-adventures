using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Entities.TileEntities;
using LynkAdventures.MathHelpers;
using LynkAdventures.World.Rooms;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A dungeon.
    /// </summary>
    public class LevelDungeon1 : Level
    {
        private byte TILE_BRICK, TILE_DARK_DIRT, TILE_VOID, TILE_DOOR, TILE_BRICK_RED;
        private Point leftTopBorder, rightBottomBorder;
        private string message1, message2, message3, message4;
        private RoomBomberman roomBomberman;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelDungeon1"/>.
        /// </summary>
        /// <param name="colorMap">The color map.</param>
        /// <param name="levels">The level manager.</param>
        public LevelDungeon1(Texture2D colorMap, LevelManager levels)
            : base(colorMap, levels)
        {
            roomBomberman = new RoomBomberman(38, 10, RoomTileMaps.ROOM_BOMBERMAN, this, manager);

            message1 = "Using shield you\ncan block attacks with\n<CTRL>.";
            message2 = "You can interact with\nenvironment using <E>.";
            message3 = "Using bow aim with\n<SHIFT> and shoot with\n<SPACE>.";
            message4 = "There will always be a\nsign near Boss's room\nto alert you.";
        }

        /// <summary>
        /// Initializes the tiles.
        /// </summary>
        protected override void InitTiles()
        {
            TILE_VOID = new TileVoid(this).ID;
            TILE_BRICK = new TileBrick(this).ID;
            TILE_DARK_DIRT = new TileDarkDirt(this).ID;
            TILE_DOOR = new TileDoor(this).ID;
            TILE_BRICK_RED = new TileRedBrick(this).ID;
        }

        /// <summary>
        /// Called when the level changes.
        /// </summary>
        public override void OnLevelChange()
        {
            Game.Camera.LeftTopBorder = Point.Zero;
            Game.Camera.RightBottomBorder = new Point(Width * Tile.TILESIZE, Height * Tile.TILESIZE);
        }

        /// <summary>
        /// Called when the level manager changes to this level.
        /// </summary>
        public override void OnLevelEntered()
        {
            Game.Camera.LeftTopBorder = leftTopBorder;
            Game.Camera.RightBottomBorder = rightBottomBorder;
        }

        /// <summary>
        /// Initializes the level.
        /// </summary>
        public override void InitLevel()
        {
            LevelBuilder lb = new LevelBuilder(this, levelColorMap);
            lb.BindCustom(ColorMath.ToABGR(0xFF4CFF00),
                delegate(int xTile, int yTile)
                {
                    SpawnPoint = new Point(xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                });
            lb.BindTile(ColorMath.ToABGR(0xFF4CFF00), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFA80000), TILE_BRICK_RED);
            lb.BindTile(ColorMath.ToABGR(0xFFA80001), TILE_BRICK_RED, 1);
            lb.BindTile(ColorMath.ToABGR(0xFF0026FF), TILE_BRICK);
            lb.BindTile(ColorMath.ToABGR(0xFF0026F0), TILE_BRICK, 1);
            lb.BindTile(ColorMath.ToABGR(0xFF004A7F), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A00), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFFD800), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF267F00), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFF7FE0), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFF7FE1), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFF7FE2), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFFFF7FE3), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0000), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0001), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0002), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0003), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0004), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3300), TILE_DARK_DIRT);
            lb.BindTile(ColorMath.ToABGR(0xFF000000), TILE_VOID);
            lb.BindTile(ColorMath.ToABGR(0xFF00137F), TILE_DOOR);
            lb.BindEntity(ColorMath.ToABGR(0xFF0094F0), typeof(TileEntityDoorButton), 14, 27, TILE_DARK_DIRT, TILE_DOOR, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF0094F1), typeof(TileEntityDoorButton), 10, 47, TILE_DARK_DIRT, TILE_DOOR, this, manager);
            lb.BindCustom(ColorMath.ToABGR(0xFFFF0000),
                delegate(int xTile, int yTile)
                {
                    leftTopBorder = new Point(xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                });
            lb.BindCustom(ColorMath.ToABGR(0xFFFF0001),
                delegate(int xTile, int yTile)
                {
                    rightBottomBorder = new Point(xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                });
            lb.BindEntity(ColorMath.ToABGR(0xFFB6FF00), typeof(TileEntityTeleporter), 41, 13, Game.LEVEL_FOREST, 23, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A00), typeof(EntityBat), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFFD800), typeof(EntitySpider), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF267F00), typeof(EntitySlime), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF7FE0), typeof(TileEntitySign), message1, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF7FE1), typeof(TileEntitySign), message2, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF7FE2), typeof(TileEntitySign), message3, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF7FE3), typeof(TileEntitySign), message4, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0000), typeof(EntityItemShield), manager, 3, 10);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0001), typeof(EntityItemSword), manager, 5, 10, 3, new Point(25, 15), new BoundingBox2D(22, 40, 25, 32));
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0002), typeof(EntityItemBow), manager, 5, 7, 2, 200);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0003), typeof(EntityItemArrow), manager, 5);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0004), typeof(EntityItemRuby), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3300), typeof(EntityItemHealth), 1, manager);

            lb.BuildLevel();

            roomBomberman.InitRoom();
        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        public override void Update()
        {
            base.Update();
            roomBomberman.Update();
        }
    }
}
