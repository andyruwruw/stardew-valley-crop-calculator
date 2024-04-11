using StardewModdingAPI;
using StardewModdingAPI.Events;
using CropCalculator.Utilities;
using CropCalculator.Helpers;
using CropCalculator.Options;
using GenericModConfigMenu;
using CropCalculator.Menu;

namespace CropCalculator
{
    /// <summary>
    /// The mod entry point.
    /// </summary>
    public class ModEntry : Mod
    {
        /// <summary>
        /// Mod config.
        /// </summary>
        public static ModConfig ModConfig;

        /// <summary>
        /// Saved form options.
        /// </summary>
        public static ModOptions ModOptions;

        /// <summary>
        /// Menu page.
        /// </summary>
        private MenuPageLoader? _menuLoader;

        /// <summary>
        /// The mod entry point, called after the mod is first loaded.
        /// </summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            SetStaticReferences(helper);
            SetEventListeners(helper);

            CheckForUiInfoSuite2();

            _menuLoader = new CalculatorPageLoader(ModOptions);
            ModConfig = Helper.ReadConfig<ModConfig>();
        }

        /// <summary>
        /// Sets all required static references from <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="helper"><see cref="IModHelper"/> from <see cref="Entry(IModHelper)"/>.</param>
        private void SetStaticReferences(IModHelper helper)
        {
            // Set static reference to monitor for logging.
            Logger.SetMonitor(Monitor);

            // Set static references to mod helper.
            MenuPageLoader.SetModHelper(helper);
            Translations.SetModHelper(helper);
        }

        /// <summary>
        /// Sets all required event listeners from <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="helper"><see cref="IModHelper"/> from <see cref="Entry(IModHelper)"/>.</param>
        private void SetEventListeners(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.Saved += OnSaved;
            helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
        }

        /// <summary>
        /// Called on game launch.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event details.</param>
        private void OnGameLaunched(
            object? sender,
            GameLaunchedEventArgs e
        )
        {
            GenericModConfigMenuSetup();
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
        }

        /// <summary>
        /// On a save loaded.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event details.</param>
        private void OnSaveLoaded(
            object? sender,
            SaveLoadedEventArgs e
        )
        {
            if (Context.ScreenId != 0)
            {
                return;
            }

            ModOptions = Helper.Data.ReadJsonFile<ModOptions>($"data/{Constants.SaveFolderName}.json") ??
                  Helper.Data.ReadJsonFile<ModOptions>($"data/{ModConfig.ApplyDefaultSettingsFromThisSave}.json") ??
                  new ModOptions();

            _menuLoader = new CalculatorPageLoader(ModOptions);
        }

        /// <summary>
        /// On game save.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event details.</param>
        private void OnSaved(
            object? sender,
            EventArgs e
        )
        {
            if (Context.ScreenId != 0)
            {
                return;
            }

            Helper.Data.WriteJsonFile(
                $"data/{Constants.SaveFolderName}.json",
                ModOptions
            );
        }

        /// <summary>
        /// Checks if UI Info suite is present.
        /// </summary>
        private void CheckForUiInfoSuite2()
        {
            GameData.UiInfoSuiteEnabled = Helper.ModRegistry.Get("Annosz.UiInfoSuite2") != null;
        }

        /// <summary>
        /// Place details in Generic Mod Config Menu.
        /// </summary>
        private void GenericModConfigMenuSetup()
        {
            ISemanticVersion? modVersion = Helper.ModRegistry.Get("spacechase0.GenericModConfigMenu")?.Manifest?.Version;
            var minModVersion = "1.6.0";

            if (modVersion?.IsOlderThan(minModVersion) == true)
            {
                Logger.Info($"Detected Generic Mod Config Menu {modVersion} but expected {minModVersion} or newer. Disabling integration with that mod.");
                return;
            }

            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
            {
                return;
            }

            configMenu.Register(
                ModManifest,
                () => ModConfig = new ModConfig(),
                () => Helper.WriteConfig(ModConfig)
            );

            configMenu.AddBoolOption(
              ModManifest,
              name: () => "Show options in in-game menu",
              tooltip: () => "Enables an extra tab in the in-game menu where you can access the calculator.",
              getValue: () => ModConfig.ShowOptionsTabInMenu,
              setValue: value => ModConfig.ShowOptionsTabInMenu = value
            );

            configMenu.AddTextOption(
              ModManifest,
              name: () => "Apply default settings from this save",
              tooltip: () => "New characters will inherit the settings for the mod from this save file.",
              getValue: () => ModConfig.ApplyDefaultSettingsFromThisSave,
              setValue: value => ModConfig.ApplyDefaultSettingsFromThisSave = value
            );

            configMenu.AddKeybindList(
              ModManifest,
              name: () => "Open calculator keybind",
              tooltip: () => "Opens the calculator tab.",
              getValue: () => ModConfig.OpenCalculatorKeybind,
              setValue: value => ModConfig.OpenCalculatorKeybind = value
            );
        }
    }
}