namespace S036_OrderSkdApp
{
    partial class F403_OrdMstDInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F403_OrdMstDInfo));
            this.panel1 = new System.Windows.Forms.Panel();
            this.instrCodeLabel = new System.Windows.Forms.Label();
            this.instrCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instrCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.instrCodeLabel);
            this.panel1.Controls.Add(this.instrCodeC1TextBox);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 42);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // instrCodeLabel
            // 
            this.instrCodeLabel.AutoSize = true;
            this.instrCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instrCodeLabel.Location = new System.Drawing.Point(25, 12);
            this.instrCodeLabel.Name = "instrCodeLabel";
            this.instrCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.instrCodeLabel.TabIndex = 615;
            this.instrCodeLabel.Text = "指示番号";
            // 
            // instrCodeC1TextBox
            // 
            this.instrCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.instrCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.instrCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.instrCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.instrCodeC1TextBox.Enabled = false;
            this.instrCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instrCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.instrCodeC1TextBox.Location = new System.Drawing.Point(110, 9);
            this.instrCodeC1TextBox.MaxLength = 12;
            this.instrCodeC1TextBox.Name = "instrCodeC1TextBox";
            this.instrCodeC1TextBox.Size = new System.Drawing.Size(196, 21);
            this.instrCodeC1TextBox.TabIndex = 614;
            this.instrCodeC1TextBox.TabStop = false;
            this.instrCodeC1TextBox.Tag = null;
            // 
            // c1TrueDBGrid
            // 
            this.c1TrueDBGrid.AllowColMove = false;
            this.c1TrueDBGrid.AllowRowSizing = C1.Win.C1TrueDBGrid.RowSizingEnum.None;
            this.c1TrueDBGrid.AlternatingRows = true;
            this.c1TrueDBGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid.CaptionHeight = 16;
            this.c1TrueDBGrid.ColumnFooters = true;
            this.c1TrueDBGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid.Images"))));
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 192);
            this.c1TrueDBGrid.Name = "c1TrueDBGrid";
            this.c1TrueDBGrid.PreviewInfo.Caption = "印刷プレビューウィンドウ";
            this.c1TrueDBGrid.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.c1TrueDBGrid.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.c1TrueDBGrid.PreviewInfo.ZoomFactor = 75D;
            this.c1TrueDBGrid.PrintInfo.MeasurementDevice = C1.Win.C1TrueDBGrid.PrintInfo.MeasurementDeviceEnum.Screen;
            this.c1TrueDBGrid.PrintInfo.MeasurementPrinterName = null;
            this.c1TrueDBGrid.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("c1TrueDBGrid.PrintInfo.PageSettings")));
            this.c1TrueDBGrid.RecordSelectors = false;
            this.c1TrueDBGrid.RowHeight = 20;
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1184, 472);
            this.c1TrueDBGrid.TabIndex = 3;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // F403_OrdMstDInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F403_OrdMstDInfo";
            this.Text = "F403_OrdMstDInfo";
            this.Load += new System.EventHandler(this.F403_OrdMstDInfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instrCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label instrCodeLabel;
        public C1.Win.C1Input.C1TextBox instrCodeC1TextBox;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
    }
}