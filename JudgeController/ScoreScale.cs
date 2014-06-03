using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public partial class ScoreScale : UserControl
    {
        protected int count;
        protected Button[] buttons;
        protected string[] names;
        protected double[] values;
        protected int index;

        public Color foreColor = Color.Black;
        public Color foreColorSelected = Color.DarkGreen;

        public Color backColor = Color.White;
        public Color backColorSelected = Color.LightGray;

        public double Value
        {
            get
            {
                return this.values[this.index];
            }
            set
            {
                for (int i = 0; i < this.count; i++)
                {
                    if (this.values[i] == value)
                    {
                        this.Index = i;
                        return;
                    }
                }
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                if (value < 0) this.index = 0;
                else if (value >= this.count) this.index = this.count - 1;
                else this.index = value;

                this.clearButtons();
                this.toggleButton(this.index, true);

                OnScoreChanged(new EventArgs());
            }
        }

        public ScoreScale()
        {
            InitializeComponent();

            this.values = new double[] { 1.0, 0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2, 0.1, 0.0 };
            this.names = new string[] { "1.0", "0.9", "0.8", "0.7", "0.6", "0.5", "0.4", "0.3", "0.2", "0.1", "0.0" };

            this.initialize();
        }

        public ScoreScale(double[] values, string[] names, int width)
        {
            InitializeComponent();

            this.values = values;
            this.names = names;

            this.initialize(width);
        }

        public void initialize(int width = 0)
        {
            if (this.buttons != null)
                foreach (Button button in this.buttons)
                    this.Controls.Remove(button);

            this.count = Math.Min(this.values.Length, this.names.Length);
            this.buttons = new Button[this.count];

            Rectangle bounds = this.ClientRectangle;
            if (width == 0) width = (int)((double)bounds.Width / (double)this.count);
            else width = (int)((double)width / (double)this.count);
            int height = bounds.Height;

            //MessageBox.Show("Creating ScoreScale with elements of size (" + width + ", " + height + ").");

            for (int i = 0; i < this.count; i++)
            {
                this.buttons[i] = new Button();
                this.buttons[i].Text = this.names[i];
                this.buttons[i].Name = "button_" + this.names[i];

                this.buttons[i].Size = new System.Drawing.Size(width + 1, height);
                this.buttons[i].Location = new System.Drawing.Point(i * width, 0);

                this.buttons[i].Margin = new System.Windows.Forms.Padding(0);
                this.buttons[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                //this.buttons[i].FlatAppearance.BorderSize = 0;
                this.buttons[i].Cursor = System.Windows.Forms.Cursors.Hand;
                this.buttons[i].UseVisualStyleBackColor = true;

                int id = i;
                this.buttons[i].Click += (object o, EventArgs a) => { this.Index = id; };

                this.Controls.Add(this.buttons[i]);
            }

            this.Index = 0;
        }

        private void toggleButton(int score, bool selected)
        {
            Button button = this.buttons[score];
            if (selected)
            {
                /*button.BackgroundImage =
                    score == 0 ? global::PoomsaeBoard.Properties.Resources.button_left_over :
                    score == this.count - 1 ? global::PoomsaeBoard.Properties.Resources.button_right_over :
                    global::PoomsaeBoard.Properties.Resources.button_middle_over;*/
                button.ForeColor = this.foreColorSelected;
                button.BackColor = this.backColorSelected;
            }
            else
            {
                /*button.BackgroundImage =
                    score == 0 ? global::PoomsaeBoard.Properties.Resources.button_left :
                    score == this.count - 1 ? global::PoomsaeBoard.Properties.Resources.button_right :
                    global::PoomsaeBoard.Properties.Resources.button_middle;*/
                button.ForeColor = this.foreColor;
                button.BackColor = this.backColor;
            }
        }

        private void clearButtons()
        {
            for (int i = 0; i < this.count; i++)
                this.toggleButton(i, false);
        }
        
        public event EventHandler ScoreChanged;
        protected virtual void OnScoreChanged(EventArgs e)
        {
            if (ScoreChanged != null) ScoreChanged(this, e);
        }
    }
}
