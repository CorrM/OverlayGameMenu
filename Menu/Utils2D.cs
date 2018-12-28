using GameMath;
using Overlay.NET.Directx;
using System;
using System.Drawing;

namespace OverlayGameMenu
{
    public static class Utils2D
    {
        public static Rectangle GetWindowRect(IntPtr ControlHandle, out Vector2 WindowSize)
        {
            RECT rect = new RECT();
            Managed.GetWindowRect(ControlHandle, out rect);
            WindowSize = new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);

            return new Rectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        public static Size GetDrawingTextSize(string text, D2DFont font)
        {
            Font stringFont = new Font(font.FontFamilyName, font.FontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            Image fakeImage = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(fakeImage);
            SizeF textSize = graphics.MeasureString(text, stringFont);

            int x1SizeW = (int)textSize.Width;
            int x1SizeH = (int)textSize.Height;
            return new Size(x1SizeW, x1SizeH);
        }
        public static Size GetDrawingTextSize(string text, string fontName, float fontSize)
        {
            Font stringFont = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            Image fakeImage = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(fakeImage);
            SizeF textSize = graphics.MeasureString(text, stringFont);

            int x1SizeW = (int)textSize.Width;
            int x1SizeH = (int)textSize.Height;
            return new Size(x1SizeW, x1SizeH);
        }
        public static Vector2 CenterOfPoint(Vector2 p1WidthHight, Vector2 p2WidthHight)
        {
            return new Vector2((p1WidthHight.X - p2WidthHight.X) / 2, (p1WidthHight.Y - p2WidthHight.Y) / 2);
        }
    }
}
