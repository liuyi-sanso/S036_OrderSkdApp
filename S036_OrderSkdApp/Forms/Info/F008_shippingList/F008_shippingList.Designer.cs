namespace S036_OrderSkdApp
{
    partial class F008_shippingList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F008_shippingList));
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nextCusCodeC1NumericEdit = new C1.Win.C1Input.C1NumericEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.stateNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.stateCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.partsCodeLabel = new System.Windows.Forms.Label();
            this.partsCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.endDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.dateLabel = new System.Windows.Forms.Label();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.groupCodeNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.startDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.groupCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.poCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeC1NumericEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poCodeC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // c1TrueDBGrid
            // 
            this.c1TrueDBGrid.AllowUpdate = false;
            this.c1TrueDBGrid.AlternatingRows = true;
            this.c1TrueDBGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.c1TrueDBGrid.CaptionHeight = 18;
            this.c1TrueDBGrid.FetchRowStyles = true;
            this.c1TrueDBGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.c1TrueDBGrid.GroupByCaption = "列でグループ化するには、ここに列ヘッダをドラッグします。";
            this.c1TrueDBGrid.Images.Add(((System.Drawing.Image)(resources.GetObject("c1TrueDBGrid.Images"))));
            this.c1TrueDBGrid.LinesPerRow = 4;
            this.c1TrueDBGrid.Location = new System.Drawing.Point(12, 286);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1182, 383);
            this.c1TrueDBGrid.TabIndex = 592;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.poCodeC1TextBox);
            this.panel1.Controls.Add(this.nextCusCodeC1NumericEdit);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.stateNameC1TextBox);
            this.panel1.Controls.Add(this.stateCodeC1ComboBox);
            this.panel1.Controls.Add(this.partsCodeLabel);
            this.panel1.Controls.Add(this.partsCodeC1TextBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.endDateC1DateEdit);
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.groupCodeNameC1TextBox);
            this.panel1.Controls.Add(this.startDateC1DateEdit);
            this.panel1.Controls.Add(this.groupCodeC1ComboBox);
            this.panel1.Controls.Add(this.dateLabel);
            this.panel1.Location = new System.Drawing.Point(1, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 110);
            this.panel1.TabIndex = 591;
            this.panel1.TabStop = true;
            // 
            // nextCusCodeC1NumericEdit
            // 
            this.nextCusCodeC1NumericEdit.BackColor = System.Drawing.Color.White;
            this.nextCusCodeC1NumericEdit.BorderColor = System.Drawing.Color.Black;
            this.nextCusCodeC1NumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextCusCodeC1NumericEdit.CustomFormat = "###0";
            this.nextCusCodeC1NumericEdit.DisabledForeColor = System.Drawing.Color.Black;
            this.nextCusCodeC1NumericEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)((((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat) 
            | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.nextCusCodeC1NumericEdit.EditFormat.EmptyAsNull = false;
            this.nextCusCodeC1NumericEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)((((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat) 
            | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.nextCusCodeC1NumericEdit.EmptyAsNull = true;
            this.nextCusCodeC1NumericEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nextCusCodeC1NumericEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.CustomFormat;
            this.nextCusCodeC1NumericEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.nextCusCodeC1NumericEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nextCusCodeC1NumericEdit.Location = new System.Drawing.Point(120, 43);
            this.nextCusCodeC1NumericEdit.Name = "nextCusCodeC1NumericEdit";
            this.nextCusCodeC1NumericEdit.ParseInfo.Inherit = ((C1.Win.C1Input.ParseInfoInheritFlags)(((((((C1.Win.C1Input.ParseInfoInheritFlags.CaseSensitive | C1.Win.C1Input.ParseInfoInheritFlags.FormatType) 
            | C1.Win.C1Input.ParseInfoInheritFlags.NullText) 
            | C1.Win.C1Input.ParseInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.ParseInfoInheritFlags.ErrorMessage) 
            | C1.Win.C1Input.ParseInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.ParseInfoInheritFlags.TrimEnd)));
            this.nextCusCodeC1NumericEdit.ParseInfo.NumberStyle = ((C1.Win.C1Input.NumberStyleFlags)((((C1.Win.C1Input.NumberStyleFlags.AllowLeadingWhite | C1.Win.C1Input.NumberStyleFlags.AllowTrailingWhite) 
            | C1.Win.C1Input.NumberStyleFlags.AllowDecimalPoint) 
            | C1.Win.C1Input.NumberStyleFlags.AllowExponent)));
            this.nextCusCodeC1NumericEdit.PostValidation.Intervals.AddRange(new C1.Win.C1Input.ValueInterval[] {
            new C1.Win.C1Input.ValueInterval(new decimal(new int[] {
                            9999999,
                            0,
                            0,
                            -2147483648}), new decimal(new int[] {
                            9999999,
                            0,
                            0,
                            0}), true, true)});
            this.nextCusCodeC1NumericEdit.Size = new System.Drawing.Size(110, 21);
            this.nextCusCodeC1NumericEdit.TabIndex = 0;
            this.nextCusCodeC1NumericEdit.Tag = null;
            this.nextCusCodeC1NumericEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.None;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(27, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 725;
            this.label5.Text = "次工程";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(494, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 721;
            this.label4.Text = "注文番号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(494, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 719;
            this.label3.Text = "ステータス";
            // 
            // stateNameC1TextBox
            // 
            this.stateNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.stateNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.stateNameC1TextBox.Enabled = false;
            this.stateNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stateNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.stateNameC1TextBox.Location = new System.Drawing.Point(639, 43);
            this.stateNameC1TextBox.MaxLength = 12;
            this.stateNameC1TextBox.Name = "stateNameC1TextBox";
            this.stateNameC1TextBox.Size = new System.Drawing.Size(183, 21);
            this.stateNameC1TextBox.TabIndex = 718;
            this.stateNameC1TextBox.TabStop = false;
            this.stateNameC1TextBox.Tag = null;
            // 
            // stateCodeC1ComboBox
            // 
            this.stateCodeC1ComboBox.AllowSpinLoop = false;
            this.stateCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.stateCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.stateCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.stateCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.stateCodeC1ComboBox.DropDownWidth = -1;
            this.stateCodeC1ComboBox.ErrorInfo.ShowErrorMessage = false;
            this.stateCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stateCodeC1ComboBox.GapHeight = 0;
            this.stateCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.stateCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stateCodeC1ComboBox.ItemsDisplayMember = "";
            this.stateCodeC1ComboBox.ItemsValueMember = "";
            this.stateCodeC1ComboBox.Label = this.label3;
            this.stateCodeC1ComboBox.Location = new System.Drawing.Point(583, 43);
            this.stateCodeC1ComboBox.MaxLength = 4;
            this.stateCodeC1ComboBox.Name = "stateCodeC1ComboBox";
            this.stateCodeC1ComboBox.Size = new System.Drawing.Size(50, 21);
            this.stateCodeC1ComboBox.TabIndex = 3;
            this.stateCodeC1ComboBox.Tag = null;
            this.stateCodeC1ComboBox.TextDetached = true;
            this.stateCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.stateCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.stateCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.stateCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // partsCodeLabel
            // 
            this.partsCodeLabel.AutoSize = true;
            this.partsCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.partsCodeLabel.Location = new System.Drawing.Point(27, 75);
            this.partsCodeLabel.Name = "partsCodeLabel";
            this.partsCodeLabel.Size = new System.Drawing.Size(76, 16);
            this.partsCodeLabel.TabIndex = 715;
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
            this.partsCodeC1TextBox.Location = new System.Drawing.Point(120, 73);
            this.partsCodeC1TextBox.MaxLength = 12;
            this.partsCodeC1TextBox.Name = "partsCodeC1TextBox";
            this.partsCodeC1TextBox.Size = new System.Drawing.Size(129, 21);
            this.partsCodeC1TextBox.TabIndex = 1;
            this.partsCodeC1TextBox.Tag = null;
            this.partsCodeC1TextBox.TextDetached = true;
            this.partsCodeC1TextBox.Value = "a";
            this.partsCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.partsCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(758, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 590;
            this.label2.Text = "～";
            // 
            // endDateC1DateEdit
            // 
            this.endDateC1DateEdit.AllowSpinLoop = false;
            this.endDateC1DateEdit.AutoSize = false;
            this.endDateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.endDateC1DateEdit.Calendar.DayNamesFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.endDateC1DateEdit.Calendar.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.endDateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.endDateC1DateEdit.Calendar.TitleFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.endDateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.endDateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.endDateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.endDateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.endDateC1DateEdit.DisplayFormat.NullText = "――/―/―";
            this.endDateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.endDateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.endDateC1DateEdit.EmptyAsNull = true;
            this.endDateC1DateEdit.ErrorInfo.ShowErrorMessage = false;
            this.endDateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.endDateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.endDateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.endDateC1DateEdit.Label = this.dateLabel;
            this.endDateC1DateEdit.Location = new System.Drawing.Point(782, 11);
            this.endDateC1DateEdit.Name = "endDateC1DateEdit";
            this.endDateC1DateEdit.NullText = "――/―/―";
            this.endDateC1DateEdit.Size = new System.Drawing.Size(161, 23);
            this.endDateC1DateEdit.TabIndex = 3;
            this.endDateC1DateEdit.Tag = null;
            this.endDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.endDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.endDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.dateLabel.Location = new System.Drawing.Point(494, 15);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(88, 16);
            this.dateLabel.TabIndex = 585;
            this.dateLabel.Text = "納入指示日";
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeLabel.Location = new System.Drawing.Point(27, 15);
            this.groupCodeLabel.Name = "groupCodeLabel";
            this.groupCodeLabel.Size = new System.Drawing.Size(92, 16);
            this.groupCodeLabel.TabIndex = 584;
            this.groupCodeLabel.Text = "発送先コード";
            // 
            // groupCodeNameC1TextBox
            // 
            this.groupCodeNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeNameC1TextBox.Enabled = false;
            this.groupCodeNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeNameC1TextBox.Location = new System.Drawing.Point(218, 13);
            this.groupCodeNameC1TextBox.MaxLength = 12;
            this.groupCodeNameC1TextBox.Name = "groupCodeNameC1TextBox";
            this.groupCodeNameC1TextBox.Size = new System.Drawing.Size(183, 21);
            this.groupCodeNameC1TextBox.TabIndex = 583;
            this.groupCodeNameC1TextBox.TabStop = false;
            this.groupCodeNameC1TextBox.Tag = null;
            // 
            // startDateC1DateEdit
            // 
            this.startDateC1DateEdit.AllowSpinLoop = false;
            this.startDateC1DateEdit.AutoSize = false;
            this.startDateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.startDateC1DateEdit.Calendar.DayNamesFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.startDateC1DateEdit.Calendar.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.startDateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.startDateC1DateEdit.Calendar.TitleFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.startDateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.startDateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.startDateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.startDateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.startDateC1DateEdit.DisplayFormat.NullText = "――/―/―";
            this.startDateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.startDateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.startDateC1DateEdit.EmptyAsNull = true;
            this.startDateC1DateEdit.ErrorInfo.ShowErrorMessage = false;
            this.startDateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.startDateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.startDateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.startDateC1DateEdit.Label = this.dateLabel;
            this.startDateC1DateEdit.Location = new System.Drawing.Point(583, 11);
            this.startDateC1DateEdit.Name = "startDateC1DateEdit";
            this.startDateC1DateEdit.NullText = "――/―/―";
            this.startDateC1DateEdit.Size = new System.Drawing.Size(161, 23);
            this.startDateC1DateEdit.TabIndex = 2;
            this.startDateC1DateEdit.Tag = null;
            this.startDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.startDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.startDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // groupCodeC1ComboBox
            // 
            this.groupCodeC1ComboBox.AllowSpinLoop = false;
            this.groupCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.groupCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.groupCodeC1ComboBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.groupCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeC1ComboBox.DropDownWidth = -1;
            this.groupCodeC1ComboBox.ErrorInfo.ShowErrorMessage = false;
            this.groupCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeC1ComboBox.GapHeight = 0;
            this.groupCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.groupCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupCodeC1ComboBox.ItemsDisplayMember = "";
            this.groupCodeC1ComboBox.ItemsValueMember = "";
            this.groupCodeC1ComboBox.Label = this.groupCodeLabel;
            this.groupCodeC1ComboBox.Location = new System.Drawing.Point(120, 13);
            this.groupCodeC1ComboBox.MaxLength = 4;
            this.groupCodeC1ComboBox.Name = "groupCodeC1ComboBox";
            this.groupCodeC1ComboBox.ReadOnly = true;
            this.groupCodeC1ComboBox.Size = new System.Drawing.Size(91, 21);
            this.groupCodeC1ComboBox.TabIndex = 1;
            this.groupCodeC1ComboBox.Tag = null;
            this.groupCodeC1ComboBox.TextDetached = true;
            this.groupCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.groupCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.groupCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.groupCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // poCodeC1TextBox
            // 
            this.poCodeC1TextBox.BackColor = System.Drawing.Color.White;
            this.poCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.poCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.poCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.poCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.poCodeC1TextBox.Label = this.partsCodeLabel;
            this.poCodeC1TextBox.Location = new System.Drawing.Point(583, 73);
            this.poCodeC1TextBox.MaxLength = 12;
            this.poCodeC1TextBox.Name = "poCodeC1TextBox";
            this.poCodeC1TextBox.Size = new System.Drawing.Size(129, 21);
            this.poCodeC1TextBox.TabIndex = 726;
            this.poCodeC1TextBox.Tag = null;
            this.poCodeC1TextBox.TextDetached = true;
            this.poCodeC1TextBox.Value = "a";
            // 
            // F008_shippingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F008_shippingList";
            this.Text = "F008_shippingList";
            this.Load += new System.EventHandler(this.F008_shippingList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeC1NumericEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poCodeC1TextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private C1.Win.Calendar.C1DateEdit endDateC1DateEdit;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label groupCodeLabel;
        private C1.Win.C1Input.C1TextBox groupCodeNameC1TextBox;
        private C1.Win.Calendar.C1DateEdit startDateC1DateEdit;
        private C1.Win.C1Input.C1ComboBox groupCodeC1ComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private C1.Win.C1Input.C1TextBox stateNameC1TextBox;
        private C1.Win.C1Input.C1ComboBox stateCodeC1ComboBox;
        private System.Windows.Forms.Label partsCodeLabel;
        private C1.Win.C1Input.C1TextBox partsCodeC1TextBox;
        private System.Windows.Forms.Label label5;
        private C1.Win.C1Input.C1NumericEdit nextCusCodeC1NumericEdit;
        private C1.Win.C1Input.C1TextBox poCodeC1TextBox;
    }
}