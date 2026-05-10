using System.Drawing.Drawing2D;

namespace AOOP_PROJECTT.SupportClasses
{
    public static class UIHelper
    {
        public static void ApplyRoundedStyle(Panel panel, int radius = 10)
        {
            panel.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, panel.Width, panel.Height, radius, radius));

            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(50, 60, 80), 1.5f);
                using var path = RoundedRect(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), radius);
                e.Graphics.DrawPath(pen, path);
            };

            panel.Resize += (s, e) => panel.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, panel.Width, panel.Height, radius, radius));
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(
    int nLeftRect, int nTopRect,
    int nRightRect, int nBottomRect,
    int nWidthEllipse, int nHeightEllipse);
    }
}