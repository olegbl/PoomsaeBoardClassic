using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public class TextBoxWrapper
    {
        private TextBox text = null;

        public TextBoxWrapper(TextBox text)
        {
            this.text = text;
        }

        private delegate void setValueDelegate(double value);
        private void setValue(double value)
        {
            this.Value = value;
        }

        public double Value
        {
            get
            {
                double result = 0.0;
                double.TryParse(this.text.Text, out result);
                return result;
            }
            set
            {
                if (this.text.InvokeRequired)
                    this.text.Invoke(new setValueDelegate(setValue), value);
                else
                    this.text.Text = string.Format("{0:0.0}", value);
            }
        }

        public string String
        {
            get
            {
                return string.Format("{0:0.0}", this.Value);
            }
            set
            {
                double result = 0.0;
                double.TryParse(value, out result);
                this.Value = result;
            }
        }
    }
}
