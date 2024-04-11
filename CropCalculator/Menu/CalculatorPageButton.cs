using Microsoft.Xna.Framework;
using StardewValley.Menus;
using CropCalculator.Render;
using CropCalculator.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace CropCalculator.Menu
{
    /// <summary>
    /// Button that opens the calculator page.
    /// </summary>
    internal class CalculatorPageButton : MenuPageButton
    {
        /// <summary>
        /// Instantiates a new calculator page button.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public CalculatorPageButton(
            int x,
            int y
        ) : base()
        {
            // Situated above the organize button.
            this._clickableComponent = new ClickableTextureComponent(
                this.GetName(),
                new Rectangle(
                    x,
                    y,
                    64,
                    64
                ),
                "",
                Translations.GetModName(),
                Textures.Default,
                Textures.MenuButton,
                4f
            )
            {
                myID = 20001,
                upNeighborID = 898,
                downNeighborID = 106,
                leftNeighborID = 11,
                tryDefaultIfNoDownNeighborExists = true,
                fullyImmutable = true
            };
        }

        /// <inheritdoc cref="MenuPageButton.GetName"/>
        public override string GetName()
        {
            return "crop-calc-button";
        }

        /// <inheritdoc cref="MenuPageButton.Draw(SpriteBatch)"/>
        public override void Draw(SpriteBatch batch)
        {
            ((ClickableTextureComponent)this._clickableComponent).draw(batch);
        }
    }
}
