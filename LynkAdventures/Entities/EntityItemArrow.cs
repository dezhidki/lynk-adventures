using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Entities.Weapons;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities
{
    /// <summary>
    /// An arrow item.
    /// </summary>
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    public class EntityItemArrow : EntityItem, IDropable
    {
        private bool wasUsed = false;
        private float dY = 0.0F;
        private float velocityY = 0.0F, acceleration = 0.0F, accelerationDecrease = 0.0F;
        private int velocityX = 0;
        private bool hasDropMovement = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemArrow"/>.
        /// </summary>
        /// <param name="manager"Level manager.</param>
        /// <param name="amount">The amount of arrows this item yields.</param>
        public EntityItemArrow(LevelManager manager, int amount)
            : base(manager)
        {
            itemAmount = amount;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_ARROW, 9, 5, true);
        }

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - 10, YCenter + 10, XCenter + 10, YCenter + 8, this);
        }

        /// <summary>
        /// Picks up this item.
        /// </summary>
        /// <param name="ent">The entity which picks this item up.</param>
        public override void PickUp(Entity ent)
        {
            if (wasUsed) return;

            EntityPlayer ep = (EntityPlayer)ent;
            if (ep.Weapons[EntityPlayer.WEAPON_BOW] != null)
            {
                WeaponBow bow = (WeaponBow)ep.Weapons[EntityPlayer.WEAPON_BOW];
                ep.Arrows += itemAmount;
                wasUsed = true;
                base.PickUp(ent);
            }
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
            return (ent is EntityPlayer);
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
        /// Gets the sprite's Y position in the sprite sheet, which is determined by the amount of arrows the item yields.
        /// </summary>
        /// <returns>The sprite's Y position in the sprite sheet.</returns>
        private int GetSpritePosY()
        {
            return (int)MathHelper.Clamp(itemAmount - 1, 0, 3);
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, GetSpritePosY(), GameSpriteSheets.SPRITESHEET_ITEM_ARROW_SHADOWS, Color.White, RenderLayer.LAYER_DETAIL);
            renderer.RenderTile(X, Y, itemAnimation.CurrentSpriteID, GetSpritePosY(), itemAnimation.SpriteSheet, Color.White, hasDropMovement ? RenderLayer.LAYER_TILE_SOLID - 0.1F : layer);
        }
    }
}
