using LynkAdventures.Graphics;
using LynkAdventures.World;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A dummy entity.
    /// </summary>
    public class EntityRubyTest : EntityLiving
    {
        private AnimationHelper testAnim;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRubyTest"/>.
        /// </summary>
        /// <param name="levelManager">The level manager.</param>
        public EntityRubyTest(LevelManager levelManager)
            : base(levelManager)
        {
            xRadius = 15;
            yRadius = 15;
            maxHealth = 100;
            hasInfinteHealth = false;
            pushResistancePower = 1;
            hasHealthBar = true;
            testAnim = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_RUBY, 11, 5, true);
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
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();
            testAnim.UpdateStep();
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, testAnim.CurrentSpriteID, testAnim.SpriteSheet, currentGlow, layer);
        }
    }
}
