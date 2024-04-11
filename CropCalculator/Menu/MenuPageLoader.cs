using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using CropCalculator.Utilities;
using CropCalculator.Helpers;
using CropCalculator.Options;

namespace CropCalculator.Menu
{
    /// <summary>
    /// Handles implementing a new menu page.
    /// </summary>
    internal abstract class MenuPageLoader
    {
        /// <summary>
        /// Statick reference to mod helper.
        /// </summary>
        protected static IModHelper? ModHelper;

        /// <summary>
        /// The menu page loaded.
        /// </summary>
        protected PerScreen<IClickableMenu?> _menuPage = new();

        /// <summary>
        /// Our mod's tab page number.
        /// </summary>
        protected readonly PerScreen<int?> _pageNumber = new();

        /// <summary>
        /// Button for switching to the calculator.
        /// </summary>
        protected readonly PerScreen<MenuPageButton?> _pageButton = new();

        /// <summary>
        /// Track last menu.
        /// </summary>
        protected readonly PerScreen<IClickableMenu?> _lastMenu = new();

        /// <summary>
        /// Track last menu index.
        /// </summary>
        protected readonly PerScreen<int?> _lastMenuIndex = new();

        /// <summary>
        /// For the map page workaround.
        /// </summary>
        protected readonly PerScreen<bool> _changeToOurTabAfterTick = new();

        /// <summary>
        /// Workaround.
        /// </summary>
        protected bool _addOurTabBeforeTick;

        /// <summary>
        /// Signifies the window is resizing.
        /// </summary>
        protected bool _windowResizing;

        /// <summary>
        /// Instantiates a calculator page handler.
        /// </summary>
        public MenuPageLoader()
        {
            this.SetEventHandlers();
        }

        /// <summary>
        /// Creates the menu page.
        /// </summary>
        protected abstract IClickableMenu CreateMenuPage();

        /// <summary>
        /// Creates the menu page button.
        /// </summary>
        protected abstract MenuPageButton CreatePageButton();

        /// <summary>
        /// Sets event handlers.
        /// </summary>
        protected void SetEventHandlers()
        {
            if (MenuPageLoader.ModHelper  != null)
            {
                MenuPageLoader.ModHelper.Events.Input.ButtonPressed += OnButtonPressed;
                MenuPageLoader.ModHelper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
                MenuPageLoader.ModHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
                MenuPageLoader.ModHelper.Events.Display.RenderingActiveMenu += OnRenderingMenu;
                MenuPageLoader.ModHelper.Events.Display.RenderedActiveMenu += OnRenderedMenu;
                MenuPageLoader.ModHelper.Events.Display.WindowResized += OnWindowResized;
            }
            
            GameRunner.instance.Window.ClientSizeChanged += OnWindowClientSizeChanged;
        }

