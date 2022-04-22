namespace S036_OrderSkdApp
{
    partial class F210_InsideInProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F210_InsideInProcess));
            this.panel1 = new System.Windows.Forms.Panel();
            this.sendGroupCodeLabel = new System.Windows.Forms.Label();
            this.sendGroupCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeLabel = new System.Windows.Forms.Label();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.groupNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.groupCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sendGroupCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.sendGroupCodeLabel);
            this.panel1.Controls.Add(this.sendGroupCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeLabel);
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.groupNameC1TextBox);
            this.panel1.Controls.Add(this.groupCodeC1ComboBox);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 43);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // sendGroupCodeLabel
            // 
            this.sendGroupCodeLabel.AutoSize = true;
            this.sendGroupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.sendGroupCodeLabel.Location = new System.Drawing.Point(684, 13);
            this.sendGroupCodeLabel.Name = "sendGroupCodeLabel";
            this.sendGroupCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.sendGroupCodeLabel.TabIndex = 600;
            this.sendGroupCodeLabel.Text = "送信部門";
            // 
            // sendGroupCodeC1TextBox
            // 
            this.sendGroupCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.sendGroupCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sendGroupCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.sendGroupCodeC1TextBox.Enabled = false;
            this.sendGroupCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sendGroupCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.sendGroupCodeC1TextBox.Label = this.sendGroupCodeLabel;
            this.sendGroupCodeC1TextBox.Location = new System.Drawing.Point(765, 10);
            this.sendGroupCodeC1TextBox.MaxLength = 12;
            this.sendGroupCodeC1TextBox.Name = "sendGroupCodeC1TextBox";
            this.sendGroupCodeC1TextBox.Size = new System.Drawing.Size(55, 21);
            this.sendGroupCodeC1TextBox.TabIndex = 599;
            this.sendGroupCodeC1TextBox.TabStop = false;
            this.sendGroupCodeC1TextBox.Tag = null;
            // 
            // batchCodeC1TextBox
            // 
            this.batchCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.batchCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.batchCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.batchCodeC1TextBox.EmptyAsNull = true;
            this.batchCodeC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.batchCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.batchCodeC1TextBox.Label = this.batchCodeLabel;
            this.batchCodeC1TextBox.Location = new System.Drawing.Point(489, 10);
            this.batchCodeC1TextBox.MaxLength = 10;
            this.batchCodeC1TextBox.Name = "batchCodeC1TextBox";
            this.batchCodeC1TextBox.Size = new System.Drawing.Size(133, 21);
            this.batchCodeC1TextBox.TabIndex = 1;
            this.batchCodeC1TextBox.Tag = null;
            this.batchCodeC1TextBox.TextDetached = true;
            this.batchCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.batchCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.batchCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.batchCodeC1TextBox_Validating);
            this.batchCodeC1TextBox.Validated += new System.EventHandler(this.batchCodeC1TextBox_Validated);
            // 
            // batchCodeLabel
            // 
            this.batchCodeLabel.AutoSize = true;
            this.batchCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeLabel.Location = new System.Drawing.Point(407, 13);
            this.batchCodeLabel.Name = "batchCodeLabel";
            this.batchCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.batchCodeLabel.TabIndex = 585;
            this.batchCodeLabel.Text = "バッチ番号";
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.groupCodeLabel.Location = new System.Drawing.Point(24, 13);
            this.groupCodeLabel.Name = "groupCodeLabel";
            this.groupCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.groupCodeLabel.TabIndex = 557;
            this.groupCodeLabel.Text = "組立部門";
            // 
            // groupNameC1TextBox
            // 
            this.groupNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupNameC1TextBox.Enabled = false;
            this.groupNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupNameC1TextBox.Location = new System.Drawing.Point(171, 10);
            this.groupNameC1TextBox.MaxLength = 12;
            this.groupNameC1TextBox.Name = "groupNameC1TextBox";
            this.groupNameC1TextBox.Size = new System.Drawing.Size(177, 21);
            this.groupNameC1TextBox.TabIndex = 556;
            this.groupNameC1TextBox.TabStop = false;
            this.groupNameC1TextBox.Tag = null;
            // 
            // groupCodeC1ComboBox
            // 
            this.groupCodeC1ComboBox.AllowSpinLoop = false;
            this.groupCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.groupCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.groupCodeC1ComboBox.BorderColor = System.Drawing.Color.Red;
            this.groupCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.groupCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeC1ComboBox.DropDownWidth = -1;
            this.groupCodeC1ComboBox.ErrorInfo.ShowErrorMessage = false;
            this.groupCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeC1ComboBox.GapHeight = 0;
            this.groupCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.groupCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeC1ComboBox.ItemsDisplayMember = "";
            this.groupCodeC1ComboBox.ItemsValueMember = "";
            this.groupCodeC1ComboBox.Label = this.groupCodeLabel;
            this.groupCodeC1ComboBox.Location = new System.Drawing.Point(103, 10);
            this.groupCodeC1ComboBox.MaxLength = 4;
            this.groupCodeC1ComboBox.Name = "groupCodeC1ComboBox";
            this.groupCodeC1ComboBox.Size = new System.Drawing.Size(66, 21);
            this.groupCodeC1ComboBox.TabIndex = 0;
            this.groupCodeC1ComboBox.Tag = null;
            this.groupCodeC1ComboBox.TextDetached = true;
            this.groupCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.groupCodeC1ComboBox_SelectedIndexChanged);
            this.groupCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.groupCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.groupCodeC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.groupCodeC1ComboBox.Validated += new System.EventHandler(this.groupCodeC1ComboBox_Validated);
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
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 396);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1183, 266);
            this.c1TrueDBGrid.TabIndex = 1;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.UnboundColumnFetch += new C1.Win.C1TrueDBGrid.UnboundColumnFetchEventHandler(this.c1TrueDBGrid_UnboundColumnFetch);
            this.c1TrueDBGrid.ButtonClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_ButtonClick);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.statusC1TextBox);
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(0, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1183, 203);
            this.panel2.TabIndex = 5;
            // 
            // statusC1TextBox
            // 
            this.statusC1TextBox.AutoSize = false;
            this.statusC1TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.statusC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.statusC1TextBox.EmptyAsNull = true;
            this.statusC1TextBox.Enabled = false;
            this.statusC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.statusC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.statusC1TextBox.Label = this.statusLabel;
            this.statusC1TextBox.Location = new System.Drawing.Point(9, 25);
            this.statusC1TextBox.MaxLength = 10;
            this.statusC1TextBox.Multiline = true;
            this.statusC1TextBox.Name = "statusC1TextBox";
            this.statusC1TextBox.Size = new System.Drawing.Size(550, 171);
            this.statusC1TextBox.TabIndex = 283;
            this.statusC1TextBox.TabStop = false;
            this.statusC1TextBox.Tag = null;
            this.statusC1TextBox.TextDetached = true;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusLabel.Location = new System.Drawing.Point(8, 6);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(94, 16);
            this.statusLabel.TabIndex = 282;
            this.statusLabel.Text = "<状況表示>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(559, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(623, 48);
            this.label1.TabIndex = 558;
            this.label1.Text = "※注意　\r\n　　2013/5/20より、社内移行受付はバーコード端末を利用してリアルタイム処理を実施。　\r\n　　塗装以外は一括取込処理しないようにしてください。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(559, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(606, 48);
            this.label2.TabIndex = 559;
            this.label2.Text = "※注意　\r\n　　組立部門を変更する時は、一度この画面を終了し再度組立部門を選んでください。　\r\n　　同じバッチ番号が複数の送信部門になっているときは、一括で処理さ" +
    "れます。";
            // 
            // F210_InsideInProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F210_InsideInProcess";
            this.Text = "F210_InsideInProcess";
            this.Load += new System.EventHandler(this.F210_InsideInProcess_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sendGroupCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusC1TextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Label groupCodeLabel;
        private C1.Win.C1Input.C1TextBox groupNameC1TextBox;
        private C1.Win.C1Input.C1ComboBox groupCodeC1ComboBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label statusLabel;
        private C1.Win.C1Input.C1TextBox batchCodeC1TextBox;
        private System.Windows.Forms.Label batchCodeLabel;
        private C1.Win.C1Input.C1TextBox statusC1TextBox;
        private System.Windows.Forms.Label sendGroupCodeLabel;
        private C1.Win.C1Input.C1TextBox sendGroupCodeC1TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}