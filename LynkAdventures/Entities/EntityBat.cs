using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using Microsoft.Xna.Framework;
using System;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A bat.
    /// </summary>
    public class EntityBat : EntityLiving
    {
        private AnimationHelper walkingAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_MOB_BAT, 2, 5, true);
        private int moveChance = 4;
        private int moveDir = -1, oldMoveDir = 0;
        private TimeSpan interval = TimeSpan.FromSeconds(0.4);
        private DateTime lastTime = DateTime.Now;
        private Point moveDelta = Point.Zero;
        private int pushPower = 10;
        private int damage = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBat"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntityBat(LevelManager manager)
            : base(manager)
        {
            MoveSpeed = 2;
            hasInfinteHealth = false;
            pushResistancePower = 1;
            maxHealth = 15;
            xRadius = 20;
            yRadius = 32;
            CanGlowOnHit = true;
            hasHealthBar = true;
        }

        /// <summary>
        /// Checks if this entity is solid to the given entity.
        /// </summary>
        /// <param name="ent">The entity to check with.</param>
        /// <returns>
        /// <c>true</c> if this entity is solid to the given entity; otherwise, <c>false</c>.
        /// </returns>
        public override bool SolidToEntity(Entity ent)
        {
            return ent is EntityPlayer;
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
            if (Game.Random.Next(2) == 0)
            {
                IDropable dropable = null;
                int randomDrop = Game.Random.Next(3);
                switch (randomDrop)
                {
                    case 0: dropable = new EntityItemArrow(levelManager, Game.Random.Next(1, 11)); break;
                    case 1: dropable = new EntityItemHealth(Game.Random.Next(1, 3), levelManager); break;
                    case 2: dropable = new EntityItemRuby(levelManager); break;
                }

                if (dropable != null)
                {
                    levelManager.CurrentLevel.AddEntity((Entity)dropable, X, Y);
                    dropable.Drop(Game.Random.Next(-3, 4), -10.0F);
                }
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

            if (moveDir == Directions.UP)
            {
                oldMoveDir = moveDir;
                Direction = Directions.UP;
                Move(0, -MoveSpeed);
            }
            else if (moveDir == Directions.DOWN)
            {
                oldMoveDir = moveDir;
                Direction = Directions.DOWN;
                Move(0, MoveSpeed);
            }
            else if (moveDir == Directions.LEFT)
            {
                oldMoveDir = moveDir;
                Direction = Directions.LEFT;
                Move(-MoveSpeed, 0);
            }
            else if (moveDir == Directions.RIGHT)
            {
                oldMoveDir = moveDir;
                Direction = Directions.RIGHT;
                Move(MoveSpeed, 0);
            }
            else
            {
                Direction = Directions.NONE;
            }

            walkingAnimation.UpdateStep();
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, walkingAnimation.CurrentSpriteID, oldMoveDir, walkingAnimation.SpriteSheet, CurrentGlow, layer);
        }

    }
}
