namespace S036_OrderSkdApp
{
    partial class F219_DocuReprint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F219_DocuReprint));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.partsCodeLabel = new System.Windows.Forms.Label();
            this.partsNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.partsSearchBt = new System.Windows.Forms.Button();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupCodeC1TextBox);
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.partsCodeLabel);
            this.panel1.Controls.Add(this.partsNameC1TextBox);
            this.panel1.Controls.Add(this.partsCodeC1TextBox);
            this.panel1.Controls.Add(this.partsSearchBt);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 43);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeLabel.Location = new System.Drawing.Point(539, 12);
            this.groupCodeLabel.Name = "groupCodeLabel";
            this.groupCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.groupCodeLabel.TabIndex = 617;
            this.groupCodeLabel.Text = "課別コード";
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
            this.partsNameC1TextBox.Location = new System.Drawing.Point(303, 9);
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
            this.partsCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.partsCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.partsCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.partsCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.partsCodeC1TextBox.Label = this.partsCodeLabel;
            this.partsCodeC1TextBox.Location = new System.Drawing.Point(106, 9);
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
            this.partsSearchBt.Location = new System.Drawing.Point(237, 8);
            this.partsSearchBt.Name = "partsSearchBt";
            this.partsSearchBt.Size = new System.Drawing.Size(65, 24);
            this.partsSearchBt.TabIndex = 0;
            this.partsSearchBt.TabStop = false;
            this.partsSearchBt.Text = "検索(F4)";
            this.partsSearchBt.UseVisualStyleBackColor = true;
            this.partsSearchBt.Click += new System.EventHandler(this.partsSearchBt_Click);
            // 
            // c1TrueDBGrid
            // 
            this.c1TrueDBGrid.AllowColMove = false;
            this.c1TrueDBGrid.AllowRowSizing = C1.Win.C1TrueDBGrid.RowSizingEnum.None;
            this.c1TrueDBGrid.AllowUpdate = false;
            this.c1TrueDBGrid.AlternatingRows = true;
            this.c1TrueDBGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid.CaptionHeight = 16;
            this.c1TrueDBGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid.Images"))));
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 242);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1184, 421);
            this.c1TrueDBGrid.TabIndex = 1;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.UnboundColumnFetch += new C1.Win.C1TrueDBGrid.UnboundColumnFetchEventHandler(this.c1TrueDBGrid_UnboundColumnFetch);
            this.c1TrueDBGrid.ButtonClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_ButtonClick);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(26, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(720, 13);
            this.label1.TabIndex = 616;
            this.label1.Text = "D=ﾃﾞｰﾀ区分  1：一般仕入  2：内部仕入  3：外注仕入  4：出庫  5：客先無償支給  6：有償支給  8：内部売上  D：輸入  S：生産実績 ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(26, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(789, 13);
            this.label2.TabIndex = 617;
            this.label2.Text = "S=出庫区分  2：部品出庫  3：不良処分  4：仕損品  5：部品ｵｰﾀﾞ出庫  6：変更・改造  9：調整出庫  A：設計変更  B：部品切替  X：実施棚" +
    "卸調整";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(843, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(254, 13);
            this.label3.TabIndex = 618;
            this.label3.Text = "在庫=在庫P  空白：素材在庫  Z：完成品在庫";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(843, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(218, 13);
            this.label4.TabIndex = 619;
            this.label4.Text = "有償=有償支給区分  Y：有償  M：無償";
            // 
            // groupCodeC1TextBox
            // 
            this.groupCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeC1TextBox.Enabled = false;
            this.groupCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeC1TextBox.Label = this.groupCodeLabel;
            this.groupCodeC1TextBox.Location = new System.Drawing.Point(621, 10);
            this.groupCodeC1TextBox.MaxLength = 12;
            this.groupCodeC1TextBox.Name = "groupCodeC1TextBox";
            this.groupCodeC1TextBox.Size = new System.Drawing.Size(98, 21);
            this.groupCodeC1TextBox.TabIndex = 619;
            this.groupCodeC1TextBox.TabStop = false;
            this.groupCodeC1TextBox.Tag = null;
            // 
            // F219_DocuReprint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F219_DocuReprint";
            this.Text = "F219_DocuReprint";
            this.Load += new System.EventHandler(this.F219_DocuReprint_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partsNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1TextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button partsSearchBt;
        private System.Windows.Forms.Label partsCodeLabel;
        public C1.Win.C1Input.C1TextBox partsCodeC1TextBox;
        public C1.Win.C1Input.C1TextBox partsNameC1TextBox;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Label groupCodeLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private C1.Win.C1Input.C1TextBox groupCodeC1TextBox;
    }
}