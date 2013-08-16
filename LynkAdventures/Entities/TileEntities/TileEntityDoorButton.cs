using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities.TileEntities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A door button.
    /// </summary>
    public class TileEntityDoorButton : TileEntity
    {
        private int xTile_Door, yTile_Door;
        private byte openDoorID, closedDoorID;
        private bool isDoorOpen = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntityDoorButton"/>.
        /// </summary>
        /// <param name="xTileDoor">Door's coordinate on the level grid (X).</param>
        /// <param name="yTileDoor">Door's coordinate on the level grid (Y).</param>
        /// <param name="openDoorID">Open door's tileID.</param>
        /// <param name="closedDoorID">Closed door's tileID.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">Level manager.</param>
        public TileEntityDoorButton(int xTileDoor, int yTileDoor, byte openDoorID, byte closedDoorID, Level level, LevelManager manager)
            : base(level, manager)
        {
            this.openDoorID = openDoorID;
            this.closedDoorID = closedDoorID;
            xTile_Door = xTileDoor;
            yTile_Door = yTileDoor;
        }

        /// <summary>
        /// Checks if this entity is solid to the given entity.
        /// </summary>
        /// <param name="ent">The entity to check with.</param>
        /// <returns>
        ///   <c>true</c> if this entity is solid to the given entity; otherwise, <c>false</c>.
        /// </returns>
        public override bool SolidToEntity(Entity ent)
        {
            return true;
        }

        /// <summary>
        /// Called when this entity gets interacted by another one.
        /// </summary>
        /// <param name="ent">The entity that interacted with this one.</param>
        public override void OnInteractBy(Entity ent)
        {
            isDoorOpen = !isDoorOpen;

            if (isDoorOpen)
                level.SetTile(xTile_Door, yTile_Door, openDoorID);
            else
                level.SetTile(xTile_Door, yTile_Door, closedDoorID);
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, 29, GameSpriteSheets.SPRITESHEET_TILES, Color.White, RenderLayer.LAYER_TILE - 0.01f);
        }
    }
}
