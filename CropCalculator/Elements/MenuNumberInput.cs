using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator.CropCalculator.Elements
{
    internal class MenuNumberInput : OptionsTextEntry
    {
        public MenuNumberInput(
            string label,
            int index,
            int x = -1,
            int y = -1
        ): base (
            label,
            index,
            x,
            y
        )
        {
            this.textBox = new MenuNumberInputTextBox(
                Game1.content.Load<Texture2D>("LooseSprites\\textBox"),
                null,
                Game1.smallFont,
                Color.Black
            );
            this.textBox.Width = base.bounds.Width;
        }
    }
}
