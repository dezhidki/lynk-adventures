using System;

namespace LynkAdventures.Graphics
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// A helper class for animations and timing.
    /// </summary>
    public class AnimationHelper
    {
        private int updatesPerStep = 1;
        private int currentUpdate = 0;
        private int maxSpriteID;
        private int spriteSheet;
        private int currentSprite = 0;
        private bool isRepeated = true;
        private bool isComplete = false;
        private bool isPaused = false;

        /// <summary>
        /// Occurs when animation stops.
        /// </summary>
        public event Action AnimationReady;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationHelper"/>.
        /// </summary>
        /// <param name="spriteSheetID">The sprite sheet ID.</param>
        /// <param name="maxSpriteID">The largest sprite ID that can be reached.</param>
        /// <param name="updatesPerStep">The amount of updates needed for a step in animation.</param>
        /// <param name="repeat">if set to <c>true</c>, the animation will repeat until stopped manually.</param>
        /// <param name="currentSprite">The current sprite ID.</param>
        public AnimationHelper(int spriteSheetID, int maxSpriteID, int updatesPerStep = 1, bool repeat = false, int currentSprite = 0)
        {
            this.updatesPerStep = updatesPerStep;
            this.currentSprite = currentSprite;
            this.spriteSheet = spriteSheetID;
            this.maxSpriteID = maxSpriteID;
            isRepeated = repeat;
        }

        /// <summary>
        /// Gets the current sprite ID (current frame).
        /// </summary>
        /// <value>
        /// The current sprite ID (current frame).
        /// </value>
        public int CurrentSpriteID { get { return currentSprite; } }

        /// <summary>
        /// Gets the sprite sheet ID.
        /// </summary>
        /// <value>
        /// The sprite sheet ID.
        /// </value>
        public int SpriteSheet { get { return spriteSheet; } }

        /// <summary>
        /// Gets a value indicating whether the animation is not repeated and it has reached the largest sprite ID.
        /// </summary>
        /// <value>
        /// <c>true</c> if the animation is not repeated and it has reached the largest sprite ID; otherwise, <c>false</c>.
        /// </value>
        public bool HasFinished { get { return isComplete; } }

        /// <summary>
        /// Updates animation by one step.
        /// </summary>
        public void UpdateStep()
        {
            if (isComplete || isPaused) return;

            currentUpdate++;
            if (currentUpdate >= updatesPerStep)
            {
                currentUpdate = 0;
                currentSprite++;
                if (currentSprite > maxSpriteID)
                {
                    if (!isRepeated)
                    {
                        StopAnimation();
                        if (AnimationReady != null)
                            AnimationReady();
                        return;
                    }
                    currentSprite = 0;
                }
            }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        /// <param name="spriteID">The sprite ID to begin from. Default: 0.</param>
        public void StartAnimation(int spriteID = 0)
        {
            Reset(spriteID);
            isPaused = false;
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void StopAnimation()
        {
            isPaused = true;
        }

        /// <summary>
        /// Resets the specified sprite ID. Doesn't start, if paused.
        /// </summary>
        /// <param name="spriteID">The sprite ID to continue from.</param>
        public void Reset(int spriteID = 0)
        {
            currentUpdate = 0;
            currentSprite = spriteID;

        }

        /// <summary>
        /// Restarts the specified current sprite.
        /// </summary>
        /// <param name="currentSprite">The sprite ID to begin from.</param>
        public void Restart(int currentSprite = 0)
        {
            Reset(currentSprite);
            isComplete = false;
            isPaused = false;
        }
    }
}
