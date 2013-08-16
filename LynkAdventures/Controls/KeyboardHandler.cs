using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LynkAdventures.Controls
{
    /// @author Denis Zhdikikh
    /// @version 26.4.2013
    /// <summary>
    /// A class representing a keyboard key.
    /// </summary>
    public class Key
    {
        private bool nextState = false, wasDown = false, isDown = false;

        /// <summary>
        /// Gets a value indicating whether the key has been pressed, but not yet released or pressed again.
        /// </summary>
        /// <value>
        /// <c>true</c> if the key has been pressed, but not yet released or pressed again; otherwise, <c>false</c>.
        /// </value>
        public bool IsPressed { get { return !wasDown && isDown; } }

        /// <summary>
        /// Gets a value indicating whether the key is currently held down.
        /// </summary>
        /// <value>
        /// <c>true</c> if the key is currently held down; otherwise, <c>false</c>.
        /// </value>
        public bool IsDown { get { return isDown; } }

        /// <summary>
        /// Updates the key.
        /// </summary>
        internal void Update()
        {
            wasDown = isDown;
            isDown = nextState;
        }

        /// <summary>
        /// Schedule the next state to be triggered on the next Update.
        /// </summary>
        /// <param name="state">The state of the key. <c>true</c> if pressed; otherwise <c>false</c>.</param>
        internal void SetState(bool state)
        {
            nextState = state;
        }
    }

    /// @author Denis Zhdikikh
    /// @version 26.4.2013
    /// <summary>
    /// Custom keyboard handler for the game.
    /// </summary>
    /// <remarks>
    /// Only one instance of the class should be created in order for the handler to work properly.
    /// All registering and updating methods can be used only in the initialized instance. All the state checking (IsKeyDown, IsKeyPressed), 
    /// however, is implemented in static methods.
    /// </remarks>
    public class KeyboardHandler
    {
        private static Dictionary<Keys, Key> registeredKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardHandler"/> class.
        /// </summary>
        public KeyboardHandler()
        {
            registeredKeys = new Dictionary<Keys, Key>();
        }

        /// <summary>
        /// Updates all the registered keys.
        /// </summary>
        public void Update()
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (KeyValuePair<Keys, Key> key in registeredKeys)
            {
                if (currentState.IsKeyDown(key.Key))
                    key.Value.SetState(true);
                else
                    key.Value.SetState(false);

                key.Value.Update();
            }
        }

        /// <summary>
        /// Registers the key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RegisterKey(Keys key)
        {
            registeredKeys.Add(key, new Key());
        }

        /// <summary>
        /// Determines whether the key has been pressed, but not yet released or pressed again.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// <c>true</c> if the key has been pressed, but not yet released or pressed again; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(Keys key)
        {
            Key k;
            bool gotKey = registeredKeys.TryGetValue(key, out k);
            if (!gotKey) return false;
            return k.IsPressed;
        }

        /// <summary>
        /// Determines whether the key is currently held down.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the key is currently held down; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyDown(Keys key)
        {
            Key k;
            bool gotKey = registeredKeys.TryGetValue(key, out k);
            if (!gotKey) return false;
            return k.IsDown;
        }
    }
}
