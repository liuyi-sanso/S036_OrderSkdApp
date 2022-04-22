namespace S036_OrderSkdApp
{
    partial class F211_CompanyDelivProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F211_CompanyDelivProcess));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.dateLabel = new System.Windows.Forms.Label();
            this.sendGroupCodeLabel = new System.Windows.Forms.Label();
            this.sendGroupCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeLabel = new System.Windows.Forms.Label();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.groupNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.groupCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkListReprintButton = new System.Windows.Forms.Button();
            this.batchCodeReprintC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.batchCodeReprintLabel = new System.Windows.Forms.Label();
            this.inPossibleListButton = new System.Windows.Forms.Button();
            this.notProcesseListButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.errorListButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sendGroupCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusC1TextBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeReprintC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dateC1DateEdit);
            this.panel1.Controls.Add(this.dateLabel);
            this.panel1.Controls.Add(this.sendGroupCodeLabel);
            this.panel1.Controls.Add(this.sendGroupCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeC1TextBox);
            this.panel1.Controls.Add(this.batchCodeLabel);
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.groupNameC1TextBox);
            this.panel1.Controls.Add(this.groupCodeC1ComboBox);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 100);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // dateC1DateEdit
            // 
            this.dateC1DateEdit.AllowSpinLoop = false;
            this.dateC1DateEdit.BorderColor = System.Drawing.Color.Red;
            this.dateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dateC1DateEdit.Calendar.DayNamesFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateC1DateEdit.Calendar.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.dateC1DateEdit.Calendar.TitleFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.dateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.dateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.dateC1DateEdit.DisplayFormat.NullText = "――/―/―";
            this.dateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.dateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.dateC1DateEdit.EmptyAsNull = true;
            this.dateC1DateEdit.ErrorInfo.ShowErrorMessage = false;
            this.dateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.dateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.dateC1DateEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dateC1DateEdit.Label = this.dateLabel;
            this.dateC1DateEdit.Location = new System.Drawing.Point(107, 39);
            this.dateC1DateEdit.Name = "dateC1DateEdit";
            this.dateC1DateEdit.Size = new System.Drawing.Size(109, 21);
            this.dateC1DateEdit.TabIndex = 1;
            this.dateC1DateEdit.Tag = null;
            this.dateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.dateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.dateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.dateC1DateEdit.Validating += new System.ComponentModel.CancelEventHandler(this.dateC1DateEdit_Validating);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.dateLabel.Location = new System.Drawing.Point(20, 42);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(72, 16);
            this.dateLabel.TabIndex = 602;
            this.dateLabel.Text = "納入日付";
            // 
            // sendGroupCodeLabel
            // 
            this.sendGroupCodeLabel.AutoSize = true;
            this.sendGroupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.sendGroupCodeLabel.Location = new System.Drawing.Point(259, 72);
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
            this.sendGroupCodeC1TextBox.Location = new System.Drawing.Point(336, 69);
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
            this.batchCodeC1TextBox.Location = new System.Drawing.Point(107, 69);
            this.batchCodeC1TextBox.MaxLength = 10;
            this.batchCodeC1TextBox.Name = "batchCodeC1TextBox";
            this.batchCodeC1TextBox.Size = new System.Drawing.Size(109, 21);
            this.batchCodeC1TextBox.TabIndex = 2;
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
            this.batchCodeLabel.Location = new System.Drawing.Point(20, 72);
            this.batchCodeLabel.Name = "batchCodeLabel";
            this.batchCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.batchCodeLabel.TabIndex = 585;
            this.batchCodeLabel.Text = "バッチ番号";
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.groupCodeLabel.Location = new System.Drawing.Point(20, 12);
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
            this.groupNameC1TextBox.Location = new System.Drawing.Point(179, 9);
            this.groupNameC1TextBox.MaxLength = 12;
            this.groupNameC1TextBox.Name = "groupNameC1TextBox";
            this.groupNameC1TextBox.Size = new System.Drawing.Size(212, 21);
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
            this.groupCodeC1ComboBox.Location = new System.Drawing.Point(107, 9);
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
            this.c1TrueDBGrid.Location = new System.Drawing.Point(420, 353);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(763, 309);
            this.c1TrueDBGrid.TabIndex = 1;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.UnboundColumnFetch += new C1.Win.C1TrueDBGrid.UnboundColumnFetchEventHandler(this.c1TrueDBGrid_UnboundColumnFetch);
            this.c1TrueDBGrid.ButtonClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.c1TrueDBGrid_ButtonClick);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
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
            // statusC1TextBox
            // 
            this.statusC1TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.statusC1TextBox.Size = new System.Drawing.Size(742, 171);
            this.statusC1TextBox.TabIndex = 283;
            this.statusC1TextBox.TabStop = false;
            this.statusC1TextBox.Tag = null;
            this.statusC1TextBox.TextDetached = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.statusC1TextBox);
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(420, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(763, 203);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.checkListReprintButton);
            this.panel3.Controls.Add(this.batchCodeReprintC1TextBox);
            this.panel3.Controls.Add(this.inPossibleListButton);
            this.panel3.Controls.Add(this.batchCodeReprintLabel);
            this.panel3.Controls.Add(this.notProcesseListButton);
            this.panel3.Controls.Add(this.startButton);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.errorListButton);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.updateButton);
            this.panel3.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel3.ForeColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(0, 250);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(414, 412);
            this.panel3.TabIndex = 1;
            this.panel3.TabStop = true;
            // 
            // checkListReprintButton
            // 
            this.checkListReprintButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkListReprintButton.Location = new System.Drawing.Point(237, 374);
            this.checkListReprintButton.Name = "checkListReprintButton";
            this.checkListReprintButton.Size = new System.Drawing.Size(146, 28);
            this.checkListReprintButton.TabIndex = 292;
            this.checkListReprintButton.TabStop = false;
            this.checkListReprintButton.Text = "チェックリスト再発行";
            this.checkListReprintButton.UseVisualStyleBackColor = true;
            this.checkListReprintButton.Click += new System.EventHandler(this.checkListReprintButton_Click);
            // 
            // batchCodeReprintC1TextBox
            // 
            this.batchCodeReprintC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.batchCodeReprintC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.batchCodeReprintC1TextBox.EmptyAsNull = true;
            this.batchCodeReprintC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.batchCodeReprintC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeReprintC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.batchCodeReprintC1TextBox.Label = this.batchCodeReprintLabel;
            this.batchCodeReprintC1TextBox.Location = new System.Drawing.Point(107, 377);
            this.batchCodeReprintC1TextBox.MaxLength = 10;
            this.batchCodeReprintC1TextBox.Name = "batchCodeReprintC1TextBox";
            this.batchCodeReprintC1TextBox.Size = new System.Drawing.Size(109, 21);
            this.batchCodeReprintC1TextBox.TabIndex = 5;
            this.batchCodeReprintC1TextBox.Tag = null;
            this.batchCodeReprintC1TextBox.TextDetached = true;
            this.batchCodeReprintC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.batchCodeReprintC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // batchCodeReprintLabel
            // 
            this.batchCodeReprintLabel.AutoSize = true;
            this.batchCodeReprintLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.batchCodeReprintLabel.Location = new System.Drawing.Point(20, 380);
            this.batchCodeReprintLabel.Name = "batchCodeReprintLabel";
            this.batchCodeReprintLabel.Size = new System.Drawing.Size(76, 16);
            this.batchCodeReprintLabel.TabIndex = 604;
            this.batchCodeReprintLabel.Text = "バッチ番号";
            // 
            // inPossibleListButton
            // 
            this.inPossibleListButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.inPossibleListButton.Location = new System.Drawing.Point(90, 220);
            this.inPossibleListButton.Name = "inPossibleListButton";
            this.inPossibleListButton.Size = new System.Drawing.Size(233, 34);
            this.inPossibleListButton.TabIndex = 3;
            this.inPossibleListButton.Text = "入庫可能リスト";
            this.inPossibleListButton.UseVisualStyleBackColor = true;
            this.inPossibleListButton.Click += new System.EventHandler(this.inPossibleListButton_Click);
            // 
            // notProcesseListButton
            // 
            this.notProcesseListButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.notProcesseListButton.Location = new System.Drawing.Point(90, 98);
            this.notProcesseListButton.Name = "notProcesseListButton";
            this.notProcesseListButton.Size = new System.Drawing.Size(233, 34);
            this.notProcesseListButton.TabIndex = 1;
            this.notProcesseListButton.Text = "未処理一覧（検収）(Excel)";
            this.notProcesseListButton.UseVisualStyleBackColor = true;
            this.notProcesseListButton.Click += new System.EventHandler(this.notProcesseListButton_Click);
            // 
            // startButton
            // 
            this.startButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.startButton.Location = new System.Drawing.Point(90, 18);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(233, 34);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "取込み開始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label7.Location = new System.Drawing.Point(179, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 39);
            this.label7.TabIndex = 291;
            this.label7.Text = "↓";
            // 
            // errorListButton
            // 
            this.errorListButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.errorListButton.Location = new System.Drawing.Point(90, 159);
            this.errorListButton.Name = "errorListButton";
            this.errorListButton.Size = new System.Drawing.Size(233, 34);
            this.errorListButton.TabIndex = 2;
            this.errorListButton.Text = "エラーリスト";
            this.errorListButton.UseVisualStyleBackColor = true;
            this.errorListButton.Click += new System.EventHandler(this.errorListButton_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label6.Location = new System.Drawing.Point(179, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 39);
            this.label6.TabIndex = 290;
            this.label6.Text = "↓";
            // 
            // updateButton
            // 
            this.updateButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.updateButton.Location = new System.Drawing.Point(90, 300);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(233, 34);
            this.updateButton.TabIndex = 4;
            this.updateButton.Text = "納入受付　更新";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // F211_CompanyDelivProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F211_CompanyDelivProcess";
            this.Text = "F211_CompanyDelivProcess";
            this.Load += new System.EventHandler(this.F211_CompanyDelivProcess_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sendGroupCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusC1TextBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.batchCodeReprintC1TextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Label groupCodeLabel;
        private C1.Win.C1Input.C1TextBox groupNameC1TextBox;
        private C1.Win.C1Input.C1ComboBox groupCodeC1ComboBox;
        private C1.Win.C1Input.C1TextBox batchCodeC1TextBox;
        private System.Windows.Forms.Label batchCodeLabel;
        private System.Windows.Forms.Label sendGroupCodeLabel;
        private C1.Win.C1Input.C1TextBox sendGroupCodeC1TextBox;
        private System.Windows.Forms.Label statusLabel;
        private C1.Win.C1Input.C1TextBox statusC1TextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button inPossibleListButton;
        private System.Windows.Forms.Button notProcesseListButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button errorListButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button updateButton;
        private C1.Win.Calendar.C1DateEdit dateC1DateEdit;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Button checkListReprintButton;
        private C1.Win.C1Input.C1TextBox batchCodeReprintC1TextBox;
        private System.Windows.Forms.Label batchCodeReprintLabel;
    }
}