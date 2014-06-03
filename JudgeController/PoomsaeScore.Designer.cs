namespace PoomsaeBoard
{
    partial class PoomsaeScore
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.control = new System.Windows.Forms.Panel();
            this.minor_undo = new System.Windows.Forms.Button();
            this.major_undo = new System.Windows.Forms.Button();
            this.minor = new System.Windows.Forms.Button();
            this.major = new System.Windows.Forms.Button();
            this.technical_label = new System.Windows.Forms.Label();
            this.presentation_label = new System.Windows.Forms.Label();
            this.technical = new System.Windows.Forms.TextBox();
            this.presentation = new System.Windows.Forms.TextBox();
            this.control.SuspendLayout();
            this.SuspendLayout();
            // 
            // control
            // 
            this.control.BackColor = System.Drawing.SystemColors.Window;
            this.control.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.control.Controls.Add(this.presentation);
            this.control.Controls.Add(this.minor_undo);
            this.control.Controls.Add(this.major_undo);
            this.control.Controls.Add(this.minor);
            this.control.Controls.Add(this.major);
            this.control.Controls.Add(this.technical_label);
            this.control.Controls.Add(this.presentation_label);
            this.control.Controls.Add(this.technical);
            this.control.Location = new System.Drawing.Point(0, 0);
            this.control.Margin = new System.Windows.Forms.Padding(0);
            this.control.Name = "control";
            this.control.Size = new System.Drawing.Size(536, 358);
            this.control.TabIndex = 10;
            // 
            // minor_undo
            // 
            this.minor_undo.ForeColor = System.Drawing.Color.Green;
            this.minor_undo.Location = new System.Drawing.Point(3, 32);
            this.minor_undo.Name = "minor_undo";
            this.minor_undo.Size = new System.Drawing.Size(204, 23);
            this.minor_undo.TabIndex = 10;
            this.minor_undo.Text = "Undo Minor Deduction";
            this.minor_undo.UseVisualStyleBackColor = true;
            this.minor_undo.Click += new System.EventHandler(this.minor_undo_Click);
            // 
            // major_undo
            // 
            this.major_undo.ForeColor = System.Drawing.Color.Green;
            this.major_undo.Location = new System.Drawing.Point(324, 32);
            this.major_undo.Name = "major_undo";
            this.major_undo.Size = new System.Drawing.Size(204, 23);
            this.major_undo.TabIndex = 9;
            this.major_undo.Text = "Undo Major Deduction";
            this.major_undo.UseVisualStyleBackColor = true;
            this.major_undo.Click += new System.EventHandler(this.major_undo_Click);
            // 
            // minor
            // 
            this.minor.ForeColor = System.Drawing.Color.Maroon;
            this.minor.Location = new System.Drawing.Point(3, 3);
            this.minor.Name = "minor";
            this.minor.Size = new System.Drawing.Size(204, 28);
            this.minor.TabIndex = 8;
            this.minor.Text = "Minor Deduction";
            this.minor.UseVisualStyleBackColor = true;
            this.minor.Click += new System.EventHandler(this.minor_Click);
            // 
            // major
            // 
            this.major.ForeColor = System.Drawing.Color.Maroon;
            this.major.Location = new System.Drawing.Point(324, 3);
            this.major.Name = "major";
            this.major.Size = new System.Drawing.Size(204, 28);
            this.major.TabIndex = 7;
            this.major.Text = "Major Deduction";
            this.major.UseVisualStyleBackColor = true;
            this.major.Click += new System.EventHandler(this.major_Click);
            // 
            // technical_label
            // 
            this.technical_label.AutoSize = true;
            this.technical_label.Location = new System.Drawing.Point(213, 3);
            this.technical_label.Name = "technical_label";
            this.technical_label.Size = new System.Drawing.Size(36, 13);
            this.technical_label.TabIndex = 2;
            this.technical_label.Text = "TECH";
            // 
            // presentation_label
            // 
            this.presentation_label.AutoSize = true;
            this.presentation_label.Location = new System.Drawing.Point(282, 3);
            this.presentation_label.Name = "presentation_label";
            this.presentation_label.Size = new System.Drawing.Size(36, 13);
            this.presentation_label.TabIndex = 2;
            this.presentation_label.Text = "PRES";
            // 
            // technical
            // 
            this.technical.BackColor = System.Drawing.Color.White;
            this.technical.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.technical.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.technical.ForeColor = System.Drawing.Color.SteelBlue;
            this.technical.Location = new System.Drawing.Point(213, 19);
            this.technical.Name = "technical";
            this.technical.ReadOnly = true;
            this.technical.Size = new System.Drawing.Size(40, 31);
            this.technical.TabIndex = 3;
            this.technical.Text = "0.0";
            this.technical.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // presentation
            // 
            this.presentation.BackColor = System.Drawing.Color.White;
            this.presentation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.presentation.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presentation.ForeColor = System.Drawing.Color.Crimson;
            this.presentation.Location = new System.Drawing.Point(278, 19);
            this.presentation.Name = "presentation";
            this.presentation.ReadOnly = true;
            this.presentation.Size = new System.Drawing.Size(40, 31);
            this.presentation.TabIndex = 11;
            this.presentation.Text = "0.0";
            this.presentation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PoomsaeScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.control);
            this.Name = "PoomsaeScore";
            this.Size = new System.Drawing.Size(536, 358);
            this.control.ResumeLayout(false);
            this.control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel control;
        private System.Windows.Forms.Button minor_undo;
        private System.Windows.Forms.Button major_undo;
        private System.Windows.Forms.Button minor;
        private System.Windows.Forms.Button major;
        private System.Windows.Forms.Label technical_label;
        private System.Windows.Forms.Label presentation_label;
        private System.Windows.Forms.TextBox technical;
        private System.Windows.Forms.TextBox presentation;
    }
}
