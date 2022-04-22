namespace S036_OrderSkdApp
{
    partial class F406_ProcessUnitPriceInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F406_ProcessUnitPriceInfo));
            this.parts3Label = new System.Windows.Forms.Label();
            this.parts1Label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.drawingCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.drawingCodeLabel = new System.Windows.Forms.Label();
            this.partsNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsCodeLabel = new System.Windows.Forms.Label();
            this.partsCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsSearchBt = new System.Windows.Forms.Button();
            this.parts2Label = new System.Windows.Forms.Label();
            this.c1TrueDBGrid2 = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.c1TrueDBGrid1 = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.c1TrueDBGrid3 = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawingCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid3)).BeginInit();
            this.SuspendLayout();
            // 
            // parts3Label
            // 
            this.parts3Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.parts3Label.AutoSize = true;
            this.parts3Label.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.parts3Label.Location = new System.Drawing.Point(5, 508);
            this.parts3Label.Name = "parts3Label";
            this.parts3Label.Size = new System.Drawing.Size(77, 16);
            this.parts3Label.TabIndex = 588;
            this.parts3Label.Text = "<子部品>";
            // 
            // parts1Label
            // 
            this.parts1Label.AutoSize = true;
            this.parts1Label.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.parts1Label.Location = new System.Drawing.Point(5, 233);
            this.parts1Label.Name = "parts1Label";
            this.parts1Label.Size = new System.Drawing.Size(77, 16);
            this.parts1Label.TabIndex = 585;
            this.parts1Label.Text = "<親部品>";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.drawingCodeC1TextBox);
            this.panel1.Controls.Add(this.drawingCodeLabel);
            this.panel1.Controls.Add(this.partsNameC1TextBox);
            this.panel1.Controls.Add(this.partsCodeLabel);
            this.panel1.Controls.Add(this.partsCodeC1TextBox);
            this.panel1.Controls.Add(this.partsSearchBt);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1172, 73);
            this.panel1.TabIndex = 584;
            this.panel1.TabStop = true;
            // 
            // drawingCodeC1TextBox
            // 
            this.drawingCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.drawingCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.drawingCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.drawingCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.drawingCodeC1TextBox.Label = this.drawingCodeLabel;
            this.drawingCodeC1TextBox.Location = new System.Drawing.Point(130, 35);
            this.drawingCodeC1TextBox.MaxLength = 12;
            this.drawingCodeC1TextBox.Name = "drawingCodeC1TextBox";
            this.drawingCodeC1TextBox.Size = new System.Drawing.Size(130, 21);
            this.drawingCodeC1TextBox.TabIndex = 2;
            this.drawingCodeC1TextBox.Tag = null;
            this.drawingCodeC1TextBox.TextDetached = true;
            this.drawingCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.drawingCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.drawingCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.drawingCodeC1TextBox_Validating);
            // 
            // drawingCodeLabel
            // 
            this.drawingCodeLabel.AutoSize = true;
            this.drawingCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.drawingCodeLabel.Location = new System.Drawing.Point(20, 37);
            this.drawingCodeLabel.Name = "drawingCodeLabel";
            this.drawingCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.drawingCodeLabel.TabIndex = 552;
            this.drawingCodeLabel.Text = "図面番号";
            // 
            // partsNameC1TextBox
            // 
            this.partsNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.partsNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.partsNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.partsNameC1TextBox.Enabled = false;
            this.partsNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.partsNameC1TextBox.Label = this.partsCodeLabel;
            this.partsNameC1TextBox.Location = new System.Drawing.Point(336, 9);
            this.partsNameC1TextBox.MaxLength = 12;
            this.partsNameC1TextBox.Name = "partsNameC1TextBox";
            this.partsNameC1TextBox.Size = new System.Drawing.Size(185, 21);
            this.partsNameC1TextBox.TabIndex = 437;
            this.partsNameC1TextBox.TabStop = false;
            this.partsNameC1TextBox.Tag = null;
            // 
            // partsCodeLabel
            // 
            this.partsCodeLabel.AutoSize = true;
            this.partsCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeLabel.Location = new System.Drawing.Point(20, 12);
            this.partsCodeLabel.Name = "partsCodeLabel";
            this.partsCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.partsCodeLabel.TabIndex = 435;
            this.partsCodeLabel.Text = "部品コード";
            // 
            // partsCodeC1TextBox
            // 
            this.partsCodeC1TextBox.BackColor = System.Drawing.Color.White;
            this.partsCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.partsCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.partsCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.partsCodeC1TextBox.Label = this.partsCodeLabel;
            this.partsCodeC1TextBox.Location = new System.Drawing.Point(130, 9);
            this.partsCodeC1TextBox.MaxLength = 12;
            this.partsCodeC1TextBox.Name = "partsCodeC1TextBox";
            this.partsCodeC1TextBox.Size = new System.Drawing.Size(130, 21);
            this.partsCodeC1TextBox.TabIndex = 1;
            this.partsCodeC1TextBox.Tag = null;
            this.partsCodeC1TextBox.TextDetached = true;
            this.partsCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.partsCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.partsCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.partsCodeC1TextBox_Validating);
            this.partsCodeC1TextBox.Validated += new System.EventHandler(this.partsCodeC1TextBox_Validated);
            // 
            // partsSearchBt
            // 
            this.partsSearchBt.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsSearchBt.Location = new System.Drawing.Point(266, 8);
            this.partsSearchBt.Name = "partsSearchBt";
            this.partsSearchBt.Size = new System.Drawing.Size(64, 24);
            this.partsSearchBt.TabIndex = 436;
            this.partsSearchBt.TabStop = false;
            this.partsSearchBt.Text = "検索(F4)";
            this.partsSearchBt.UseVisualStyleBackColor = true;
            this.partsSearchBt.Click += new System.EventHandler(this.partsSearchBt_Click);
            // 
            // parts2Label
            // 
            this.parts2Label.AutoSize = true;
            this.parts2Label.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.parts2Label.Location = new System.Drawing.Point(5, 394);
            this.parts2Label.Name = "parts2Label";
            this.parts2Label.Size = new System.Drawing.Size(94, 16);
            this.parts2Label.TabIndex = 590;
            this.parts2Label.Text = "<対象部品>";
            // 
            // c1TrueDBGrid2
            // 
            this.c1TrueDBGrid2.AllowUpdate = false;
            this.c1TrueDBGrid2.AlternatingRows = true;
            this.c1TrueDBGrid2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid2.CaptionHeight = 18;
            this.c1TrueDBGrid2.FetchRowStyles = true;
            this.c1TrueDBGrid2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid2.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid2.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid2.Images"))));
            this.c1TrueDBGrid2.LinesPerRow = 4;
            this.c1TrueDBGrid2.Location = new System.Drawing.Point(0, 413);
            this.c1TrueDBGrid2.Name = "c1TrueDBGrid2";
            this.c1TrueDBGrid2.PreviewInfo.Caption = "印刷プレビューウィンドウ";
            this.c1TrueDBGrid2.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.c1TrueDBGrid2.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.c1TrueDBGrid2.PreviewInfo.ZoomFactor = 75D;
            this.c1TrueDBGrid2.PrintInfo.MeasurementDevice = C1.Win.C1TrueDBGrid.PrintInfo.MeasurementDeviceEnum.Screen;
            this.c1TrueDBGrid2.PrintInfo.MeasurementPrinterName = null;
            this.c1TrueDBGrid2.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("c1TrueDBGrid2.PrintInfo.PageSettings")));
            this.c1TrueDBGrid2.RecordSelectors = false;
            this.c1TrueDBGrid2.RowHeight = 20;
            this.c1TrueDBGrid2.Size = new System.Drawing.Size(1172, 81);
            this.c1TrueDBGrid2.TabIndex = 589;
            this.c1TrueDBGrid2.TabStop = false;
            this.c1TrueDBGrid2.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid2.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid2.PropBag = resources.GetString("c1TrueDBGrid2.PropBag");
            // 
            // c1TrueDBGrid1
            // 
            this.c1TrueDBGrid1.AllowUpdate = false;
            this.c1TrueDBGrid1.AlternatingRows = true;
            this.c1TrueDBGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid1.CaptionHeight = 18;
            this.c1TrueDBGrid1.FetchRowStyles = true;
            this.c1TrueDBGrid1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid1.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid1.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid1.Images"))));
            this.c1TrueDBGrid1.LinesPerRow = 4;
            this.c1TrueDBGrid1.Location = new System.Drawing.Point(0, 252);
            this.c1TrueDBGrid1.Name = "c1TrueDBGrid1";
            this.c1TrueDBGrid1.PreviewInfo.Caption = "印刷プレビューウィンドウ";
            this.c1TrueDBGrid1.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.c1TrueDBGrid1.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.c1TrueDBGrid1.PreviewInfo.ZoomFactor = 75D;
            this.c1TrueDBGrid1.PrintInfo.MeasurementDevice = C1.Win.C1TrueDBGrid.PrintInfo.MeasurementDeviceEnum.Screen;
            this.c1TrueDBGrid1.PrintInfo.MeasurementPrinterName = null;
            this.c1TrueDBGrid1.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("c1TrueDBGrid1.PrintInfo.PageSettings")));
            this.c1TrueDBGrid1.RecordSelectors = false;
            this.c1TrueDBGrid1.RowHeight = 20;
            this.c1TrueDBGrid1.Size = new System.Drawing.Size(1172, 130);
            this.c1TrueDBGrid1.TabIndex = 593;
            this.c1TrueDBGrid1.TabStop = false;
            this.c1TrueDBGrid1.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid1.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.c1TrueDBGrid1_MouseDown);
            this.c1TrueDBGrid1.PropBag = resources.GetString("c1TrueDBGrid1.PropBag");
            // 
            // c1TrueDBGrid3
            // 
            this.c1TrueDBGrid3.AllowUpdate = false;
            this.c1TrueDBGrid3.AlternatingRows = true;
            this.c1TrueDBGrid3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid3.CaptionHeight = 18;
            this.c1TrueDBGrid3.FetchRowStyles = true;
            this.c1TrueDBGrid3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid3.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid3.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid3.Images"))));
            this.c1TrueDBGrid3.LinesPerRow = 4;
            this.c1TrueDBGrid3.Location = new System.Drawing.Point(0, 527);
            this.c1TrueDBGrid3.Name = "c1TrueDBGrid3";
            this.c1TrueDBGrid3.PreviewInfo.Caption = "印刷プレビューウィンドウ";
            this.c1TrueDBGrid3.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.c1TrueDBGrid3.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.c1TrueDBGrid3.PreviewInfo.ZoomFactor = 75D;
            this.c1TrueDBGrid3.PrintInfo.MeasurementDevice = C1.Win.C1TrueDBGrid.PrintInfo.MeasurementDeviceEnum.Screen;
            this.c1TrueDBGrid3.PrintInfo.MeasurementPrinterName = null;
            this.c1TrueDBGrid3.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("c1TrueDBGrid3.PrintInfo.PageSettings")));
            this.c1TrueDBGrid3.RecordSelectors = false;
            this.c1TrueDBGrid3.RowHeight = 20;
            this.c1TrueDBGrid3.Size = new System.Drawing.Size(1172, 136);
            this.c1TrueDBGrid3.TabIndex = 594;
            this.c1TrueDBGrid3.TabStop = false;
            this.c1TrueDBGrid3.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid3.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.c1TrueDBGrid3_MouseDown);
            this.c1TrueDBGrid3.PropBag = resources.GetString("c1TrueDBGrid3.PropBag");
            // 
            // F406_ProcessUnitPriceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid3);
            this.Controls.Add(this.c1TrueDBGrid1);
            this.Controls.Add(this.parts2Label);
            this.Controls.Add(this.c1TrueDBGrid2);
            this.Controls.Add(this.parts3Label);
            this.Controls.Add(this.parts1Label);
            this.Controls.Add(this.panel1);
            this.Name = "F406_ProcessUnitPriceInfo";
            this.Text = "F406_ProcessUnitPriceInfo";
            this.Load += new System.EventHandler(this.F406_ProcessUnitPriceInfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawingCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label parts3Label;
        private System.Windows.Forms.Label parts1Label;
        private System.Windows.Forms.Panel panel1;
        public C1.Win.C1Input.C1TextBox drawingCodeC1TextBox;
        private System.Windows.Forms.Label partsCodeLabel;
        private System.Windows.Forms.Label drawingCodeLabel;
        public C1.Win.C1Input.C1TextBox partsNameC1TextBox;
        public C1.Win.C1Input.C1TextBox partsCodeC1TextBox;
        private System.Windows.Forms.Button partsSearchBt;
        private System.Windows.Forms.Label parts2Label;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid2;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid1;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid3;
    }
}