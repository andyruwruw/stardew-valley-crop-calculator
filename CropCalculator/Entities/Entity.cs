using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CropCalculator.Enums;
using CropCalculator.Helpers;
using CropCalculator.Primitives;
using CropCalculator.Control;
using CropCalculator.Render;
using CropCalculator.Render.Filters;
using CropCalculator.Render.Filters.Transitions;

namespace CropCalculator.Entities
{
    /// <inheritdoc cref="IEntity"/>
    internal abstract class Entity : IEntity
    {
        /// <summary>
		/// <see cref="IEntity">IEntity's</see> anchor, or position.
		/// </summary>
		protected Vector2 _anchor;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> <see cref="IDrawer"/> for rendering.
        /// </summary>
        protected IDrawer? _drawer;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> entering <see cref="Transition"/>.
        /// </summary>
        protected IFilter _enteringTransition;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> exiting <see cref="Transition"/>.
        /// </summary>
        protected IFilter _exitingTransition;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> perminant <see cref="IFilter">IFilters</see>.
        /// </summary>
        protected IList<IFilter> _filters = new List<IFilter>();

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> unique identifier.
        /// </summary>
        protected string _id;

        /// <summary>
        /// Whether the <see cref="Entity"/> is being hovered by the cursor.
        /// </summary>
        protected bool _isHovered;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> layer depth for rendering.
        /// </summary>
        protected float _layerDepth;

        /// <summary>
        /// Anchor's relation to <see cref="IEntity">IEntity's</see> position.
        /// </summary>
        protected Origin _origin = Origin.CenterCenter;

        /// <summary>
        /// <see cref="IEntity">IEntity's</see> current <see cref="TransitionState"/>.
        /// </summary>
        protected TransitionState _transitionState = TransitionState.Present;

        /// <summary>
		/// <para>Instantiates an <see cref="Entity"/>.</para>
		/// </summary>
		/// <param name="origin">Anchor's relation to <see cref="IEntity">IEntity's</see> position</param>
		/// <param name="anchor"><see cref="IEntity">IEntity's</see> anchor, or position<inheritdoc cref="_anchor"/></param>
		/// <param name="layerDepth"><see cref="IEntity">IEntity's</see> layer depth for rendering</param>
		/// <param name="enteringTransition"><see cref="IEntity">IEntity's</see> entering <see cref="Transition"/></param>
		/// <param name="exitingTransition"><see cref="IEntity">IEntity's</see> exiting <see cref="Transition"/></param>
		public Entity(
            Origin origin,
            Vector2 anchor,
            float layerDepth,
            IFilter enteringTransition,
            IFilter exitingTransition
        )
        {
            this._anchor = anchor;
            this._drawer = null;
            this._enteringTransition = enteringTransition;
            this._exitingTransition = exitingTransition;
            this._isHovered = false;
            this._id = Guid.NewGuid().ToString();
            this._layerDepth = layerDepth;
            this._origin = origin;

            InitializeFilters();
            InitializeTransitionState();
        }

        /// <inheritdoc cref="IEntity.ClickCallback"/>
		public virtual void ClickCallback()
        {
        }

        /// <inheritdoc cref="IEntity.GetAnchor"/>
        public virtual Vector2 GetAnchor()
        {
            return this._anchor;
        }

        /// <inheritdoc cref="IEntity.GetBoundary"/>
        public virtual IRange GetBoundary()
        {
            return new Primitives.Rectangle(
                GetTopLeft(),
                GetTotalWidth(),
                GetTotalHeight()
            );
        }

