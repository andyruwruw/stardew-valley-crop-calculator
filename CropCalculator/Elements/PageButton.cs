using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CropCalculator.Utilities;

namespace CropCalculator.Elements
{
    /// <summary>
    /// Button for navigating to Menu Page.
    /// </summary>
    internal class PageButton
    {
        /// <summary>
        /// X position of element on screen.
        /// </summary>
        private int _pageX;

        /// <summary>
        /// Y position of element on screen.
        /// </summary>
        private int _pageY;

        /// <summary>
        /// Sets X position of element on screen.
        /// </summary>
        /// <returns>X position.</returns>
        public void SetPageX(int x)
        {
            this._pageX = x;
        }

        /// <summary>
        /// Sets Y position of element on screen.
        /// </summary>
        /// <returns>Y position.</returns>
        public void SetPageY(int y)
        {
            this._pageY = y;
        }

        /// <summary>
        /// Draws the element.
        /// </summary>
        /// <param name="b"><see cref="SpriteBatch"/> instance.</param>
        public void draw(SpriteBatch b)
        {
            b.Draw(
              Game1.mouseCursors,
              new Vector2(
                  _pageX,
                  _pageY
              ),
              new Rectangle(
                  16,
                  368,
                  16,
                  16
              ),
              Color.White,
              0.0f,
              Vector2.Zero,
              Game1.pixelZoom,
              SpriteEffects.None,
              1f
            );

            b.Draw(
              Game1.objectSpriteSheet,
              new Vector2(
                  _pageX + 8,
                  _pageY + 14
              ),
              new Rectangle(
                  256,
                  304,
                  16,
                  16
              ),
              Color.White,
              0.0f,
              Vector2.Zero,
              3f,
              SpriteEffects.None,
              1f
            );
        }
    }
}
