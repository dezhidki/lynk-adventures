using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities;
using LynkAdventures.Entities.TileEntities;
using LynkAdventures.MathHelpers;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace LynkAdventures.World
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A forest.
    /// </summary>
    public class LevelForest : Level
    {
        private byte TILE_GRASS, TILE_TREE, TILE_DARK_GRASS, TILE_WATER, TILE_BRIDGE, TILE_ROCK, TILE_ROCKBLEND1, TILE_ROCKBLEND2,
            TILE_ROCKBLEND3, TILE_ROCKBLEND4, TILE_ROCKCORNER1, TILE_ROCKCORNER2, TILE_ROCKCORNER3, TILE_GRASSBLEND, TILE_FENCE, TILE_GATE, TILE_ROCK_ROOF, TILE_ROCK_ROOF_SOLID;

        private string message1, message2, message3, message4, message5, message6;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelForest"/>.
        /// </summary>
        /// <param name="colorMap">A <see cref="Microsoft.Xna.Framework.Graphics.Texture2D" /> that has information about tiles in the level.</param>
        /// <param name="manager">The level manager.</param>
        public LevelForest(Texture2D colorMap, LevelManager manager)
            : base(colorMap, manager)
        {
            message1 = new StringBuilder("          Welcome to\n").Append(Game.TITLE).Append("\n            ").Append(Game.VERSION).ToString();
            message2 = "Unfortunately the actual\nworld isn't ready.\nThat's why you will have\nto play on a smaller\ngameplay demonstration\nmap.";
            message3 = "Thanks, and have fun! :D\n\nP.S: This map has a lot of\nsigns to tell about the\ngame mechanics.";
            message4 = "Every time you step on\na weapon, its stats will be\nshown. You can also see\nstats' difference between\nwhat you are currently\nholding and what you can\npick up.";
            message5 = "You can use sword with\n<SPACE>.";
            message6 = "This is a cave.\nOne can never know\nwhat lies in there...";
        }

        /// <summary>
        /// Initializes the tiles.
        /// </summary>
        protected override void InitTiles()
        {
            TILE_GRASS = new TileGrass(this).ID;
            TILE_TREE = new TileTree(this).ID;
            TILE_DARK_GRASS = new TileDarkGrass(this).ID;
            TILE_WATER = new TileWater(this).ID;
            TILE_BRIDGE = new TileBridge(this).ID;
            TILE_ROCK = new TileRock(this).ID;
            TILE_ROCKBLEND1 = new TileRockBlend(this, Directions.UP).ID;
            TILE_ROCKBLEND2 = new TileRockBlend(this, Directions.DOWN).ID;
            TILE_ROCKBLEND3 = new TileRockBlend(this, Directions.RIGHT).ID;
            TILE_ROCKBLEND4 = new TileRockBlend(this, Directions.LEFT).ID;
            TILE_ROCKCORNER1 = new TileRockCorner(this, Directions.DOWN).ID;
            TILE_ROCKCORNER2 = new TileRockCorner(this, Directions.RIGHT).ID;
            TILE_ROCKCORNER3 = new TileRockCorner(this, Directions.UP).ID;
            TILE_GRASSBLEND = new TileBlend(this, 11, 13, GameSpriteSheets.SPRITESHEET_TILES, Directions.UP).ID;
            TILE_FENCE = new TileFence(this).ID;
            TILE_GATE = new TileGate(this).ID;
            TILE_ROCK_ROOF = new TileOverEntity(13, false, this).ID;
            TILE_ROCK_ROOF_SOLID = new TileOverEntity(13, true, this).ID;
        }

        /// <summary>
        /// Initializes the level.
        /// </summary>
        public override void InitLevel()
        {
            LevelBuilder lb = new LevelBuilder(this, levelColorMap);

            lb.BindTile(ColorMath.ToABGR(0xFF0026FF), TILE_WATER);
            lb.BindCustom(ColorMath.ToABGR(0xFF267F00),
                delegate(int xTile, int yTile)
                {
                    SetTile(xTile, yTile, TILE_GRASS);
                    SetData((byte)(random.Next(0, 3)), xTile, yTile);
                });
            lb.BindTile(ColorMath.ToABGR(0xFF7F3300), TILE_BRIDGE, 16);
            lb.BindTile(ColorMath.ToABGR(0xFF209320), TILE_DARK_GRASS);
            lb.BindTile(ColorMath.ToABGR(0xFF529652), TILE_GRASSBLEND, 13);
            lb.BindTile(ColorMath.ToABGR(0xFF123D01), TILE_TREE);
            lb.BindTile(ColorMath.ToABGR(0xFF808080), TILE_ROCKBLEND1);
            lb.BindTile(ColorMath.ToABGR(0xFF808081), TILE_ROCKBLEND2);
            lb.BindTile(ColorMath.ToABGR(0xFF808082), TILE_ROCKBLEND3);
            lb.BindTile(ColorMath.ToABGR(0xFF808083), TILE_ROCKBLEND4);
            lb.BindTile(ColorMath.ToABGR(0xFF404040), TILE_ROCK);
            lb.BindTile(ColorMath.ToABGR(0xFF1E1E1E), TILE_ROCK_ROOF);
            lb.BindTile(ColorMath.ToABGR(0xFF363638), TILE_ROCK_ROOF_SOLID);
            lb.BindTile(ColorMath.ToABGR(0xFF72FF00), TILE_GRASS);
            lb.BindCustom(ColorMath.ToABGR(0xFF72FF00),
                delegate(int xTile, int yTile)
                {
                    SpawnPoint = new Point(xTile * Tile.TILESIZE, yTile * Tile.TILESIZE);
                });
            lb.BindTile(ColorMath.ToABGR(0xFF808090), TILE_ROCKCORNER1);
            lb.BindTile(ColorMath.ToABGR(0xFF808091), TILE_ROCKCORNER2);
            lb.BindTile(ColorMath.ToABGR(0xFF808093), TILE_ROCKCORNER3);
            lb.BindTile(ColorMath.ToABGR(0xFF3D1800), TILE_FENCE);
            lb.BindTile(ColorMath.ToABGR(0xFF562100), TILE_GATE);
            lb.BindEntity(ColorMath.ToABGR(0xFF10C400), typeof(EntitySlime), manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF0000), typeof(EntityItemSword), manager, 3, 4, 3, new Point(25, 15), new BoundingBox2D(22, 40, 25, 32));
            lb.BindEntity(ColorMath.ToABGR(0xFFFFD800), typeof(TileEntityTeleporter), 2, 13, Game.LEVEL_DUNGEON, 13, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A00), typeof(TileEntitySign), message1, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A01), typeof(TileEntitySign), message2, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A02), typeof(TileEntitySign), message3, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A03), typeof(TileEntitySign), message4, 0.62F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A04), typeof(TileEntitySign), message5, 0.7F, this, manager);
            lb.BindEntity(ColorMath.ToABGR(0xFFFF6A05), typeof(TileEntitySign), message6, 0.7F, this, manager);

            lb.BuildLevel();
        }
    }
}
