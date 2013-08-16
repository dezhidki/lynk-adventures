using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities.TileEntities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Bomberman's bomb's explosion.
    /// </summary>
    public class TileEntityBombermanExplosion : TileEntity
    {
        private AnimationHelper animation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_OBJECT_EXPLOSION, 3, 8, false);
        private int damage, pushPower;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntityBombermanExplosion"/>.
        /// </summary>
        /// <param name="damage">Damage the bomb inflicts.</param>
        /// <param name="pushPower">The push power.</param>
        /// <param name="level">The level.</param>
        /// <param name="manager">Level manager.</param>
        public TileEntityBombermanExplosion(int damage, int pushPower, Level level, LevelManager manager)
            : base(level, manager)
        {
            this.damage = damage;
            this.pushPower = pushPower;
            animation.AnimationReady += Die;
        }

        /// <summary>
        /// Called when this entity gets touched by another.
        /// </summary>
        /// <param name="ent">The entity that touched.</param>
        public override void OnTouch(Entity ent)
        {
            if (ent is EntityPlayer)
            {
                HitSource source = new HitSource(Directions.GetOppositeDir(ent.Direction), pushPower, this, HitSource.OwnerType.OTHER);
                ent.Hit(source, damage);
            }
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            animation.UpdateStep();

            List<Entity> entities = level.GetEntities(GetBoundingBox());
            foreach (Entity ent in entities)
            {
                if (ent is EntityPlayer)
                {
                    HitSource source = new HitSource(Directions.GetOppositeDir(ent.Direction), pushPower, this, HitSource.OwnerType.OTHER);
                    ent.Hit(source, damage);
                    break;
                } 
            }
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, animation.CurrentSpriteID, animation.SpriteSheet, Color.White, RenderLayer.LAYER_DETAIL);
        }
    }
}
