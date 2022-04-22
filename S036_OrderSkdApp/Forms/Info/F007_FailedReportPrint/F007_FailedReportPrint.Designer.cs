
namespace S036_OrderSkdApp
{
    partial class F007_FailedReportPrint
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
            this.printButton1 = new System.Windows.Forms.Button();
            this.dateLabel = new System.Windows.Forms.Label();
            this.endDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.startDateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.printButton2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // printButton1
            // 
            this.printButton1.AutoSize = true;
            this.printButton1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.printButton1.Location = new System.Drawing.Point(14, 101);
            this.printButton1.Name = "printButton1";
            this.printButton1.Size = new System.Drawing.Size(155, 45);
            this.printButton1.TabIndex = 4;
            this.printButton1.TabStop = false;
            this.printButton1.Text = "印刷";
            this.printButton1.UseVisualStyleBackColor = true;
            this.printButton1.Click += new System.EventHandler(this.printButton1_Click);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.dateLabel.Location = new System.Drawing.Point(11, 68);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(40, 16);
            this.dateLabel.TabIndex = 581;
            this.dateLabel.Text = "期間";
            // 
            // endDateC1DateEdit
            // 
            this.endDateC1DateEdit.AllowSpinLoop = false;
            this.endDateC1DateEdit.BorderColor = System.Drawing.Color.Red;
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
            this.endDateC1DateEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.endDateC1DateEdit.Label = this.dateLabel;
            this.endDateC1DateEdit.Location = new System.Drawing.Point(207, 66);
            this.endDateC1DateEdit.Name = "endDateC1DateEdit";
            this.endDateC1DateEdit.Size = new System.Drawing.Size(121, 21);
            this.endDateC1DateEdit.TabIndex = 2;
            this.endDateC1DateEdit.Tag = null;
            this.endDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.endDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.endDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(181, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 580;
            this.label1.Text = "～";
            // 
            // startDateC1DateEdit
            // 
            this.startDateC1DateEdit.AllowSpinLoop = false;
            this.startDateC1DateEdit.BorderColor = System.Drawing.Color.Red;
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
            this.startDateC1DateEdit.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.startDateC1DateEdit.Label = this.dateLabel;
            this.startDateC1DateEdit.Location = new System.Drawing.Point(54, 66);
            this.startDateC1DateEdit.Name = "startDateC1DateEdit";
            this.startDateC1DateEdit.Size = new System.Drawing.Size(121, 21);
            this.startDateC1DateEdit.TabIndex = 1;
            this.startDateC1DateEdit.Tag = null;
            this.startDateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.startDateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.startDateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(11, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 16);
            this.label2.TabIndex = 595;
            this.label2.Text = "【不適合品 社内移行データ一覧】";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.printButton2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.printButton1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.startDateC1DateEdit);
            this.panel2.Controls.Add(this.dateLabel);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.endDateC1DateEdit);
            this.panel2.Location = new System.Drawing.Point(0, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1184, 499);
            this.panel2.TabIndex = 594;
            this.panel2.TabStop = true;
            // 
            // printButton2
            // 
            this.printButton2.AutoSize = true;
            this.printButton2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.printButton2.Location = new System.Drawing.Point(14, 230);
            this.printButton2.Name = "printButton2";
            this.printButton2.Size = new System.Drawing.Size(155, 45);
            this.printButton2.TabIndex = 598;
            this.printButton2.TabStop = false;
            this.printButton2.Text = "印刷";
            this.printButton2.UseVisualStyleBackColor = true;
            this.printButton2.Click += new System.EventHandler(this.printButton2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(11, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 16);
            this.label3.TabIndex = 601;
            this.label3.Text = "【不適合品 未処理一覧】";
            // 
            // F007_FailedReportPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel2);
            this.Name = "F007_FailedReportPrint";
            this.Text = "F007_FailedReportPrint";
            this.Load += new System.EventHandler(this.F007_FailedReportPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.endDateC1DateEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateC1DateEdit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button printButton1;
        private System.Windows.Forms.Label dateLabel;
        private C1.Win.Calendar.C1DateEdit endDateC1DateEdit;
        private System.Windows.Forms.Label label1;
        private C1.Win.Calendar.C1DateEdit startDateC1DateEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button printButton2;
        private System.Windows.Forms.Label label3;
    }
}