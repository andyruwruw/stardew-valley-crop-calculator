using CropCalculator.CropCalculator.Elements;
using CropCalculator.Elements;
using CropCalculator.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace CropCalculator.Pages
{
    internal class MenuPage : IClickableMenu
    {
        /// <summary>
        /// Statick reference to mod helper.
        /// </summary>
        private static IModHelper? ModHelper;

        /// <summary>
        /// Number of visible slots.
        /// </summary>
        public static int VisibleSlots = 7;

        /// <summary>
        /// Width of the menu.
        /// </summary>
        public static int Width = 880;

        /// <summary>
        /// Which option slot is selected.
        /// </summary>
        private int _optionsSlotHeld = -1;

        /// <summary>
        /// Current item index.
        /// </summary>
        private int _currentItemIndex = new();

        /// <summary>
        /// Hover text to display.
        /// </summary>
        private string _hoverText = "";

        /// <summary>
        /// Menu element options.
        /// </summary>
        private readonly IList<OptionsElement> _options = new List<OptionsElement>();

        /// <summary>
        /// Down arrow element.
        /// </summary>
        private ClickableTextureComponent _downArrow;

        /// <summary>
        /// Scroll bar element.
        /// </summary>
        private ClickableTextureComponent _scrollBar;

        /// <summary>
        /// Up arrow element.
        /// </summary>
        private ClickableTextureComponent _upArrow;

        /// <summary>
        /// Scroll bar runner element.
        /// </summary>
        private Rectangle _scrollBarRunner;

        /// <summary>
        /// Whether the user is scrolling.
        /// </summary>
        private bool _isScrolling = false;

        /// <summary>
        /// Visible option slots.
        /// </summary>
        public List<ClickableComponent> _optionSlots = new();

        /// <summary>
        /// Input element.
        /// </summary>
        private OptionsCheckbox enableCrossSeason = new OptionsCheckbox(
            Translations.GetOptionsEnableCrossSeasonLabel(),
            1
        );

        /// <summary>
        /// Input element.
        /// </summary>
        private OptionsCheckbox useAvailableCapital = new OptionsCheckbox(
            Translations.GetOptionsUseAvailableCapitalLabel(),
            2
        );

        private OptionsTextEntry cropCount = new MenuNumberInput(
            Translations.GetOptionsCropCountLabel(),
            3
        );

        private OptionsDropDown produceType = new OptionsDropDown(
            Translations.GetOptionsProduceTypeLabel(),
            4
        );

        private OptionsTextEntry availableEquipment = new MenuNumberInput(
            Translations.GetOptionsAvailableEquipmentLabel(),
            5
        );

        private OptionsDropDown aging = new OptionsDropDown(
            Translations.GetOptionsAgingLabel(),
            6
        );

        private OptionsDropDown profitDisplay = new OptionsDropDown(
            Translations.GetOptionsProfitDisplayLabel(),
            7
        );

        /// <summary>
        /// Instantiates a menu page.
        /// </summary>
        public MenuPage() : base(
            Game1.activeClickableMenu.xPositionOnScreen,
            Game1.activeClickableMenu.yPositionOnScreen + 10,
            MenuPage.Width,
            Game1.activeClickableMenu.height
        )
        {
            this.CreateComponents();

            if (MenuPage.ModHelper != null)
            {
                MenuPage.ModHelper.Events.Display.MenuChanged += this.OnMenuChanged;
            }

            _upArrow = new ClickableTextureComponent(
              new Rectangle(
                xPositionOnScreen + width + Game1.tileSize / 4,
                yPositionOnScreen + Game1.tileSize,
                11 * Game1.pixelZoom,
                12 * Game1.pixelZoom
              ),
              Game1.mouseCursors,
              new Rectangle(421, 459, 11, 12),
              Game1.pixelZoom
            );

            _downArrow = new ClickableTextureComponent(
              new Rectangle(
                _upArrow.bounds.X,
                yPositionOnScreen + height - Game1.tileSize,
                _upArrow.bounds.Width,
                _upArrow.bounds.Height
              ),
              Game1.mouseCursors,
              new Rectangle(421, 472, 11, 12),
              Game1.pixelZoom
            );

            _scrollBar = new ClickableTextureComponent(
              new Rectangle(
                _upArrow.bounds.X + Game1.pixelZoom * 3,
                _upArrow.bounds.Y + _upArrow.bounds.Height + Game1.pixelZoom,
                6 * Game1.pixelZoom,
                10 * Game1.pixelZoom
              ),
              Game1.mouseCursors,
              new Rectangle(435, 463, 6, 10),
              Game1.pixelZoom
            );

            _scrollBarRunner = new Rectangle(
              _scrollBar.bounds.X,
              _scrollBar.bounds.Y,
              _scrollBar.bounds.Width,
              height - Game1.tileSize * 2 - _upArrow.bounds.Height - Game1.pixelZoom * 2
            );
        }

        /// <summary>
        /// Creates components.
        /// </summary>
        private void CreateComponents()
        {
            this.enableCrossSeason.isChecked = true;
            this.useAvailableCapital.isChecked = true;
            this.produceType.dropDownDisplayOptions = (List<string>)Translations.GetOptionsProduceTypeValues();
            this.aging.dropDownDisplayOptions = (List<string>)Translations.GetOptionsAgingValues();
            this.profitDisplay.dropDownDisplayOptions = (List<string>)Translations.GetOptionsProfitDisplayValues();

            this._options.Add(new OptionsElement(Translations.GetModName()));
            this._options.Add(this.enableCrossSeason);
            this._options.Add(this.useAvailableCapital);
            //this._options.Add(this.cropCount);
            this._options.Add(this.produceType);
            //this._options.Add(this.availableEquipment);
            this._options.Add(this.aging);
            this._options.Add(this.profitDisplay);

            for (var i = 0; i < MenuPage.VisibleSlots; ++i)
            {
                var component = new ClickableComponent(
                  new Rectangle(
                    xPositionOnScreen + Game1.tileSize / 4,
                    yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (height - Game1.tileSize * 2) / MenuPage.VisibleSlots,
                    width - Game1.tileSize / 2,
                    (height - Game1.tileSize * 2) / MenuPage.VisibleSlots + Game1.pixelZoom
                  ),
                  i.ToString()
                )
                {
                    myID = i,
                    downNeighborID = i + 1 < MenuPage.VisibleSlots ? i + 1 : ClickableComponent.CUSTOM_SNAP_BEHAVIOR,
                    upNeighborID = i - 1 >= 0 ? i - 1 : ClickableComponent.CUSTOM_SNAP_BEHAVIOR,
                    fullyImmutable = true
                };
                _optionSlots.Add(component);
            }
        }

        /// <summary>
        /// Handles menu change events.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void OnMenuChanged(
            object? sender,
            MenuChangedEventArgs e
        )
        {
            if (e.NewMenu is GameMenu)
            {
                xPositionOnScreen = Game1.activeClickableMenu.xPositionOnScreen;
                yPositionOnScreen = Game1.activeClickableMenu.yPositionOnScreen + 10;
                height = Game1.activeClickableMenu.height;

                for (var i = 0; i < _optionSlots.Count; ++i)
                {
                    ClickableComponent next = _optionSlots[i];

                    next.bounds.X = xPositionOnScreen + Game1.tileSize / 4;
                    next.bounds.Y = yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * (height - Game1.tileSize * 2) / 7;
                    next.bounds.Width = width - Game1.tileSize / 2;
                    next.bounds.Height = (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom;

                    _upArrow.bounds.X = xPositionOnScreen + width + Game1.tileSize / 4;
                    _upArrow.bounds.Y = yPositionOnScreen + Game1.tileSize;
                    _upArrow.bounds.Width = 11 * Game1.pixelZoom;
                    _upArrow.bounds.Height = 12 * Game1.pixelZoom;

                    _downArrow.bounds.X = _upArrow.bounds.X;
                    _downArrow.bounds.Y = yPositionOnScreen + height - Game1.tileSize;
                    _downArrow.bounds.Width = _upArrow.bounds.Width;
                    _downArrow.bounds.Height = _upArrow.bounds.Height;

                    _scrollBar.bounds.X = _upArrow.bounds.X + Game1.pixelZoom * 3;
                    _scrollBar.bounds.Y = _upArrow.bounds.Y + _upArrow.bounds.Height + Game1.pixelZoom;
                    _scrollBar.bounds.Width = 6 * Game1.pixelZoom;
                    _scrollBar.bounds.Height = 10 * Game1.pixelZoom;

                    _scrollBarRunner.X = _scrollBar.bounds.X;
                    _scrollBarRunner.Y = _scrollBar.bounds.Y;
                    _scrollBarRunner.Width = _scrollBar.bounds.Width;
                    _scrollBarRunner.Height = height - Game1.tileSize * 2 - _upArrow.bounds.Height - Game1.pixelZoom * 2;
                }
            }
        }

        /// <summary>
        /// Defines what component to snap to.
        /// </summary>
        public override void snapToDefaultClickableComponent()
        {
            currentlySnappedComponent = getComponentWithID(1);
            snapCursorToCurrentSnappedComponent();
        }

        /// <summary>
        /// Defines a snap behavior.
        /// </summary>
        /// <param name="direction">Which direction.</param>
        /// <param name="oldRegion">From what region.</param>
        /// <param name="oldID">From what Id.</param>
        protected override void customSnapBehavior(
            int direction,
            int oldRegion,
            int oldID
        )
        {
            if (oldID == VisibleSlots - 1 && direction == Game1.down)
            {
                if (_currentItemIndex + VisibleSlots < _options.Count)
                {
                    DownArrowPressed();
                    Game1.playSound("shiny4");
                }
            }
            else if (oldID == 0 && direction == Game1.up)
            {
                if (_currentItemIndex > 0)
                {
                    UpArrowPressed();
                    Game1.playSound("shiny4");
                }
                else
                {
                    // Already at the top, move to the menu tab
                    currentlySnappedComponent = getComponentWithID(12348);
                    if (currentlySnappedComponent != null)
                    {
                        // Set the down neighbor of the tab to the first slot, instead of the default (which is the second slot)
                        currentlySnappedComponent.downNeighborID = 0;
                    }

                    snapCursorToCurrentSnappedComponent();
                }
            }
        }

        /// <summary>
        /// Snaps to a given component.
        /// </summary>
        public override void snapCursorToCurrentSnappedComponent()
        {
            if (currentlySnappedComponent != null)
            {
                OptionsElement? snappedElement = this.GetVisibleOption(currentlySnappedComponent.myID);
                if (snappedElement != null)
                {
                    Point? maybePos = ((MenuElement)snappedElement).GetRelativeSnapPoint(currentlySnappedComponent.bounds);
                    if (maybePos is Point pos)
                    {
                        Game1.setMousePosition(
                          currentlySnappedComponent.bounds.X + pos.X,
                          currentlySnappedComponent.bounds.Y + pos.Y
                        );
                        return;
                    }
                }

                if (currentlySnappedComponent.myID < VisibleSlots)
                {
                    Game1.setMousePosition(
                      currentlySnappedComponent.bounds.Left + 48,
                      currentlySnappedComponent.bounds.Center.Y - 12
                    );
                }
                else
                {
                    base.snapCursorToCurrentSnappedComponent();
                }
            }
        }

        /// <summary>
        /// Sets the scroll bar to current item.
        /// </summary>
        private void SetScrollBarToCurrentItem()
        {
            if (_options.Count > 0)
            {
                _scrollBar.bounds.Y = _scrollBarRunner.Height / Math.Max(1, _options.Count - 7 + 1) * _currentItemIndex +
                                      _upArrow.bounds.Bottom +
                                      Game1.pixelZoom;

                if (_currentItemIndex == _options.Count - 7)
                {
                    _scrollBar.bounds.Y = _downArrow.bounds.Y - _scrollBar.bounds.Height - Game1.pixelZoom;
                }
            }
        }

        /// <summary>
        /// Handles left click being held.
        /// </summary>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="x">Mouse event X value.</param>
        public override void leftClickHeld(
            int x,
            int y
        )
        {
            if (!GameMenu.forcePreventClose)
            {
                base.leftClickHeld(x, y);

                if (_isScrolling)
                {
                    int yBefore = _scrollBar.bounds.Y;

                    _scrollBar.bounds.Y = Math.Min(
                      yPositionOnScreen + height - Game1.tileSize - Game1.pixelZoom * 3 - _scrollBar.bounds.Height,
                      Math.Max(y, yPositionOnScreen + _upArrow.bounds.Height + Game1.pixelZoom * 5)
                    );

                    _currentItemIndex = Math.Max(
                      0,
                      Math.Min(_options.Count - VisibleSlots, _options.Count * (y - _scrollBarRunner.Y) / _scrollBarRunner.Height)
                    );

                    SetScrollBarToCurrentItem();

                    if (yBefore != _scrollBar.bounds.Y)
                    {
                        Game1.playSound("shiny4");
                    }
                }
                else if (_optionsSlotHeld > -1 && _optionsSlotHeld + _currentItemIndex < _options.Count)
                {
                    _options[_currentItemIndex + _optionsSlotHeld].leftClickHeld(
                        x - _optionSlots[_optionsSlotHeld].bounds.X,
                        y - _optionSlots[_optionsSlotHeld].bounds.Y
                    );
                }
            }
        }

        /// <summary>
        /// Recieves a key press event.
        /// </summary>
        /// <param name="Keys">Key pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            if (_optionsSlotHeld > -1 && _optionsSlotHeld + _currentItemIndex < _options.Count)
            {
                _options[_currentItemIndex + _optionsSlotHeld].receiveKeyPress(key);
            }
            else
            {
                // The base implementation handles gamepad movement
                base.receiveKeyPress(key);
            }
        }

        /// <summary>
        /// Handles scroll wheel events.
        /// </summary>
        /// <param name="direction">Which way the scroll wheel was turned.</param>
        public override void receiveScrollWheelAction(int direction)
        {
            if (!GameMenu.forcePreventClose)
            {
                base.receiveScrollWheelAction(direction);

                if (direction > 0 && _currentItemIndex > 0)
                {
                    UpArrowPressed();
                    Game1.playSound("shiny4");
                }
                else if (direction < 0 && _currentItemIndex + VisibleSlots < _options.Count)
                {
                    DownArrowPressed();
                    Game1.playSound("shiny4");
                }
            }
        }

        /// <summary>
        /// Handles left click being released.
        /// </summary>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="x">Mouse event X value.</param>
        public override void releaseLeftClick(
            int x,
            int y
        )
        {
            if (!GameMenu.forcePreventClose)
            {
                base.releaseLeftClick(x, y);

                if (_optionsSlotHeld > -1 && _optionsSlotHeld + _currentItemIndex < _options.Count)
                {
                    Logger.Debug("not holding anymore!");
                    ClickableComponent optionSlot = _optionSlots[_optionsSlotHeld];
                    _options[_currentItemIndex + _optionsSlotHeld].leftClickReleased(
                        x - optionSlot.bounds.X,
                        y - optionSlot.bounds.Y
                    );
                }

                _optionsSlotHeld = -1;
                _isScrolling = false;
            }
        }

        public bool IsDropdownActive()
        {
            if (this._optionsSlotHeld != -1 && this._optionsSlotHeld + this._currentItemIndex < this._options.Count && this._options[this._currentItemIndex + this._optionsSlotHeld] is OptionsDropDown)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles the down arrow being pressed.
        /// </summary>
        private void DownArrowPressed()
        {
            if (!this.IsDropdownActive())
            {
                this.UnsubscribeFromSelectedTextbox();
                _downArrow.scale = _downArrow.baseScale;
                ++_currentItemIndex;
                SetScrollBarToCurrentItem();
            }
        }

        /// <summary>
        /// Handles the up arrow being pressed.
        /// </summary>
        private void UpArrowPressed()
        {
            if (!this.IsDropdownActive())
            {
                this.UnsubscribeFromSelectedTextbox();
                _upArrow.scale = _upArrow.baseScale;
                --_currentItemIndex;
                SetScrollBarToCurrentItem();
            }
        }

        public virtual void UnsubscribeFromSelectedTextbox()
        {
            if (Game1.keyboardDispatcher.Subscriber == null)
            {
                return;
            }
            foreach (OptionsElement option in this._options)
            {
                OptionsTextEntry entry = option as OptionsTextEntry;
                if (entry != null && Game1.keyboardDispatcher.Subscriber == entry.textBox)
                {
                    Game1.keyboardDispatcher.Subscriber = null;
                    break;
                }
            }
        }

        /// <summary>
        /// Handles left click.
        /// </summary>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="playSound">Whether to play a sound.</param>
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (!GameMenu.forcePreventClose)
            {
                if (_downArrow.containsPoint(x, y) && _currentItemIndex < Math.Max(0, _options.Count - 7))
                {
                    DownArrowPressed();
                    Game1.playSound("shwip");
                }
                else if (_upArrow.containsPoint(x, y) && _currentItemIndex > 0)
                {
                    UpArrowPressed();
                    Game1.playSound("shwip");
                }
                else if (_scrollBar.containsPoint(x, y))
                {
                    _isScrolling = true;
                }
                else if (!_downArrow.containsPoint(x, y) &&
                         x > xPositionOnScreen + width &&
                         x < xPositionOnScreen + width + Game1.tileSize * 2 &&
                         y > yPositionOnScreen &&
                         y < yPositionOnScreen + height)
                {
                    // Handle scrollbar click even if the player clicked right next to it, but do not enable scrollbar dragging
                    // NB the leniency area is based on the option page's, so it's too large
                    _isScrolling = true;
                    base.leftClickHeld(x, y);
                    base.releaseLeftClick(x, y);
                }

                _currentItemIndex = Math.Max(0, Math.Min(_options.Count - VisibleSlots, _currentItemIndex));
                this.UnsubscribeFromSelectedTextbox();

                for (var i = 0; i < _optionSlots.Count; ++i)
                {
                    if (this._optionSlots[i].bounds.Contains(x, y)
                        && this._currentItemIndex + i < this._options.Count
                        && this._options[this._currentItemIndex + i].bounds.Contains(x - this._optionSlots[i].bounds.X, y - this._optionSlots[i].bounds.Y))
                    {
                        this._options[this._currentItemIndex + i].receiveLeftClick(x - this._optionSlots[i].bounds.X, y - this._optionSlots[i].bounds.Y);
                        this._optionsSlotHeld = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles right click.
        /// </summary>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="playSound">Whether to play a sound.</param>
        public override void receiveRightClick(
            int x,
            int y,
            bool playSound = true
        )
        {
        }

        /// <summary>
        /// Handles hover event.
        /// </summary>
        /// <param name="x">Mouse event X value.</param>
        /// <param name="x">Mouse event X value.</param>
        public override void performHoverAction(
            int x,
            int y
        )
        {
            if (!GameMenu.forcePreventClose)
            {
                _hoverText = "";
                _upArrow.tryHover(x, y);
                _downArrow.tryHover(x, y);
                _scrollBar.tryHover(x, y);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        /// <param name="batch">Drawing <see cref="SpriteBatch"/>.</param>
        public override void draw(SpriteBatch batch)
        {
            Game1.drawDialogueBox(
                xPositionOnScreen,
                yPositionOnScreen - 10,
                width,
                height,
                false,
                true
            );
            batch.End();

            batch.Begin(
                SpriteSortMode.FrontToBack,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp
            );

            for (var i = 0; i < _optionSlots.Count; ++i)
            {
                if (_currentItemIndex >= 0 && _currentItemIndex + i < _options.Count)
                {
                    _options[_currentItemIndex + i].draw(
                        batch,
                        _optionSlots[i].bounds.X,
                        _optionSlots[i].bounds.Y,
                        this
                    );
                }
            }

            batch.End();

            batch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp
            );

            if (!GameMenu.forcePreventClose)
            {
                _upArrow.draw(batch);
                _downArrow.draw(batch);
                if (_options.Count > 7)
                {
                    drawTextureBox(
                      batch,
                      Game1.mouseCursors,
                      new Rectangle(403, 383, 6, 6),
                      _scrollBarRunner.X,
                      _scrollBarRunner.Y,
                      _scrollBarRunner.Width,
                      _scrollBarRunner.Height,
                      Color.White,
                      Game1.pixelZoom,
                      false
                    );
                    _scrollBar.draw(batch);
                }
            }

            if (_hoverText != "")
            {
                drawHoverText(
                    batch,
                    _hoverText,
                    Game1.smallFont
                );
            }
        }

        /// <summary>Returns the <see cref="OptionsElement" /> that corresponds to the component ID</summary>
        /// <returns>the mod options element, or null if it is invalid</returns>
        private OptionsElement? GetVisibleOption(int componentId)
        {
            if (componentId >= VisibleSlots)
            {
                return null;
            }

            int index = _currentItemIndex + componentId;
            if (0 <= index && index < _options.Count)
            {
                return _options[index];
            }

            return null;
        }

        /// <summary>
        /// Sets static reference to <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="modHelper">Reference to <see cref="IModHelper"/>.</param>
        public static void SetModHelper(IModHelper modHelper)
        {
            MenuPage.ModHelper = modHelper;
        }
    }
}
