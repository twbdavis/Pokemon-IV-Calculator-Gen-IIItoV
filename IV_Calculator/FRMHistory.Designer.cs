namespace IV_Calculator
{
    partial class History
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(History));
            this.BTNBack = new System.Windows.Forms.Button();
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            this.BTNClear = new System.Windows.Forms.Button();
            this.BTNClearRow = new System.Windows.Forms.Button();
            this.BTNAddExample = new System.Windows.Forms.Button();
            this.BTNResetExamples = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // BTNBack
            // 
            this.BTNBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTNBack.Location = new System.Drawing.Point(12, 392);
            this.BTNBack.Name = "BTNBack";
            this.BTNBack.Size = new System.Drawing.Size(183, 46);
            this.BTNBack.TabIndex = 2;
            this.BTNBack.Text = "Back";
            this.BTNBack.UseVisualStyleBackColor = true;
            this.BTNBack.Click += new System.EventHandler(this.BTNBack_Click);
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistory.Location = new System.Drawing.Point(1, -2);
            this.dataGridViewHistory.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.RowHeadersWidth = 82;
            this.dataGridViewHistory.Size = new System.Drawing.Size(963, 379);
            this.dataGridViewHistory.TabIndex = 3;
            // 
            // BTNClear
            // 
            this.BTNClear.Location = new System.Drawing.Point(398, 392);
            this.BTNClear.Name = "BTNClear";
            this.BTNClear.Size = new System.Drawing.Size(182, 46);
            this.BTNClear.TabIndex = 4;
            this.BTNClear.Text = "Clear All History";
            this.BTNClear.UseVisualStyleBackColor = true;
            this.BTNClear.Click += new System.EventHandler(this.BTNClear_Click);
            // 
            // BTNClearRow
            // 
            this.BTNClearRow.Location = new System.Drawing.Point(201, 392);
            this.BTNClearRow.Name = "BTNClearRow";
            this.BTNClearRow.Size = new System.Drawing.Size(191, 46);
            this.BTNClearRow.TabIndex = 5;
            this.BTNClearRow.Text = "Clear Row";
            this.BTNClearRow.UseVisualStyleBackColor = true;
            this.BTNClearRow.Click += new System.EventHandler(this.BTNClearRow_Click);
            // 
            // BTNAddExample
            // 
            this.BTNAddExample.Location = new System.Drawing.Point(586, 392);
            this.BTNAddExample.Name = "BTNAddExample";
            this.BTNAddExample.Size = new System.Drawing.Size(179, 46);
            this.BTNAddExample.TabIndex = 6;
            this.BTNAddExample.Text = "Add as Example";
            this.BTNAddExample.UseVisualStyleBackColor = true;
            this.BTNAddExample.Click += new System.EventHandler(this.BTNAddExample_Click);
            // 
            // BTNResetExamples
            // 
            this.BTNResetExamples.Location = new System.Drawing.Point(771, 392);
            this.BTNResetExamples.Name = "BTNResetExamples";
            this.BTNResetExamples.Size = new System.Drawing.Size(181, 46);
            this.BTNResetExamples.TabIndex = 7;
            this.BTNResetExamples.Text = "Reset Examples";
            this.BTNResetExamples.UseVisualStyleBackColor = true;
            this.BTNResetExamples.Click += new System.EventHandler(this.BTNResetExamples_Click);
            // 
            // History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 450);
            this.ControlBox = false;
            this.Controls.Add(this.BTNResetExamples);
            this.Controls.Add(this.BTNAddExample);
            this.Controls.Add(this.BTNClearRow);
            this.Controls.Add(this.BTNClear);
            this.Controls.Add(this.dataGridViewHistory);
            this.Controls.Add(this.BTNBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "History";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculation History";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTNBack;
        private System.Windows.Forms.DataGridView dataGridViewHistory;
        private System.Windows.Forms.Button BTNClear;
        private System.Windows.Forms.Button BTNClearRow;
        private System.Windows.Forms.Button BTNAddExample;
        private System.Windows.Forms.Button BTNResetExamples;
    }
}