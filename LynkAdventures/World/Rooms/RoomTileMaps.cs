using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LynkAdventures.World.Rooms
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// Room map resources.
    /// </summary>
    public class RoomTileMaps
    {
        public static Texture2D ROOM_BOMBERMAN { get; private set; }

        /// <summary>
        /// Loads the rooms.
        /// </summary>
        /// <param name="manager">The content manager.</param>
        public static void LoadRooms(ContentManager manager)
        {
            ROOM_BOMBERMAN = manager.Load<Texture2D>("Rooms/bomberman");
        }
    }
}
