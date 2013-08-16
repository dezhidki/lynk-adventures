using LynkAdventures.Entities.TileEntities;
using LynkAdventures.Graphics;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using LynkAdventures.World.Tiles;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Bomberman's bomb.
    /// </summary>
    public class EntityBombermanBomb : Entity
    {
        private AnimationHelper animation;
        private EntityBomberman bomberman;
        private int explosionWidth, explosionHeight;
        private int explosionDamage = 1, explosionPower = 15;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBombermanBomb"/>.
        /// </summary>
        /// <param name="timer">The time until the explosion happens. Works also as animation speed.</param>
        /// <param name="explosionWidth">Width of the explosion.</param>
        /// <param name="explosionHeight">Height of the explosion.</param>
        /// <param name="bomberman">The bomberman.</param>
        /// <param name="manager">Level manager.</param>
        public EntityBombermanBomb(int timer, int explosionWidth, int explosionHeight, EntityBomberman bomberman, LevelManager manager)
            : base(manager)
        {
            animation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_OBJECT_BOMB, 11, timer, false);
            animation.AnimationReady += Explode;
            this.bomberman = bomberman;
            this.explosionWidth = explosionWidth;
            this.explosionHeight = explosionHeight;
        }

        /// <summary>
        /// Explodes the bomb. Replicates the original bomberman's bomb.
        /// </summary>
        private void Explode()
        {
            Die();
            bomberman.CanSpawnBombs = true;
            Sound.Explosion.Play();

            bool[] hasSucseededToExplodeRight = new bool[explosionWidth / 2];
            bool[] hasSucseededToExplodeLeft = new bool[explosionWidth / 2];
            bool[] hasSucseededToExplodeUp = new bool[explosionHeight / 2];
            bool[] hasSucseededToExplodeDown = new bool[explosionHeight / 2];

            for (int i = 0; i < explosionWidth / 2; i++)
            {
                if (!levelManager.CurrentLevel.GetTile((XCenter >> Level.TILESHIFT) + i + 1, YCenter >> Level.TILESHIFT).IsSolid(this) && (i == 0 ? true : hasSucseededToExplodeRight[i - 1]))
                {
                    levelManager.CurrentLevel.AddEntity(new TileEntityBombermanExplosion(explosionDamage, explosionPower, levelManager.CurrentLevel, levelManager), ((XCenter >> Level.TILESHIFT) + i + 1) * Tile.TILESIZE, (YCenter >> Level.TILESHIFT) * Tile.TILESIZE);
                    hasSucseededToExplodeRight[i] = true;
                }
                else
                    hasSucseededToExplodeRight[i] = false;

                if (!levelManager.CurrentLevel.GetTile((XCenter >> Level.TILESHIFT) - i - 1, YCenter >> Level.TILESHIFT).IsSolid(this) && (i == 0 ? true : hasSucseededToExplodeLeft[i - 1]))
                {
                    levelManager.CurrentLevel.AddEntity(new TileEntityBombermanExplosion(explosionDamage, explosionPower, levelManager.CurrentLevel, levelManager), ((XCenter >> Level.TILESHIFT) - i - 1) * Tile.TILESIZE, (YCenter >> Level.TILESHIFT) * Tile.TILESIZE);
                    hasSucseededToExplodeLeft[i] = true;
                }
                else
                    hasSucseededToExplodeLeft[i] = false;
            }

            for (int i = 0; i < explosionHeight / 2; i++)
            {
                if (!levelManager.CurrentLevel.GetTile(XCenter >> Level.TILESHIFT, (YCenter >> Level.TILESHIFT) + i + 1).IsSolid(this) && (i == 0 ? true : hasSucseededToExplodeDown[i - 1]))
                {
                    levelManager.CurrentLevel.AddEntity(new TileEntityBombermanExplosion(explosionDamage, explosionPower, levelManager.CurrentLevel, levelManager), (XCenter >> Level.TILESHIFT) * Tile.TILESIZE, ((YCenter >> Level.TILESHIFT) + i + 1) * Tile.TILESIZE);
                    hasSucseededToExplodeDown[i] = true;
                }
                else
                    hasSucseededToExplodeDown[i] = false;

                if (!levelManager.CurrentLevel.GetTile(XCenter >> Level.TILESHIFT, (YCenter >> Level.TILESHIFT) - i - 1).IsSolid(this) && (i == 0 ? true : hasSucseededToExplodeUp[i - 1]))
                {
                    levelManager.CurrentLevel.AddEntity(new TileEntityBombermanExplosion(explosionDamage, explosionPower, levelManager.CurrentLevel, levelManager), (XCenter >> Level.TILESHIFT) * Tile.TILESIZE, ((YCenter >> Level.TILESHIFT) - i - 1) * Tile.TILESIZE);
                    hasSucseededToExplodeUp[i] = true;
                }
                else
                    hasSucseededToExplodeUp[i] = false;
            }

            if (!levelManager.CurrentLevel.GetTile(XCenter >> Level.TILESHIFT, YCenter >> Level.TILESHIFT).IsSolid(this))
                levelManager.CurrentLevel.AddEntity(new TileEntityBombermanExplosion(explosionDamage, explosionPower, levelManager.CurrentLevel, levelManager), (XCenter >> Level.TILESHIFT) * Tile.TILESIZE, (YCenter >> Level.TILESHIFT) * Tile.TILESIZE);
        }

        /// <summary>
        /// Updates this entity.
        /// </summary>
        public override void Update()
        {
            animation.UpdateStep();
        }

        /// <summary>
        /// Renders this entity.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer this entity gets drawn to.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y, animation.CurrentSpriteID, animation.SpriteSheet, Color.White, layer);
        }
    }
}
