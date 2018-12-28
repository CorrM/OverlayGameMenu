# OverlayGameMenu
it's basic Full Dynamic external overlay game menu to easy controlling on off options.

#### Result
![#1](https://github.com/CorrM/OverlayGameMenu/blob/master/Imgs/1.png?raw=true)
![#2](https://github.com/CorrM/OverlayGameMenu/blob/master/Imgs/2.png?raw=true)

#### Requirements
1. Overlay.Net My Edition [HERE](https://github.com/CorrM/Overlay.NET)
2. GameMath.Net [HERE](https://www.nuget.org/packages/GameMath.Net/)
3. InputSimulator [HERE](https://www.nuget.org/packages/InputSimulator)
4. SharpDX [HERE](https://www.nuget.org/packages/SharpDX)
6. SharpDX.Direct2D1 [HERE](https://www.nuget.org/packages/SharpDX.Direct2D1)

#### Example
```
// (Menu Items)
public static MenuItemID Esp = new MenuItemID(0, true);
public static MenuItemID Esp_Player = new MenuItemID(1, true);
public static MenuItemID Esp_Items = new MenuItemID(2, true);

public static MenuItemID AimBot = new MenuItemID(10, true);
public static MenuItemID NoGrass = new MenuItemID(20, true);

public static MenuItemID DEBUG = new MenuItemID(200, false);
public static MenuItemID DEBUG_Player = new MenuItemID(201, false);
public static MenuItemID DEBUG_Items = new MenuItemID(202, false);
```
```
// (Initialize)
// OverlayWindow is your DirectXOverlayPlugin
// Menu (in your DirectXOverlayPlugin Initialize func)
var MainMenu = new MenuBase(OverlayWindow, true)
{
  OptFont = OverlayWindow.Graphics.CreateFont("Courier New", 14),
  Title = "~|@| Pubgm CorrM |@|~",
  Footer = "Good Hack",
  TitleColor = Color.White,
  X = 40,
  Y = 30,
  Width = 250,
  BackGroundColor = Color.FromArgb(8, 8, 8),
  SelectOptColor = Color.FromArgb(25, 42, 86),
  SelectSubColor = Color.FromArgb(38, 38, 38),
  BorderColor = Color.Black,
  FooterColor = Color.Red,
  OptOnColor = Color.FromArgb(106, 176, 76),
  OptOffColor = Color.FromArgb(235, 77, 75),
  SubOptOnColor = Color.Green,
  SubOptOffColor = Color.Red,
  TextColor = Color.Orange
};

// ESP
MainMenu.AddOpt(new MenuItem(Esp, "ESP", typeof(MenuItemTypes.OnOffItem), true) { OptState = (int)MenuItemTypes.OnOffItem.On });
MainMenu.AddSubOption(new MenuItem(Esp_Player, "Players", typeof(MenuItemTypes.EspMethod)) { OptState = (int)MenuItemTypes.EspMethod.Array });
MainMenu.AddSubOption(new MenuItem(Esp_Items, "Items", typeof(MenuItemTypes.OnOffItem)) { OptState = (int)MenuItemTypes.OnOffItem.On });

// Main Hacks
MainMenu.AddOpt(new MenuItem(AimBot, "Aimbot", typeof(MenuItemTypes.AimbotItem)) { OptState = (int)MenuItemTypes.AimbotItem.Off });
MainMenu.AddOpt(new MenuItem(NoGrass, "No Grass", typeof(MenuItemTypes.OnOffItem)) { OptState = (int)MenuItemTypes.OnOffItem.Off });

// Debug
MainMenu.AddOpt(new MenuItem(DEBUG, "DEBUG", typeof(MenuItemTypes.OnOffItem), true) { OptState = (int)MenuItemTypes.OnOffItem.Off });
MainMenu.AddSubOption(new MenuItem(DEBUG_Player, "Players", typeof(MenuItemTypes.OnOffItem)) { OptState = (int)MenuItemTypes.OnOffItem.Off });
MainMenu.AddSubOption(new MenuItem(DEBUG_Items, "Items", typeof(MenuItemTypes.OnOffItem)) { OptState = (int)MenuItemTypes.OnOffItem.Off });

MainMenu.OnOptionToggled += Menu_OnOptionToggled;
MainMenu.Inti();
```
```
// Item Toggle Handler
public static void Menu_OnOptionToggled(MenuItem item)
{
  bool IsOff = item.OptState == (int)MenuItemTypes.OnOffItem.Off; // Off must be equal 0 in any menu item enum (First Enum Option)

  if (item.ItemID.ID == Esp.ID)
  {
    // Do Stuff
  }
}
```
```
// (DON'T Forget)
// Set it before [Graphics.EndScene()]
MainMenu.Update();
```
