namespace PoomsaeBoard
{
    partial class JudgeControllerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "client")]
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JudgeControllerForm));
            this.port_label = new System.Windows.Forms.Label();
            this.host_label = new System.Windows.Forms.Label();
            this.port_textbox = new System.Windows.Forms.TextBox();
            this.host_textbox = new System.Windows.Forms.TextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.passphrase_textbox = new System.Windows.Forms.TextBox();
            this.passphrase_label = new System.Windows.Forms.Label();
            this.connect_control = new System.Windows.Forms.Panel();
            this.judge_textbox = new System.Windows.Forms.TextBox();
            this.ring_textbox = new System.Windows.Forms.TextBox();
            this.name_textbox = new System.Windows.Forms.TextBox();
            this.info_control = new System.Windows.Forms.Panel();
            this.poomsae_textbox = new System.Windows.Forms.TextBox();
            this.poomsae = new PoomsaeBoard.PoomsaeScore();
            this.connect_control.SuspendLayout();
            this.info_control.SuspendLayout();
            this.SuspendLayout();
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(3, 32);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(26, 13);
            this.port_label.TabIndex = 4;
            this.port_label.Text = "Port";
            // 
            // host_label
            // 
            this.host_label.AutoSize = true;
            this.host_label.Location = new System.Drawing.Point(3, 6);
            this.host_label.Name = "host_label";
            this.host_label.Size = new System.Drawing.Size(29, 13);
            this.host_label.TabIndex = 5;
            this.host_label.Text = "Host";
            // 
            // port_textbox
            // 
            this.port_textbox.Location = new System.Drawing.Point(71, 29);
            this.port_textbox.Name = "port_textbox";
            this.port_textbox.Size = new System.Drawing.Size(193, 20);
            this.port_textbox.TabIndex = 2;
            // 
            // host_textbox
            // 
            this.host_textbox.Location = new System.Drawing.Point(71, 3);
            this.host_textbox.Name = "host_textbox";
            this.host_textbox.Size = new System.Drawing.Size(193, 20);
            this.host_textbox.TabIndex = 3;
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(189, 81);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(75, 23);
            this.connect_button.TabIndex = 6;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.onConnectClick);
            // 
            // passphrase_textbox
            // 
            this.passphrase_textbox.Location = new System.Drawing.Point(71, 55);
            this.passphrase_textbox.Name = "passphrase_textbox";
            this.passphrase_textbox.Size = new System.Drawing.Size(193, 20);
            this.passphrase_textbox.TabIndex = 2;
            this.passphrase_textbox.Text = "Judge1";
            // 
            // passphrase_label
            // 
            this.passphrase_label.AutoSize = true;
            this.passphrase_label.Location = new System.Drawing.Point(3, 57);
            this.passphrase_label.Name = "passphrase_label";
            this.passphrase_label.Size = new System.Drawing.Size(62, 13);
            this.passphrase_label.TabIndex = 4;
            this.passphrase_label.Text = "Passphrase";
            // 
            // connect_control
            // 
            this.connect_control.Controls.Add(this.host_textbox);
            this.connect_control.Controls.Add(this.connect_button);
            this.connect_control.Controls.Add(this.port_textbox);
            this.connect_control.Controls.Add(this.passphrase_label);
            this.connect_control.Controls.Add(this.passphrase_textbox);
            this.connect_control.Controls.Add(this.port_label);
            this.connect_control.Controls.Add(this.host_label);
            this.connect_control.Location = new System.Drawing.Point(12, 12);
            this.connect_control.Name = "connect_control";
            this.connect_control.Size = new System.Drawing.Size(268, 109);
            this.connect_control.TabIndex = 7;
            // 
            // judge_textbox
            // 
            this.judge_textbox.BackColor = System.Drawing.SystemColors.Window;
            this.judge_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.judge_textbox.Location = new System.Drawing.Point(3, 22);
            this.judge_textbox.Name = "judge_textbox";
            this.judge_textbox.ReadOnly = true;
            this.judge_textbox.Size = new System.Drawing.Size(260, 13);
            this.judge_textbox.TabIndex = 4;
            this.judge_textbox.TabStop = false;
            this.judge_textbox.Text = "Judge";
            this.judge_textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ring_textbox
            // 
            this.ring_textbox.BackColor = System.Drawing.SystemColors.Window;
            this.ring_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ring_textbox.Location = new System.Drawing.Point(3, 3);
            this.ring_textbox.Name = "ring_textbox";
            this.ring_textbox.ReadOnly = true;
            this.ring_textbox.Size = new System.Drawing.Size(260, 13);
            this.ring_textbox.TabIndex = 4;
            this.ring_textbox.TabStop = false;
            this.ring_textbox.Text = "Ring";
            this.ring_textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // name_textbox
            // 
            this.name_textbox.BackColor = System.Drawing.SystemColors.Window;
            this.name_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.name_textbox.Location = new System.Drawing.Point(3, 41);
            this.name_textbox.Name = "name_textbox";
            this.name_textbox.ReadOnly = true;
            this.name_textbox.Size = new System.Drawing.Size(260, 13);
            this.name_textbox.TabIndex = 4;
            this.name_textbox.TabStop = false;
            this.name_textbox.Text = "Name";
            this.name_textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // info_control
            // 
            this.info_control.BackColor = System.Drawing.SystemColors.Window;
            this.info_control.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.info_control.Controls.Add(this.poomsae_textbox);
            this.info_control.Controls.Add(this.ring_textbox);
            this.info_control.Controls.Add(this.judge_textbox);
            this.info_control.Controls.Add(this.name_textbox);
            this.info_control.Location = new System.Drawing.Point(12, 127);
            this.info_control.Name = "info_control";
            this.info_control.Size = new System.Drawing.Size(268, 243);
            this.info_control.TabIndex = 8;
            // 
            // poomsae_textbox
            // 
            this.poomsae_textbox.BackColor = System.Drawing.SystemColors.Window;
            this.poomsae_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.poomsae_textbox.Location = new System.Drawing.Point(3, 60);
            this.poomsae_textbox.Name = "poomsae_textbox";
            this.poomsae_textbox.ReadOnly = true;
            this.poomsae_textbox.Size = new System.Drawing.Size(260, 13);
            this.poomsae_textbox.TabIndex = 5;
            this.poomsae_textbox.TabStop = false;
            this.poomsae_textbox.Text = "Poomsae";
            this.poomsae_textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // poomsae
            // 
            this.poomsae.Location = new System.Drawing.Point(287, 12);
            this.poomsae.Name = "poomsae";
            this.poomsae.Size = new System.Drawing.Size(536, 358);
            this.poomsae.TabIndex = 11;
            this.poomsae.Title = "";
            this.poomsae.ScoreChanged += new System.EventHandler(this.onPoomsaeScoreChanged);
            // 
            // JudgeControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 382);
            this.Controls.Add(this.poomsae);
            this.Controls.Add(this.info_control);
            this.Controls.Add(this.connect_control);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "JudgeControllerForm";
            this.Text = "PoomsaeBoard Judge Controller";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onFormClosed);
            this.Load += new System.EventHandler(this.onFormLoad);
            this.connect_control.ResumeLayout(false);
            this.connect_control.PerformLayout();
            this.info_control.ResumeLayout(false);
            this.info_control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label host_label;
        private System.Windows.Forms.TextBox port_textbox;
        private System.Windows.Forms.TextBox host_textbox;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.TextBox passphrase_textbox;
        private System.Windows.Forms.Label passphrase_label;
        private System.Windows.Forms.Panel connect_control;
        private System.Windows.Forms.TextBox judge_textbox;
        private System.Windows.Forms.TextBox ring_textbox;
        private System.Windows.Forms.TextBox name_textbox;
        private System.Windows.Forms.Panel info_control;
        private PoomsaeScore poomsae;
        private System.Windows.Forms.TextBox poomsae_textbox;
    }
}