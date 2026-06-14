namespace Photo_Reviewer
{
    partial class FormReviewer
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
            this.panelPhoto = new System.Windows.Forms.Panel();
            this.textBoxJump = new System.Windows.Forms.TextBox();
            this.progressBarPosition = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelDeleted = new System.Windows.Forms.Label();
            this.labelFlagged = new System.Windows.Forms.Label();
            this.labelFolder = new System.Windows.Forms.Label();
            this.propertyGridPhoto = new System.Windows.Forms.PropertyGrid();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panelPhoto.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPhoto
            // 
            this.tableLayoutPanel.SetColumnSpan(this.panelPhoto, 4);
            this.panelPhoto.Controls.Add(this.textBoxJump);
            this.panelPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPhoto.Location = new System.Drawing.Point(3, 35);
            this.panelPhoto.Name = "panelPhoto";
            this.panelPhoto.Size = new System.Drawing.Size(478, 523);
            this.panelPhoto.TabIndex = 0;
            this.panelPhoto.TabStop = true;
            this.panelPhoto.Click += new System.EventHandler(this.panelPhoto_Click);
            this.panelPhoto.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPhoto_Paint);
            // 
            // textBoxJump
            // 
            this.textBoxJump.Location = new System.Drawing.Point(9, 3);
            this.textBoxJump.Name = "textBoxJump";
            this.textBoxJump.Size = new System.Drawing.Size(100, 23);
            this.textBoxJump.TabIndex = 1;
            this.textBoxJump.Visible = false;
            this.textBoxJump.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxJump_KeyDown);
            // 
            // progressBarPosition
            // 
            this.progressBarPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarPosition.Location = new System.Drawing.Point(487, 3);
            this.progressBarPosition.Name = "progressBarPosition";
            this.progressBarPosition.Size = new System.Drawing.Size(294, 26);
            this.progressBarPosition.TabIndex = 2;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.Controls.Add(this.labelDeleted, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelFlagged, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelFolder, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panelPhoto, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.progressBarPosition, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.propertyGridPhoto, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonBrowse, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(784, 561);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelDeleted
            // 
            this.labelDeleted.AutoSize = true;
            this.labelDeleted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDeleted.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeleted.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelDeleted.Location = new System.Drawing.Point(382, 0);
            this.labelDeleted.Name = "labelDeleted";
            this.labelDeleted.Size = new System.Drawing.Size(99, 32);
            this.labelDeleted.TabIndex = 6;
            this.labelDeleted.Text = "Deleted";
            this.labelDeleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDeleted.UseMnemonic = false;
            this.labelDeleted.Click += new System.EventHandler(this.labelState_Click);
            // 
            // labelFlagged
            // 
            this.labelFlagged.AutoSize = true;
            this.labelFlagged.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFlagged.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlagged.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelFlagged.Location = new System.Drawing.Point(276, 0);
            this.labelFlagged.Name = "labelFlagged";
            this.labelFlagged.Size = new System.Drawing.Size(100, 32);
            this.labelFlagged.TabIndex = 4;
            this.labelFlagged.Text = "Flagged";
            this.labelFlagged.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelFlagged.UseMnemonic = false;
            this.labelFlagged.Click += new System.EventHandler(this.labelState_Click);
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFolder.Location = new System.Drawing.Point(3, 0);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(186, 32);
            this.labelFolder.TabIndex = 0;
            this.labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFolder.UseMnemonic = false;
            // 
            // propertyGridPhoto
            // 
            this.propertyGridPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridPhoto.Location = new System.Drawing.Point(487, 35);
            this.propertyGridPhoto.Name = "propertyGridPhoto";
            this.propertyGridPhoto.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridPhoto.Size = new System.Drawing.Size(294, 523);
            this.propertyGridPhoto.TabIndex = 3;
            this.propertyGridPhoto.ToolbarVisible = false;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBrowse.Location = new System.Drawing.Point(195, 3);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 26);
            this.buttonBrowse.TabIndex = 5;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // FormReviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormReviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Photo Reviewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormReviewer_KeyDown);
            this.panelPhoto.ResumeLayout(false);
            this.panelPhoto.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPhoto;
        private System.Windows.Forms.ProgressBar progressBarPosition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.TextBox textBoxJump;
        private System.Windows.Forms.PropertyGrid propertyGridPhoto;
        private System.Windows.Forms.Label labelFlagged;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label labelDeleted;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