        /// <summary>
        /// Handles button press events.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnButtonPressed(
            object? sender,
            ButtonPressedEventArgs e
        )
        {
            // If we're in the menu.
            if (Game1.activeClickableMenu is GameMenu gameMenu)
            {
                // If the right trigger was pressed.
                if (e.Button == SButton.RightTrigger && !e.IsSuppressed())
                {
                    // And our tab is next.
                    if (gameMenu.currentTab + 1 == this._pageNumber.Value && gameMenu.readyToClose())
                    {
                        // Change to our tab.
                        this.ChangeToOurTab(gameMenu);

                        if (MenuPageLoader.ModHelper != null)
                        {
                            MenuPageLoader.ModHelper.Input.Suppress(SButton.RightTrigger);
                        }
                        
                    }
                }

                // If select was pressed.
                if ((e.Button == SButton.MouseLeft || e.Button == SButton.ControllerA) && !e.IsSuppressed())
                {
                    // Workaround when exiting the map page because it calls GameMenu.changeTab and fails
                    if (gameMenu.currentTab == GameMenu.mapTab
                        && gameMenu.lastOpenedNonMapTab == this._pageNumber.Value)
                    {
                        this._changeToOurTabAfterTick.Value = true;

                        gameMenu.lastOpenedNonMapTab = GameMenu.optionsTab;
                    }

                    // If they clicked in the menu.
                    if (!gameMenu.invisible && !GameMenu.forcePreventClose)
                    {
                        const bool uiScale = true;

                        // If they clicked our tab button.
                        if (this._pageButton.Value != null
                            && this._pageButton.Value.GetClickableComponent() != null
                            && this._pageButton.Value.GetClickableComponent().containsPoint(
                                Game1.getMouseX(uiScale),
                                Game1.getMouseY(uiScale)
                            ) == true
                            && gameMenu.currentTab != this._pageNumber.Value
                            && gameMenu.readyToClose())
                        {
                            this.ChangeToOurTab(gameMenu);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles update ticking.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnUpdateTicking(
            object? sender,
            EventArgs e
        )
        {
            // This is to handle the window resize workaround.
            if (this._addOurTabBeforeTick)
            {
                this._addOurTabBeforeTick = false;

                GameRunner.instance.ExecuteForInstances(
                  instance =>
                  {
                      if (this._lastMenu.Value != Game1.activeClickableMenu)
                      {
                          EarlyOnMenuChanged(
                              this._lastMenu.Value,
                              Game1.activeClickableMenu
                          );
                          this._lastMenu.Value = Game1.activeClickableMenu;
                      }
                  }
                );
            }
        }

        /// <summary>
        /// Handles an update ticked.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnUpdateTicked(
            object? sender,
            EventArgs e
        )
        {
            var gameMenu = Game1.activeClickableMenu as GameMenu;

            // The map was closed and the last opened tab was ours.
            if (this._changeToOurTabAfterTick.Value)
            {
                this._changeToOurTabAfterTick.Value = false;
                if (gameMenu != null)
                {
                    this.ChangeToOurTab(gameMenu);
                }
            }

            if (this._lastMenu.Value != Game1.activeClickableMenu)
            {
                this.EarlyOnMenuChanged(
                    this._lastMenu.Value,
                    Game1.activeClickableMenu
                );
                this._lastMenu.Value = Game1.activeClickableMenu;
            }

            if (this._lastMenuIndex.Value != gameMenu?.currentTab)
            {
                this.OnGameMenuTabChanged(gameMenu);
                this._lastMenuIndex.Value = gameMenu?.currentTab;
            }
        }

        /// <summary>
        /// Early because it is called during GameLoop.UpdateTicked instead of later during Display.MenuChanged.
        /// </summary>
        /// <param name="oldMenu">Old menu call.</param>
        /// <param name="newMenu">New menu call.</param>
        protected void EarlyOnMenuChanged(
            IClickableMenu? oldMenu,
            IClickableMenu? newMenu
        )
        {
            // Remove from old menu
            if (oldMenu is GameMenu oldGameMenu)
            {
                if (this._menuPage.Value != null)
                {
                    oldGameMenu.pages.Remove(this._menuPage.Value);
                    this._menuPage.Value = null;
                }

                if (this._pageButton.Value != null)
                {
                    this._pageButton.Value.ResetClickableComponent();
                    this._pageButton.Value = null;
                }

                this._pageNumber.Value = null;
            }

            // Add to new menu
            if (newMenu is GameMenu newGameMenu)
            {
                // Both modOptions variables require Game1.activeClickableMenu to not be null.
                if (this._menuPage.Value == null)
                {
                    this._menuPage.Value = this.CreateMenuPage();
                }

                if (this._pageButton.Value == null)
                {
                    this._pageButton.Value = this.CreatePageButton();
                }

                List<IClickableMenu> tabPages = newGameMenu.pages;

                this._pageNumber.Value = tabPages.Count;

                tabPages.Add(this._menuPage.Value);
            }
        }

        /// <summary>
        /// Handles the game menu tab changing.
        /// </summary>
        /// <param name="gameMenu">Current game menu.</param>
        protected void OnGameMenuTabChanged(GameMenu? gameMenu)
        {
            if (gameMenu != null)
            {
                if (gameMenu.currentTab == GameMenu.inventoryTab)
                {
                    if (this._pageButton.Value != null)
                    {
                        int organizeId = ((InventoryPage)gameMenu.GetCurrentPage()).organizeButton.myID;

                        this._pageButton.Value.SetDownNeightborId(organizeId);
                    } else
                    {
                        this._pageButton.Value = this.CreatePageButton();

                        AddOurTabToClickableComponents(
                            gameMenu,
                            this._pageButton.Value.GetClickableComponent()
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Handles the menu getting rendered.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnRenderingMenu(
            object? sender,
            RenderingActiveMenuEventArgs e
        )
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu
                && gameMenu.currentTab == GameMenu.inventoryTab
                && this._pageButton.Value != null)
            {
                this._pageButton.Value.Draw(Game1.spriteBatch);
            }
        }

        /// <summary>
        /// Handles the menu having been rendered.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnRenderedMenu(
            object? sender,
            RenderedActiveMenuEventArgs e
        )
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu
                && gameMenu.currentTab == GameMenu.inventoryTab)
            {
                this._pageButton.Value.Draw(Game1.spriteBatch);

                RenderingHelper.DrawMouseCursor();

                // Draw the game menu's hover text again so it displays above our tab
                if (!gameMenu.hoverText.Equals(""))
                {
                    IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        gameMenu.hoverText,
                        Game1.smallFont
                    );
                }

                // Draw our tab's hover text
                if (this._pageButton.Value?.GetClickableComponent().containsPoint(
                    Game1.getMouseX(),
                    Game1.getMouseY()
                ) == true)
                {
                    IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        Translations.GetModName(),
                        Game1.smallFont
                    );
                }
            }
        }

        /// <summary>
        /// Handles the window size changing.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnWindowClientSizeChanged(
            object? sender,
            EventArgs e
        )
        {
            this._windowResizing = true;
        }

        /// <summary>
        /// Handles window resize.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event details.</param>
        protected void OnWindowResized(
            object? sender,
            EventArgs e
        )
        {
            if (this._windowResizing)
            {
                this._windowResizing = false;
            }
        }

        /// <summary>
        /// Based on <see cref="GameMenu.changeTab" />
        /// </summary>
        protected void ChangeToOurTab(GameMenu gameMenu)
        {
            var modOptionsTabIndex = (int)this._pageNumber.Value!;

            gameMenu.currentTab = modOptionsTabIndex;
            gameMenu.lastOpenedNonMapTab = modOptionsTabIndex;
            gameMenu.initializeUpperRightCloseButton();
            gameMenu.invisible = false;

            Game1.playSound("smallSelect");

            gameMenu.GetCurrentPage().populateClickableComponentList();
            AddOurTabToClickableComponents(
                gameMenu,
                this._pageButton.Value!.GetClickableComponent()
            );

            gameMenu.setTabNeighborsForCurrentPage();

            if (Game1.options.SnappyMenus)
            {
                gameMenu.snapToDefaultClickableComponent();
            }
        }

        /// <summary>
        /// Add a clickable component to the game menu.
        /// </summary>
        protected void AddOurTabToClickableComponents(
            GameMenu gameMenu,
            ClickableComponent clickableComponent
        )
        {
            IClickableMenu currentPage = gameMenu.GetCurrentPage()!;

            if (currentPage.allClickableComponents == null)
            {
                currentPage.populateClickableComponentList();
            }

            if (!currentPage.allClickableComponents!.Contains(clickableComponent))
            {
                currentPage.allClickableComponents.Add(clickableComponent);
            }
        }

        /// <summary>
        /// Name of the new menu page.
        /// </summary>
        protected abstract string GetPageId();

        /// <summary>
        /// Sets static reference to <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="modHelper">Reference to <see cref="IModHelper"/>.</param>
        public static void SetModHelper(IModHelper modHelper)
        {
            MenuPageLoader.ModHelper = modHelper;
        }
    }
}
