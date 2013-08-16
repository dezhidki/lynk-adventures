using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A slime mob.
    /// </summary>
    public class EntitySlime : EntityLiving
    {
        private AnimationHelper walkAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_MOB_SLIME, 2, 5, true);
        private int moveChance = 6;
        private int moveDir = -1;
        private TimeSpan interval = TimeSpan.FromSeconds(1.0);
        private DateTime lastTime = DateTime.Now;
        private bool hasMoved = false;
        private Point moveDelta = Point.Zero;
        private int pushPower = 20;
        private int renderDir = 0;
        private int damage = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySlime"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntitySlime(LevelManager manager)
            : base(manager)
        {
            MoveSpeed = 2;
            hasInfinteHealth = false;
            pushResistancePower = 2;
            maxHealth = 10;
            xRadius = 25;
            yRadius = 25;
            CanGlowOnHit = true;
            hasHealthBar = true;
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
            return ent is EntityPlayer || typeof(EntityLiving).IsAssignableFrom(ent.GetType());
        }

        /// <summary>
        /// An event for collider when it collides with an entity.
        /// </summary>
        /// <param name="ent">The entity that collided.</param>
        /// <param name="bb">The bounding box of the entity.</param>
        public override void OnCollidedWidth(Entity ent, BoundingBox2D bb)
        {
            if (ent is EntityPlayer)
            {
                HitSource hit = new HitSource(Directions.GetOppositeDir(ent.Direction), pushPower, this, HitSource.OwnerType.ENTITY);
                ent.Hit(hit, damage);
            }
        }

        /// <summary>
        /// An event for collided entity when it collides with an object.
        /// </summary>
        /// <param name="owner">Collider.</param>
        /// <param name="bb">Collider's bounding box.</param>
        public override void OnCollidedBy(BoundingBoxOwner owner, BoundingBox2D bb)
        {
            if (!typeof(Entity).IsAssignableFrom(owner.GetType())) return;

            Entity ent = (Entity)owner;
            if (ent is EntityPlayer)
            {
                HitSource hit = new HitSource(Direction, pushPower, this, HitSource.OwnerType.ENTITY);
                ent.Hit(hit, damage);
            }
        }

        /// <summary>
        /// Called when entity is dead and is being removed.
        /// </summary>
        public override void OnDeath()
        {
            base.OnDeath();

            List<IDropable> dropables = new List<IDropable>();
            int dropAmount = Game.Random.Next(3);

            for (int i = 0; i < dropAmount; i++)
            {
                int randomDrop = Game.Random.Next(3);
                switch (randomDrop)
                {
                    case 0: dropables.Add(new EntityItemArrow(levelManager, Game.Random.Next(1, 11))); break;
                    case 1: dropables.Add(new EntityItemHealth(Game.Random.Next(1, 3), levelManager)); break;
                    case 2: dropables.Add(new EntityItemRuby(levelManager)); break;
                }
            }

            foreach (IDropable dropable in dropables)
            {
                levelManager.CurrentLevel.AddEntity((Entity)dropable, X, Y);
                dropable.Drop(Game.Random.Next(-3, 4), -10.0F);
            }
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();

            DateTime currentTime = DateTime.Now;
            if (currentTime - lastTime > interval)
            {
                moveDir = Game.Random.Next(moveChance);
                lastTime = currentTime;
            }

            hasMoved = false;
            if (moveDir == Directions.UP)
            {
                Direction = Directions.UP;
                hasMoved |= Move(0, -MoveSpeed);
            }
            else if (moveDir == Directions.DOWN)
            {
                Direction = Directions.DOWN;
                hasMoved |= Move(0, MoveSpeed);
            }
            else if (moveDir == Directions.LEFT)
            {
                Direction = Directions.LEFT;
                hasMoved |= Move(-MoveSpeed, 0);
            }
            else if (moveDir == Directions.RIGHT)
            {
                Direction = Directions.RIGHT;
                hasMoved |= Move(MoveSpeed, 0);
            }
            else
            {
                Direction = Directions.NONE;
            }

            if (hasMoved)
                walkAnimation.UpdateStep();

            renderDir = Direction.ID < 0 ? 0 : Direction.ID;
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            //renderer.RenderTile(X, Y, walkAnimation.CurrentSpriteID, walkAnimation.SpriteSheet, currentGlow, layer);
            renderer.RenderTile(X, Y, walkAnimation.CurrentSpriteID, renderDir, GameSpriteSheets.SPRITESHEET_MOB_SLIME, CurrentGlow, layer);
        }
    }
}
