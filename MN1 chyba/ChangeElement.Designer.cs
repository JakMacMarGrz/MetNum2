namespace MN1_chyba
{
    partial class ChangeElement
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_RN = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.comboBox_elementType = new System.Windows.Forms.ComboBox();
            this.pictureBox_Z = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Z)).BeginInit();
            this.SuspendLayout();
            // 
            // label_RN
            // 
            this.label_RN.AutoSize = true;
            this.label_RN.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold);
            this.label_RN.Location = new System.Drawing.Point(119, 15);
            this.label_RN.Name = "label_RN";
            this.label_RN.Size = new System.Drawing.Size(49, 24);
            this.label_RN.TabIndex = 44;
            this.label_RN.Text = "RN1";
            // 
            // button_ok
            // 
            this.button_ok.BackColor = System.Drawing.Color.YellowGreen;
            this.button_ok.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_ok.Location = new System.Drawing.Point(289, 194);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(111, 37);
            this.button_ok.TabIndex = 46;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.BackColor = System.Drawing.Color.Coral;
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_cancel.Location = new System.Drawing.Point(406, 206);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(90, 25);
            this.button_cancel.TabIndex = 47;
            this.button_cancel.Text = "ANULUJ";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // comboBox_elementType
            // 
            this.comboBox_elementType.BackColor = System.Drawing.Color.White;
            this.comboBox_elementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_elementType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_elementType.FormattingEnabled = true;
            this.comboBox_elementType.Items.AddRange(new object[] {
            "1.",
            "2.",
            "3."});
            this.comboBox_elementType.Location = new System.Drawing.Point(290, 42);
            this.comboBox_elementType.Name = "comboBox_elementType";
            this.comboBox_elementType.Size = new System.Drawing.Size(207, 28);
            this.comboBox_elementType.TabIndex = 48;
            // 
            // pictureBox_Z
            // 
            this.pictureBox_Z.Image = global::MN1_chyba.Properties.Resources.resistorN_h;
            this.pictureBox_Z.Location = new System.Drawing.Point(27, 42);
            this.pictureBox_Z.Name = "pictureBox_Z";
            this.pictureBox_Z.Size = new System.Drawing.Size(232, 142);
            this.pictureBox_Z.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Z.TabIndex = 43;
            this.pictureBox_Z.TabStop = false;
            // 
            // ChangeElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(509, 240);
            this.Controls.Add(this.comboBox_elementType);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.label_RN);
            this.Controls.Add(this.pictureBox_Z);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeElement";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zmiana parametrów elementu";
            this.Load += new System.EventHandler(this.ChangeElement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Z)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_RN;
        private System.Windows.Forms.PictureBox pictureBox_Z;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ComboBox comboBox_elementType;
    }
}