using StardewValley;
using StardewValley.BellsAndWhistles;
using Microsoft.Xna.Framework.Graphics;
using CropCalculator.Elements;
using StardewValley.Menus;

namespace CropCalculator.CropCalculator.Elements
{
    /// <summary>
    /// Menu title element.
    /// </summary>
    internal class MenuTitleElement : MenuElement
    {
        /// <summary>
        /// Instantiates a new title.
        /// </summary>
        /// <param name="label">Title text.</param>
        /// <param name="index">Index of the element.</param>
        public MenuTitleElement(
            string label,
            int index = 0
        ) : base (
            label,
            index
        )
        {
        }

        /// <summary>
        /// Draws the current element.
        /// </summary>
        /// <param name="batch">Current <see cref="SpriteBatch"/>.</param>
        /// <param name="slotX">X of slot to draw in.</param>
        /// <param name="slotY">Y of slot to draw in.</param>
        public override void draw(
            SpriteBatch batch,
            int slotX,
            int slotY,
            IClickableMenu? context = null
        )
        {
            SpriteText.drawString(
                batch,
                _label,
                slotX + this.bounds.X + 32,
                slotY + this.bounds.Y + Game1.pixelZoom * 3,
                999,
                -1,
                999,
                1,
                0.1f
            );
        }
    }
}
