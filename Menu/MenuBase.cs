using GameMath;
using Overlay.NET.Directx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WindowsInput.Native;

namespace OverlayGameMenu
{
    public class MenuBase
    {
        // Events
        public delegate void _OnOptionToggled(MenuItem item);
        public event _OnOptionToggled OnOptionToggled;

        private List<MenuItem> Options { get; set; } = new List<MenuItem>();
        private List<VirtualKeyCode> keyCodes = new List<VirtualKeyCode>()
        {
            VirtualKeyCode.INSERT,
            VirtualKeyCode.UP,
            VirtualKeyCode.DOWN,
            VirtualKeyCode.LEFT,
            VirtualKeyCode.RIGHT
        };

        private DirectXOverlayWindow OverlayWindow;
        private Direct2DRenderer g;

        private bool _inti;
        private int _lastParent, _selectedIndex;
        private int bodyY, _titleHeight = 24;
        private int halfWidth, safeSpace = 7;
        private int optHeight = 20;

        private MenuItem selectedOpt;

        private D2DSolidColorBrush _backGroundColor, _borderColor, _titleColor, _footerColor, _optOnColor, _optOffColor, _subOptOnColor, _subOptOffColor, _selectedColor, _selectSubColor, _textColor;
        private D2DSolidColorBrush _blackBrush, _greenBrush, _redBrush, _whiteBrush;

        private D2DFont _smalFont, _midFont, _bigFont, _optFont;

        public D2DFont OptFont { set => _optFont = value; }

        public Color BackGroundColor { set => _backGroundColor = g.CreateSolidColorBrush(value); }
        public Color BorderColor { set => _borderColor = g.CreateSolidColorBrush(value); }
        public Color SelectOptColor { set => _selectedColor = g.CreateSolidColorBrush(value); }
        public Color SelectSubColor { set => _selectSubColor = g.CreateSolidColorBrush(value); }
        public Color TextColor { set => _textColor = g.CreateSolidColorBrush(value); }

        public Color OptOnColor { set => _optOnColor = g.CreateSolidColorBrush(value); }
        public Color OptOffColor { set => _optOffColor = g.CreateSolidColorBrush(value); }
        public Color SubOptOnColor { set => _subOptOnColor = g.CreateSolidColorBrush(value); }
        public Color SubOptOffColor { set => _subOptOffColor = g.CreateSolidColorBrush(value); }

        public Color TitleColor { set => _titleColor = g.CreateSolidColorBrush(value); }
        public Color FooterColor { set => _footerColor = g.CreateSolidColorBrush(value); }

        public string Title { get; set; }
        public string Footer { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }

        public MenuBase(DirectXOverlayWindow overlayWindow, bool showByDefault = false)
        {
            Visible = showByDefault;
            OverlayWindow = overlayWindow;
            g = overlayWindow.Graphics;

            halfWidth = Width / 2;

            _inti = false;
            KeyBordManager.KeyDetector(this, keyCodes);
        }

        public void MenuKeyPressed(VirtualKeyCode virtualKey)
        {
            if (virtualKey == VirtualKeyCode.INSERT)
            {
                Visible = !Visible;
                return;
            }

            if (!Visible)
                return;

            switch (virtualKey)
            {
                case VirtualKeyCode.UP:
                    selectedOpt = GetPrevItem();
                    break;

                case VirtualKeyCode.DOWN:
                    selectedOpt = GetNextItem();
                    break;

                case VirtualKeyCode.RIGHT:
                    selectedOpt.NextValue();
                    OnOptionToggled?.Invoke(selectedOpt);
                    break;

                case VirtualKeyCode.LEFT:
                    selectedOpt.PrevValue();
                    OnOptionToggled?.Invoke(selectedOpt);
                    break;
            }
        }
        public void Inti()
        {
            _inti = true;

            _blackBrush = OverlayWindow.Graphics.CreateSolidColorBrush(Color.Black);
            _greenBrush = OverlayWindow.Graphics.CreateSolidColorBrush(Color.Green);
            _redBrush = OverlayWindow.Graphics.CreateSolidColorBrush(Color.Red);
            _whiteBrush = OverlayWindow.Graphics.CreateSolidColorBrush(Color.White);

            _smalFont = OverlayWindow.Graphics.CreateFont("Segoe UI", 14);
            _midFont = OverlayWindow.Graphics.CreateFont("Segoe UI", 16);
            _bigFont = OverlayWindow.Graphics.CreateFont("Segoe UI", 18);

            selectedOpt = Options[0];
        }
        public bool Update()
        {
            if (!_inti || !Visible)
                return false;

            // Title
            DrawTitleBox();

            // Body
            DrawBody();

            // Footer
            DrawFooter();

            return true;
        }

