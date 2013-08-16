using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// A spider mob.
    /// </summary>
    public class EntitySpider : EntityLiving
    {
        private float minDistanceToFollow = 290.0F;
        private int randomMoveDir = 0;
        private int moveChanse = 6;
        private TimeSpan rollInterval = TimeSpan.FromSeconds(1.0);
        private DateTime lastTime = DateTime.Now;
        private AnimationHelper walkingAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_MOB_SPIDER, 2, 5, true);
        private bool hasSpottedPlayer = false;
        private int pushPower = 10;
        private int damage = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySpider"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntitySpider(LevelManager manager)
            : base(manager)
        {
            maxHealth = 20;
            CanGlowOnHit = true;
            pushResistancePower = 1;
            MoveSpeed = 2;
            hasHealthBar = true;
            xRadius = 30;
            yRadius = 30;
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
            int dropAmount = Game.Random.Next(4);

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
        /// Gets the distance to player.
        /// </summary>
        /// <param name="ep">The player.</param>
        /// <returns>Distance to player.</returns>
        private float DistanceToPlayer(EntityPlayer ep)
        {
            float dX = X - ep.X;
            float dY = Y - ep.Y;
            return (float)Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Gets the movement vector to the player.
        /// </summary>
        /// <param name="ep">The player.</param>
        /// <returns>Vector, that points towards the player.</returns>
        private Point GetMoveToPlayer(EntityPlayer ep)
        {
            float dX = ep.X - X;
            float dY = ep.Y - Y;

            Vector2 vec = new Vector2(dX, dY);
            vec.Normalize();
            vec *= MoveSpeed * 1.5f;

            return new Point((int)vec.X, (int)vec.Y);
        }

        /// <summary>
        /// Performs the random movement.
        /// </summary>
        private void PerformRandomMovement()
        {
            DateTime currentTime = DateTime.Now;

            if (currentTime - lastTime >= rollInterval)
            {
                randomMoveDir = Game.Random.Next(0, moveChanse);
                lastTime = currentTime;
            }

            bool hasMoved = false;

            if (randomMoveDir == Directions.UP)
            {
                Direction = Directions.UP;
                hasMoved |= Move(0, -MoveSpeed);
            }
            else if (randomMoveDir == Directions.DOWN)
            {
                Direction = Directions.DOWN;
                hasMoved |= Move(0, MoveSpeed);
            }
            else if (randomMoveDir == Directions.LEFT)
            {
                Direction = Directions.LEFT;
                hasMoved |= Move(-MoveSpeed, 0);
            }
            else if (randomMoveDir == Directions.RIGHT)
            {
                Direction = Directions.RIGHT;
                hasMoved |= Move(MoveSpeed, 0);
            }

            if (hasMoved)
                walkingAnimation.UpdateStep();
        }

        /// <summary>
        /// Performs the player chasing movement.
        /// </summary>
        private void PerformChaseMovement()
        {
            Point move = GetMoveToPlayer(levelManager.CurrentLevel.Player);

            if (Move(move.X, move.Y))
            {
                walkingAnimation.UpdateStep();

                if (move.X > 0) Direction = Directions.RIGHT;
                else if (move.X < 0) Direction = Directions.LEFT;
                if (move.Y > 0) Direction = Directions.DOWN;
                else if (move.Y < 0) Direction = Directions.UP;
            }
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();

            hasSpottedPlayer = DistanceToPlayer(levelManager.CurrentLevel.Player) <= minDistanceToFollow;

            if (!hasSpottedPlayer)
                PerformRandomMovement();
            else
                PerformChaseMovement();
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, walkingAnimation.CurrentSpriteID, Direction.ID, walkingAnimation.SpriteSheet, CurrentGlow, layer);
        }
    }
}
