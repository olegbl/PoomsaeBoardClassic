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
    public partial class PoomsaeScore : UserControl
    {
        private ScoreScale[] scales;
        private Label[] labels;

        public double min = 0.0, max = 0.0, minord = 0.0, majord = 0.0;

        public PoomsaeScore()
        {
            InitializeComponent();

            this.technicalWrapper = new TextBoxWrapper(this.technical);
            this.presentationWrapper = new TextBoxWrapper(this.presentation);

            scales = new ScoreScale[0];
            labels = new Label[0];
        }
        public delegate void updateScalesDelegate(String[] args);
        public void updateScales(params String[] args)
        {
            if (this.InvokeRequired)
            {
                try { this.Invoke(new updateScalesDelegate(updateScales), new object[] { args }); }
                catch { };
                return;
            }

            //MessageBox.Show("PoomsaeScore got # " + args.Length + " : " + args.ToString());

            double pmin = 0.0, pmax = 0.0, step = 0.0;
            
            double.TryParse(args[1], out this.min);
            double.TryParse(args[2], out this.max);
            double.TryParse(args[3], out this.minord);
            double.TryParse(args[4], out this.majord);

            int xl = 3,     // Label X
                xs = 7,     // Scale X
                y = 61,    // Current Y
                ys = 15,    // Scale Y Offset
                yd = 44;    // Delta Y

            foreach (ScoreScale scale in this.scales)
                this.control.Controls.Remove(scale);

            foreach (Label label in this.labels)
                this.control.Controls.Remove(label);

            int scalecount = (args.Length - 5) / 4;
            this.scales = new ScoreScale[scalecount];
            this.labels = new Label[scalecount];

            for (int i = 5; i < args.Length; i += 4)
            {
                pmin = 0.0;
                pmax = 1.0;
                step = 0.1;

                int id = (i - 5) / 4;

                Label label = new Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(xl, y);
                label.Size = new System.Drawing.Size(200, 13);
                label.Text = args[i + 0];
                label.Name = i + "_label";
                this.labels[id] = label;

                if (args.Length > i + 1) double.TryParse(args[i + 1], out pmin);
                if (args.Length > i + 2) double.TryParse(args[i + 2], out pmax);
                if (args.Length > i + 3) double.TryParse(args[i + 3], out step);

                int length = (int)((pmax - pmin) / step) + 1;
                double[] values = new double[length];
                string[] names = new string[length];

                for (int j = 0; j < length; j ++)
                {
                    values[j] = pmax - step * j;
                    names[j] = (pmax - step * j).ToString();
                }

                ScoreScale scale = new ScoreScale(values, names, 518);
                scale.Location = new System.Drawing.Point(xs, y + ys);
                scale.Size = new System.Drawing.Size(520, 25);
                scale.ScoreChanged += new System.EventHandler(this.partial_ScoreChanged);
                scale.Name = i + "_scale";
                this.scales[id] = scale;

                y += yd;
            }

            //MessageBox.Show("I have " + this.Controls.Count + " controls, and I want to add " + this.scales.Length + " + " + this.labels.Length + " controls!");

            foreach (ScoreScale scale in this.scales)
                this.control.Controls.Add(scale);

            foreach (Label label in this.labels)
                this.control.Controls.Add(label);

            //MessageBox.Show("Now, I have " + this.Controls.Count + " controls!");
        }

        delegate void setTitleDelegate(String value);
        private void setTitle(String value) {
            this.Title = value;
        }

        public String Title
        {
            get
            {
                //return this.title.Text;
                return "";
            }
            set
            {
                //if (this.title.InvokeRequired) this.title.Invoke(new setTitleDelegate(setTitle), value);
                //else this.title.Text = value;
            }
        }

        private TextBoxWrapper technicalWrapper;
        public TextBoxWrapper Technical {
            get {
                return this.technicalWrapper;
            }
        }
        

        private TextBoxWrapper presentationWrapper;
        public TextBoxWrapper Presentation {
            get {
                return this.presentationWrapper;
            }
        }

        public void clearScore()
        {
            this.Technical.Value = max;
            foreach (ScoreScale scale in this.scales)
                scale.Index = 0;
            updateScore();
        }

        protected void updateScore()
        {
            this.Presentation.Value = 0.0;
            foreach (ScoreScale scale in this.scales)
                this.Presentation.Value += scale.Value;
            this.OnScoreChanged(new EventArgs());
        }

        // Events
        public event EventHandler ScoreChanged;
        protected virtual void OnScoreChanged(EventArgs e) { if (ScoreChanged != null) ScoreChanged(this, e); }

        private void minor_Click(object sender, EventArgs e)
        {
            this.Technical.Value = Math.Max(this.min, this.Technical.Value - this.minord);
            this.updateScore();
        }

        private void major_Click(object sender, EventArgs e)
        {
            this.Technical.Value = Math.Max(this.min, this.Technical.Value - this.majord);
            this.updateScore();
        }

        private void minor_undo_Click(object sender, EventArgs e)
        {
            this.Technical.Value = Math.Min(this.max, this.Technical.Value + this.minord);
            this.updateScore();
        }

        private void major_undo_Click(object sender, EventArgs e)
        {
            this.Technical.Value = Math.Min(this.max, this.Technical.Value + this.majord);
            this.updateScore();
        }

        private void partial_ScoreChanged(object sender, EventArgs e)
        {
            updateScore();
        }
    }
}