        public void AddOpt(MenuItem Option)
        {
            // Check There item with the same id
            if (Options.Find(i => i.ItemID.ID == Option.ItemID.ID) != null)
                throw new Exception("There is another item with the same id.");

            Option.index = Options.Count;
            if (Option.OptType == OptionType.Expanded && !Option.IsSubOpt)
                _lastParent = Option.index;

            Options.Add(Option);
        }
        public void AddSubOption(MenuItem Option)
        {
            // Check There item with the same id
            if (Options.Find(i => i.ItemID.ID == Option.ItemID.ID) != null)
                throw new Exception("There is another item with the same id.");

            if (Options[_lastParent].OptType != OptionType.Expanded)
                throw new Exception("The Parent not Expanded item !!");

            Option.index = Options.Count;
            Option.IsSubOpt = true;
            Option.Parent = Options[_lastParent];
            Options[_lastParent].ChieldCount += 1;
            Options.Add(Option);
        }

        private MenuItem GetNextItem()
        {
            // Check if there more item
            if (Options.ElementAtOrDefault(selectedOpt.index + 1) != null)
            {
                // if the expanded
                if (Options[_selectedIndex].OptType == OptionType.Expanded)
                {
                    // opt is expanded
                    if (Options[_selectedIndex].OptState != (int)MenuItemTypes.OnOffItem.Off)
                    {
                        _selectedIndex += 1;
                    }
                    else // not expanded
                    {
                        // Check if there more item
                        if (Options.ElementAtOrDefault(selectedOpt.index + Options[_lastParent].ChieldCount + 1) != null)
                        {
                            _selectedIndex += Options[_lastParent].ChieldCount + 1;
                        }
                    }
                }
                else
                {
                    _selectedIndex += 1;
                }
            }

            return Options[_selectedIndex];
        }
        private MenuItem GetPrevItem()
        {
            // Check if there more item
            if (Options.ElementAtOrDefault(selectedOpt.index - 1) != null)
            {
                // Is SubOpt
                if (Options[_selectedIndex - 1].IsSubOpt)
                {
                    // The parent is expanded type
                    if (Options[_selectedIndex - 1].Parent.OptType == OptionType.Expanded)
                    {
                        if (Options[_selectedIndex - 1].Parent.OptState != (int)MenuItemTypes.OnOffItem.Off)
                        {
                            _selectedIndex -= 1;
                        }
                        else
                        {
                            _selectedIndex -= Options[_lastParent].ChieldCount + 1;
                        }
                    }
                }
                else
                {
                    _selectedIndex -= 1;
                }
            }

            return Options[_selectedIndex];
        }

