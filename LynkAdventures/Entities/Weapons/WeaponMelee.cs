using LynkAdventures.BasicPhysicsUtils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LynkAdventures.Entities.Weapons
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A melee weapon.
    /// </summary>
    public class WeaponMelee : Weapon
    {
        protected BoundingBox2D hitRadiusBox = BoundingBox2D.Zero;
        protected Point hitAreaRadiusPoint = Point.Zero; // X = side radius, Y = bottom and top radius
        protected int weaponLength = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponMelee"/>.
        /// </summary>
        /// <param name="ent">The owner.</param>
        public WeaponMelee(Entity ent)
            : base(ent)
        {
            weaponType = WeaponType.MELEE;
        }

        /// <summary>
        /// Gets the entities to interact with.
        /// </summary>
        /// <param name="dir">The direction to look from.</param>
        /// <returns>List of entities that are within the weapon's interaction radius.</returns>
        protected List<Entity> GetEntitiesToInteractWith(Direction dir)
        {
            int x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            BoundingBox2D bb = owner.GetBoundingBox();

            if (dir == Directions.UP)
            {
                x0 = bb.XLeft - hitAreaRadiusPoint.Y;
                y0 = bb.YTop - hitRadiusBox.YTop;
                x1 = bb.XRight + hitAreaRadiusPoint.Y;
                y1 = bb.YTop;
            }
            else if (dir == Directions.DOWN)
            {
                x0 = bb.XLeft - hitAreaRadiusPoint.Y;
                y0 = bb.YBottom;
                x1 = bb.XRight + hitAreaRadiusPoint.Y;
                y1 = bb.YBottom + hitRadiusBox.YBottom;
            }
            else if (dir == Directions.LEFT)
            {
                x0 = bb.XLeft - hitRadiusBox.XLeft;
                y0 = bb.YTop - hitAreaRadiusPoint.X;
                x1 = bb.XLeft;
                y1 = bb.YBottom + hitAreaRadiusPoint.X;
            }
            else if (dir == Directions.RIGHT)
            {
                x0 = bb.XRight;
                y0 = bb.YTop - hitAreaRadiusPoint.X;
                x1 = bb.XRight + hitRadiusBox.XRight;
                y1 = bb.YBottom + hitAreaRadiusPoint.X;
            }

            return owner.LevelManager.CurrentLevel.GetEntities(new BoundingBox2D(x0, y0, x1, y1));
        }

    }
}
