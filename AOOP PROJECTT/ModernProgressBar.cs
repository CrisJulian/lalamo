using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CommonCents
{
    #pragma warning disable WFO1000 // Silences the modern designer serialization warning
    public class ModernProgressBar : Control
    {
        // Private backing fields to store property data
        private int _value = 50;
        private int _maximum = 100;
        private Color _progressColor = Color.FromArgb(245, 166, 35); // Accent Yellow/Orange
        private Color _trackColor = Color.FromArgb(45, 52, 70);     // Dark Gray Blue

        public ModernProgressBar()
        {
            // Prevents flickering during updates
            this.DoubleBuffered = true;
            this.Size = new Size(200, 12);
        }

        [Category("Appearance")]
        [Description("The current progress value.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Value
        {
            get => _value;
            set 
            { 
                _value = Math.Max(0, Math.Min(value, Maximum)); 
                Invalidate(); // Redraws the control when value changes
            }
        }

        [Category("Appearance")]
        [Description("The maximum value of the progress bar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Maximum
        {
            get => _maximum;
            set 
            { 
                if (value > 0) _maximum = value; 
                Invalidate(); 
            }
        }

        [Category("Appearance")]
        [Description("The color of the progressing bar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ProgressColor
        {
            get => _progressColor;
            set { _progressColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("The color of the background track.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TrackColor
        {
            get => _trackColor;
            set { _trackColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Define the bounding rectangle for the track
            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            int radius = Math.Max(this.Height / 2 - 1, 2);

            // 1. Draw the Background Track
            using (GraphicsPath path = GetRoundedRect(rect, radius))
            using (SolidBrush brush = new SolidBrush(TrackColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            // 2. Draw the Progress Fill
            if (Value > 0)
            {
                float percent = (float)Value / Maximum;
                int fillWidth = (int)(rect.Width * percent);

                // Only draw fill if it's wide enough to render
                if (fillWidth > radius)
                {
                    var fillRect = new Rectangle(0, 0, fillWidth, rect.Height);
                    using (GraphicsPath path = GetRoundedRect(fillRect, radius))
                    using (SolidBrush brush = new SolidBrush(ProgressColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            }
        }

        // Helper method to create rounded rectangle paths
        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            Rectangle arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);

            // Top Left
            path.AddArc(arc, 180, 90);
            // Top Right
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            // Bottom Right
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            // Bottom Left
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}