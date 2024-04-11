using CropCalculator.Helpers;
using CropCalculator.Options;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator.Menu
{
    /// <summary>
    /// In-game crop calculator page.
    /// </summary>
    internal class CalculatorPage : IClickableMenu
    {
        /// <summary>
        /// Stored mod options.
        /// </summary>
        protected ModOptions _options;

        /// <summary>
        /// Actual calculations.
        /// </summary>
        private Calculator calculator = new Calculator();

        /// <summary>
        /// The tooltip text being drawn.
        /// </summary>
		public string hoverText = "";

        /// <summary>
        /// Instantiates the calculator page.
        /// </summary>
        /// <param name="x">X value of page.</param>
        /// <param name="y">Y value of page.</param>
        /// <param name="width">Width of page.</param>
        /// <param name="height">Height of page.</param>
        /// <param name="options">Mod options.</param>
        public CalculatorPage(
            int x,
            int y,
            int width,
            int height,
            ModOptions options
        ) : base(
            x,
            y,
            width,
            height
        )
        {
            this._options = options;
        }

        /// <summary>
        /// Handles left clicks.
        /// </summary>
        /// <param name="x">X location of mouse event.</param>
        /// <param name="y">Y location of mouse event.</param>
        /// <param name="playSound">Whether to play a sound.</param>
        public override void receiveLeftClick(
            int x,
            int y,
            bool playSound = true
        )
        {
            if (Game1.activeClickableMenu != null)
            {
                GameMenu gameMenu = (GameMenu)Game1.activeClickableMenu;

                gameMenu.changeTab(gameMenu.lastOpenedNonMapTab);
            }
        }

        /// <summary>
        /// Handles key presses.
        /// </summary>
        /// <param name="key">Key pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            //if (Game1.options.doesInputListContain(Game1.options.mapButton, key) && this.readyToClose())
            //{
            //    base.exitThisMenu();
            //}
            //base.receiveKeyPress(key);
        }

        /// <summary>
        /// Handles hover events.
        /// </summary>
        /// <param name="x">X location of mouse event.</param>
        /// <param name="y">Y location of mouse event.</param>
        public override void performHoverAction(
            int x,
            int y
        )
        {
            this.hoverText = "";
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="batch"><see cref="SpriteBatch"/> to draw to.</param>
        public override void draw(SpriteBatch batch)
        {
        }
    }
}
