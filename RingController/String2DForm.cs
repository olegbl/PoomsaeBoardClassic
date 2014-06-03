using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public class String2DForm : Form
    {
        public List<String2D> strings;

        public String2DForm()
        {
            strings = new List<String2D>();
        }

        public virtual void redraw()
        {
            this.Invalidate();
        }

        public void clear()
        {
            this.strings = new List<String2D>();
        }

        protected override void OnResize(EventArgs a)
        {
            base.OnResize(a);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs a)
        {
            base.OnPaint(a);
            a.Graphics.Clear(Color.Black);

            if (this.strings == null || this.strings.Count == 0) return;

            double width = ClientRectangle.Size.Width,
                   height = ClientRectangle.Size.Height;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            foreach (String2D s in strings)
            {
                float x = (float)(s.x / 100.0 * width),
                      y = (float)(s.y / 100.0 * height),
                      w = (float)(s.w / 100.0 * width),
                      h = (float)(s.h / 100.0 * height);

                if (s.bg != null) a.Graphics.FillRectangle(s.bg, x, y, w, h);
                SizeF size;
                a.Graphics.DrawString(s.t, AppropriateFont(a.Graphics, 6.0f, 500.0f, new Size((int)w, (int)h), s.t, s.f, out size), s.fg, x + w / 2, y + h / 2, stringFormat);
            }
        }

        private static Font AppropriateFont(Graphics g, float minFontSize, float maxFontSize, Size layoutSize, string s, Font f, out SizeF extent)
        {
            if (maxFontSize == minFontSize)
                f = new Font(f.FontFamily, minFontSize, f.Style);

            extent = g.MeasureString(s, f);

            if (maxFontSize <= minFontSize)
                return f;

            float hRatio = layoutSize.Height / extent.Height;
            float wRatio = layoutSize.Width / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            float newSize = f.Size * ratio;

            if (newSize < minFontSize)
                newSize = minFontSize;
            else if (newSize > maxFontSize)
                newSize = maxFontSize;

            f = new Font(f.FontFamily, newSize, f.Style);
            extent = g.MeasureString(s, f);

            return f;
        }
    }
}
