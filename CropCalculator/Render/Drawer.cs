using CropCalculator.Helpers;
using CropCalculator.Entities;
using CropCalculator.Enums;
using CropCalculator.Render.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace CropCalculator.Render
{
    internal abstract class Drawer : IDrawer
    {
        /// <summary>
        /// Local reference to the <see cref="IEntity"/>.
        /// </summary>
        protected IEntity _entity;

        /// <summary>
        /// Instantiates a new <see cref="IDrawer"/> for an <see cref="IEntity"/>.
        /// </summary>
        /// <param name="entity"><see cref="IEntity"/> to be drawn.</param>
        public Drawer(IEntity entity)
        {
            this._entity = entity;
        }

        /// <inheritdoc cref="IDrawer.Draw"/>
        public virtual void Draw(
            SpriteBatch batch,
            Vector2? overrideDestination = null,
            Rectangle? overrideSource = null,
            Color? overrideColor = null,
            float? overrideRotation = null,
            Vector2? overrideOrigin = null,
            float? overrideScale = null,
            SpriteEffects? overrideEffects = null,
            float? overrideLayerDepth = null)
        {
            batch.Draw(
                this.GetTileset(),
                this.GetDestination(overrideDestination),
                this.GetSource(overrideSource),
                this.GetColor(overrideColor),
                this.GetRotation(overrideRotation),
                this.GetOrigin(overrideOrigin),
                this.GetScale(overrideScale),
                this.GetEffects(overrideEffects),
                this.GetLayerDepth(overrideLayerDepth)
            );
        }

        /// <inheritdoc cref="IDrawer.GetEntity"/>
        public IEntity GetEntity()
        {
            return this._entity;
        }

        /// <inheritdoc cref="IDrawer.ShouldDraw"/>
        public virtual bool ShouldDraw()
        {
            return this._entity.GetTransitionState() != TransitionState.Dead;
        }

        /// <summary>
        /// Retrieves the color based on override.
        /// </summary>
        /// <param name="overrideColor">Optional override for color.</param>
        /// <returns>Resolved color.</returns>
        protected Color GetColor(Color? overrideColor = null)
        {
            Color color = overrideColor == null ? this.GetRawColor() : (Color)overrideColor;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                color = filter.ExecuteColor(color);
            }

            return color;
        }

        /// <summary>
        /// Retrieves the destination based on override.
        /// </summary>
        /// <param name="overrideDestination">Optional override for destination.</param>
        /// <returns>Resolved destination.</returns>
        protected virtual Vector2 GetDestination(Vector2? overrideDestination = null)
        {
            Vector2 destination = overrideDestination == null ? this.GetRawDestination() : (Vector2)overrideDestination;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                destination = filter.ExecuteDestination(destination);
            }

            return destination;
        }

        /// <summary>
        /// Retrieves the effects based on override.
        /// </summary>
        /// <param name="overrideEffects">Optional override for effects.</param>
        /// <returns>Resolved effects.</returns>
        protected SpriteEffects GetEffects(SpriteEffects? overrideEffects = null)
        {
            SpriteEffects effects = overrideEffects == null ? this.GetRawEffects() : (SpriteEffects)overrideEffects;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                effects = filter.ExecuteEffects(effects);
            }

            return effects;
        }

        /// <summary>
        /// Retrieves entering transition.
        /// </summary>
        /// <returns>Entering transition of entity.</returns>
        protected IFilter GetEnteringTransition()
        {
            return this._entity.GetEnteringTransition();
        }

        /// <summary>
        /// Retrieves exiting transition.
        /// </summary>
        /// <returns>Exiting transition of entity.</returns>
        protected IFilter GetExitingTransition()
        {
            return this._entity.GetExitingTransition();
        }

        /// <summary>
        /// Retrieves the layer depth based on override.
        /// </summary>
        /// <param name="overrideLayerDepth">Optional override for layer depth.</param>
        /// <returns>Resolved layer depth.</returns>
        protected float GetLayerDepth(float? overrideLayerDepth = null)
        {
            float layerDepth = overrideLayerDepth == null ? this.GetRawLayerDepth() : (float)overrideLayerDepth;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                layerDepth = filter.ExecuteLayerDepth(layerDepth);
            }

            return layerDepth;
        }

        /// <summary>
        /// Retrieves the origin based on override.
        /// </summary>
        /// <param name="overrideOrigin">Optional override for origin.</param>
        /// <returns>Resolved origin.</returns>
        protected Vector2 GetOrigin(Vector2? overrideOrigin = null)
        {
            Vector2 origin = overrideOrigin == null ? this.GetRawOrigin() : (Vector2)overrideOrigin;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                origin = filter.ExecuteOrigin(origin);
            }

            return origin;
        }

        /// <summary>
        /// Retrieves the raw color.
        /// </summary>
        protected virtual Color GetRawColor()
        {
            return Color.White;
        }

        /// <summary>
        /// Gets the default destination.
        /// </summary>
        protected virtual Vector2 GetRawDestination()
        {
            Vector2 topLeft = this._entity.GetTopLeft();
            return new Vector2(
                topLeft.X * RenderingHelper.TileScale() + RenderingHelper.AdjustedScreen.Margin.Width(),
                topLeft.Y * RenderingHelper.TileScale() + RenderingHelper.AdjustedScreen.Margin.Height());
        }

        /// <summary>
        /// Gets default effects.
        /// </summary>
        /// <returns></returns>
        protected virtual SpriteEffects GetRawEffects()
        {
            return SpriteEffects.None;
        }

        /// <summary>
        /// Gets the default layer depth.
        /// </summary>/
        protected virtual float GetRawLayerDepth()
        {
            return this._entity.GetLayerDepth();
        }

        /// <summary>
        /// Gets the default origin.
        /// </summary>
        protected virtual Vector2 GetRawOrigin()
        {
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Gets the default rotation.
        /// </summary>
        protected virtual float GetRawRotation()
        {
            return 0f;
        }

        /// <summary>
        /// Gets the default scale.
        /// </summary>
        protected virtual float GetRawScale()
        {
            return RenderingHelper.TileScale();
        }

        /// <summary>
        /// Gets the default source.
        /// </summary>
        protected abstract Rectangle GetRawSource();

        /// <summary>
        /// Retrieves the rotation based on override.
        /// </summary>
        /// <param name="overrideRotation">Optional override for rotation.</param>
        /// <returns>Resolved rotation.</returns>
        protected float GetRotation(float? overrideRotation = null)
        {
            float rotation = overrideRotation == null ? this.GetRawRotation() : (float)overrideRotation;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                rotation = filter.ExecuteRotation(rotation);
            }

            return rotation;
        }

        /// <summary>
        /// Retrieves the scale based on override.
        /// </summary>
        /// <param name="overrideScale">Optional override for scale.</param>
        /// <returns>Resolved scale.</returns>
        protected float GetScale(float? overrideScale = null)
        {
            float scale = overrideScale == null ? this.GetRawScale() : (float)overrideScale * RenderingHelper.TileScale();

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                scale = filter.ExecuteScale(scale);
            }

            if (scale == 0)
            {
                scale = 0.001f;
            }

            return scale;
        }

        /// <summary>
        /// Retrieves the source based on override.
        /// </summary>
        /// <param name="overrideSource">Optional override for source.</param>
        /// <returns>Resolved source.</returns>
        protected Rectangle GetSource(Rectangle? overrideSource = null)
        {
            Rectangle source = overrideSource == null ? this.GetRawSource() : (Rectangle)overrideSource;

            IList<IFilter> filters = this._entity.GetFilters();

            foreach (IFilter filter in filters)
            {
                source = filter.ExecuteSource(source);
            }

            return source;
        }

        /// <summary>
        /// Grabs the tileset this sprite belongs to.
        /// </summary>
        protected virtual Texture2D GetTileset()
        {
            return Textures.Default;
        }

        /// <summary>
        /// Gets the <see cref="TransitionState"/> of the entity.
        /// </summary>
        /// <returns></returns>
        protected TransitionState GetTransitionState()
        {
            return this._entity.GetTransitionState();
        }

        /// <summary>
        /// Draws a debug point.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="point"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="isRaw"></param>
        public static void DrawDebugPoint(
            SpriteBatch batch,
            Vector2 point,
            Color? color = null,
            int size = 4,
            bool isRaw = false
        )
        {
            Color adjustedColor = color ?? Color.GreenYellow;

            Vector2 adjustedPoint = point;
            if (!isRaw)
            {
                adjustedPoint = RenderingHelper.ConvertAdjustedScreenToRaw(adjustedPoint);
            }

            batch.Draw(
                Game1.staminaRect,
                new Rectangle(
                    (int)Math.Round(adjustedPoint.X - (size / 2)),
                    (int)Math.Round(adjustedPoint.Y - (size / 2)),
                    size,
                    size),
                Game1.staminaRect.Bounds,
                (Color)adjustedColor,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.90000f);
        }

        public static void DrawDebugCircle(SpriteBatch batch, Vector2 center, float radius, Color? color = null, int size = 1, bool isRaw = false)
        {
            int maxLineLength = 3;
            float circumference = (float)(radius * 2 * Math.PI);
            int segments = (int)Math.Floor(circumference / maxLineLength);

            float angle = (float)(2 * Math.PI) / segments;
            Vector2 firstPoint = new Vector2(center.X + radius, center.Y);
            Vector2 lastPoint = firstPoint;

            for (int i = 1; i < segments; i++)
            {
                Vector2 newPoint = Vector2.Add(center, Vector2.Multiply(Vector2.Normalize(VectorHelper.RadiansToVector(angle * i)), radius));
                DrawDebugLine(
                    batch,
                    lastPoint,
                    newPoint,
                    color,
                    size,
                    isRaw);
                lastPoint = newPoint;
            }

            DrawDebugLine(
                batch,
                lastPoint,
                firstPoint,
                color,
                size,
                isRaw);
        }

        public static void DrawDebugLine(
            SpriteBatch batch,
            Vector2 point1,
            Vector2 point2,
            Color? color = null,
            int size = 4,
            bool isRaw = false)
        {
            Color? adjustedColor = color;
            if (color == null)
            {
                adjustedColor = Color.Yellow;
            }

            Vector2 adjustedPoint1 = point1;
            Vector2 adjustedPoint2 = point2;
            if (!isRaw)
            {
                adjustedPoint1 = RenderingHelper.ConvertAdjustedScreenToRaw(point1);
                adjustedPoint2 = RenderingHelper.ConvertAdjustedScreenToRaw(point2);
            }

            Vector2 difference = Vector2.Subtract(adjustedPoint2, adjustedPoint1);

            batch.Draw(
                Game1.staminaRect,
                new Rectangle(
                    (int)Math.Round(adjustedPoint1.X - (size / 2)),
                    (int)Math.Round(adjustedPoint1.Y - (size / 2)),
                    (int)Math.Round(VectorHelper.Pythagorean(
                        adjustedPoint1,
                        adjustedPoint2)),
                    size),
                Game1.staminaRect.Bounds,
                (Color)adjustedColor,
                VectorHelper.VectorToRadians(difference),
                Vector2.Zero,
                SpriteEffects.None,
                1f);
        }
    }
}
