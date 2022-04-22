namespace S036_OrderSkdApp
{
    partial class F226_DelivSlipReception
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.codeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.barcodeLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.poCodeLabel = new System.Windows.Forms.Label();
            this.poCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.webEDICateC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.webEDICateCLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.stateC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.webEDICateNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.doCodeLabel = new System.Windows.Forms.Label();
            this.doCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.delivNumC1NumericEdit = new C1.Win.C1Input.C1NumericEdit();
            this.delivNumCLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.skdCodeLabel = new System.Windows.Forms.Label();
            this.skdCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.delivDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.delivDateLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.codeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poCodeC1TextBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webEDICateC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webEDICateNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delivNumC1NumericEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skdCodeC1TextBox)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delivDateC1DateEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.codeC1TextBox);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.barcodeLabel);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 43);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // codeC1TextBox
            // 
            this.codeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.codeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.codeC1TextBox.EmptyAsNull = true;
            this.codeC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.codeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.codeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.codeC1TextBox.Label = this.barcodeLabel;
            this.codeC1TextBox.Location = new System.Drawing.Point(123, 11);
            this.codeC1TextBox.MaxLength = 15;
            this.codeC1TextBox.Name = "codeC1TextBox";
            this.codeC1TextBox.Size = new System.Drawing.Size(159, 21);
            this.codeC1TextBox.TabIndex = 587;
            this.codeC1TextBox.Tag = null;
            this.codeC1TextBox.TextDetached = true;
            this.codeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.codeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.codeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.codeC1TextBox_Validating);
            this.codeC1TextBox.Validated += new System.EventHandler(this.codeC1TextBox_Validated);
            // 
            // barcodeLabel
            // 
            this.barcodeLabel.AutoSize = true;
            this.barcodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.barcodeLabel.Location = new System.Drawing.Point(32, 13);
            this.barcodeLabel.Name = "barcodeLabel";
            this.barcodeLabel.Size = new System.Drawing.Size(88, 16);
            this.barcodeLabel.TabIndex = 585;
            this.barcodeLabel.Text = "納品書番号";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(19, 17);
            this.button1.TabIndex = 586;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // poCodeLabel
            // 
            this.poCodeLabel.AutoSize = true;
            this.poCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.poCodeLabel.Location = new System.Drawing.Point(128, 36);
            this.poCodeLabel.Name = "poCodeLabel";
            this.poCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.poCodeLabel.TabIndex = 559;
            this.poCodeLabel.Text = "注文番号";
            // 
            // poCodeC1TextBox
            // 
            this.poCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.poCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.poCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.poCodeC1TextBox.Enabled = false;
            this.poCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.poCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.poCodeC1TextBox.Label = this.poCodeLabel;
            this.poCodeC1TextBox.Location = new System.Drawing.Point(249, 33);
            this.poCodeC1TextBox.MaxLength = 12;
            this.poCodeC1TextBox.Name = "poCodeC1TextBox";
            this.poCodeC1TextBox.Size = new System.Drawing.Size(163, 21);
            this.poCodeC1TextBox.TabIndex = 558;
            this.poCodeC1TextBox.TabStop = false;
            this.poCodeC1TextBox.Tag = null;
            this.poCodeC1TextBox.TextDetached = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.webEDICateC1TextBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.webEDICateCLabel);
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Controls.Add(this.stateC1TextBox);
            this.panel2.Controls.Add(this.webEDICateNameC1TextBox);
            this.panel2.Location = new System.Drawing.Point(0, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1183, 97);
            this.panel2.TabIndex = 1;
            // 
            // webEDICateC1TextBox
            // 
            this.webEDICateC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.webEDICateC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.webEDICateC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.webEDICateC1TextBox.EmptyAsNull = true;
            this.webEDICateC1TextBox.Enabled = false;
            this.webEDICateC1TextBox.ErrorInfo.ShowErrorMessage = false;
            this.webEDICateC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.webEDICateC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.webEDICateC1TextBox.Label = this.webEDICateCLabel;
            this.webEDICateC1TextBox.Location = new System.Drawing.Point(249, 61);
            this.webEDICateC1TextBox.MaxLength = 10;
            this.webEDICateC1TextBox.Name = "webEDICateC1TextBox";
            this.webEDICateC1TextBox.Size = new System.Drawing.Size(36, 21);
            this.webEDICateC1TextBox.TabIndex = 697;
            this.webEDICateC1TextBox.TabStop = false;
            this.webEDICateC1TextBox.Tag = null;
            this.webEDICateC1TextBox.TextDetached = true;
            // 
            // webEDICateCLabel
            // 
            this.webEDICateCLabel.AutoSize = true;
            this.webEDICateCLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.webEDICateCLabel.Location = new System.Drawing.Point(128, 64);
            this.webEDICateCLabel.Name = "webEDICateCLabel";
            this.webEDICateCLabel.Size = new System.Drawing.Size(115, 16);
            this.webEDICateCLabel.TabIndex = 698;
            this.webEDICateCLabel.Text = "WebEDI区分/名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(26, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 564;
            this.label3.Text = "＜処理＞";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.statusLabel.Location = new System.Drawing.Point(128, 34);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(68, 16);
            this.statusLabel.TabIndex = 563;
            this.statusLabel.Text = "ステータス";
            // 
            // stateC1TextBox
            // 
            this.stateC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.stateC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.stateC1TextBox.Enabled = false;
            this.stateC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.stateC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.stateC1TextBox.Label = this.statusLabel;
            this.stateC1TextBox.Location = new System.Drawing.Point(249, 31);
            this.stateC1TextBox.MaxLength = 12;
            this.stateC1TextBox.Name = "stateC1TextBox";
            this.stateC1TextBox.Size = new System.Drawing.Size(36, 21);
            this.stateC1TextBox.TabIndex = 562;
            this.stateC1TextBox.TabStop = false;
            this.stateC1TextBox.Tag = null;
            this.stateC1TextBox.TextDetached = true;
            // 
            // webEDICateNameC1TextBox
            // 
            this.webEDICateNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.webEDICateNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.webEDICateNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.webEDICateNameC1TextBox.Enabled = false;
            this.webEDICateNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.webEDICateNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.webEDICateNameC1TextBox.Location = new System.Drawing.Point(290, 61);
            this.webEDICateNameC1TextBox.MaxLength = 12;
            this.webEDICateNameC1TextBox.Name = "webEDICateNameC1TextBox";
            this.webEDICateNameC1TextBox.Size = new System.Drawing.Size(122, 21);
            this.webEDICateNameC1TextBox.TabIndex = 699;
            this.webEDICateNameC1TextBox.TabStop = false;
            this.webEDICateNameC1TextBox.Tag = null;
            this.webEDICateNameC1TextBox.TextDetached = true;
            // 
            // doCodeLabel
            // 
            this.doCodeLabel.AutoSize = true;
            this.doCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.doCodeLabel.Location = new System.Drawing.Point(128, 66);
            this.doCodeLabel.Name = "doCodeLabel";
            this.doCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.doCodeLabel.TabIndex = 561;
            this.doCodeLabel.Text = "伝票番号";
            // 
            // doCodeC1TextBox
            // 
            this.doCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.doCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.doCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.doCodeC1TextBox.Enabled = false;
            this.doCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.doCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.doCodeC1TextBox.Label = this.doCodeLabel;
            this.doCodeC1TextBox.Location = new System.Drawing.Point(249, 63);
            this.doCodeC1TextBox.MaxLength = 12;
            this.doCodeC1TextBox.Name = "doCodeC1TextBox";
            this.doCodeC1TextBox.Size = new System.Drawing.Size(163, 21);
            this.doCodeC1TextBox.TabIndex = 560;
            this.doCodeC1TextBox.TabStop = false;
            this.doCodeC1TextBox.Tag = null;
            this.doCodeC1TextBox.TextDetached = true;
            // 
            // delivNumC1NumericEdit
            // 
            this.delivNumC1NumericEdit.BackColor = System.Drawing.Color.PeachPuff;
            this.delivNumC1NumericEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.delivNumC1NumericEdit.CustomFormat = "#,###,###";
            this.delivNumC1NumericEdit.DisabledForeColor = System.Drawing.Color.Black;
            this.delivNumC1NumericEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)((((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat) 
            | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.delivNumC1NumericEdit.EditFormat.EmptyAsNull = false;
            this.delivNumC1NumericEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)((((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.CustomFormat) 
            | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.delivNumC1NumericEdit.EmptyAsNull = true;
            this.delivNumC1NumericEdit.Enabled = false;
            this.delivNumC1NumericEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivNumC1NumericEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.CustomFormat;
            this.delivNumC1NumericEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.delivNumC1NumericEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.delivNumC1NumericEdit.Label = this.delivNumCLabel;
            this.delivNumC1NumericEdit.Location = new System.Drawing.Point(249, 123);
            this.delivNumC1NumericEdit.Name = "delivNumC1NumericEdit";
            this.delivNumC1NumericEdit.ParseInfo.Inherit = ((C1.Win.C1Input.ParseInfoInheritFlags)(((((((C1.Win.C1Input.ParseInfoInheritFlags.CaseSensitive | C1.Win.C1Input.ParseInfoInheritFlags.FormatType) 
            | C1.Win.C1Input.ParseInfoInheritFlags.NullText) 
            | C1.Win.C1Input.ParseInfoInheritFlags.EmptyAsNull) 
            | C1.Win.C1Input.ParseInfoInheritFlags.ErrorMessage) 
            | C1.Win.C1Input.ParseInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.ParseInfoInheritFlags.TrimEnd)));
            this.delivNumC1NumericEdit.ParseInfo.NumberStyle = ((C1.Win.C1Input.NumberStyleFlags)((((C1.Win.C1Input.NumberStyleFlags.AllowLeadingWhite | C1.Win.C1Input.NumberStyleFlags.AllowTrailingWhite) 
            | C1.Win.C1Input.NumberStyleFlags.AllowDecimalPoint) 
            | C1.Win.C1Input.NumberStyleFlags.AllowExponent)));
            this.delivNumC1NumericEdit.PostValidation.Intervals.AddRange(new C1.Win.C1Input.ValueInterval[] {
            new C1.Win.C1Input.ValueInterval(new decimal(new int[] {
                            9999999,
                            0,
                            0,
                            -2147483648}), new decimal(new int[] {
                            9999999,
                            0,
                            0,
                            0}), true, true)});
            this.delivNumC1NumericEdit.Size = new System.Drawing.Size(76, 21);
            this.delivNumC1NumericEdit.TabIndex = 0;
            this.delivNumC1NumericEdit.Tag = null;
            this.delivNumC1NumericEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.delivNumC1NumericEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.None;
            // 
            // delivNumCLabel
            // 
            this.delivNumCLabel.AutoSize = true;
            this.delivNumCLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivNumCLabel.ForeColor = System.Drawing.Color.Black;
            this.delivNumCLabel.Location = new System.Drawing.Point(128, 126);
            this.delivNumCLabel.Name = "delivNumCLabel";
            this.delivNumCLabel.Size = new System.Drawing.Size(56, 16);
            this.delivNumCLabel.TabIndex = 702;
            this.delivNumCLabel.Text = "納入数";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(26, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 16);
            this.label15.TabIndex = 704;
            this.label15.Text = "＜情報＞";
            // 
            // skdCodeLabel
            // 
            this.skdCodeLabel.AutoSize = true;
            this.skdCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.skdCodeLabel.Location = new System.Drawing.Point(128, 96);
            this.skdCodeLabel.Name = "skdCodeLabel";
            this.skdCodeLabel.Size = new System.Drawing.Size(72, 16);
            this.skdCodeLabel.TabIndex = 703;
            this.skdCodeLabel.Text = "日程番号";
            // 
            // skdCodeC1TextBox
            // 
            this.skdCodeC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.skdCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.skdCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.skdCodeC1TextBox.Enabled = false;
            this.skdCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.skdCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.skdCodeC1TextBox.Label = this.skdCodeLabel;
            this.skdCodeC1TextBox.Location = new System.Drawing.Point(249, 93);
            this.skdCodeC1TextBox.MaxLength = 12;
            this.skdCodeC1TextBox.Name = "skdCodeC1TextBox";
            this.skdCodeC1TextBox.Size = new System.Drawing.Size(163, 21);
            this.skdCodeC1TextBox.TabIndex = 700;
            this.skdCodeC1TextBox.TabStop = false;
            this.skdCodeC1TextBox.Tag = null;
            this.skdCodeC1TextBox.TextDetached = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.delivDateC1DateEdit);
            this.panel3.Controls.Add(this.delivDateLabel);
            this.panel3.Controls.Add(this.delivNumC1NumericEdit);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.skdCodeLabel);
            this.panel3.Controls.Add(this.delivNumCLabel);
            this.panel3.Controls.Add(this.poCodeC1TextBox);
            this.panel3.Controls.Add(this.skdCodeC1TextBox);
            this.panel3.Controls.Add(this.poCodeLabel);
            this.panel3.Controls.Add(this.doCodeC1TextBox);
            this.panel3.Controls.Add(this.doCodeLabel);
            this.panel3.Location = new System.Drawing.Point(0, 302);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1183, 362);
            this.panel3.TabIndex = 1;
            this.panel3.TabStop = true;
            // 
            // delivDateC1DateEdit
            // 
            this.delivDateC1DateEdit.AllowSpinLoop = false;
            this.delivDateC1DateEdit.BackColor = System.Drawing.Color.White;
            this.delivDateC1DateEdit.BorderColor = System.Drawing.Color.Red;
            this.delivDateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.delivDateC1DateEdit.Calendar.DayNamesFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivDateC1DateEdit.Calendar.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivDateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.delivDateC1DateEdit.Calendar.TitleFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivDateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.delivDateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.delivDateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.delivDateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.delivDateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.delivDateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((((C1.Win.C1Input.FormatInfoInheritFlags.FormatType | C1.Win.C1Input.FormatInfoInheritFlags.NullText) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.delivDateC1DateEdit.EmptyAsNull = true;
            this.delivDateC1DateEdit.ErrorInfo.ShowErrorMessage = false;
            this.delivDateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.delivDateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.ShortDate;
            this.delivDateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.delivDateC1DateEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.delivDateC1DateEdit.Label = this.delivDateLabel;
            this.delivDateC1DateEdit.Location = new System.Drawing.Point(249, 153);
            this.delivDateC1DateEdit.Name = "delivDateC1DateEdit";
            this.delivDateC1DateEdit.Size = new System.Drawing.Size(109, 21);
            this.delivDateC1DateEdit.TabIndex = 0;
            this.delivDateC1DateEdit.Tag = null;
            this.delivDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.delivDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.delivDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // delivDateLabel
            // 
            this.delivDateLabel.AutoSize = true;
            this.delivDateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.delivDateLabel.Location = new System.Drawing.Point(128, 156);
            this.delivDateLabel.Name = "delivDateLabel";
            this.delivDateLabel.Size = new System.Drawing.Size(72, 16);
            this.delivDateLabel.TabIndex = 706;
            this.delivDateLabel.Text = "納入日付";
            // 
            // F226_DelivSlipReception
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F226_DelivSlipReception";
            this.Text = "F226_DelivSlipReception";
            this.Load += new System.EventHandler(this.F226_DelivSlipReception_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.codeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poCodeC1TextBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webEDICateC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webEDICateNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delivNumC1NumericEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skdCodeC1TextBox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delivDateC1DateEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label barcodeLabel;
        private System.Windows.Forms.Label poCodeLabel;
        private C1.Win.C1Input.C1TextBox poCodeC1TextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label statusLabel;
        private C1.Win.C1Input.C1TextBox stateC1TextBox;
        private System.Windows.Forms.Label doCodeLabel;
        private C1.Win.C1Input.C1TextBox doCodeC1TextBox;
        private C1.Win.C1Input.C1NumericEdit delivNumC1NumericEdit;
        private System.Windows.Forms.Label delivNumCLabel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label skdCodeLabel;
        private C1.Win.C1Input.C1TextBox skdCodeC1TextBox;
        private C1.Win.C1Input.C1TextBox webEDICateNameC1TextBox;
        private C1.Win.C1Input.C1TextBox webEDICateC1TextBox;
        private System.Windows.Forms.Label webEDICateCLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private C1.Win.C1Input.C1TextBox codeC1TextBox;
        private C1.Win.Calendar.C1DateEdit delivDateC1DateEdit;
        private System.Windows.Forms.Label delivDateLabel;
    }
}