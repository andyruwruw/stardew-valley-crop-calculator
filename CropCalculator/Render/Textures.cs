using StardewValley;
using Microsoft.Xna.Framework.Graphics;
using CropCalculator.Utilities;
using Microsoft.Xna.Framework;

namespace CropCalculator.Render
{
    /// <summary>
    /// Static reference to textures.
    /// </summary>
    internal class Textures
    {
        /// <summary>
        /// Tile size on tile sheet.
        /// </summary>
		public static int TileSize = 30;

        /// <summary>
        /// Basic tilesheet for mod.
        /// </summary>
        public static Texture2D? Default;

        /// <summary>
        /// Font tilesheet for mod.
        /// </summary>
        public static Texture2D? Font;

        /// <summary>
		/// Loads tilesheets.
		/// </summary>
		public static void LoadTextures()
        {
            Logger.Info("Loading Tilesets");

            Textures.Default = Game1.content.Load<Texture2D>("Minigames\\CropCalculator");
            Textures.Font = Game1.content.Load<Texture2D>("Minigames\\CropCalculatorFont");
        }

        /// <summary>
        /// Tablet textures.
        /// </summary>
        public class Tablet
        {
            /// <summary>
            /// General screen texture.
            /// </summary>
            public static Rectangle SCREEN = new Rectangle();

            /// <summary>
            /// Textures for tablet corners.
            /// </summary>
            public class Corner
            {
                /// <summary>
                /// North east corner texture.
                /// </summary>
                public static Rectangle NORTH_EAST = new Rectangle();

                /// <summary>
                /// North west corner texture.
                /// </summary>
                public static Rectangle NORTH_WEST = new Rectangle();

                /// <summary>
                /// North center east corner texture, or edge of screen..
                /// </summary>
                public static Rectangle NORTH_CENTER_EAST = new Rectangle();

                /// <summary>
                /// South east corner texture.
                /// </summary>
                public static Rectangle SOUTH_EAST = new Rectangle();

                /// <summary>
                /// South west corner texture.
                /// </summary>
                public static Rectangle SOUTH_WEST = new Rectangle();

                /// <summary>
                /// South center east corner texture, or edge of screen..
                /// </summary>
                public static Rectangle SOUTH_CENTER_EAST = new Rectangle();
            }

            /// <summary>
            /// Textures for tablet edges.
            /// </summary>
            public class Edge
            {
                /// <summary>
                /// North edge texture.
                /// </summary>
                public static Rectangle NORTH = new Rectangle();

                /// <summary>
                /// East edge texture.
                /// </summary>
                public static Rectangle EAST = new Rectangle();

                /// <summary>
                /// South edge texture.
                /// </summary>
                public static Rectangle SOUTH = new Rectangle();

                /// <summary>
                /// West edge texture.
                /// </summary>
                public static Rectangle WEST = new Rectangle();

                /// <summary>
                /// Center east edge texture, or edge of screen on east side.
                /// </summary>
                public static Rectangle CENTER_EAST = new Rectangle();
            }
        }

        /// <summary>
        /// Textures for power button.
        /// </summary>
        public class PowerButton
        {
            /// <summary>
            /// Texture for power button off.
            /// </summary>
            public static Rectangle OFF = new Rectangle();

            /// <summary>
            /// Texture for power button on.
            /// </summary>
            public static Rectangle ON = new Rectangle();

            /// <summary>
            /// Textures for transitioning on.
            /// </summary>
            public class Transition
            {
                public static Rectangle FRAME_1 = new Rectangle();

                public static Rectangle FRAME_2 = new Rectangle();

                public static Rectangle FRAME_3 = new Rectangle();

                public static Rectangle FRAME_4 = new Rectangle();

                public static Rectangle FRAME_5 = new Rectangle();

                public static Rectangle FRAME_6 = new Rectangle();
            }
        }

        /// <summary>
        /// Texture for menu button.
        /// </summary>
        public static Rectangle MenuButton = new Rectangle();
    }
}
