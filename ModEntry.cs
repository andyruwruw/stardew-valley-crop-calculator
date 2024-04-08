using StardewModdingAPI;
using StardewModdingAPI.Events;
using CropCalculator.Utilities;
using CropCalculator.Helpers;
using CropCalculator.Pages;

namespace CropCalculator
{
    /// <summary>
    /// The mod entry point.
    /// </summary>
    public class ModEntry : Mod
    {
        /// <summary>
        /// Menu page.
        /// </summary>
        private MenuHandler? _menuHandler;

        /// <summary>
        /// The mod entry point, called after the mod is first loaded.
        /// </summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.SetStaticReferences(helper);
            this.SetEventListeners(helper);
            this.CheckForUiInfoSuite2();

            this._menuHandler = new MenuHandler();
        }

        /// <summary>
        /// Sets all required static references from <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="helper"><see cref="IModHelper"/> from <see cref="Entry(IModHelper)"/>.</param>
        private void SetStaticReferences(IModHelper helper)
        {
            // Set static reference to monitor for logging.
            Logger.SetMonitor(Monitor);
            MenuHandler.SetModHelper(helper);
            MenuPage.SetModHelper(helper);
            Translations.SetModHelper(helper);
        }

        /// <summary>
        /// Sets all required event listeners from <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="helper"><see cref="IModHelper"/> from <see cref="Entry(IModHelper)"/>.</param>
        private void SetEventListeners(IModHelper helper)
        {
            helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        /// <summary>
        /// Checks if UI Info suite is present.
        /// </summary>
        private void CheckForUiInfoSuite2()
        {
            GameData.UiInfoSuiteEnabled = Helper.ModRegistry.Get("Annosz.UiInfoSuite2") != null;
        }

        /// <summary>
        /// Called on return to game title page.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event details.</param>
        private void OnReturnedToTitle(
            object? sender,
            ReturnedToTitleEventArgs e
        )
        {
            if (Context.ScreenId != 0)
            {
                return;
            }

            this._menuHandler?.Dispose();
            this._menuHandler = null;
        }

        /// <summary>
        /// On a save loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveLoaded(
            object? sender,
            SaveLoadedEventArgs e
        )
        {
            if (Context.ScreenId != 0)
            {
                return;
            }

            this._menuHandler = new MenuHandler();
        }
    }
}