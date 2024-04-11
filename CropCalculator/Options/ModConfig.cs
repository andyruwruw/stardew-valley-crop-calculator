using StardewModdingAPI.Utilities;
using StardewModdingAPI;

namespace CropCalculator.Options
{
    /// <summary>
    /// General mod config.
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// Whether to show the tab in the menu.
        /// </summary>
        public bool ShowOptionsTabInMenu { get; set; } = true;

        /// <summary>
        /// Default save temporary.
        /// </summary>
        public string ApplyDefaultSettingsFromThisSave { get; set; } = "JohnDoe_123456789";

        /// <summary>
        /// Keybinding for opening calculator.
        /// </summary>
        public KeybindList OpenCalculatorKeybind { get; set; } = KeybindList.ForSingle(SButton.OemTilde);
    }
}
