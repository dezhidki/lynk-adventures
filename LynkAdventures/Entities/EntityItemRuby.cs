using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A ruby item.
    /// </summary>
    public class EntityItemRuby : EntityItem, IDropable
    {
        private bool wasUsed = false;
        private float dY = 0.0F;
        private float velocityY = 0.0F, acceleration = 0.0F, accelerationDecrease = 0.0F;
        private int velocityX = 0;
        private bool hasDropMovement = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemRuby"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntityItemRuby(LevelManager manager)
            : base(manager)
        {
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_RUBY, 11, 5, true);
        }

        private int xOffs0 = 6 * Game.SCALE, xOffs1 = 5 * Game.SCALE, yOffs0 = 4 * Game.SCALE, yOffs1 = 7 * Game.SCALE;

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - xOffs0, YCenter - yOffs0, XCenter + xOffs1, YCenter + yOffs1, this);
        }

        /// <summary>
        /// Determines whether the entity can pick up this item.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity can pick up this item; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanPickUp(Entity ent)
        {
            return ent is EntityPlayer;
        }

        /// <summary>
        /// Picks up this item.
        /// </summary>
        /// <param name="ent">The entity which picks this item up.</param>
        public override void PickUp(Entity ent)
        {
            if (wasUsed || hasDropMovement) return;

            EntityPlayer player = (EntityPlayer)ent;
            player.Rupies++;
            wasUsed = true;
            Sound.Coin.Play();
            base.PickUp(ent);
        }

        /// <summary>
        /// Drops the item.
        /// </summary>
        /// <param name="xVelocity">Velocity on X plane.</param>
        /// <param name="height">The height from which the item drops.</param>
        public void Drop(int xVelocity, float height)
        {
            dY = height;
            acceleration = -1.0F;
            velocityX = xVelocity;
            accelerationDecrease = 0.1F;
            hasDropMovement = true;
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (hasDropMovement)
            {
                velocityY += acceleration;
                acceleration += accelerationDecrease;
                dY += velocityY;

                Move(velocityX, 0);

                if (dY >= 0.0F)
                {
                    hasDropMovement = false;
                    dY = 0.0F;
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
            renderer.RenderTile(X, Y + dY, itemAnimation.CurrentSpriteID, itemAnimation.SpriteSheet, Color.White, hasDropMovement ? RenderLayer.LAYER_TILE_SOLID - 0.1F : layer);
        }
    }
}