        private void DrawTitleBox()
        {
            Size titleSize = Utils2D.GetDrawingTextSize(Title, "Segoe UI", 14);

            Vector2 p = Utils2D.CenterOfPoint(new Vector2(Width, _titleHeight), new Vector2(titleSize.Width, titleSize.Height));
            int textX = (int)p.X + X + 5;
            int textY = (int)p.Y + Y;

            g.BorderedRectangle(X, Y, Width, _titleHeight, 1, 2, _borderColor, _backGroundColor);
            g.DrawBox2D(X, Y, Width, _titleHeight, 0, _borderColor, _blackBrush);
            g.DrawText(Title, _smalFont, _titleColor, textX, textY);
        }
        private void DrawItem(int x, int y, MenuItem item)
        {
            D2DSolidColorBrush stateColor;
            D2DSolidColorBrush subDotColor;
            string stateText;

            if (item.OptState == (int)MenuItemTypes.OnOffItem.Off)
            {
                stateColor = _optOffColor;
                subDotColor = _subOptOffColor;
                stateText = item.GetValueName();
            }
            else
            {
                stateColor = _optOnColor;
                subDotColor = _subOptOnColor;
                stateText = item.GetValueName();
            }

            // Highlight
            if (item.index == selectedOpt.index)
                g.DrawBox2D(x - safeSpace, y - 2, Width, optHeight, 0, _selectedColor, _selectedColor);

            // Expended Option ??, Option Name
            if (item.OptType == OptionType.Expanded && item.OptState != (int)MenuItemTypes.OnOffItem.Off)
                g.DrawText("≪ " + item.OptName, _optFont, _textColor, x, y - 3);
            else if (item.OptType == OptionType.Expanded && item.OptState != (int)MenuItemTypes.OnOffItem.Off)
                g.DrawText("≫ " + item.OptName, _optFont, _textColor, x, y - 3);
            else
                g.DrawText(item.OptName, _optFont, _textColor, x, y);

            // Option State
            Size nameSize = Utils2D.GetDrawingTextSize(stateText, "Courier New", 14);
            int stateX = (Width - nameSize.Width) + X;
            g.DrawText(stateText, _optFont, stateColor, stateX, y);
            bodyY += optHeight;

            // Draw Childs
            if (item.OptType == OptionType.Expanded && item.OptState != (int)MenuItemTypes.OnOffItem.Off)
            {
                for (int xi = 1; xi < item.ChieldCount + 1; xi++)
                {
                    var subItem = Options[item.index + xi];

                    if (!subItem.IsSubOpt)
                        break;

                    if (subItem.OptState == (int)MenuItemTypes.OnOffItem.Off)
                    {
                        stateColor = _subOptOffColor;
                        subDotColor = _redBrush;
                        stateText = subItem.GetValueName();
                    }
                    else
                    {
                        stateColor = _subOptOnColor;
                        subDotColor = _greenBrush;
                        stateText = subItem.GetValueName();
                    }

                    // Highlight
                    if (subItem.index == selectedOpt.index)
                        g.DrawBox2D(X, bodyY - 1, Width, optHeight, 0, _selectSubColor, _selectSubColor);

                    // Option Dot
                    g.FillCircle(x + (safeSpace * 2), bodyY + (optHeight / 2), 2, subDotColor);

                    // Option Name
                    g.DrawText(subItem.OptName, _optFont, _whiteBrush, x + (safeSpace * 4), bodyY + 1);

                    // Option State
                    nameSize = Utils2D.GetDrawingTextSize(stateText, "Courier New", 14);
                    stateX = (Width - nameSize.Width) + X;
                    g.DrawText(stateText, _optFont, stateColor, stateX, bodyY);

                    bodyY += optHeight;
                }
            }
        }
        private void DrawBody()
        {
            // Calc body heigh
            int bodyHeight = 0;
            bool lastExpandSatetOn = false;
            for (int i = 0; i < Options.Count - 1; i++)
            {
                bodyHeight += 1;

                bool lastExpand = (Options[i].OptType == OptionType.Expanded && !Options[i].IsSubOpt);
                lastExpandSatetOn = Options[i].OptState != (int)MenuItemTypes.OnOffItem.Off;

                if (lastExpand && lastExpandSatetOn)
                {
                    bodyHeight += Options[i].ChieldCount;
                    i += Options[i].ChieldCount;
                }
                else if (lastExpand && !lastExpandSatetOn)
                {
                    i += Options[i].ChieldCount;
                }
            }

            bodyHeight *= optHeight;
            bodyY = Y + _titleHeight + safeSpace;

            g.BorderedRectangle(X, bodyY, Width, bodyHeight + 2, 1, 2, _borderColor, _backGroundColor);
            g.DrawBox2D(X, bodyY, Width, bodyHeight + 2, 0, _borderColor, _backGroundColor);

            int itemX = X + safeSpace;
            bodyY += 2;

            for (int i = 0; i < Options.Count; i++)
            {
                var item = Options[i];
                DrawItem(itemX, bodyY, item);
                i += item.ChieldCount;
            }
        }
        private void DrawFooter()
        {
            Size textSize = Utils2D.GetDrawingTextSize(Footer, "Segoe UI", 14);

            int footerY = bodyY;
            int textX = (Width - textSize.Width) / 2 + X + 5;
            int textY = (_titleHeight - textSize.Height) / 2 + footerY + safeSpace;

            g.BorderedRectangle(X, footerY + safeSpace, Width, _titleHeight, 1, 2, _borderColor, _backGroundColor);
            g.DrawBox2D(X, footerY + safeSpace, Width, _titleHeight, 0, _borderColor, _blackBrush);
            g.DrawText(Footer, _smalFont, _footerColor, textX, textY);
        }
    }
}
