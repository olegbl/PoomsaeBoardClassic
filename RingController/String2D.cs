using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PoomsaeBoard
{
    public class String2D
    {
        public String t;
        public double x, y, w, h;
        public Brush fg, bg;
        public Font f;

        public String2D(String t = "", double x = 0, double y = 0, double w = 100, double h = 20, Brush fg = null, Brush bg = null, Font f = null)
        {
            if (fg == null) fg = Brushes.White;
            if (f == null) f = SystemFonts.DefaultFont;

            this.t = t;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.fg = fg;
            this.bg = bg;
            this.f = f;
        }
    }
}
