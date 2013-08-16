using LynkAdventures.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LynkAdventures.Gui
{
    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// A manager class that is responsible for drawing and updating GUIs.
    /// </summary>
    public class GuiManager
    {
        private Renderer renderer;
        private GraphicsDevice device;
        private ContentManager manager;
        private List<Gui> loadedGuis = new List<Gui>();
        private Gui activeGui;

        /// <summary>
        /// Gets a value indicating whether there is an active GUI (has update priority over other GUIs).
        /// </summary>
        /// <value>
        /// <c>true</c> if there is an active GUI; otherwise, <c>false</c>.
        /// </value>
        public bool HasActiveGui { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiManager"/>.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="device">The graphics device.</param>
        /// <param name="contentManager">The content manager.</param>
        public GuiManager(Renderer renderer, GraphicsDevice device, ContentManager contentManager)
        {
            this.renderer = renderer;
            this.device = device;
            this.manager = contentManager;
            HasActiveGui = false;
            InitializeContent();
        }

        /// <summary>
        /// Gets or sets the current active GUI.
        /// </summary>
        /// <value>
        /// The active GUI.
        /// </value>
        public Gui ActiveGui
        {
            get { return activeGui; }
            set
            {
                HasActiveGui = value != null;
                activeGui = value;
            }
        }

        /// <summary>
        /// Initializes the content and resources.
        /// </summary>
        public void InitializeContent()
        {
            GuiResources.InitializeResources(manager);
        }

        /// <summary>
        /// Called when a registered GUI is closing.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        public void OnRegisteredGuiClosed(Gui gui)
        {
            if (gui == ActiveGui)
                ActiveGui = null;
        }

        /// <summary>
        /// Loads as active GUI.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        public void LoadAsActiveGui(Gui gui)
        {
            if (!gui.IsInitialized)
                gui.Init();

            gui.Activate(true);
        }

        /// <summary>
        /// Registers and loads the GUI.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        /// <param name="activate">if set to <c>true</c> set as active.</param>
        /// <returns><c>true</c>, if was registered sucsessfuly; otherwise <c>false</c>.</returns>
        public bool LoadGui(Gui gui, bool activate = true)
        {
            if (loadedGuis.Contains(gui)) return false;
            if (!gui.IsInitialized)
                gui.Init();

            loadedGuis.Add(gui);
            if (activate)
                gui.Activate();
            return true;
        }

        /// <summary>
        /// Removes the GUI.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        /// <returns><c>true</c>, if gets sucsessfully removed, <c>false</c> if the GUI is not registered in this manager.</returns>
        public bool RemoveGui(Gui gui)
        {
            if (gui == ActiveGui)
                ActiveGui = null;
            if (!loadedGuis.Contains(gui)) return false;
            gui.OnRemove();
            loadedGuis.Remove(gui);
            return true;
        }

        /// <summary>
        /// Updates all the guis, if there is no active GUI; otherwise updates only the active GUI.
        /// </summary>
        public void Update()
        {
            if (HasActiveGui)
                ActiveGui.Update();

            for (int i = 0; i < loadedGuis.Count; i++)
            {
                Gui gui = loadedGuis[i];
                if (gui.IsActive)
                    gui.Update();
            }
        }

        /// <summary>
        /// Renders all activated GUIs.
        /// </summary>
        public void RenderGuis()
        {
            if (HasActiveGui)
                ActiveGui.Render(renderer, RenderLayer.LAYER_GUI);

            float[] renderLayers = BuildDepthValues();
            for (int i = 0; i < loadedGuis.Count; i++)
            {
                Gui gui = loadedGuis[i];
                if (gui.IsActive)
                    gui.Render(renderer, renderLayers[i]);
            }
        }

        /// <summary>
        /// Helper method to build depth values for rendering in the right order.
        /// </summary>
        /// <returns>An array of floats with depth values in the same order as the list of registered GUIs.</returns>
        protected float[] BuildDepthValues()
        {
            List<float> result = new List<float>();
            float farthestDepth = RenderLayer.LAYER_GUI + 0.2F;
            float nearestDepth = RenderLayer.LAYER_GUI;
            float step = (farthestDepth - nearestDepth) / loadedGuis.Count;
            float currentStep = farthestDepth;
            foreach (Gui gui in loadedGuis)
            {
                result.Add(currentStep);
                currentStep -= step;
            }

            return result.ToArray();
        }


    }
}