        /// <inheritdoc cref="IEntity.GetCenter"/>
        public virtual Vector2 GetCenter()
        {
            if (GetOrigin() == Origin.TopLeft)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / 2, GetTotalHeight() / 2));
            }

            if (GetOrigin() == Origin.TopCenter)
            {
                return Vector2.Add(this._anchor, new Vector2(0, GetTotalHeight() / 2));
            }

            if (GetOrigin() == Origin.TopRight)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / -2, GetTotalHeight() / 2));
            }
                

            if (GetOrigin() == Origin.CenterLeft)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / 2, 0));
            }

            if (GetOrigin() == Origin.CenterCenter)
            {
                return this._anchor;
            }

            if (GetOrigin() == Origin.CenterRight)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / -2, 0));
            }

            if (GetOrigin() == Origin.BottomLeft)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / 2, GetTotalHeight() / -2));
            }
                

            if (GetOrigin() == Origin.BottomCenter)
            {
                return Vector2.Add(this._anchor, new Vector2(0, GetTotalHeight() / -2));
            }

            if (GetOrigin() == Origin.BottomRight)
            {
                return Vector2.Add(this._anchor, new Vector2(GetTotalWidth() / -2, GetTotalHeight() / -2));
            }

            return this._anchor;
        }

        /// <inheritdoc cref="IEntity.GetDrawer"/>
        public virtual IDrawer GetDrawer()
        {
            return this._drawer;
        }

        /// <inheritdoc cref="IEntity.GetEnteringTransition"/>
        public virtual IFilter GetEnteringTransition()
        {
            return this._enteringTransition;
        }

        /// <inheritdoc cref="IEntity.GetExitingTransition"/>
        public virtual IFilter GetExitingTransition()
        {
            return this._exitingTransition;
        }

        /// <inheritdoc cref="IEntity.GetFilters"/>
        public virtual IList<IFilter> GetFilters()
        {
            IList<IFilter> filters = new List<IFilter>();

            if (GetTransitionState() == TransitionState.Exiting && this._exitingTransition != null)
            {
                filters.Add(this._exitingTransition);
            }
                

            if (GetTransitionState() == TransitionState.Entering && this._enteringTransition != null)
            {
                filters.Add(this._enteringTransition);
            }

            foreach (var filter in this._filters)
            {
                filters.Add(filter);
            }

            return filters;
        }

        /// <inheritdoc cref="IEntity.GetId"/>
        public abstract string GetId();

        /// <inheritdoc cref="IEntity.GetLayerDepth"/>
        public virtual float GetLayerDepth()
        {
            return this._layerDepth;
        }

        /// <inheritdoc cref="IEntity.GetOrigin"/>
        public virtual Origin GetOrigin()
        {
            return this._origin;
        }

        /// <inheritdoc cref="IEntity.GetRawBoundary"/>
        public virtual Primitives.Rectangle GetRawBoundary()
        {
            var topLeft = this.GetTopLeft();

            return new Primitives.Rectangle(
                new Vector2(
                    topLeft.X * RenderingHelper.TileScale() + RenderingHelper.AdjustedScreen.Margin.Width(),
                    topLeft.Y * RenderingHelper.TileScale() + RenderingHelper.AdjustedScreen.Margin.Height()
                ),
                this.GetTotalWidth() * RenderingHelper.TileScale(),
                this.GetTotalHeight() * RenderingHelper.TileScale()
            );
        }

        /// <inheritdoc cref="IEntity.GetTopLeft"/>
        public virtual Vector2 GetTopLeft()
        {
            if (GetOrigin() == Origin.TopLeft)
            {
                return this._anchor;
            }

            if (GetOrigin() == Origin.TopCenter)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth() / 2,
                    this._anchor.Y
                );
            }

            if (GetOrigin() == Origin.TopRight)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth(),
                    this._anchor.Y
                );
            }

            if (GetOrigin() == Origin.CenterLeft)
            {
                return new Vector2(
                    this._anchor.X,
                    this._anchor.Y - GetTotalHeight() / 2
                );
            }

            if (GetOrigin() == Origin.CenterCenter)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth() / 2,
                    this._anchor.Y - GetTotalHeight() / 2
                );
            }

            if (GetOrigin() == Origin.CenterRight)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth(),
                    this._anchor.Y - GetTotalHeight() / 2
                );
            }

            if (GetOrigin() == Origin.BottomLeft)
            {
                return new Vector2(
                    this._anchor.X,
                    this._anchor.Y - GetTotalHeight()
                );
            }

            if (GetOrigin() == Origin.BottomCenter)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth() / 2,
                    this._anchor.Y - GetTotalHeight()
                );
            }

            if (GetOrigin() == Origin.BottomRight)
            {
                return new Vector2(
                    this._anchor.X - GetTotalWidth(),
                    this._anchor.Y - GetTotalHeight()
                );
            }

            return this._anchor;
        }

        /// <inheritdoc cref="IEntity.GetTotalWidth"/>
        public abstract float GetTotalHeight();

        /// <inheritdoc cref="IEntity.GetTotalHeight"/>
        public abstract float GetTotalWidth();

        /// <inheritdoc cref="IEntity.GetTransitionState"/>
        public virtual TransitionState GetTransitionState()
        {
            return this._transitionState;
        }

        /// <inheritdoc cref="IEntity.HoverCallback"/>
        public virtual void HoverCallback()
        {
        }

        /// <inheritdoc cref="IEntity.IsHovered"/>
        public bool IsHovered()
        {
            return this._isHovered;
        }

        /// <inheritdoc cref="IEntity.SetAnchor(Vector2)"/>
        public virtual void SetAnchor(Vector2 anchor)
        {
            this._anchor = anchor;
        }

        /// <inheritdoc cref="IEntity.SetEnteringTransition"/>
        public void SetEnteringTransition(IFilter transition)
        {
            this._enteringTransition = transition;
        }

        /// <inheritdoc cref="IEntity.SetExitingTransition"/>
        public void SetExitingTransition(IFilter transition)
        {
            this._exitingTransition = transition;
        }

        /// <inheritdoc cref="IEntity.SetTransitionState(TransitionState, bool)"/>
        public virtual void SetTransitionState(
            TransitionState transitionState,
            bool start = false
        )
        {
            this._transitionState = transitionState;

            if (this._transitionState == TransitionState.Entering
                && this._enteringTransition != null
                && start)
            {
                ((Transition)this._enteringTransition).ResetTransition();
                ((Transition)this._enteringTransition).SetKey(this._id);
                ((Transition)this._enteringTransition).StartFilter();
            }

            if (this._transitionState == TransitionState.Exiting
                && this._exitingTransition != null
                && start)
            {
                ((Transition)this._exitingTransition).ResetTransition();
                ((Transition)this._exitingTransition).SetKey(this._id);
                ((Transition)this._exitingTransition).StartFilter();
            }
        }

        /// <inheritdoc cref="IEntity.Update"/>
        public virtual void Update()
        {
            UpdateTransitionState();
            UpdateHover();
        }

        /// <summary>
        /// Initializes <see cref="_filters"/> as an empty list of <see cref="IFilter">IFilters</see>.
        /// </summary>
        protected virtual void InitializeFilters()
        {
            _filters = new List<IFilter>();
        }

        /// <summary>
        /// Initializes <see cref="_transitionState"/> based on presence of <see cref="_enteringTransition"/>.
        /// </summary>
        protected virtual void InitializeTransitionState()
        {
            if (this._enteringTransition != null)
            {
                this._transitionState = TransitionState.Entering;
                this._enteringTransition.SetKey(this._id);
                ((Transition)this._enteringTransition).StartFilter();
            }
            else
            {
                this._transitionState = TransitionState.Present;
            }
        }

        /// <summary>
        /// Sets <see cref="Entity">Entity's</see> <see cref="_drawer"/> for rendering.
        /// </summary>
        /// <param name="drawer"><see cref="IDrawer"/> for <see cref="Entity"/></param>
        protected virtual void SetDrawer(IDrawer drawer)
        {
            _drawer = drawer;
        }

        /// <summary>
        /// Updates <see cref="Entity">Entity's</see> <see cref="_isHovered"/> based on <see cref="Mouse"/> position.
        /// </summary>
        protected void UpdateHover()
        {
            if (_transitionState == TransitionState.Present && GetRawBoundary().Contains(Control.Mouse.Position))
            {
                if (!_isHovered) HoverCallback();
                _isHovered = true;
            }
            else if (_isHovered)
            {
                _isHovered = false;
            }
        }

        /// <summary>
        /// Updates <see cref="Entity">Entity's</see> <see cref="_transitionState"/> based on current <see cref="Transition"/>.
        /// </summary>
        protected void UpdateTransitionState()
        {
            if (_transitionState == TransitionState.Present || _transitionState == TransitionState.Dead) return;

            if (_transitionState == TransitionState.Entering
                && _enteringTransition != null
                && ((Transition)_enteringTransition).IsFinished())
            {
                _transitionState = TransitionState.Present;
            }  
            else if (_transitionState == TransitionState.Exiting
                && (_exitingTransition != null && ((Transition)_exitingTransition).IsFinished()
                    || _exitingTransition == null))
            {
                _transitionState = TransitionState.Dead;
            }
        }
    }
}

