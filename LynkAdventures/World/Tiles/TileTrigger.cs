using LynkAdventures.Entities;
using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LynkAdventures.World.Tiles
{

    /// <summary>
    /// A trigger method.
    /// </summary>
    /// <param name="xTile">The x position of tile.</param>
    /// <param name="yTile">The y position of tile.</param>
    /// <param name="level">The level.</param>
    public delegate void TriggerMethod(int xTile, int yTile, Level level);

    /// @author Denis Zhidkikh
    /// @version 6.4.2013    
    /// <summary>
    /// A trigger.
    /// </summary>
    public class TileTrigger : Tile
    {
        private TriggerMethod method;
        private bool solid;
        private bool hasBeenTriggered = false;
        private bool isOneTime;
        private List<Type> triggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileTrigger"/>.
        /// </summary>
        /// <param name="method">The method which will be called upon the activation.</param>
        /// <param name="solid">if set to <c>true</c> the tile is solid.</param>
        /// <param name="isOneTime">if set to <c>true</c> the tile will be triggered only one time.</param>
        /// <param name="spriteID">The sprite ID.</param>
        /// <param name="level">The level.</param>
        /// <param name="entitiesToTriggerOn">The entities to trigger on.</param>
        public TileTrigger(TriggerMethod method, bool solid, bool isOneTime, int spriteID, Level level, params Type[] entitiesToTriggerOn)
            : base(level)
        {
            this.method = method;
            this.solid = solid;
            this.spriteID = spriteID;
            this.isOneTime = isOneTime;
            triggers = entitiesToTriggerOn.ToList();
        }

        /// <summary>
        /// Determines whether the tile is solid to the specified entity.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the tile is solid to the entity; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSolid(Entities.Entity ent)
        {
            return solid;
        }

        /// <summary>
        /// Called when an entity steps on the tile.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="level">The level.</param>
        public override void OnStepped(Entity ent, int xTile, int yTile, Level level)
        {
            if (triggers.Contains(ent.GetType()) && !hasBeenTriggered)
            {
                if (isOneTime)
                    hasBeenTriggered = true;
                method(xTile, yTile, level);
            }
        }

        /// <summary>
        /// Renders the tile.
        /// </summary>
        /// <param name="xTile">The x position of tile.</param>
        /// <param name="yTile">The y position of tile.</param>
        /// <param name="renderer">The renderer.</param>
        /// <param name="level">The level.</param>
        /// <param name="data">The data value.</param>
        public override void Render(int xTile, int yTile, Renderer renderer, Level level, byte data = 0)
        {
            renderer.RenderTile(xTile * TILESIZE, yTile * TILESIZE, spriteID, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE);
        }

    }
}
