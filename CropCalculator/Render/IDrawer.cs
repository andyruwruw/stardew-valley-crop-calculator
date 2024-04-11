using CropCalculator.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CropCalculator.Render
{
    /// <summary>
    /// Helper for drawing entities.
    /// </summary>
    internal interface IDrawer
    {
        /// <summary>
        /// Draw an entity.
        /// </summary>
        /// <param name="batch"><see cref="SpriteBatch"/> to be added to.</param>
        /// <param name="overrideDestination">Optional override for destination.</param>
        /// <param name="overrideSource">Optional override for source.</param>
        /// <param name="overrideColor">Optional override for color.</param>
        /// <param name="overrideRotation">Optional override for rotation.</param>
        /// <param name="overrideOrigin">Optional override for origin.</param>
        /// <param name="overrideScale">Optional override for scale.</param>
        /// <param name="overrideEffects">Optional override for effects.</param>
        /// <param name="overrideLayerDepth">Optional override for layer depth.</param>
        void Draw(
            SpriteBatch batch,
            Vector2? overrideDestination = null,
            Rectangle? overrideSource = null,
            Color? overrideColor = null,
            float? overrideRotation = null,
            Vector2? overrideOrigin = null,
            float? overrideScale = null,
            SpriteEffects? overrideEffects = null,
            float? overrideLayerDepth = null
        );

        /// <summary>
        /// Returns the entity this drawer draws.
        /// </summary>
        /// <returns><see cref="IEntity"/> drawn.</returns>
        IEntity GetEntity();

        /// <summary>
        /// Whether this entity should be drawn.
        /// </summary>
        bool ShouldDraw();
    }
}
