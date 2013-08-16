using LynkAdventures.Graphics;
using Microsoft.Xna.Framework;

namespace LynkAdventures.Gui
{
    /// @author Denis Zhidkikh
    /// @version 27.4.2013
    /// <summary>
    /// A graphical user interface.
    /// </summary>
    public class Gui
    {
        private bool isRemoved = false;
        private bool isInitialized = false;
        protected Point pos = Point.Zero;

        /// <summary>
        /// Gets or sets width of the GUI.
        /// </summary>
        /// <value>
        /// Width of the GUI.
        /// </value>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets the height of the GUI.
        /// </summary>
        /// <value>
        /// Height of the GUI.
        /// </value>
        public int Height { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this GUI is active (updated and rendered on the screen).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this GUI is active (updated and rendered on the screen); otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the position on the screen.
        /// </summary>
        /// <value>
        /// The position on the screen.
        /// </value>
        public Point Position { get { return pos; } }

        /// <summary>
        /// Gets or sets a value indicating whether this GUI is removed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this GUI is removed; otherwise, <c>false</c>.
        /// </value>
        public bool Removed
        {
            get { return isRemoved; }
            protected set { isRemoved = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this GUI is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this GUI is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return isInitialized; }
            protected set { isInitialized = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gui"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="position">The position on the screen.</param>
        public Gui(int width, int height, Point position)
        {
            Width = width;
            Height = height;
            IsActive = false;
            pos = position;
        }

        /// <summary>
        /// Initializes the GUI.
        /// </summary>
        public virtual void Init()
        {
            IsInitialized = true;
        }

        /// <summary>
        /// Called when this GUI is activated.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Activates this GUI.
        /// </summary>
        /// <param name="asActiveGui">if set to <c>true</c> activate the gui as the active GUI (the only GUI that will be updated).</param>
        public void Activate(bool asActiveGui = false)
        {
            IsActive = true;
            isRemoved = false;
            OnActivated();

            if (asActiveGui)
                Game.GuiManager.ActiveGui = this;
        }

        /// <summary>
        /// Closes the GUI.
        /// </summary>
        /// <param name="remove">if set to <c>true</c> remove this GUI from GuiManager.</param>
        public void Close(bool remove = false)
        {
            IsActive = false;
            isRemoved = remove;
            Game.GuiManager.OnRegisteredGuiClosed(this);
            if (remove)
                Game.GuiManager.RemoveGui(this);
        }

        /// <summary>
        /// Called when this GUI is removed.
        /// </summary>
        public virtual void OnRemove()
        {
        }

        /// <summary>
        /// Updates this GUI.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Renders the GUI.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="layer">The layer.</param>
        public virtual void Render(Renderer renderer, float layer)
        {
        }

    }
}
