namespace S036_OrderSkdApp
{
    partial class F227_DocumentPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F227_DocumentPrint));
            this.panel1 = new System.Windows.Forms.Panel();
            this.startDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.dateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.endDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.searchButton = new System.Windows.Forms.Button();
            this.reportCateC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.reportCateLabel = new System.Windows.Forms.Label();
            this.nextCusCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.nextCusCodeLabel = new System.Windows.Forms.Label();
            this.nextCusCodeNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.allChoiceButton = new System.Windows.Forms.Button();
            this.clearChoiceButton = new System.Windows.Forms.Button();
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.acceptDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportCateC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptDateC1DateEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.startDateC1DateEdit);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.endDateC1DateEdit);
            this.panel1.Controls.Add(this.searchButton);
            this.panel1.Controls.Add(this.reportCateC1ComboBox);
            this.panel1.Controls.Add(this.dateLabel);
            this.panel1.Controls.Add(this.reportCateLabel);
            this.panel1.Controls.Add(this.nextCusCodeC1ComboBox);
            this.panel1.Controls.Add(this.nextCusCodeNameC1TextBox);
            this.panel1.Controls.Add(this.nextCusCodeLabel);
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 61);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
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
            this.startDateC1DateEdit.Location = new System.Drawing.Point(695, 21);
            this.startDateC1DateEdit.Name = "startDateC1DateEdit";
            this.startDateC1DateEdit.NullText = "――/―/―";
            this.startDateC1DateEdit.Size = new System.Drawing.Size(161, 23);
            this.startDateC1DateEdit.TabIndex = 10;
            this.startDateC1DateEdit.Tag = null;
            this.startDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.startDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.startDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.dateLabel.Location = new System.Drawing.Point(595, 24);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(104, 16);
            this.dateLabel.TabIndex = 585;
            this.dateLabel.Text = "現品票読込日";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(858, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 584;
            this.label1.Text = "～";
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
            this.endDateC1DateEdit.Location = new System.Drawing.Point(882, 21);
            this.endDateC1DateEdit.Name = "endDateC1DateEdit";
            this.endDateC1DateEdit.NullText = "――/―/―";
            this.endDateC1DateEdit.Size = new System.Drawing.Size(161, 23);
            this.endDateC1DateEdit.TabIndex = 11;
            this.endDateC1DateEdit.Tag = null;
            this.endDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.endDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.endDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchButton.Location = new System.Drawing.Point(1064, 12);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(100, 39);
            this.searchButton.TabIndex = 587;
            this.searchButton.TabStop = false;
            this.searchButton.Text = "検索";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // reportCateC1ComboBox
            // 
            this.reportCateC1ComboBox.AllowSpinLoop = false;
            this.reportCateC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.reportCateC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.reportCateC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportCateC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.reportCateC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.reportCateC1ComboBox.DropDownWidth = -1;
            this.reportCateC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.reportCateC1ComboBox.GapHeight = 0;
            this.reportCateC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.reportCateC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.reportCateC1ComboBox.ItemsDisplayMember = "";
            this.reportCateC1ComboBox.ItemsValueMember = "";
            this.reportCateC1ComboBox.Label = this.reportCateLabel;
            this.reportCateC1ComboBox.Location = new System.Drawing.Point(431, 22);
            this.reportCateC1ComboBox.MaxLength = 20;
            this.reportCateC1ComboBox.Name = "reportCateC1ComboBox";
            this.reportCateC1ComboBox.Size = new System.Drawing.Size(149, 21);
            this.reportCateC1ComboBox.TabIndex = 3;
            this.reportCateC1ComboBox.Tag = null;
            this.reportCateC1ComboBox.TextDetached = true;
            this.reportCateC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.reportCateC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.reportCateC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            // 
            // reportCateLabel
            // 
            this.reportCateLabel.AutoSize = true;
            this.reportCateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.reportCateLabel.Location = new System.Drawing.Point(360, 24);
            this.reportCateLabel.Name = "reportCateLabel";
            this.reportCateLabel.Size = new System.Drawing.Size(72, 16);
            this.reportCateLabel.TabIndex = 588;
            this.reportCateLabel.Text = "伝票種別";
            // 
            // nextCusCodeC1ComboBox
            // 
            this.nextCusCodeC1ComboBox.AllowSpinLoop = false;
            this.nextCusCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.nextCusCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.nextCusCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextCusCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nextCusCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.nextCusCodeC1ComboBox.DropDownWidth = -1;
            this.nextCusCodeC1ComboBox.ErrorInfo.ShowErrorMessage = false;
            this.nextCusCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nextCusCodeC1ComboBox.GapHeight = 0;
            this.nextCusCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.nextCusCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.nextCusCodeC1ComboBox.ItemsDisplayMember = "";
            this.nextCusCodeC1ComboBox.ItemsValueMember = "";
            this.nextCusCodeC1ComboBox.Label = this.nextCusCodeLabel;
            this.nextCusCodeC1ComboBox.Location = new System.Drawing.Point(69, 22);
            this.nextCusCodeC1ComboBox.MaxLength = 4;
            this.nextCusCodeC1ComboBox.Name = "nextCusCodeC1ComboBox";
            this.nextCusCodeC1ComboBox.Size = new System.Drawing.Size(91, 21);
            this.nextCusCodeC1ComboBox.TabIndex = 1;
            this.nextCusCodeC1ComboBox.Tag = null;
            this.nextCusCodeC1ComboBox.TextDetached = true;
            this.nextCusCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.nextCusCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.nextCusCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.nextCusCodeC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.nextCusCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // nextCusCodeLabel
            // 
            this.nextCusCodeLabel.AutoSize = true;
            this.nextCusCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nextCusCodeLabel.Location = new System.Drawing.Point(14, 24);
            this.nextCusCodeLabel.Name = "nextCusCodeLabel";
            this.nextCusCodeLabel.Size = new System.Drawing.Size(56, 16);
            this.nextCusCodeLabel.TabIndex = 584;
            this.nextCusCodeLabel.Text = "次工程";
            // 
            // nextCusCodeNameC1TextBox
            // 
            this.nextCusCodeNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.nextCusCodeNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextCusCodeNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.nextCusCodeNameC1TextBox.Enabled = false;
            this.nextCusCodeNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nextCusCodeNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nextCusCodeNameC1TextBox.Location = new System.Drawing.Point(163, 22);
            this.nextCusCodeNameC1TextBox.MaxLength = 12;
            this.nextCusCodeNameC1TextBox.Name = "nextCusCodeNameC1TextBox";
            this.nextCusCodeNameC1TextBox.Size = new System.Drawing.Size(183, 21);
            this.nextCusCodeNameC1TextBox.TabIndex = 583;
            this.nextCusCodeNameC1TextBox.TabStop = false;
            this.nextCusCodeNameC1TextBox.Tag = null;
            // 
            // allChoiceButton
            // 
            this.allChoiceButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.allChoiceButton.Location = new System.Drawing.Point(316, 7);
            this.allChoiceButton.Name = "allChoiceButton";
            this.allChoiceButton.Size = new System.Drawing.Size(101, 30);
            this.allChoiceButton.TabIndex = 583;
            this.allChoiceButton.TabStop = false;
            this.allChoiceButton.Text = "全てを選択";
            this.allChoiceButton.UseVisualStyleBackColor = true;
            this.allChoiceButton.Click += new System.EventHandler(this.allChoiceButton_Click);
            // 
            // clearChoiceButton
            // 
            this.clearChoiceButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.clearChoiceButton.Location = new System.Drawing.Point(432, 7);
            this.clearChoiceButton.Name = "clearChoiceButton";
            this.clearChoiceButton.Size = new System.Drawing.Size(101, 30);
            this.clearChoiceButton.TabIndex = 584;
            this.clearChoiceButton.TabStop = false;
            this.clearChoiceButton.Text = "選択解除";
            this.clearChoiceButton.UseVisualStyleBackColor = true;
            this.clearChoiceButton.Click += new System.EventHandler(this.clearChoiceButton_Click);
            // 
            // c1TrueDBGrid
            // 
            this.c1TrueDBGrid.AllowColMove = false;
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
            this.c1TrueDBGrid.Location = new System.Drawing.Point(-1, 45);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1182, 380);
            this.c1TrueDBGrid.TabIndex = 585;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.acceptDateC1DateEdit);
            this.panel2.Controls.Add(this.allChoiceButton);
            this.panel2.Controls.Add(this.clearChoiceButton);
            this.panel2.Controls.Add(this.c1TrueDBGrid);
            this.panel2.Location = new System.Drawing.Point(0, 222);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1183, 430);
            this.panel2.TabIndex = 586;
            this.panel2.TabStop = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.label2.Location = new System.Drawing.Point(21, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 588;
            this.label2.Text = "受払年月日";
            // 
            // acceptDateC1DateEdit
            // 
            this.acceptDateC1DateEdit.AllowSpinLoop = false;
            this.acceptDateC1DateEdit.AutoSize = false;
            this.acceptDateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.acceptDateC1DateEdit.Calendar.DayNamesFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.acceptDateC1DateEdit.Calendar.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.acceptDateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.acceptDateC1DateEdit.Calendar.TitleFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.acceptDateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.acceptDateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.acceptDateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.acceptDateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.acceptDateC1DateEdit.DisplayFormat.NullText = "――/―/―";
            this.acceptDateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.acceptDateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.acceptDateC1DateEdit.EmptyAsNull = true;
            this.acceptDateC1DateEdit.ErrorInfo.ShowErrorMessage = false;
            this.acceptDateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.acceptDateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.acceptDateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.acceptDateC1DateEdit.Label = this.label2;
            this.acceptDateC1DateEdit.Location = new System.Drawing.Point(109, 11);
            this.acceptDateC1DateEdit.Name = "acceptDateC1DateEdit";
            this.acceptDateC1DateEdit.NullText = "――/―/―";
            this.acceptDateC1DateEdit.Size = new System.Drawing.Size(161, 23);
            this.acceptDateC1DateEdit.TabIndex = 587;
            this.acceptDateC1DateEdit.Tag = null;
            this.acceptDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.acceptDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.acceptDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // F227_DocumentPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F227_DocumentPrint";
            this.Text = "F227_DocumentPring";
            this.Load += new System.EventHandler(this.F227_DocumentPrint_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportCateC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextCusCodeNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acceptDateC1DateEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label dateLabel;
        private C1.Win.Calendar.C1DateEdit startDateC1DateEdit;
        private C1.Win.C1Input.C1ComboBox nextCusCodeC1ComboBox;
        private System.Windows.Forms.Label nextCusCodeLabel;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1Input.C1TextBox nextCusCodeNameC1TextBox;
        private C1.Win.Calendar.C1DateEdit endDateC1DateEdit;
        private System.Windows.Forms.Button allChoiceButton;
        private System.Windows.Forms.Button clearChoiceButton;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private C1.Win.Calendar.C1DateEdit acceptDateC1DateEdit;
        private C1.Win.C1Input.C1ComboBox reportCateC1ComboBox;
        private System.Windows.Forms.Label reportCateLabel;
        private System.Windows.Forms.Button searchButton;
    }
}