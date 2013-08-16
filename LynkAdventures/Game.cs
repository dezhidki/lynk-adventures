using LynkAdventures.Controls;
using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.Gui;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using LynkAdventures.World.Rooms;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpNeatLib.Maths;
using System;

namespace LynkAdventures
{
    /// <summary>
    /// Game's main class.
    /// </summary>
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    public class Game : Microsoft.Xna.Framework.Game
    {
        public const int SCALE = 2;
        public const int WIDTH = 800;
        public const int HEIGHT = 600;
        public const string TITLE = "The Adventures of Lynk";
        public const string VERSION = "Beta 1.5.2";
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Renderer renderer;
        private bool isGameLoaded = false;

        #region Timer (FPS)
        private int lastSecond = DateTime.Now.Second;
        private int frames = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Gets frames per second.
        /// </summary>
        /// <value>
        /// The frames per second.
        /// </value>
        public int FPS { get { return frames; } }
        #endregion

        private LevelManager levelManager;
        private static GuiManager guiManager;
        private EntityPlayer player;
        private static Camera camera;
        private GuiDebug debugGui;
        private GuiMainMenu mainMenu;
        private static GuiDeath deathMenu;
        private KeyboardHandler keyboardHandler;

        /// <summary>
        /// Gets the death message.
        /// </summary>
        /// <value>
        /// The death message GUI.
        /// </value>
        public static GuiDeath DeathMenu { get { return deathMenu; } }

        /// <summary>
        /// Gets a value indicating whether the game is loaded and playable.
        /// </summary>
        /// <value>
        /// <c>true</c> if the game is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameLoaded { get { return isGameLoaded; } }

        /// <summary>
        /// Gets the instance of the level manager responsible for loading and updating all the levels in the game.
        /// </summary>
        /// <value>
        /// The level manager instance.
        /// </value>
        public LevelManager LevelManager { get { return levelManager; } }

        /// <summary>
        /// Gets the camera instance.
        /// </summary>
        /// <value>
        /// The camera instance.
        /// </value>
        public static Camera Camera { get { return camera; } }

        /// <summary>
        /// Gets the random number generator instance.
        /// </summary>
        /// <value>
        /// The random number generator.
        /// </value>
        public static FastRandom Random { get; private set; }

        /// <summary>
        /// Gets the GUI manager responsible for updating and rendering all the registered GUIs in the game.
        /// </summary>
        /// <value>
        /// The GUI manager instance.
        /// </value>
        public static GuiManager GuiManager { get { return guiManager; } }

        public static int LEVEL_FOREST { get; private set; }
        public static int LEVEL_DUNGEON { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            levelManager = new LevelManager(0);
            Random = new FastRandom();
        }

        /// <summary>
        /// Called after the Game and GraphicsDevice are created, but before LoadContent.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = TITLE + " " + VERSION;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            Window.AllowUserResizing = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            keyboardHandler = new KeyboardHandler();
            InitKeys();

            StartMainMenu();
        }

        /// <summary>
        /// Registers the keys needed used in the game.
        /// </summary>
        private void InitKeys()
        {
            keyboardHandler.RegisterKey(Keys.F3);
            keyboardHandler.RegisterKey(Keys.F4);
            keyboardHandler.RegisterKey(Keys.Escape);
            keyboardHandler.RegisterKey(Keys.Space);
            keyboardHandler.RegisterKey(Keys.LeftShift);
            keyboardHandler.RegisterKey(Keys.LeftControl);
            keyboardHandler.RegisterKey(Keys.E);
            keyboardHandler.RegisterKey(Keys.W);
            keyboardHandler.RegisterKey(Keys.A);
            keyboardHandler.RegisterKey(Keys.S);
            keyboardHandler.RegisterKey(Keys.D);
            keyboardHandler.RegisterKey(Keys.Down);
            keyboardHandler.RegisterKey(Keys.Up);
            keyboardHandler.RegisterKey(Keys.Enter);
        }

        /// <summary>
        /// Loads all the resources used in the game.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderer = new Renderer(spriteBatch, GraphicsDevice);
            guiManager = new GuiManager(renderer, GraphicsDevice, Content);
            GameSpriteSheets.LoadSpriteSheets(this.Content, renderer);
            Sound.LoadSounds(this.Content);
            Shaders.LoadShaders(this.Content, renderer);
            RoomTileMaps.LoadRooms(this.Content);
        }

        /// <summary>
        /// Ends current game and returns to the main menu.
        /// </summary>
        public void StartMainMenu()
        {
            levelManager = new LevelManager(0);
            guiManager = new GuiManager(renderer, GraphicsDevice, Content);
            mainMenu = new GuiMainMenu(this);
            System.GC.Collect();
            isGameLoaded = false;
            guiManager.LoadAsActiveGui(mainMenu);
        }

        /// <summary>
        /// Reinitializes all the game components (except resources and GuiManager) and starts the game.
        /// </summary>
        public void StartGame()
        {
            guiManager = new GuiManager(renderer, GraphicsDevice, Content);
            levelManager = new LevelManager(0);
            LevelForest forest = new LevelForest(this.Content.Load<Texture2D>("Levels/level1"), levelManager);
            LevelDungeon1 ld = new LevelDungeon1(this.Content.Load<Texture2D>("Levels/dungeon1"), levelManager);
            LEVEL_FOREST = levelManager.AddLevel(forest);
            LEVEL_DUNGEON = levelManager.AddLevel(ld);
            levelManager.InitAllLevels();

            player = new EntityPlayer(levelManager);
            levelManager.CurrentLevel.AddEntity(player, 0, 0);

            camera = new Camera(0, 0, this, new Point(0, 0), new Point(levelManager.CurrentLevel.Width * Tile.TILESIZE, levelManager.CurrentLevel.Height * Tile.TILESIZE));
            camera.FollowEntity(player);

            guiManager.LoadGui(new GuiIngame(player));

            debugGui = new GuiDebug(100, 100, new Point(0, 0), this, player);
            guiManager.LoadGui(debugGui, false);
            deathMenu = new GuiDeath(this);

            isGameLoaded = true;
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardHandler.Update();

            if (KeyboardHandler.IsKeyPressed(Keys.Escape))
                if (IsGameLoaded && !mainMenu.IsActive && !deathMenu.IsActive)
                    mainMenu.Activate(true);

            if (isGameLoaded)
            {
                if (!GuiManager.HasActiveGui)
                    levelManager.CurrentLevel.Update();

                if (KeyboardHandler.IsKeyPressed(Keys.F3))
                {
                    if (!debugGui.IsActive)
                        debugGui.Activate();
                    else
                        debugGui.Close();
                }
                if (KeyboardHandler.IsKeyPressed(Keys.F4))
                    IsFixedTimeStep = !IsFixedTimeStep;
            }

            GuiManager.Update();

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frames = frameCounter;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws everything on the screen.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Textures[0] = null;
            GraphicsDevice.SetRenderTarget(null);


            renderer.BeginRender();

            if (isGameLoaded)
            {
                camera.Update();
                levelManager.RenderLevel(camera, renderer);
            }

            GuiManager.RenderGuis();

            renderer.EndRender();
            frameCounter++;

            base.Draw(gameTime);
        }
    }
}
