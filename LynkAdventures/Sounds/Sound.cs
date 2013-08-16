using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LynkAdventures.Sounds
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Sound resources.
    /// </summary>
    public class Sound
    {
        public static SoundEffect Hit { get; private set; }
        public static SoundEffect Hurt { get; private set; }
        public static SoundEffect Powerup { get; private set; }
        public static SoundEffect Coin { get; private set; }
        public static SoundEffect Explosion { get; private set; }

        /// <summary>
        /// Loads the sounds.
        /// </summary>
        /// <param name="manager">The content manager.</param>
        public static void LoadSounds(ContentManager manager)
        {
            Sound.Hit = manager.Load<SoundEffect>("Sound/hit");
            Sound.Hurt = manager.Load<SoundEffect>("Sound/hurt");
            Sound.Powerup = manager.Load<SoundEffect>("Sound/powerup");
            Sound.Coin = manager.Load<SoundEffect>("Sound/coin");
            Sound.Explosion = manager.Load<SoundEffect>("Sound/boom");
        }
    }
}
