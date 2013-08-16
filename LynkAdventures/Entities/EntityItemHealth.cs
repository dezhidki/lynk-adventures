using LynkAdventures.BasicPhysicsUtils;
using LynkAdventures.Graphics;
using LynkAdventures.Sounds;
using LynkAdventures.World;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Elämäesine.
    /// </summary>
    public class EntityItemHealth : EntityItem, IDropable
    {
        private int healAmount;
        private float dY = 0.0F;
        private float velocityY = 0.0F, acceleration = 0.0F, accelerationDecrease = 0.0F;
        private int velocityX = 0;
        private bool hasDropMovement = false;

        /// <summary>
        /// Alustaa elämän.
        /// </summary>
        /// <param name="healAmount">Parantumisen määrä.</param>
        /// <param name="manager">Tasojen manageri.</param>
        public EntityItemHealth(int healAmount, LevelManager manager)
            : base(manager)
        {
            this.healAmount = healAmount;
            itemAnimation = new AnimationHelper(GameSpriteSheets.SPRITESHEET_ITEM_HEALTH, 11, 6, true);
        }

        private int xOffs0 = 6 * Game.SCALE, xOffs1 = 5 * Game.SCALE, yOffs = 8 * Game.SCALE;

        /// <summary>
        /// Hakee törmäyslaatikon, jossa olio on. Tarvitaan fysikkaa varten.
        /// </summary>
        /// <returns>Olion törmäyslaatikko. Alkuperäissä metodissa törmäyslaatikko on koko olion tekstuuri.
        /// Voi ylikirjoittaa saadakseen tarkempia arvoja.</returns>
        public override BoundingBox2D GetBoundingBox()
        {
            return new BoundingBox2D(XCenter - xOffs0, YCenter - yOffs, XCenter + xOffs1, YCenter + yOffs, this);
        }

        /// <summary>
        /// Voiko olio ottaa tämän esineen.
        /// </summary>
        /// <param name="ent">Olio.</param>
        /// <returns>True, jos olio voi ottaa tämän esineen.</returns>
        public override bool CanPickUp(Entity ent)
        {
            return ent is EntityPlayer;
        }

        /// <summary>
        /// Poimii esineen maasta.
        /// </summary>
        /// <param name="ent">Olio, joka otti.</param>
        public override void PickUp(Entity ent)
        {
            if (IsDead) return;
            EntityPlayer player = (EntityPlayer)ent;
            if (player.Health == player.MaxHealth) return;

            player.Health += healAmount;
            if (player.Health > player.MaxHealth)
                player.Health = player.MaxHealth;
            Sound.Powerup.Play();

            base.PickUp(ent);
        }

        /// <summary>
        /// Tiputtaa esineen.
        /// </summary>
        /// <param name="xVelocity">Nopeus x -akselissa.</param>
        /// <param name="height">Korkeus, josta tiputetaan.</param>
        public void Drop(int xVelocity, float height)
        {
            dY = height;
            acceleration = -1.0F;
            velocityX = xVelocity;
            accelerationDecrease = 0.1F;
            hasDropMovement = true;
        }

        /// <summary>
        /// Olion päivitys.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (hasDropMovement)
            {
                velocityY += acceleration;
                acceleration += accelerationDecrease;
                dY += velocityY;

                Move(velocityX, 0);

                if (dY >= 0.0F)
                {
                    hasDropMovement = false;
                    dY = 0.0F;
                }
            }
        }

        /// <summary>
        /// Olion piirtometodi.
        /// </summary>
        /// <param name="renderer">Piirtoluokka, jonka kautta piiretään näytölle.</param>
        /// <param name="layer">Syvyystaso, johon oliota piirretään.</param>
        public override void Render(Renderer renderer, float layer)
        {
            renderer.RenderTile(X, Y + dY, itemAnimation.CurrentSpriteID, itemAnimation.SpriteSheet, Color.White, hasDropMovement ? RenderLayer.LAYER_TILE_SOLID - 0.1F : layer);
        }
    }
}
