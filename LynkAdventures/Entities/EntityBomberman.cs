using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.World;
using LynkAdventures.World.Tiles;
using System;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// The bomberman entity.
    /// </summary>
    public class EntityBomberman : EntityLiving
    {
        private AnimationHelper walkingAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_MOB_BOMBERMAN, 1, 10, true);
        private float minDistanceToChangeState = 200.0F;
        private bool hasMoved = false;
        private int randomMoveDir = 0;
        private int moveChanseNormal = 6, moveChanseFast = 4, moveChanse = 0;
        private TimeSpan rollIntervalNormal = TimeSpan.FromSeconds(1.0), rollIntervalFast = TimeSpan.FromSeconds(0.5), rollInterval = TimeSpan.FromSeconds(1.0);
        private DateTime lastTime = DateTime.Now;
        private int spriteID = 0;
        private int bombChanseNormal = 150, bombChanseFast = 10, bombChanse = 0;
        private bool canSpawnBombs = true;

        /// <summary>
        /// Gets or sets a value indicating whether the bomberman can spawn bombs.
        /// </summary>
        /// <value>
        /// <c>true</c> if the bomberman can spawn bombs; otherwise, <c>false</c>.
        /// </value>
        public bool CanSpawnBombs
        {
            get { return canSpawnBombs; }
            set { canSpawnBombs = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBomberman"/>.
        /// </summary>
        /// <param name="manager">Level manager.</param>
        public EntityBomberman(LevelManager manager)
            : base(manager)
        {
            maxHealth = 70;
            pushResistancePower = 2;
            MoveSpeed = 2;
            hasHealthBar = true;
            xRadius = 8 * Game.SCALE;
            yRadius = 10 * Game.SCALE;
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
        /// Gets the distance to the player.
        /// </summary>
        /// <param name="ep">The player.</param>
        /// <returns>Distance to the player.</returns>
        private float DistanceToPlayer(EntityPlayer ep)
        {
            float dX = X - ep.X;
            float dY = Y - ep.Y;
            return (float)Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (DistanceToPlayer(levelManager.CurrentLevel.Player) <= minDistanceToChangeState)
            {
                rollInterval = rollIntervalFast;
                moveChanse = moveChanseFast;
                MoveSpeed = 3;
                bombChanse = bombChanseFast;
            }
            else
            {
                rollInterval = rollIntervalNormal;
                moveChanse = moveChanseNormal;
                MoveSpeed = 2;
                bombChanse = bombChanseNormal;
            }

            if (Game.Random.Next(bombChanse) == 0 && canSpawnBombs)
            {
                levelManager.CurrentLevel.AddEntity(new EntityBombermanBomb(10, 5, 5, this, levelManager), (XCenter >> Level.TILESHIFT) * Tile.TILESIZE, (YCenter >> Level.TILESHIFT) * Tile.TILESIZE);
                canSpawnBombs = false;
            }

            DateTime currentTime = DateTime.Now;

            if (currentTime - lastTime >= rollInterval)
            {
                randomMoveDir = Game.Random.Next(0, moveChanse);
                lastTime = currentTime;
            }

            hasMoved = false;

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

            spriteID = hasMoved ? walkingAnimation.CurrentSpriteID + Direction.ID * 2 : Direction.ID;
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, spriteID, hasMoved ? 0 : 1, walkingAnimation.SpriteSheet, CurrentGlow, layer);
        }
    }
}
