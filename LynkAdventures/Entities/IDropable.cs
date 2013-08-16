
namespace LynkAdventures.Entities
{

    /// @author Denis Zhidkikh
    /// @version 6.4.2013
    /// <summary>
    /// An iterface for item that can be dropped.
    /// </summary>
    public interface IDropable
    {
        /// <summary>
        /// Drops the item.
        /// </summary>
        /// <param name="xVelocity">Velocity on X plane.</param>
        /// <param name="height">The height from which the item drops.</param>
        void Drop(int xVelocity, float height);
    }
}
