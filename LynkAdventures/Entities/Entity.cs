using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.MathHelpers;
using LynkAdventures.World;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LynkAdventures.Entities
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// An unique object in the world, that can move and interact with other game objects.
    /// </summary>
    public class Entity : BoundingBoxOwner
    {
        protected Point pos = Point.Zero;
        protected LevelManager levelManager;
        protected int xRadius = 0, yRadius = 0;
        protected int width = Game.SCALE * 32, height = Game.SCALE * 32;
        private int currentPushPower = 0;
        protected int pushResistancePower = 0;
        protected Direction pushDirection = Directions.DOWN;
        private bool hasPhysicsPush = false;
        protected bool canBePushed = true;
        protected bool isGlowing = false;
        protected Color currentGlow = Color.White;
        protected int maxGlowTime = 15, currentGlowTime = 0;
        private bool hasInited = false;
        protected BoundingBox2D interactArea = BoundingBox2D.Zero;
        protected Point interactRadius = Point.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/>, but doesn't add it to the <see cref="Level"/> yet.
        /// </summary>
        /// <param name="levelManager">The level manager.</param>
        public Entity(LevelManager levelManager)
        {
            GlowColor = Color.Red;
            CanGlowOnHit = true;
            MoveSpeed = 2;
            Direction = Directions.UP;
            this.levelManager = levelManager;
        }

        #region Attributes

        /// <summary>
        /// Gets a value indicating whether this entity can be pushed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this entity can be pushed; otherwise, <c>false</c>.
        /// </value>
        public bool CanBePushed { get { return canBePushed; } }

        /// <summary>
        /// Gets or sets a value indicating whether this entity is dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this entity is dead; otherwise, <c>false</c>.
        /// </value>
        public bool IsDead { get; protected set; }

        /// <summary>
        /// Gets or sets the direction of this entity.
        /// </summary>
        /// <value>
        /// The direction of this entity.
        /// </value>
        public Direction Direction { get; protected set; }

        /// <summary>
        /// Gets or sets the move speed of this entity.
        /// </summary>
        /// <value>
        /// The move speed of this entity.
        /// </value>
        public int MoveSpeed { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this entity can temporary change its color when being hit.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can temporary change its color when being hit.; otherwise, <c>false</c>.
        /// </value>
        public bool CanGlowOnHit { get; protected set; }

        /// <summary>
        /// Gets or sets the color of this entity when it is being hit given that it can change the color.
        /// </summary>
        /// <value>
        /// The color of this entity when it is being hit given that it can change the color.
        /// </value>
        public Color GlowColor { get; protected set; }

        /// <summary>
        /// Gets the current color of this entity.
        /// </summary>
        /// <value>
        /// The current color of this entity.
        /// </value>
        public Color CurrentGlow { get { return currentGlow; } }

        /// <summary>
        /// Gets the level manager.
        /// </summary>
        /// <value>
        /// The level manager.
        /// </value>
        public LevelManager LevelManager { get { return levelManager; } }

        /// <summary>
        /// Gets the width of the entity's texture.
        /// </summary>
        /// <value>
        /// The width of the entity's texture.
        /// </value>
        public int Width { get { return width; } }

        /// <summary>
        /// Gets the height of the entity's texture.
        /// </summary>
        /// <value>
        /// The height of the entity's texture.
        /// </value>
        public int Height { get { return height; } }

        /// <summary>
        /// Gets or sets the entity's position on the current <see cref="Level"/>. Counts as entity's top left corner.
        /// </summary>
        /// <value>
        /// The entity's position on the current <see cref="Level"/>. Counts as entity's top left corner.
        /// </value>
        public Point Position
        {
            get { return pos; }
            set { pos = value; }
        }

        /// <summary>
        /// Gets the radius of this entity on the X plane. Used to determine generic bounding box.
        /// </summary>
        /// <value>
        /// The radius of this entity on the X plane. Used to determine generic bounding box.
        /// </value>
        public int XRadius { get { return xRadius; } }

        /// <summary>
        /// Gets the radius of this entity on the Y plane. Used to determine generic bounding box.
        /// </summary>
        /// <value>
        /// The radius of this entity on the Y plane. Used to determine generic bounding box.
        /// </value>
        public int YRadius { get { return yRadius; } }

        /// <summary>
        /// Gets or sets the X coordinate of the entity. Counts as entity's left side.
        /// </summary>
        /// <value>
        /// The X coordinate of the entity. Counts as entity's left side.
        /// </value>
        public int X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the entity. Counts as entity's left side.
        /// </summary>
        /// <value>
        /// The Y coordinate of the entity. Counts as entity's left side.
        /// </value>
        public int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        /// <summary>
        /// Gets the entity's center on the X plane.
        /// </summary>
        /// <value>
        /// The entity's center on the X plane.
        /// </value>
        public int XCenter { get { return pos.X + width / 2; } }

        /// <summary>
        /// Gets the entity's center on the Y plane.
        /// </summary>
        /// <value>
        /// The entity's center on the Y plane.
        /// </value>
        public int YCenter { get { return pos.Y + height / 2; } }

        #endregion

        /// <summary>
        /// Initializes the entity right before adding to the <see cref="Level"/>.
        /// </summary>
        public virtual void Init()
        {
            if (hasInited) return;
            hasInited = true;
        }

        /// <summary>
        /// Called when this entity gets interacted by another one.
        /// </summary>
        /// <param name="ent">The entity that interacted with this one.</param>
        public virtual void OnInteractBy(Entity ent)
        {
        }

        /// <summary>
        /// Searches the given area for the entities to interact with and calls <see cref="OnInteractBy"/> on them.
        /// </summary>
        /// <param name="dir">The direction.</param>
        /// <param name="includeOwnerBB">If set to <c>true</c>, include owner's bounding box in the search.</param>
        public void Interact(Direction dir, bool includeOwnerBB = false)
        {
            BoundingBox2D bb = GetBoundingBox();
            BoundingBox2D bb2 = BoundingBox2D.Zero;

            if (dir == Directions.UP)
            {
                bb2.XLeft = bb.XLeft - interactRadius.Y;
                bb2.YTop = bb.YTop - interactArea.YTop;
                bb2.XRight = bb.XRight + interactRadius.Y;
                bb2.YBottom = bb.YTop;
            }
            else if (dir == Directions.DOWN)
            {
                bb2.XLeft = bb.XLeft - interactRadius.Y;
                bb2.YTop = bb.YBottom;
                bb2.XRight = bb.XRight + interactRadius.Y;
                bb2.YBottom = bb.YBottom + interactArea.YBottom;
            }
            else if (dir == Directions.LEFT)
            {
                bb2.XLeft = bb.XLeft - interactArea.XLeft;
                bb2.YTop = bb.YTop - interactRadius.X;
                bb2.XRight = bb.XLeft;
                bb2.YBottom = bb.YBottom + interactRadius.X;
            }
            else if (dir == Directions.RIGHT)
            {
                bb2.XLeft = bb.XRight;
                bb2.YTop = bb.YTop - interactRadius.X;
                bb2.XRight = bb.XRight + interactArea.XRight;
                bb2.YBottom = bb.YBottom + interactRadius.X;
            }

            List<Entity> entities = LevelManager.CurrentLevel.GetEntities(bb2);

            if (includeOwnerBB)
                entities = entities.Union(LevelManager.CurrentLevel.GetEntities(bb)).ToList();

            entities.ForEach(ent => ent.OnInteractBy(this));
        }

        /// <summary>
        /// Called when the level is changed.
        /// </summary>
        /// <param name="newLevel">The level <see cref="LevelManager"/> is changing to.</param>
        public virtual void OnLevelChange(Level newLevel)
        {
        }

        /// <summary>
        /// Marks the entity as dead. Dead entities are removed the next time they are updated.
        /// </summary>
        public void Die()
        {
            IsDead = true;
        }

        /// <summary>
        /// Called when entity is dead and is being removed.
        /// </summary>
        public virtual void OnDeath()
        {
        }

        /// <summary>
        /// Hits the this entity.
        /// </summary>
        /// <param name="damager">The damager.</param>
        /// <param name="damage">The damage amount.</param>
        public virtual void Hit(HitSource damager, int damage = 0)
        {
            OnHit(damage, damager);
        }

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>The bounding box of this entity.</returns>
        public virtual BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - XRadius, YCenter - YRadius, XCenter + XRadius, YCenter + YRadius, this);
        }

        /// <summary>
        /// Adds the bounding box into the bounding box list.
        /// </summary>
        /// <param name="ent">The ent.</param>
        /// <param name="list">The list.</param>
        public virtual void AddBoundingBox(Entity ent, ref List<BoundingBox2D> list)
        {
            if (ent != this)
                list.Add(GetBoundingBox());
        }

        /// <summary>
        /// Moves the entity by the given amount.
        /// </summary>
        /// <param name="dx">The amount to move in X plane.</param>
        /// <param name="dy">The amount to move in Y plane.</param>
        /// <returns><c>true</c> if the entity moved without collding to anything; otherwise, <c>false</c>.</returns>
        public bool Move(int dx, int dy)
        {
            if (X + dx < 0 || X + dx >= levelManager.CurrentLevel.Width * Tile.TILESIZE)
                dx = 0;
            if (Y + dy < 0 || Y + dy >= levelManager.CurrentLevel.Height * Tile.TILESIZE)
                dy = 0;

            if (dx != 0 || dy != 0)
            {
                int absDX = Math.Abs(dx);
                int absDY = Math.Abs(dy);
                int moveTimesX = absDX - 1;
                int moveTimesY = absDY - 1;
                int signX = GameMath.GetSign(dx);
                int signY = GameMath.GetSign(dy);

                if (moveTimesX > 1 || moveTimesY > 1)
                {
                    moveTimesX = absDX / 2;
                    moveTimesY = absDY / 2;
                    dx = absDX > 2 ? 2 * signX : dx;
                    dy = absDY > 2 ? 2 * signY : dy;
                }

                bool hasMoved = false;
                for (int moveX = 0; moveX < moveTimesX; moveX++)
                {
                    hasMoved = MoveStep(dx, 0);
                    if (!hasMoved || IsDead) goto checkYMove;
                }
                if (absDX % 2 == 1)
                    hasMoved = MoveStep(signX, 0);

            checkYMove:
                for (int moveY = 0; moveY < moveTimesY; moveY++)
                {
                    hasMoved = MoveStep(0, dy);
                    if (!hasMoved || IsDead) goto checkHasMoved;
                }
                if (absDY % 2 == 1)
                    hasMoved = MoveStep(0, signY);

            checkHasMoved:
                if (hasMoved)
                {
                    int xTile = XCenter >> Level.TILESHIFT;
                    int yTile = YCenter >> Level.TILESHIFT;
                    Tile tile = levelManager.CurrentLevel.GetTile(xTile, yTile);
                    if (tile != null)
                        tile.OnStepped(this, xTile, yTile, levelManager.CurrentLevel);
                }
                return hasMoved;
            }
            return false;
        }

        /// <summary>
        /// Attempts to move the entity by one step.
        /// </summary>
        /// <param name="dx">The amount to move on the X plane.</param>
        /// <param name="dy">The amount to move on the Y plane.</param>
        /// <returns><c>true</c> if no collisions occured during movement and the movement was done only in one plane; otherwise, <c>false</c>.</returns>
        /// <remarks>Can move only one plane at the time.</remarks>
        private bool MoveStep(int dx, int dy)
        {
            if (dx != 0 && dy != 0)
                return false;

            BoundingBox2D bb = GetBoundingBox();

            List<BoundingBox2D> newBox = levelManager.CurrentLevel.GetBoundingBoxesForEntity(this, bb + new Point(dx, dy));
            foreach (BoundingBox2D boundingBox in newBox)
            {
                if (typeof(Tile).IsAssignableFrom(boundingBox.Owner.GetType()))
                {
                    Tile tile = ((Tile)boundingBox.Owner);
                    tile.OnStepped(this, boundingBox.XLeft >> Tile.TILESIZE, boundingBox.YTop >> Tile.TILESIZE, LevelManager.CurrentLevel);
                    SteppedOnTile(tile, boundingBox.XLeft >> Tile.TILESIZE, boundingBox.YTop >> Tile.TILESIZE);
                }
                else if (typeof(Entity).IsAssignableFrom(boundingBox.Owner.GetType()))
                {
                    Entity ent = ((Entity)boundingBox.Owner);
                    ent.OnTouch(this);
                    OnTouched(ent);
                }
            }
            newBox.RemoveAll(bBox => !ShouldBeCheckedForSolidCollision(bBox));
            if (newBox.Count > 0)
            {
                foreach (BoundingBox2D boundingBox in newBox)
                {
                    boundingBox.Owner.OnCollidedWidth(this, bb);
                    if (typeof(Entity).IsAssignableFrom(boundingBox.Owner.GetType()))
                        OnCollidedBy(boundingBox.Owner, bb);
                }
                return false;
            }

            X += dx;
            Y += dy;
            return true;
        }

        /// <summary>
        /// Determines if the given bounding box should be checked for the collision with this entity.
        /// </summary>
        /// <param name="bb">The bb.</param>
        /// <returns><c>true</c>, if the entity should be checked for collision; otherwise, <c>false</c>.</returns>
        private bool ShouldBeCheckedForSolidCollision(BoundingBox2D bb)
        {
            BoundingBoxOwner owner = bb.Owner;
            if (typeof(Entity).IsAssignableFrom(owner.GetType()))
                return ((Entity)owner).SolidToEntity(this);
            return true;
        }

        /// <summary>
        /// An event for collider when it collides with an entity.
        /// </summary>
        /// <param name="ent">The entity that collided.</param>
        /// <param name="bb">The bounding box of the entity.</param>
        public virtual void OnCollidedWidth(Entity ent, BoundingBox2D bb)
        {
        }

        /// <summary>
        /// An event for collided entity when it collides with an object.
        /// </summary>
        /// <param name="owner">Collider.</param>
        /// <param name="bb">Collider's bounding box.</param>
        public virtual void OnCollidedBy(BoundingBoxOwner owner, BoundingBox2D bb)
        {
        }

        /// <summary>
        /// Creates the push movement for this entity.
        /// </summary>
        /// <param name="dir">The direction of the push.</param>
        /// <param name="power">The power of the push.</param>
        public virtual void CreatePush(Direction dir, int power)
        {
            if (!canBePushed) return;
            hasPhysicsPush = true;
            pushDirection = dir;
            currentPushPower = power;

            if (currentPushPower < 0)
                Console.WriteLine("Caution! Push power is negative (may cause weird behaviour)!");
        }

        /// <summary>
        /// Checks if this entity is solid to the given entity.
        /// </summary>
        /// <param name="ent">The entity to check with.</param>
        /// <returns><c>true</c> if this entity is solid to the given entity; otherwise, <c>false</c>.</returns>
        public virtual bool SolidToEntity(Entity ent)
        {
            return false;
        }

        /// <summary>
        /// Called when this entity steps on a walkable tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="xTile">The X position of the tile on the level grid.</param>
        /// <param name="yTile">The Y position of the tile on the level grid.</param>
        public virtual void SteppedOnTile(Tile tile, int xTile, int yTile)
        {
        }

        /// <summary>
        /// Called when this entity gets killed by some object.
        /// </summary>
        /// <param name="damageSource">The information of the damaging source.</param>
        public virtual void OnKilledBy(HitSource damageSource)
        {
        }

        /// <summary>
        /// Called when this entity gets hit.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <param name="pushSource">The information about the damage source.</param>
        public virtual void OnHit(int damage, HitSource pushSource)
        {
            if (CanGlowOnHit)
                isGlowing = true;
            CreatePush(pushSource.Direction, pushSource.PushPower);
        }

        /// <summary>
        /// Called when this entity touched another.
        /// </summary>
        /// <param name="ent">The entity that got touched.</param>
        public virtual void OnTouched(Entity ent)
        {
        }

        /// <summary>
        /// Called when this entity gets touched by another.
        /// </summary>
        /// <param name="ent">The entity that touched.</param>
        public virtual void OnTouch(Entity ent)
        {
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public virtual void Update()
        {
            UpdateGlow();
            if (hasPhysicsPush)
            {
                Point pushDir = pushDirection.DirPoint;
                Move(currentPushPower * pushDir.X, currentPushPower * pushDir.Y);
                currentPushPower -= pushResistancePower;

                if (currentPushPower <= 0)
                {
                    currentPushPower = 0;
                    pushDirection = Directions.UP;
                    hasPhysicsPush = false;
                }
            }
        }

        /// <summary>
        /// Updates the glow of the entity, if <see cref="CanGlowOnHit"/> is true.
        /// </summary>
        protected void UpdateGlow()
        {
            if (isGlowing)
            {
                currentGlowTime++;
                currentGlow = GlowColor;

                if (currentGlowTime >= maxGlowTime)
                {
                    isGlowing = false;
                    currentGlow = Color.White;
                    currentGlowTime = 0;
                }
            }
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public virtual void Render(Renderer renderer, float layer)
        {
        }
    }
}
