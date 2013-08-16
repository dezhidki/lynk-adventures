using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.Graphics
{
    /// @author Denis Zhidkikh
    /// @version 23.3.2013
    /// <summary>
    /// List of shaders.
    /// </summary>
    public static class Shaders
    {
        public static int AlphaMasking { get; private set; }
        public static int ModalRect { get; private set; }

        /// <summary>
        /// Loads shaders into the memory.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="renderer">The renderer.</param>
        public static void LoadShaders(ContentManager manager, Renderer renderer)
        {
            AlphaMasking = renderer.RegisterShader(manager.Load<Effect>("alphaMasking"));
            ModalRect = renderer.RegisterShader(manager.Load<Effect>("modalRectangle"));
        }
    }
}
