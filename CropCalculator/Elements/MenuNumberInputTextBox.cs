using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace CropCalculator.CropCalculator.Elements
{
    internal class MenuNumberInputTextBox : TextBox
    {
        public MenuNumberInputTextBox(
            Texture2D textBoxTexture,
            Texture2D caretTexture,
            SpriteFont font,
            Color textColor
        ) : base(
            textBoxTexture,
            caretTexture,
            font,
            textColor
        )
        {
        }

        public override void RecieveTextInput(char inputChar)
        {
            if (!Char.IsDigit(inputChar))
            {
                base.RecieveTextInput(inputChar);
            }
        }

        public override void RecieveTextInput(string text)
        {
            if (!Char.IsDigit(text[0]))
            {
                base.RecieveTextInput(text);
            }
        }
    }
}
