using CropCalculator.Options;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator.Menu
{
    /// <summary>
    /// Handles implementing the calculator page on the menu.
    /// </summary>
    internal class CalculatorPageLoader : MenuPageLoader
    {
        /// <summary>
        /// Stored mod options.
        /// </summary>
        protected ModOptions _options;

        /// <summary>
        /// Instantiates a calculator page loader.
        /// </summary>
        /// <param name="options"><see cref="ModOptions"/> for CropCalculator.</param>
        public CalculatorPageLoader(ModOptions options) : base()
        {
            this._options = options;
        }

        /// <inheritdoc cref="MenuPageLoader.CreateMenuPage"/>
        protected override IClickableMenu CreateMenuPage()
        {
            if (Game1.activeClickableMenu is GameMenu)
            {
                return new CalculatorPage(
                    Game1.activeClickableMenu.xPositionOnScreen,
                    Game1.activeClickableMenu.yPositionOnScreen,
                    Game1.activeClickableMenu.width,
                    Game1.activeClickableMenu.height,
                    this._options
                );
            }
            return null;
        }

        /// <inheritdoc cref="MenuPageLoader.CreatePageButton"/>
        protected override MenuPageButton CreatePageButton()
        {
            if (((GameMenu)Game1.activeClickableMenu).currentTab == GameMenu.inventoryTab)
            {
                Vector2 organizeButton = ((InventoryPage)((GameMenu)Game1.activeClickableMenu).GetCurrentPage()).organizeButton.getVector2();

                return new CalculatorPageButton(
                  (int)Math.Round(organizeButton.X),
                  (int)Math.Round(organizeButton.Y) - 40
                );
            }

            return null;
        }

        /// <inheritdoc cref="MenuPageLoader.GetPageId()"/>
        protected override string GetPageId()
        {
            return "crop-calculator";
        }
    }
}
