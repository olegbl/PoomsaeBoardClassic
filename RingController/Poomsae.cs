using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public class Poomsae
    {
        private TextBox time;
        public String Time
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.time.Text, out temp);
                return String.Format("{0:0.0000}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setText(this.time, String.Format("{0:0.0000}", temp));
                
            }
        }

        private TextBox technical;
        public String Technical
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.technical.Text, out temp);
                return String.Format("{0:0.0000}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setText(this.technical, String.Format("{0:0.0000}", temp));

            }
        }

        private TextBox presentation;
        public String Presentation
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.presentation.Text, out temp);
                return String.Format("{0:0.0000}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setText(this.presentation, String.Format("{0:0.0000}", temp));

            }
        }

        private TextBox score;
        public String Score
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.score.Text, out temp);
                return String.Format("{0:0.0000}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setText(this.score, String.Format("{0:0.0000}", temp));

            }
        }

        private delegate void setTextDelegate(TextBox textBox, String text);
        private void setText(TextBox textBox, String text)
        {
            if (textBox.InvokeRequired)
                textBox.Invoke(new setTextDelegate(setText), textBox, text);
            else
                textBox.Text = text;
        }

        public Poomsae(TextBox time, TextBox technical, TextBox presentation, TextBox score)
        {
            this.time = time;
            this.technical = technical;
            this.presentation = presentation;
            this.score = score;
        }
    }
}
