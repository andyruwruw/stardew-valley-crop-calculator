using CropCalculator.Elements;
using CropCalculator.Helpers;
using CropCalculator.Pages;
using CropCalculator.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator
{
    internal class MenuHandler : IDisposable
    {
        /// <summary>
        /// Statick reference to mod helper.
        /// </summary>
        private static IModHelper? ModHelper;

        /// <summary>
        /// Position of tab.
        /// </summary>
        private static int DownNeighborInInventory = 10;

        /// <summary>
        /// Instance of menu page.
        /// </summary>
        private PerScreen<MenuPage?> _menuPage = new();

        /// <summary>
        /// Page button for navigating to menu page.
        /// </summary>
        private readonly PerScreen<PageButton?> _pageButton = new();

        /// <summary>
        /// Elements that should be cleaned up.
        /// </summary>
        private IList<IDisposable> _elements = new List<IDisposable>();

        /// <summary>
        /// Whether to add our tab before a tick.
        /// </summary>
        private bool _addOurTabBeforeTick;

        /// <summary>
        /// Whether to change to our tab after a tick.
        /// </summary>
        private PerScreen<bool> _changeToOurTabAfterTick = new();

        /// <summary>
        /// Last open tab.
        /// </summary>
        private readonly PerScreen<IClickableMenu?> _lastMenu = new();

        /// <summary>
        /// Previous tab number.
        /// </summary>
        private readonly PerScreen<int?> _lastMenuTab = new();

        /// <summary>
        /// Whether the window is resizing.
        /// </summary>
        private bool _windowResizing;

        /// <summary>
        /// What page the menu page is on.
        /// </summary>
        private readonly PerScreen<int?> _pageNumber = new();

        /// <summary>
        /// Gamepad clickable element.
        /// </summary>
        private readonly PerScreen<ClickableComponent?> _clickableComponent = new();

        /// <summary>
        /// For a workaround.
        /// </summary>
        private readonly List<int> _instancesWithOptionsPageOpen = new();

        /// <summary>
        /// Instantiates a page handler.
        /// </summary>
        public MenuHandler()
        {
            if (GameData.UiInfoSuiteEnabled)
            {
                DownNeighborInInventory = 9;
            }

            this._setEventHandlers();
            this._createElements();
        }

        /// <summary>
        /// Dispose of all elements.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable item in _elements)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Returns instance of <see cref="MenuPage"/> if available.
        /// </summary>
        /// <returns>Instance of <see cref="MenuPage"/>.</returns>
        public MenuPage? GetMenuPage()
        {
            return this._menuPage.Value;
        }

        /// <summary>
        /// Creates all menu elements.
        /// </summary>
        private void _createElements()
        {
            _elements = new List<IDisposable>();
        }

        /// <summary>
        /// Sets various event handlers.
        /// </summary>
        private void _setEventHandlers()
        {
            if (MenuHandler.ModHelper != null)
            {
                MenuHandler.ModHelper.Events.Input.ButtonPressed += OnButtonPressed;
                MenuHandler.ModHelper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
                MenuHandler.ModHelper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
                MenuHandler.ModHelper.Events.Display.RenderingActiveMenu += OnRenderingMenu;
                MenuHandler.ModHelper.Events.Display.RenderedActiveMenu += OnRenderedMenu;
                MenuHandler.ModHelper.Events.Display.WindowResized += OnWindowResized;
            }
            
            GameRunner.instance.Window.ClientSizeChanged += OnWindowClientSizeChanged;
        }

        /// <summary>
        /// Handles button click events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu)
            {
                if (e.Button == SButton.RightTrigger && !e.IsSuppressed())
                {
                    if (gameMenu.currentTab + 1 == this._pageNumber.Value && gameMenu.readyToClose())
                    {
                        this.ChangeToOurTab(gameMenu);
                        
                        if (MenuHandler.ModHelper != null)
                        {
                            MenuHandler.ModHelper.Input.Suppress(SButton.RightTrigger);
                        }
                    }
                }

                if ((e.Button == SButton.MouseLeft
                    || e.Button == SButton.ControllerA)
                    && !e.IsSuppressed())
                {
                    if (gameMenu.currentTab == GameMenu.mapTab && gameMenu.lastOpenedNonMapTab == this._pageNumber.Value)
                    {
                        this._changeToOurTabAfterTick.Value = true;
                        gameMenu.lastOpenedNonMapTab = GameMenu.optionsTab;
                    }

                    if (!gameMenu.invisible && !GameMenu.forcePreventClose)
                    {
                        const bool uiScale = true;
                        if (this._clickableComponent.Value != null && this._clickableComponent.Value.containsPoint(Game1.getMouseX(uiScale), Game1.getMouseY(uiScale)) == true &&
                            gameMenu.currentTab != this._pageNumber.Value &&
                            gameMenu.readyToClose())
                        {
                            this.ChangeToOurTab(gameMenu);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles tick updates.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnUpdateTicking(
            object? sender,
            EventArgs e
        )
        {
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
        /// Handles post tick updates.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnUpdateTicked(
            object? sender,
            EventArgs e
        )
        {
            var gameMenu = Game1.activeClickableMenu as GameMenu;

            // The map was closed and the last opened tab was ours
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
                EarlyOnMenuChanged(
                    this._lastMenu.Value,
                    Game1.activeClickableMenu
                );
                this._lastMenu.Value = Game1.activeClickableMenu;
            }

            if (this._lastMenuTab.Value != gameMenu?.currentTab)
            {
                this.OnGameMenuTabChanged(gameMenu);
                this._lastMenuTab.Value = gameMenu?.currentTab;
            }
        }

        /// <summary>
        /// Event handler when Menu is called on <see cref="GameLoop.UpdateTicked"/>.
        /// </summary>
        /// <param name="oldMenu">Old <see cref="IClickableMenu"/> value.</param>
        /// <param name="newMenu">New <see cref="IClickableMenu"/> value.</param>
        private void EarlyOnMenuChanged(
            IClickableMenu? oldMenu,
            IClickableMenu? newMenu
        )
        {
            if (oldMenu is GameMenu oldGameMenu)
            {
                if (this._menuPage.Value != null)
                {
                    oldGameMenu.pages.Remove(this._menuPage.Value);
                    this._menuPage.Value = null;
                }

                if (this._pageButton.Value != null)
                {
                    Logger.Debug("Removed old Page Button");
                    this._pageButton.Value = null;
                }

                this._pageNumber.Value = null;
                this._clickableComponent.Value = null;
            }

            if (newMenu is GameMenu newGameMenu)
            {
                if (this._menuPage.Value == null)
                {
                    this._menuPage.Value = new MenuPage();
                }

                if (this._pageButton.Value == null)
                {
                    Logger.Debug("Created new Page Button");
                    this._pageButton.Value = new PageButton();
                    this._pageButton.Value.SetPageY(newGameMenu.yPositionOnScreen + 16);
                    this._pageButton.Value.SetPageX(this.GetButtonXPosition(newGameMenu));
                }

                IList<IClickableMenu> tabPages = newGameMenu.pages;

                this._pageNumber.Value = tabPages.Count;
                tabPages.Add(this._menuPage.Value);

                this._clickableComponent.Value = new ClickableComponent(
                    new Rectangle(
                        GetButtonXPosition(newGameMenu),
                        newGameMenu.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + 64,
                        64,
                        64
                    ),
                    "crop_calculator",
                    "crop_calculator"
                )
                {
                    myID = 12349,
                    leftNeighborID = 12347,
                    tryDefaultIfNoDownNeighborExists = true,
                    fullyImmutable = true
                };

                ClickableComponent? exitTab = newGameMenu.tabs.Find(tab => tab.myID == 12347);

                if (exitTab != null && this._clickableComponent.Value != null)
                {
                    exitTab.rightNeighborID = this._clickableComponent.Value.myID;
                    AddOurTabToClickableComponents(
                        newGameMenu,
                        this._clickableComponent.Value
                    );
                }
            }
        }

        /// <summary>
        /// Handles menu tab changing.
        /// </summary>
        /// <param name="gameMenu">Current <see cref="GameMenu"/>.</param>
        private void OnGameMenuTabChanged(GameMenu? gameMenu)
        {
            if (gameMenu != null)
            {
                if (this._pageButton.Value != null)
                {
                    this._pageButton.Value.SetPageY(gameMenu.yPositionOnScreen + 16);
                }

                if (this._clickableComponent.Value != null)
                {
                    if (gameMenu.currentTab == GameMenu.inventoryTab)
                    {
                        this._clickableComponent.Value.downNeighborID = MenuHandler.DownNeighborInInventory;
                    }
                    else if (gameMenu.currentTab == GameMenu.exitTab)
                    {
                        this._clickableComponent.Value.downNeighborID = 535;
                    }
                    else
                    {
                        this._clickableComponent.Value.downNeighborID = ClickableComponent.SNAP_TO_DEFAULT;
                    }

                    this.AddOurTabToClickableComponents(
                        gameMenu,
                        this._clickableComponent.Value
                    );
                }
            }
        }

        /// <summary>
        /// On the menu getting rendered.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRenderingMenu(object? sender, RenderingActiveMenuEventArgs e)
        {
            //if (Game1.activeClickableMenu is GameMenu gameMenu)
            //{
            //    this.DrawButton(gameMenu);
            //}
        }

        /// <summary>
        /// On the menu rendered.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRenderedMenu(
            object? sender,
            RenderedActiveMenuEventArgs e
        )
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu
                && !(gameMenu.currentTab == GameMenu.mapTab
                || (gameMenu.GetCurrentPage() is CollectionsPage cPage && cPage.letterviewerSubMenu != null)))
            {
                this.DrawButton(gameMenu);

                MenuHandler.DrawMouseCursor();

                if (!gameMenu.hoverText.Equals(""))
                {
                    IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        gameMenu.hoverText,
                        Game1.smallFont
                    );
                }

                if (this._clickableComponent.Value?.containsPoint(Game1.getMouseX(), Game1.getMouseY()) == true)
                {
                    IClickableMenu.drawHoverText(
                        Game1.spriteBatch,
                        "Crop Calculator",
                        Game1.smallFont
                    );
                }
            }
        }

        /// <summary>
        /// Draws the mouse cursor.
        /// </summary>
        public static void DrawMouseCursor()
        {
            if (!Game1.options.hardwareCursor)
            {
                int mouseCursorToRender = Game1.options.gamepadControls ? Game1.mouseCursor + 44 : Game1.mouseCursor;
                Rectangle what = Game1.getSourceRectForStandardTileSheet(
                    Game1.mouseCursors,
                    mouseCursorToRender,
                    16,
                    16
                );

                Game1.spriteBatch.Draw(
                  Game1.mouseCursors,
                  new Vector2(
                      Game1.getMouseX(),
                      Game1.getMouseY()
                  ),
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

        /// <summary>
        /// On window resize.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnWindowClientSizeChanged(
            object? sender,
            EventArgs e
        )
        {
            this._windowResizing = true;
            GameRunner.instance.ExecuteForInstances(
              instance =>
              {
                  if (Game1.activeClickableMenu is GameMenu gameMenu
                        && gameMenu.currentTab == this._pageNumber.GetValueForScreen(instance.instanceId))
                  {
                      gameMenu.currentTab = GameMenu.optionsTab;
                      this._instancesWithOptionsPageOpen.Add(instance.instanceId);
                  }
              }
            );
        }

        /// <summary>
        /// Called after Display.Rendered and Update.Ticking.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnWindowResized(
            object? sender,
            EventArgs e
        )
        {
            if (this._windowResizing)
            {
                this._windowResizing = false;
                if (this._instancesWithOptionsPageOpen.Count > 0)
                {
                    GameRunner.instance.ExecuteForInstances(
                      instance =>
                      {
                          if (this._instancesWithOptionsPageOpen.Remove(instance.instanceId))
                          {
                              if (Game1.activeClickableMenu is GameMenu gameMenu)
                              {
                                  gameMenu.currentTab = (int)this._pageNumber.GetValueForScreen(instance.instanceId)!;
                              }
                          }
                      }
                    );

                    this._addOurTabBeforeTick = true;
                }
            }
        }

        /// <summary>
        /// Based on <see cref="GameMenu.changeTab" />.
        /// </summary>
        /// <param name="gameMenu">The game menu.</param>
        private void ChangeToOurTab(GameMenu gameMenu)
        {
            this._pageButton.Value?.SetPageY(gameMenu.yPositionOnScreen + 16);
            var modOptionsTabIndex = (int)this._pageNumber.Value!;
            gameMenu.currentTab = modOptionsTabIndex;
            gameMenu.lastOpenedNonMapTab = modOptionsTabIndex;
            gameMenu.initializeUpperRightCloseButton();
            gameMenu.invisible = false;
            Game1.playSound("smallSelect");

            gameMenu.GetCurrentPage().populateClickableComponentList();
            AddOurTabToClickableComponents(
                gameMenu,
                this._clickableComponent.Value!
            );

            gameMenu.setTabNeighborsForCurrentPage();
            if (Game1.options.SnappyMenus)
            {
                gameMenu.snapToDefaultClickableComponent();
            }
        }

        /// <summary>
        /// Add the tab to the current menu page's clickable components.
        /// </summary>
        /// <param name="gameMenu">Game menu.</param>
        /// <param name="clickableComponent">Clickable component.</param>
        private void AddOurTabToClickableComponents(
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
        /// Returns the button X position.
        /// </summary>
        /// <param name="gameMenu">The game menu.</param>
        /// <returns>X value of button.</returns>
        private int GetButtonXPosition(GameMenu gameMenu)
        {
            int offset = 165;

            if (GameData.UiInfoSuiteEnabled)
            {
                offset = 180;
            }

            return gameMenu.xPositionOnScreen + gameMenu.width - offset;
        }

        /// <summary>
        /// Draws our button.
        /// </summary>
        /// <param name="gameMenu">The game menu.</param>
        private void DrawButton(GameMenu gameMenu)
        {
            PageButton button = _pageButton.Value!;
            button.draw(Game1.spriteBatch);
        }

        /// <summary>
        /// Sets static reference to <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="modHelper">Reference to <see cref="IModHelper"/>.</param>
        public static void SetModHelper(IModHelper modHelper)
        {
            MenuHandler.ModHelper = modHelper;
        }
    }
}
