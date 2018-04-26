namespace OOLaboratories.Proprietary.Signotec
{
    partial class SignotecSignatureControl
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
            this.viewport = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.viewport)).BeginInit();
            this.SuspendLayout();
            // 
            // viewport
            // 
            this.viewport.BackColor = System.Drawing.Color.White;
            this.viewport.Location = new System.Drawing.Point(0, 0);
            this.viewport.Name = "viewport";
            this.viewport.Size = new System.Drawing.Size(320, 160);
            this.viewport.TabIndex = 1;
            this.viewport.TabStop = false;
            // 
            // ucSignotecSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.viewport);
            this.MinimumSize = new System.Drawing.Size(320, 160);
            this.Name = "ucSignotecSignature";
            this.Size = new System.Drawing.Size(318, 158);
            ((System.ComponentModel.ISupportInitialize)(this.viewport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox viewport;
    }
}
