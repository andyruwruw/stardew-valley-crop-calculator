using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace CropCalculator.Menu
{
    /// <summary>
    /// Clickable component that opens the menu page.
    /// </summary>
    internal abstract class MenuPageButton
    {
        /// <summary>
        /// Button's clickable component.
        /// </summary>
        protected ClickableComponent? _clickableComponent;

        /// <summary>
        /// Instantiates a new menu page button.
        /// </summary>
        public MenuPageButton()
        {
        }

        /// <summary>
        /// Deletes clickable component.
        /// </summary>
        public void ResetClickableComponent()
        {
            this._clickableComponent = null;
        }

        /// <summary>
        /// Retrieves the name of the clickable component.
        /// </summary>
        public virtual string GetName()
        {
            return "unnamed-component";
        }

        /// <summary>
        /// Retrieves the button's <see cref="ClickableComponent"/>.
        /// </summary>
        public virtual ClickableComponent? GetClickableComponent()
        {
            return this._clickableComponent;
        }

        /// <summary>
        /// Sets a new down neighbor for the clickable component.
        /// </summary>
        /// <param name="id">Neighbor Id.</param>
        public virtual void SetDownNeightborId(int id)
        {
            if (this._clickableComponent != null)
            {
                this._clickableComponent.downNeighborID = id;
            }
        }

        /// <summary>
        /// Sets a new up neighbor for the clickable component.
        /// </summary>
        /// <param name="id">Neighbor Id.</param>
        public virtual void SetUpNeightborId(int id)
        {
            if (this._clickableComponent != null)
            {
                this._clickableComponent.upNeighborID = id;
            }
        }

        /// <summary>
        /// Sets a new right neighbor for the clickable component.
        /// </summary>
        /// <param name="id">Neighbor Id.</param>
        public virtual void SetRightNeightborId(int id)
        {
            if (this._clickableComponent != null)
            {
                this._clickableComponent.rightNeighborID = id;
            }
        }

        /// <summary>
        /// Sets a new left neighbor for the clickable component.
        /// </summary>
        /// <param name="id">Neighbor Id.</param>
        public virtual void SetLeftNeightborId(int id)
        {
            if (this._clickableComponent != null)
            {
                this._clickableComponent.leftNeighborID = id;
            }
        }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="batch"><see cref="SpriteBatch"/> drawn.</param>
        public abstract void Draw(SpriteBatch batch);
    }
}
