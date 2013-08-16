using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace LynkAdventures.Gui
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// Debug screen.
    /// </summary>
    public class GuiDebug : Gui
    {
        private Game game;
        private EntityPlayer player;
        private StringBuilder infoString;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiDebug"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pos">The position on the screen.</param>
        /// <param name="game">The game instance.</param>
        /// <param name="player">The player.</param>
        public GuiDebug(int width, int height, Point pos, Game game, EntityPlayer player)
            : base(width, height, pos)
        {
            this.game = game;
            this.player = player;
            infoString = new StringBuilder();
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public override void Update()
        {
            infoString.Clear();
            infoString.AppendLine(Game.TITLE + " " + Game.VERSION);
            infoString.AppendLine("FPS: " + game.FPS + " FixedFPS: " + game.IsFixedTimeStep);
            infoString.AppendLine("Camera TopLeft: " + Game.Camera.LeftTop.ToString());
            infoString.AppendLine("--- Player Info ---");
            infoString.AppendLine("Position: " + player.Position.ToString() + " Tile: " + "{xTile:" + (player.Position.X >> Level.TILESHIFT) + " yTile:" + (player.Position.Y >> Level.TILESHIFT) + "}");
            infoString.AppendLine("Facing: " + player.Direction.ID + " (" + player.Direction.Name + ")");
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.SpriteBatch.DrawString(GuiResources.FONT, infoString, new Vector2(Position.X, Position.Y), Color.White, 0.0f, Vector2.Zero, 0.7f, SpriteEffects.None, layer);
        }
    }
}
