namespace S036_OrderSkdApp
{
    partial class F004_NoAcceptInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F004_NoAcceptInfo));
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.endDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.dateLabel = new System.Windows.Forms.Label();
            this.groupCodeLabel = new System.Windows.Forms.Label();
            this.groupCodeNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.startDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.groupCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).BeginInit();
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
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 219);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1182, 436);
            this.c1TrueDBGrid.TabIndex = 590;
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
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.endDateC1DateEdit);
            this.panel1.Controls.Add(this.groupCodeLabel);
            this.panel1.Controls.Add(this.groupCodeNameC1TextBox);
            this.panel1.Controls.Add(this.startDateC1DateEdit);
            this.panel1.Controls.Add(this.groupCodeC1ComboBox);
            this.panel1.Controls.Add(this.dateLabel);
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 58);
            this.panel1.TabIndex = 589;
            this.panel1.TabStop = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(746, 21);
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
            this.endDateC1DateEdit.Location = new System.Drawing.Point(770, 18);
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
            this.dateLabel.Location = new System.Drawing.Point(485, 21);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(88, 16);
            this.dateLabel.TabIndex = 585;
            this.dateLabel.Text = "納入指示日";
            // 
            // groupCodeLabel
            // 
            this.groupCodeLabel.AutoSize = true;
            this.groupCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeLabel.Location = new System.Drawing.Point(27, 21);
            this.groupCodeLabel.Name = "groupCodeLabel";
            this.groupCodeLabel.Size = new System.Drawing.Size(92, 16);
            this.groupCodeLabel.TabIndex = 584;
            this.groupCodeLabel.Text = "仕入先コード";
            // 
            // groupCodeNameC1TextBox
            // 
            this.groupCodeNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeNameC1TextBox.Enabled = false;
            this.groupCodeNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeNameC1TextBox.Location = new System.Drawing.Point(218, 19);
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
            this.startDateC1DateEdit.Location = new System.Drawing.Point(575, 18);
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
            this.groupCodeC1ComboBox.Location = new System.Drawing.Point(120, 19);
            this.groupCodeC1ComboBox.MaxLength = 4;
            this.groupCodeC1ComboBox.Name = "groupCodeC1ComboBox";
            this.groupCodeC1ComboBox.Size = new System.Drawing.Size(91, 21);
            this.groupCodeC1ComboBox.TabIndex = 1;
            this.groupCodeC1ComboBox.Tag = null;
            this.groupCodeC1ComboBox.TextDetached = true;
            this.groupCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.groupCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.groupCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.groupCodeC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.groupCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // F004_NoAcceptInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Controls.Add(this.panel1);
            this.Name = "F004_NoAcceptInfo";
            this.Text = "F004_NoAcceptInfo";
            this.Load += new System.EventHandler(this.F004_NoAcceptInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private C1.Win.C1Input.C1ComboBox groupCodeC1ComboBox;
        private System.Windows.Forms.Label groupCodeLabel;
        private C1.Win.Calendar.C1DateEdit endDateC1DateEdit;
        private System.Windows.Forms.Label dateLabel;
        private C1.Win.C1Input.C1TextBox groupCodeNameC1TextBox;
        private C1.Win.Calendar.C1DateEdit startDateC1DateEdit;
    }
}