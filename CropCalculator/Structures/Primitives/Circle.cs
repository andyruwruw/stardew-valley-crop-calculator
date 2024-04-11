using CropCalculator.Helpers;
using CropCalculator.Primitives.Geometry;
using Microsoft.Xna.Framework;

namespace CropCalculator.Primitives
{
    /// <summary>
    /// Simple circle class
    /// </summary>
    internal class Circle : IRange
    {
        /// <summary>
        /// <see cref="Vector2"/> of the center of the <see cref="Circle"/>
        /// </summary>
        protected Vector2 _center;

        /// <summary>
        /// Radius of the <see cref="Circle"/>
        /// </summary>
        protected float _radius;

        /// <summary>
        /// Instantiates a new circle.
        /// </summary>
        /// <param name="center">Center of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        public Circle(
            Vector2 center,
            float radius
        )
        {
            this._center = center;
            this._radius = radius;
        }

        /// <inheritdoc cref="IRange.Contains(Vector2)"/>
        public bool Contains(Vector2 point)
        {
            return VectorHelper.Pythagorean(
                point,
                this._center
            ) <= this._radius;
        }

        /// <inheritdoc cref="IRange.GetCenter"/>
        public Vector2 GetCenter()
        {
            return this._center;
        }

        /// <summary>
        /// Returns the radius of the <see cref="Circle"/>
        /// </summary>
        /// <returns>Radius of <see cref="Circle"/></returns>
        public float GetRadius()
        {
            return this._radius;
        }

        /// <inheritdoc cref="IRange.Intersects(IRange)"/>
        public bool Intersects(IRange other)
        {
            return IntersectionHelper.IsIntersecting(
                this,
                other
            );
        }

        /// <summary>
        /// Sets the center of the <see cref="Circle"/>
        /// </summary>
        /// <param name="center"><see cref="Vector2"/> of new center</param>
        public void SetCenter(Vector2 center)
        {
            this._center = center;
        }

        /// <summary>
        /// Sets the radius of the <see cref="Circle"/>
        /// </summary>
        /// <param name="radius">New radius</param>
        public void SetRadius(float radius)
        {
            this._radius = radius;
        }
    }
}
