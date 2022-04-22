namespace S036_OrderSkdApp
{
    partial class F402_OrdMstMaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F402_OrdMstMaint));
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchButton = new System.Windows.Forms.Button();
            this.partsCodeLabel = new System.Windows.Forms.Label();
            this.partsNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsSearchBt = new System.Windows.Forms.Button();
            this.supCodeLabel = new System.Windows.Forms.Label();
            this.supNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.supCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.supSearchBt = new System.Windows.Forms.Button();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.supNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.supCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.searchButton);
            this.panel1.Controls.Add(this.partsCodeLabel);
            this.panel1.Controls.Add(this.partsNameC1TextBox);
            this.panel1.Controls.Add(this.partsCodeC1TextBox);
            this.panel1.Controls.Add(this.partsSearchBt);
            this.panel1.Controls.Add(this.supCodeLabel);
            this.panel1.Controls.Add(this.supNameC1TextBox);
            this.panel1.Controls.Add(this.supCodeC1TextBox);
            this.panel1.Controls.Add(this.supSearchBt);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 70);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // searchButton
            // 
            this.searchButton.AutoSize = true;
            this.searchButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchButton.Location = new System.Drawing.Point(579, 9);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(81, 48);
            this.searchButton.TabIndex = 2;
            this.searchButton.TabStop = false;
            this.searchButton.Text = "検索";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // partsCodeLabel
            // 
            this.partsCodeLabel.AutoSize = true;
            this.partsCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeLabel.Location = new System.Drawing.Point(25, 12);
            this.partsCodeLabel.Name = "partsCodeLabel";
            this.partsCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.partsCodeLabel.TabIndex = 615;
            this.partsCodeLabel.Text = "部品コード";
            // 
            // partsNameC1TextBox
            // 
            this.partsNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.partsNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.partsNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.partsNameC1TextBox.Enabled = false;
            this.partsNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.partsNameC1TextBox.Location = new System.Drawing.Point(330, 9);
            this.partsNameC1TextBox.MaxLength = 12;
            this.partsNameC1TextBox.Name = "partsNameC1TextBox";
            this.partsNameC1TextBox.Size = new System.Drawing.Size(196, 21);
            this.partsNameC1TextBox.TabIndex = 614;
            this.partsNameC1TextBox.TabStop = false;
            this.partsNameC1TextBox.Tag = null;
            // 
            // partsCodeC1TextBox
            // 
            this.partsCodeC1TextBox.BackColor = System.Drawing.Color.White;
            this.partsCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.partsCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.partsCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.partsCodeC1TextBox.Label = this.partsCodeLabel;
            this.partsCodeC1TextBox.Location = new System.Drawing.Point(133, 9);
            this.partsCodeC1TextBox.MaxLength = 15;
            this.partsCodeC1TextBox.Name = "partsCodeC1TextBox";
            this.partsCodeC1TextBox.Size = new System.Drawing.Size(130, 21);
            this.partsCodeC1TextBox.TabIndex = 0;
            this.partsCodeC1TextBox.Tag = null;
            this.partsCodeC1TextBox.TextDetached = true;
            this.partsCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.partsCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.partsCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.partsCodeC1TextBox_Validating);
            // 
            // partsSearchBt
            // 
            this.partsSearchBt.AutoSize = true;
            this.partsSearchBt.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsSearchBt.Location = new System.Drawing.Point(264, 8);
            this.partsSearchBt.Name = "partsSearchBt";
            this.partsSearchBt.Size = new System.Drawing.Size(65, 24);
            this.partsSearchBt.TabIndex = 0;
            this.partsSearchBt.TabStop = false;
            this.partsSearchBt.Text = "検索(F4)";
            this.partsSearchBt.UseVisualStyleBackColor = true;
            this.partsSearchBt.Click += new System.EventHandler(this.partsSearchBt_Click);
            // 
            // supCodeLabel
            // 
            this.supCodeLabel.AutoSize = true;
            this.supCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.supCodeLabel.Location = new System.Drawing.Point(25, 42);
            this.supCodeLabel.Name = "supCodeLabel";
            this.supCodeLabel.Size = new System.Drawing.Size(92, 16);
            this.supCodeLabel.TabIndex = 639;
            this.supCodeLabel.Text = "仕入先コード";
            // 
            // supNameC1TextBox
            // 
            this.supNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.supNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.supNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.supNameC1TextBox.Enabled = false;
            this.supNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.supNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.supNameC1TextBox.Location = new System.Drawing.Point(255, 38);
            this.supNameC1TextBox.MaxLength = 12;
            this.supNameC1TextBox.Name = "supNameC1TextBox";
            this.supNameC1TextBox.Size = new System.Drawing.Size(188, 21);
            this.supNameC1TextBox.TabIndex = 638;
            this.supNameC1TextBox.TabStop = false;
            this.supNameC1TextBox.Tag = null;
            // 
            // supCodeC1TextBox
            // 
            this.supCodeC1TextBox.BackColor = System.Drawing.Color.White;
            this.supCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.supCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.supCodeC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.supCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.supCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.supCodeC1TextBox.Label = this.supCodeLabel;
            this.supCodeC1TextBox.Location = new System.Drawing.Point(133, 39);
            this.supCodeC1TextBox.MaxLength = 6;
            this.supCodeC1TextBox.Name = "supCodeC1TextBox";
            this.supCodeC1TextBox.Size = new System.Drawing.Size(55, 21);
            this.supCodeC1TextBox.TabIndex = 1;
            this.supCodeC1TextBox.Tag = null;
            this.supCodeC1TextBox.TextDetached = true;
            this.supCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.supCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.supCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.supCodeC1TextBox_Validating);
            // 
            // supSearchBt
            // 
            this.supSearchBt.AutoSize = true;
            this.supSearchBt.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.supSearchBt.Location = new System.Drawing.Point(189, 37);
            this.supSearchBt.Name = "supSearchBt";
            this.supSearchBt.Size = new System.Drawing.Size(65, 24);
            this.supSearchBt.TabIndex = 1;
            this.supSearchBt.TabStop = false;
            this.supSearchBt.Text = "検索(F4)";
            this.supSearchBt.UseVisualStyleBackColor = true;
            this.supSearchBt.Click += new System.EventHandler(this.supSearchBt_Click);
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
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 220);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1184, 443);
            this.c1TrueDBGrid.TabIndex = 3;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.AfterColUpdate += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_AfterColUpdate);
            this.c1TrueDBGrid.BeforeColUpdate += new C1.Win.C1TrueDBGrid.BeforeColUpdateEventHandler(this.c1TrueDBGrid_BeforeColUpdate);
            this.c1TrueDBGrid.UnboundColumnFetch += new C1.Win.C1TrueDBGrid.UnboundColumnFetchEventHandler(this.c1TrueDBGrid_UnboundColumnFetch);
            this.c1TrueDBGrid.ButtonClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_ButtonClick);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // F402_OrdMstMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F402_OrdMstMaint";
            this.Text = "F402_OrdMstMaint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F402_OrdMstMaint_FormClosing);
            this.Load += new System.EventHandler(this.F402_OrdMstMaint_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.supNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.supCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button partsSearchBt;
        private System.Windows.Forms.Label partsCodeLabel;
        public C1.Win.C1Input.C1TextBox partsCodeC1TextBox;
        public C1.Win.C1Input.C1TextBox partsNameC1TextBox;
        private System.Windows.Forms.Button supSearchBt;
        private C1.Win.C1Input.C1TextBox supCodeC1TextBox;
        private System.Windows.Forms.Label supCodeLabel;
        private C1.Win.C1Input.C1TextBox supNameC1TextBox;
        private System.Windows.Forms.Button searchButton;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
    }
}