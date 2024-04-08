using CropCalculator.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator.Elements
{
    /// <summary>
    /// General Menu Element.
    /// </summary>
    internal class MenuElement : OptionsElement
    {
        /// <summary>
        /// Default X value.
        /// </summary>
        protected static int DefaultX = 8;

        /// <summary>
        /// Default Y value.
        /// </summary>
        protected static int DefaultY = 4;

        /// <summary>
        /// Default pixel size.
        /// </summary>
        protected static int DefaultPixelSize = 9;

        /// <summary>
        /// Element label.
        /// </summary>
        protected string _label;

        /// <summary>
        /// Index of the element.
        /// </summary>
        protected int _index;

        /// <summary>
        /// Parent <see cref="MenuElement"/> to this element.
        /// </summary>
        protected MenuElement? _parent;

        /// <summary>
        /// Instantiates a new element.
        /// </summary>
        /// <param name="label">Title text.</param>
        /// <param name="index">Index of the element.</param>
        /// <param name="parent">Parent element.</param>
        public MenuElement(
            string label,
            int index = 0,
            MenuElement? parent = null
        ) : base(label)
        {
            int x = MenuElement.DefaultX * Game1.pixelZoom;
            int y = MenuElement.DefaultY * Game1.pixelZoom;
            int width = MenuElement.DefaultPixelSize * Game1.pixelZoom;
            int height = MenuElement.DefaultPixelSize * Game1.pixelZoom;

            if (parent != null)
            {
                Logger.Debug("Parent found");
                x += DefaultX * 2 * Game1.pixelZoom;
            }

            this.bounds = new Rectangle(
                x,
                y,
                width,
                height
            );
            this._index = index;
            this._label = label;
            this._parent = parent;
        }

        ///// <summary>
        ///// Handles left clicks.
        ///// </summary>
        ///// <param name="x">Mouse event X value.</param>
        ///// <param name="y">Mouse event Y value.</param>
        //public virtual void receiveLeftClick(
        //    int x,
        //    int y
        //)
        //{
        //}

        ///// <summary>
        ///// Handles left click held.
        ///// </summary>
        ///// <param name="x">Mouse event X value.</param>
        ///// <param name="y">Mouse event Y value.</param>
        //public virtual void leftClickHeld(
        //    int x,
        //    int y
        //)
        //{
        //}

        ///// <summary>
        ///// Handles left click released.
        ///// </summary>
        ///// <param name="x">Mouse event X value.</param>
        ///// <param name="y">Mouse event Y value.</param>
        //public virtual void leftClickReleased(
        //    int x,
        //    int y
        //)
        //{
        //}

        ///// <summary>
        ///// Handles key presses.
        ///// </summary>
        ///// <param name="key">Key pushed.</param>
        //public virtual void receiveKeyPress(Keys key)
        //{
        //}

        ///// <summary>
        ///// Draws the current element.
        ///// </summary>
        ///// <param name="batch">Current <see cref="SpriteBatch"/>.</param>
        ///// <param name="slotX">X of slot to draw in.</param>
        ///// <param name="slotY">Y of slot to draw in.</param>
        //public virtual void draw(
        //    SpriteBatch batch,
        //    int slotX,
        //    int slotY
        //)
        //{
        //    Utility.drawTextWithShadow(
        //        batch,
        //        _label,
        //        Game1.dialogueFont,
        //        new Vector2(
        //            slotX + this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2,
        //            slotY + this.bounds.Y
        //        ),
        //        Game1.textColor,
        //        1f,
        //        0.1f
        //    );
        //}

        /// <summary>
        /// Provides the snap point for element.
        /// </summary>
        /// <param name="slotBounds">Bounds of slot.</param>
        /// <returns><see cref="Point"/> of snap point.</returns>
        public virtual Point? GetRelativeSnapPoint(Rectangle slotBounds)
        {
            return new Point(
                48,
                slotBounds.Height / 2 - 12
            );
        }
    }
}
