using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.MathHelpers;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A test level.
    /// </summary>
    public class TestLevel1 : Level
    {
        private byte TILE_GRASS, TILE_TREE, TILE_DARK_GRASS, TILE_WOOD_FLOOR, TILE_WOOD_WALL, TILE_BRICK;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLevel1"/>.
        /// </summary>
        /// <param name="colorMap">The color map.</param>
        /// <param name="levels">The level manager.</param>
        public TestLevel1(Texture2D colorMap, LevelManager levels)
            : base(colorMap, levels)
        {
        }

        /// <summary>
        /// Initializes the tiles.
        /// </summary>
        protected override void InitTiles()
        {
            TILE_GRASS = new TileGrass(this).ID;
            TILE_TREE = new TileTree(this).ID;
            TILE_DARK_GRASS = new TileDarkGrass(this).ID;
            TILE_WOOD_FLOOR = new TileWoodFloor(this).ID;
            TILE_WOOD_WALL = new TileWoodWall(this).ID;
            TILE_BRICK = new TileBrick(this).ID;
        }

        /// <summary>
        /// Initializes the level.
        /// </summary>
        public override void InitLevel()
        {
            LevelBuilder lb = new LevelBuilder(this, levelColorMap);

            lb.BindCustom(ColorMath.ToABGR(0xFF267F00),
                delegate(int xTile, int yTile)
                {
                    SetTile(xTile, yTile, TILE_GRASS);
                    SetData((byte)(random.Next(1, 3)), xTile, yTile);
                });
            lb.BindTile(ColorMath.ToABGR(0xFF123D00), TILE_TREE);
            lb.BindTile(ColorMath.ToABGR(0xFF0026FF), TILE_BRICK);
            lb.BindTile(ColorMath.ToABGR(0xFFB79900), TILE_WOOD_WALL);
            lb.BindTile(ColorMath.ToABGR(0xFFFFD800), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF529652), TILE_DARK_GRASS);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A00), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A00), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A01), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A02), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFFFF6A03), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3300), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3301), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3302), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3310), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3311), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F3312), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFFFF0000), TILE_DARK_GRASS);
            lb.BindTile(ColorMath.ToABGR(0xFFC40000), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF7F0000), TILE_WOOD_FLOOR);
            lb.BindTile(ColorMath.ToABGR(0xFF0094FF), TILE_GRASS);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A00), typeof(EntityItemArrow), manager, 1);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A01), typeof(EntityItemArrow), manager, 2);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A02), typeof(EntityItemArrow), manager, 3);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A03), typeof(EntityItemArrow), manager, 10);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3300), typeof(EntityItemSword), manager, 5, 10, 3, new Point(25, 15), new BoundingBox2D(22, 40, 25, 32));
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3301), typeof(EntityItemShield), manager, 5, 10);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3302), typeof(EntityItemBow), manager, 4, 10, 3, 500);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3310), typeof(EntityItemSword), manager, 2, 4, 1, new Point(25, 15), new BoundingBox2D(22, 40, 25, 32));
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3311), typeof(EntityItemShield), manager, 1, 50);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F3312), typeof(EntityItemBow), manager, 5, 7, 2, 200);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF0000), typeof(EntityItemRuby), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFC40000), typeof(EntityItemRuby), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF0094FF), typeof(EntitySpider), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFF7F0000), typeof(EntityItemHealth), 1, manager);
            lb.BindCustom(ColorMath.ToABGR(0xFF00FF00),
                delegate(int xTile, int yTile)
                {
                    SpawnPoint = new Point(xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                });

            lb.BuildLevel();
        }
    }
}
