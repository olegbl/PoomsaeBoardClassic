using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public class Display : String2DForm
    {
        // Possible Display Modes
        public enum Modes
        {
            SPORTS_POOMSAE,
            REGULAR_POOMSAE,
            TIMER
        }

        // Display Mode
        public Modes Mode;

        // Ring
        private RingControllerForm ring;
        public RingControllerForm Ring {
            set {
                this.ring = value;
            }
        }

        // Colors
        public Brush cf = Brushes.White;        // Default Foreground Color
        public Brush cb = null;                 // Default Background Color
        public Brush c1 = Brushes.Cyan;         // Default Left Color
        public Brush c2 = Brushes.OrangeRed;    // Default Right Color

        // Constructor
        public Display(RingControllerForm ring = null)
        {
            this.ring = ring;
            this.Mode = this.ring.Mode;

            this.Text = "PoomsaeBoard Display";
            this.Width = 600;
            this.Height = 400;
        }

        // Redraw the Display
        public override void redraw()
        {
            this.clear(); // Reset Display
            if (this.ring == null) return; // Check for Data

            // Setup Variables
            int count = this.ring.Judges.Count + 2;
            double rat = 1.0 / (double)count * 100.0, y;

            switch (this.Mode)
            {
                case Display.Modes.REGULAR_POOMSAE:
                    this.strings.Add(new String2D("JUDGE", 0, 0, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("TECH", 10, 0, 20, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("PRES", 30, 0, 20, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    foreach (Judge judge in this.ring.Judges)
                    {
                        double rel = (double)(judge.Id) * rat;

                        this.strings.Add(new String2D("" + judge.Id, 0, rel, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Technical1, 10, rel, 20, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Presentation1, 30, rel, 20, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    }

                    y = (count - 1) * rat;

                    this.strings.Add(new String2D("T", 0, y, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(this.ring.Poomsae1.Technical, 10, y, 20, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(this.ring.Poomsae1.Presentation, 30, y, 20, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    this.strings.Add(new String2D(this.ring.AthleteName, 50, 0, 50, 20, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(this.ring.Poomsae1.Time, 50, 20, 50, 10, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(this.ring.Poomsae1.Score, 50, 30, 50, 70, Brushes.Black, Brushes.Yellow, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    break;
                case Display.Modes.SPORTS_POOMSAE:
                    this.strings.Add(new String2D("JUDGE", 0, 0, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("TECH", 10, 0, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("PRES", 20, 0, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    this.strings.Add(new String2D("JUDGE", 90, 0, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("TECH", 70, 0, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D("PRES", 80, 0, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    foreach (Judge judge in this.ring.Judges)
                    {
                        double rel = (double)(judge.Id) * rat;

                        this.strings.Add(new String2D("" + judge.Id, 0, rel, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Technical1, 10, rel, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Presentation1, 20, rel, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                        this.strings.Add(new String2D("" + judge.Id, 90, rel, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Technical2, 70, rel, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                        this.strings.Add(new String2D(judge.Presentation2, 80, rel, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    }

                    y = (count - 1) * rat;

                    this.strings.Add(new String2D("T", 0, y, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae1.Technical), 10, y, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae1.Presentation), 20, y, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    this.strings.Add(new String2D("T", 90, y, 10, rat, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae2.Technical), 70, y, 10, rat, c1, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae2.Presentation), 80, y, 10, rat, c2, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));

                    this.strings.Add(new String2D(this.ring.AthleteName, 30, 0, 40, 20, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae1.Time), 30, 20, 20, 10, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae1.Score), 30, 30, 20, 30, Brushes.Black, c1, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae2.Time), 50, 20, 20, 10, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae2.Score), 50, 30, 20, 30, Brushes.Black, c2, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Score), 30, 60, 40, 40, Brushes.Black, Brushes.Yellow, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    break;
                case Display.Modes.TIMER:
                    this.strings.Add(new String2D(this.ring.AthleteName, 30, 0, 40, 20, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    this.strings.Add(new String2D(formatCalculatedScore(this.ring.Poomsae.Time), 0, 20, 100, 80, cf, cb, new Font(DefaultFont.FontFamily, 20, FontStyle.Bold)));
                    break;
            }

            base.redraw();
        }

        protected String formatCalculatedScore(String score)
        {
            double dscore = 0.0;
            Double.TryParse(score, out dscore);
            return String.Format("{0:0.00}", dscore);
        }
    }
}
