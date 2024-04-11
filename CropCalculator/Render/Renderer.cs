using CropCalculator.Entities;

namespace CropCalculator.Render
{
    /// <summary>
    /// Aids in rendering entities on the screen.
    /// </summary>
    internal class Renderer
    {
        /// <summary>
        /// Entities to be rendered.
        /// </summary>
        private IList<IEntity> _entities = new List<IEntity>();
    }
}
