namespace S036_OrderSkdApp
{
    partial class F211_CompanyDelivNotProcessList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F211_CompanyDelivNotProcessList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.groupCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeLabel = new System.Windows.Forms.Label();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.groupCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeLabel);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 37);
            this.panel1.TabIndex = 0;
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.groupCodeLabel.Location = new System.Drawing.Point(208, 10);
            this.groupCodeLabel.Name = "groupCodeLabel";
            this.groupCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.groupCodeLabel.TabIndex = 604;
            this.groupCodeLabel.Text = "組立部門";
            // 
            // groupCodeC1TextBox
            // 
            this.groupCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.groupCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeC1TextBox.Enabled = false;
            this.groupCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeC1TextBox.Label = this.groupCodeLabel;
            this.groupCodeC1TextBox.Location = new System.Drawing.Point(285, 7);
            this.groupCodeC1TextBox.MaxLength = 12;
            this.groupCodeC1TextBox.Name = "groupCodeC1TextBox";
            this.groupCodeC1TextBox.Size = new System.Drawing.Size(55, 21);
            this.groupCodeC1TextBox.TabIndex = 603;
            this.groupCodeC1TextBox.TabStop = false;
            this.groupCodeC1TextBox.Tag = null;
            // 
            // batchCodeC1TextBox
            // 
            this.batchCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.batchCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.batchCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.batchCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.batchCodeC1TextBox.EmptyAsNull = true;
            this.batchCodeC1TextBox.Enabled = false;
            this.batchCodeC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.batchCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.batchCodeC1TextBox.Label = this.batchCodeLabel;
            this.batchCodeC1TextBox.Location = new System.Drawing.Point(111, 7);
            this.batchCodeC1TextBox.MaxLength = 10;
            this.batchCodeC1TextBox.Name = "batchCodeC1TextBox";
            this.batchCodeC1TextBox.Size = new System.Drawing.Size(52, 21);
            this.batchCodeC1TextBox.TabIndex = 601;
            this.batchCodeC1TextBox.TabStop = false;
            this.batchCodeC1TextBox.Tag = null;
            this.batchCodeC1TextBox.TextDetached = true;
            // 
            // batchCodeLabel
            // 
            this.batchCodeLabel.AutoSize = true;
            this.batchCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeLabel.Location = new System.Drawing.Point(24, 10);
            this.batchCodeLabel.Name = "batchCodeLabel";
            this.batchCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.batchCodeLabel.TabIndex = 602;
            this.batchCodeLabel.Text = "バッチ番号";
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
            this.c1TrueDBGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid.Images"))));
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 187);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1184, 479);
            this.c1TrueDBGrid.TabIndex = 0;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.AfterColUpdate += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_AfterColUpdate);
            this.c1TrueDBGrid.BeforeColUpdate += new C1.Win.C1TrueDBGrid.BeforeColUpdateEventHandler(this.c1TrueDBGrid_BeforeColUpdate);
            this.c1TrueDBGrid.UnboundColumnFetch += new C1.Win.C1TrueDBGrid.UnboundColumnFetchEventHandler(this.c1TrueDBGrid_UnboundColumnFetch);
            this.c1TrueDBGrid.ButtonClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_ButtonClick);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // F211_CompanyDelivNotProcessList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F211_CompanyDelivNotProcessList";
            this.Text = "F211_CompanyDelivNotProcessList";
            this.Load += new System.EventHandler(this.F211_CompanyDelivNotProcessList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Label groupCodeLabel;
        private C1.Win.C1Input.C1TextBox groupCodeC1TextBox;
        private C1.Win.C1Input.C1TextBox batchCodeC1TextBox;
        private System.Windows.Forms.Label batchCodeLabel;
    }
}