using LynkAdventures.Graphics;
using LynkAdventures.Gui;
using LynkAdventures.World;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A pickup item.
    /// </summary>
    public class EntityItem : Entity
    {
        protected int itemAmount;
        protected AnimationHelper itemAnimation;
        protected GuiSwapWeapons gui;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItem"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntityItem(LevelManager manager)
            : base(manager)
        {
            itemAmount = 1;
            canBePushed = false;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_PLACEHOLDER, 14, 5, true);
            xRadius = 25;
            yRadius = 20;
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
            return false;
        }

        /// <summary>
        /// Determines whether the entity can pick up this item.
        /// </summary>
        /// <param name="ent">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity can pick up this item; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanPickUp(Entity ent)
        {
            return false;
        }

        /// <summary>
        /// Picks up this item.
        /// </summary>
        /// <param name="ent">The entity which picks this item up.</param>
        public virtual void PickUp(Entity ent)
        {
            Die();
        }

        /// <summary>
        /// Called when this entity gets touched by another.
        /// </summary>
        /// <param name="ent">The entity that touched.</param>
        public override void OnTouch(Entity ent)
        {
            if (CanPickUp(ent))
                PickUp(ent);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            itemAnimation.UpdateStep();
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
        }
    }
}
