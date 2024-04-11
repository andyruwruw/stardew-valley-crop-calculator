using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace CropCalculator.Helpers
{
    internal class RenderingHelper
    {
        /// <summary>
		/// Converts <see cref="Vector2"/> coordinates relative to <see cref="AdjustedScreen"/>, to
		/// <see cref="Vector2"/> relative to <see cref="Viewport"/>.
		/// </summary>
		/// <param name="point"><see cref="Vector2"/> relative to <see cref="AdjustedScreen"/></param>
		/// <returns><see cref="Vector2"/> relative to <see cref="Viewport"/></returns>
		public static Vector2 ConvertAdjustedScreenToRaw(Vector2 point)
        {
            return Vector2.Add(
                Vector2.Multiply(
                    point,
                    TileScale()),
                AdjustedScreen.GetNorthWest()
            );
        }

        /// <summary>
        /// Converts <see cref="Vector2"/> coordinates relative to <see cref="Viewport"/>, to <see cref="Vector2"/>
        /// relative to <see cref="AdjustedScreen"/>.
        /// </summary>
        /// <param name="point"><see cref="Vector2"/> relative to <see cref="Viewport"/></param>
        /// <returns><see cref="Vector2"/> relative to <see cref="AdjustedScreen"/></returns>
        public static Vector2 ConvertRawToAdjustedScreen(Vector2 point)
        {
            return Vector2.Divide(
                Vector2.Subtract(
                    point,
                    AdjustedScreen.GetNorthWest()),
                TileScale()
            );
        }

        /// <summary>
        /// Draws the mouse cursor.
        /// </summary>
        public static void DrawMouseCursor()
        {
            if (!Game1.options.hardwareCursor)
            {
                int mouseCursorToRender = Game1.options.gamepadControls ? Game1.mouseCursor + 44 : Game1.mouseCursor;
                Rectangle what = Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, mouseCursorToRender, 16, 16);

                Game1.spriteBatch.Draw(
                  Game1.mouseCursors,
                  new Vector2(Game1.getMouseX(), Game1.getMouseY()),
                  what,
                  Color.White,
                  0.0f,
                  Vector2.Zero,
                  Game1.pixelZoom + Game1.dialogueButtonScale / 150.0f,
                  SpriteEffects.None,
                  1f
                );
            }
        }

        /// <summary>Stardew Valley zoom settings.</summary>
        public static float PixelZoomAdjustement()
        {
            return 1f / Game1.options.zoomLevel;
        }

        /// <summary>Tile size and pixel zoom adjustment.</summary>
        /// <returns>Multiplier value</returns>
        public static float TileScale()
        {
            return 4 * PixelZoomAdjustement();
        }

        /// <summary>Dimensions of the minigame window adjusted with zoom settings and tile scale.</summary>
		public class AdjustedScreen
        {
            /// <summary>Gets center of minigame window adjusted with zoom settings and tile scale.</summary>
            public static Vector2 GetCenter()
            {
                return Vector2.Divide(
                    new Vector2(
                        Width(),
                        Height()
                    ),
                    2
                );
            }

            /// <summary>Gets top-right of minigame window adjusted with zoom settings and tile scale.</summary>
            public static Vector2 GetNorthEast()
            {
                return new Vector2(
                    Viewport.Width() - Margin.Width(),
                    Margin.Height()
                );
            }

            /// <summary>Gets top-left of minigame window adjusted with zoom settings and tile scale.</summary>
            public static Vector2 GetNorthWest()
            {
                return new Vector2(
                    Margin.Width(),
                    Margin.Height()
                );
            }

            /// <summary>Gets bottom-right of minigame window adjusted with zoom settings and tile scale.</summary>
            public static Vector2 GetSouthEast()
            {
                return new Vector2(
                    Viewport.Width() - Margin.Width(),
                    Viewport.Height() - Margin.Height()
                );
            }

            /// <summary>Gets bottom-left of minigame window adjusted with zoom settings and tile scale.</summary>
            public static Vector2 GetSouthWest()
            {
                return new Vector2(
                    Margin.Width(),
                    Viewport.Height() - Margin.Height()
                );
            }

            /// <summary>The height of the minigame window adjusted with zoom settings and tile scale.</summary>
            public static float Height()
            {
                return MenuScreen.Height * TileScale();
            }

            /// <summary>The width of the minigame window adjusted with zoom settings and tile scale.</summary>
            public static float Width()
            {
                return MenuScreen.Width * TileScale();
            }

            /// <summary>Difference between the <see cref="Viewport"/> and <see cref="AdjustedScreen"/>.</summary>
            public class Difference
            {
                /// <summary>Difference in height between the <see cref="Viewport"/> and <see cref="AdjustedScreen"/>.</summary>
                public static float Height()
                {
                    return Viewport.Height() - AdjustedScreen.Height();
                }

                /// <summary>Difference in width between the <see cref="Viewport"/> and <see cref="AdjustedScreen"/>.</summary>
                public static float Width()
                {
                    return Viewport.Width() - AdjustedScreen.Width();
                }
            }

            /// <summary>Space between <see cref="Viewport"/> and <see cref="AdjustedScreen"/>.</summary>
            public class Margin
            {
                /// <summary>Space between top of <see cref="Viewport"/> and top of <see cref="AdjustedScreen"/>.</summary>
                public static float Height()
                {
                    return Difference.Height() / 2;
                }

                /// <summary>Space between left of <see cref="Viewport"/> and left of <see cref="AdjustedScreen"/>.</summary>
                public static float Width()
                {
                    return Difference.Width() / 2;
                }
            }
        }

        /// <summary>Dimensions of the Stardew Valley window.</summary>
		public class Viewport
        {
            /// <summary>Gets center of the Stardew Valley window.</summary>
            public static Vector2 GetCenter()
            {
                return Vector2.Divide(
                    new Vector2(
                        Width(),
                        Height()
                    ),
                    2
                );
            }

            /// <summary>The height of the Stardew Valley window.</summary>
            public static int Height()
            {
                return Game1.game1.localMultiplayerWindow.Height;
            }

            /// <summary>The width of the Stardew Valley window.</summary>
            public static int Width()
            {
                return Game1.game1.localMultiplayerWindow.Width;
            }
        }

        public class MenuScreen
        {
            public static int Width = 300;

            public static int Height = 180;
        }
    }
}
