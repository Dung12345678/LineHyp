namespace BMS
{
    partial class frmCheck
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
			this.chkFat = new System.Windows.Forms.CheckBox();
			this.chkFitRow = new System.Windows.Forms.CheckBox();
			this.chkJob = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// chkFat
			// 
			this.chkFat.AutoSize = true;
			this.chkFat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkFat.Location = new System.Drawing.Point(12, 12);
			this.chkFat.Name = "chkFat";
			this.chkFat.Size = new System.Drawing.Size(44, 17);
			this.chkFat.TabIndex = 27;
			this.chkFat.Text = "Fat";
			this.chkFat.UseVisualStyleBackColor = true;
			this.chkFat.CheckedChanged += new System.EventHandler(this.chkFat_CheckedChanged);
			// 
			// chkFitRow
			// 
			this.chkFitRow.AutoSize = true;
			this.chkFitRow.Checked = true;
			this.chkFitRow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFitRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkFitRow.Location = new System.Drawing.Point(78, 12);
			this.chkFitRow.Name = "chkFitRow";
			this.chkFitRow.Size = new System.Drawing.Size(40, 17);
			this.chkFitRow.TabIndex = 26;
			this.chkFitRow.Text = "Fit";
			this.chkFitRow.UseVisualStyleBackColor = true;
			this.chkFitRow.CheckedChanged += new System.EventHandler(this.chkFitRow_CheckedChanged);
			// 
			// chkJob
			// 
			this.chkJob.AutoSize = true;
			this.chkJob.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkJob.Location = new System.Drawing.Point(136, 12);
			this.chkJob.Name = "chkJob";
			this.chkJob.Size = new System.Drawing.Size(46, 17);
			this.chkJob.TabIndex = 28;
			this.chkJob.Text = "Job";
			this.chkJob.UseVisualStyleBackColor = true;
			// 
			// frmCheck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(194, 38);
			this.Controls.Add(this.chkJob);
			this.Controls.Add(this.chkFat);
			this.Controls.Add(this.chkFitRow);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximumSize = new System.Drawing.Size(210, 77);
			this.MinimumSize = new System.Drawing.Size(210, 77);
			this.Name = "frmCheck";
			this.Text = "CHECK";
			this.Load += new System.EventHandler(this.WarningForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.CheckBox chkFat;
		private System.Windows.Forms.CheckBox chkFitRow;
		private System.Windows.Forms.CheckBox chkJob;
	}
}