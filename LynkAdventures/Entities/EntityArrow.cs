using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.MathHelpers;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities
{
    /// @author Denis Zhidkikh
    /// @version 5.4.2013
    /// <summary>
    /// An arrow entity.
    /// </summary>
    public class EntityArrow : Entity
    {
        private readonly BoundingBox2D[] BB_OFFESTS_FLY = { new BoundingBox2D(3, 8, 1, 7), new BoundingBox2D(3, 8, 1, 7), new BoundingBox2D(8, 3, 7, 1), new BoundingBox2D(8, 3, 7, 1) };
        private readonly BoundingBox2D[] BB_OFFSETS_HIT = { new BoundingBox2D(3, 6, 1, 5), new BoundingBox2D(3, 6, 1, 5), new BoundingBox2D(6, 3, 5, 1), new BoundingBox2D(6, 3, 5, 1) };
        private readonly BoundingBox2D[] BB_OFFSETS_HIT_GROUND = { new BoundingBox2D(3, 6, 1, 5), new BoundingBox2D(3, 6, 1, 5), new BoundingBox2D(2, 5, 2, 6), new BoundingBox2D(2, 5, 2, 6) };
        private const int SPRITES_FLY = 0;
        private const int SPRITES_HIT = 1;
        private const int SPRITES_GROUND = 2;

        private bool hasHit = false;
        private int flyDistance = 0;
        private Point stopPos = Point.Zero;
        private Entity owner;
        private int damage = 0;
        private int movedDistance = 0;
        private int lifeTime = 0, currentLifeTime = 0;
        private int renderSprite = 0;
        private int pushPower = 0;

        // <summary>
        // Alustaa nuoliolion.
        // </summary>
        // <param name="manager">Tasojen manageri.</param>
        // <param name="dir">Suunta, johon nuoli lentää.</param>
        // <param name="owner">Nuolen omistaja.</param>
        // <param name="damage">Nuolen tekemä vahinko.</param>
        // <param name="speed">Nuolen nopeus.</param>
        // <param name="push">Nuolen työntövoima.</param>
        // <param name="maxFlyDistance">Suurin lentoetäisyys lähtöpisteestä.</param>
        // <param name="lifeTime">Elinaika osumisen jälkeen.</param>

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityArrow"/>.
        /// </summary>
        /// <param name="manager">Level manager instance.</param>
        /// <param name="dir">Moving direction.</param>
        /// <param name="owner">Owner of the arrow.</param>
        /// <param name="damage">The amount of damage the arrow inflicts.</param>
        /// <param name="speed">The move speed of the arrow.</param>
        /// <param name="push">The push force of the arrow.</param>
        /// <param name="maxFlyDistance">The maximum fly distance. The arrow will drop to the ground after this distance is reached.</param>
        /// <param name="lifeTime">Arrow's life time after it drops or hits a solid tile.</param>
        public EntityArrow(LevelManager manager, Direction dir, Entity owner, int damage, int speed, int push, int maxFlyDistance = 10, int lifeTime = 100)
            : base(manager)
        {
            this.damage = damage;
            this.owner = owner;
            this.lifeTime = lifeTime;
            flyDistance = maxFlyDistance;
            Direction = dir;
            MoveSpeed = speed;
            width = 24 * Game.SCALE;
            height = 24 * Game.SCALE;
            HasHitGround = false;
            pushPower = push;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the arrow has hit ground.
        /// </summary>
        /// <value>
        /// <c>true</c> if the arrow has hit ground; otherwise, <c>false</c>.
        /// </value>
        public bool HasHitGround { get; protected set; }

        /// <summary>
        /// Initializes the entity right before adding to the <see cref="Level" />.
        /// </summary>
        public override void Init()
        {
            stopPos = Direction.DirPoint;
            stopPos.X *= flyDistance + X;
            stopPos.Y *= flyDistance + Y;
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
            return false;
        }

        /// <summary>
        /// Gets the bounding box of this entity.
        /// </summary>
        /// <returns>
        /// The bounding box of this entity.
        /// </returns>
        public override BoundingBox2D GetBoundingBox()
        {
            BoundingBox2D offset = new BoundingBox2D(0, 0, 0, 0);
            if (!hasHit)
                offset = BB_OFFESTS_FLY[Direction.ID];
            else if (hasHit && !HasHitGround)
                offset = BB_OFFSETS_HIT[Direction.ID];
            else if (hasHit && HasHitGround)
                offset = BB_OFFSETS_HIT_GROUND[Direction.ID];

            return new BoundingBox2D(XCenter - offset.XLeft, YCenter - offset.YTop, XCenter + offset.XRight, YCenter + offset.YBottom, this);
        }

        /// <summary>
        /// Called when the arrow hits the ground.
        /// </summary>
        private void HitGround()
        {
            HasHitGround = true;
            hasHit = true;
        }

        /// <summary>
        /// Called when this entity touched another.
        /// </summary>
        /// <param name="ent">The entity that got touched.</param>
        public override void OnTouched(Entity ent)
        {
            if (ent == owner || typeof(EntityItem).IsAssignableFrom(ent.GetType()) || ent is EntityArrow) return;

            if (!typeof(EntityLiving).IsAssignableFrom(ent.GetType()))
            {
                hasHit = true;
                return;
            }

            HitSource damager = new HitSource(Direction, pushPower, this, HitSource.OwnerType.PROJECTILE);
            ent.Hit(damager, damage);
            Sound.Hit.Play(1.0f, 0.8f, 0.0f);

            if (!(ent is EntityArrow))
                Die();
        }

        /// <summary>
        /// Called when this entity steps on a walkable tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="xTile">The X position of the tile on the level grid.</param>
        /// <param name="yTile">The Y position of the tile on the level grid.</param>
        public override void SteppedOnTile(Tile tile, int xTile, int yTile)
        {
            //if (tile.IsSolid(this))
                hasHit = true;
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            Point moveDir = Direction.DirPoint;
            moveDir.X *= MoveSpeed;
            moveDir.Y *= MoveSpeed;
            bool hasMoved = false;

            if (!hasHit)
                hasMoved = Move(moveDir.X, moveDir.Y);
            if (hasMoved)
                movedDistance += MoveSpeed;

            if (hasMoved && GameMath.AlmostSame(movedDistance, flyDistance, MoveSpeed % 2))
                HitGround();

            if (hasHit)
            {
                currentLifeTime++;
                if (lifeTime == currentLifeTime)
                {
                    Die();
                }
            }

            renderSprite = hasHit ? (HasHitGround ? SPRITES_GROUND : SPRITES_HIT) : SPRITES_FLY;
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, Direction.ID, renderSprite, GameSpriteSheets.SPRITESHEET_ARROW, Color.White, hasHit ? RenderLayer.LAYER_TILE - 0.05F : layer);
        }
    }
}
