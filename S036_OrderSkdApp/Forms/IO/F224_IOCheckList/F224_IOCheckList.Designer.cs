namespace S036_OrderSkdApp
{
    partial class F224_IOCheckList
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.inputOrderButton2 = new System.Windows.Forms.Button();
            this.accountsPayableButton = new System.Windows.Forms.Button();
            this.inputOrderButton = new System.Windows.Forms.Button();
            this.onlineInputPersonBt = new System.Windows.Forms.Button();
            this.onlineInputStencilBt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.userCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.userNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.groupCodeC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.groupCodeNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dateC1DateEdit = new C1.Win.Calendar.C1DateEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateC1DateEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.inputOrderButton2);
            this.panel2.Controls.Add(this.accountsPayableButton);
            this.panel2.Controls.Add(this.inputOrderButton);
            this.panel2.Controls.Add(this.onlineInputPersonBt);
            this.panel2.Controls.Add(this.onlineInputStencilBt);
            this.panel2.Location = new System.Drawing.Point(729, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(455, 507);
            this.panel2.TabIndex = 297;
            // 
            // inputOrderButton2
            // 
            this.inputOrderButton2.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.inputOrderButton2.Location = new System.Drawing.Point(60, 91);
            this.inputOrderButton2.Name = "inputOrderButton2";
            this.inputOrderButton2.Size = new System.Drawing.Size(336, 69);
            this.inputOrderButton2.TabIndex = 16;
            this.inputOrderButton2.TabStop = false;
            this.inputOrderButton2.Text = "買掛チェックリスト(HT/伝票レス)";
            this.inputOrderButton2.UseVisualStyleBackColor = true;
            this.inputOrderButton2.Click += new System.EventHandler(this.inputOrderButton2_Click);
            // 
            // accountsPayableButton
            // 
            this.accountsPayableButton.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.accountsPayableButton.Location = new System.Drawing.Point(60, 310);
            this.accountsPayableButton.Name = "accountsPayableButton";
            this.accountsPayableButton.Size = new System.Drawing.Size(336, 69);
            this.accountsPayableButton.TabIndex = 15;
            this.accountsPayableButton.TabStop = false;
            this.accountsPayableButton.Text = "買掛チェックリスト（非表示）";
            this.accountsPayableButton.UseVisualStyleBackColor = true;
            this.accountsPayableButton.Visible = false;
            this.accountsPayableButton.Click += new System.EventHandler(this.accountsPayableButton_Click);
            // 
            // inputOrderButton
            // 
            this.inputOrderButton.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.inputOrderButton.Location = new System.Drawing.Point(60, 18);
            this.inputOrderButton.Name = "inputOrderButton";
            this.inputOrderButton.Size = new System.Drawing.Size(336, 69);
            this.inputOrderButton.TabIndex = 14;
            this.inputOrderButton.TabStop = false;
            this.inputOrderButton.Text = "買掛チェックリスト(HT)";
            this.inputOrderButton.UseVisualStyleBackColor = true;
            this.inputOrderButton.Click += new System.EventHandler(this.inputOrderButton_Click);
            // 
            // onlineInputPersonBt
            // 
            this.onlineInputPersonBt.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.onlineInputPersonBt.Location = new System.Drawing.Point(60, 237);
            this.onlineInputPersonBt.Name = "onlineInputPersonBt";
            this.onlineInputPersonBt.Size = new System.Drawing.Size(336, 69);
            this.onlineInputPersonBt.TabIndex = 13;
            this.onlineInputPersonBt.TabStop = false;
            this.onlineInputPersonBt.Text = "オンライン入力済原紙\r\n（担当者別）";
            this.onlineInputPersonBt.UseVisualStyleBackColor = true;
            this.onlineInputPersonBt.Click += new System.EventHandler(this.onlineInputPersonBt_Click);
            // 
            // onlineInputStencilBt
            // 
            this.onlineInputStencilBt.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.onlineInputStencilBt.Location = new System.Drawing.Point(60, 164);
            this.onlineInputStencilBt.Name = "onlineInputStencilBt";
            this.onlineInputStencilBt.Size = new System.Drawing.Size(336, 69);
            this.onlineInputStencilBt.TabIndex = 12;
            this.onlineInputStencilBt.TabStop = false;
            this.onlineInputStencilBt.Text = "オンライン入力済原紙";
            this.onlineInputStencilBt.UseVisualStyleBackColor = true;
            this.onlineInputStencilBt.Click += new System.EventHandler(this.onlineInputStencilBt_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.userCodeC1ComboBox);
            this.panel1.Controls.Add(this.userNameC1TextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupCodeC1ComboBox);
            this.panel1.Controls.Add(this.groupCodeNameC1TextBox);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.dateC1DateEdit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(701, 507);
            this.panel1.TabIndex = 296;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(114, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(310, 32);
            this.label2.TabIndex = 455;
            this.label2.Text = "※担当者は買掛チェックリスト（HT）、\r\nオンライン入力済原紙（担当者別）で使用します";
            // 
            // userCodeC1ComboBox
            // 
            this.userCodeC1ComboBox.AllowSpinLoop = false;
            this.userCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.userCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.userCodeC1ComboBox.BackColor = System.Drawing.Color.White;
            this.userCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.userCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.userCodeC1ComboBox.DropDownWidth = -1;
            this.userCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.userCodeC1ComboBox.GapHeight = 0;
            this.userCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.userCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.userCodeC1ComboBox.ItemsDisplayMember = "";
            this.userCodeC1ComboBox.ItemsValueMember = "";
            this.userCodeC1ComboBox.Label = this.label1;
            this.userCodeC1ComboBox.Location = new System.Drawing.Point(117, 73);
            this.userCodeC1ComboBox.MaxLength = 5;
            this.userCodeC1ComboBox.Name = "userCodeC1ComboBox";
            this.userCodeC1ComboBox.Size = new System.Drawing.Size(86, 21);
            this.userCodeC1ComboBox.TabIndex = 1;
            this.userCodeC1ComboBox.Tag = null;
            this.userCodeC1ComboBox.TextDetached = true;
            this.userCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.userCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.userCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.userCodeC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.userCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(27, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 451;
            this.label1.Text = "担当者";
            // 
            // userNameC1TextBox
            // 
            this.userNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.userNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.userNameC1TextBox.Enabled = false;
            this.userNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.userNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.userNameC1TextBox.Location = new System.Drawing.Point(209, 75);
            this.userNameC1TextBox.MaxLength = 12;
            this.userNameC1TextBox.Name = "userNameC1TextBox";
            this.userNameC1TextBox.Size = new System.Drawing.Size(171, 21);
            this.userNameC1TextBox.TabIndex = 454;
            this.userNameC1TextBox.TabStop = false;
            this.userNameC1TextBox.Tag = null;
            // 
            // groupCodeC1ComboBox
            // 
            this.groupCodeC1ComboBox.AllowSpinLoop = false;
            this.groupCodeC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.groupCodeC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.groupCodeC1ComboBox.BackColor = System.Drawing.Color.White;
            this.groupCodeC1ComboBox.BorderColor = System.Drawing.Color.Red;
            this.groupCodeC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.groupCodeC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeC1ComboBox.DropDownWidth = -1;
            this.groupCodeC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeC1ComboBox.GapHeight = 0;
            this.groupCodeC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.groupCodeC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeC1ComboBox.ItemsDisplayMember = "";
            this.groupCodeC1ComboBox.ItemsValueMember = "";
            this.groupCodeC1ComboBox.Location = new System.Drawing.Point(117, 30);
            this.groupCodeC1ComboBox.MaxLength = 4;
            this.groupCodeC1ComboBox.Name = "groupCodeC1ComboBox";
            this.groupCodeC1ComboBox.Size = new System.Drawing.Size(86, 21);
            this.groupCodeC1ComboBox.TabIndex = 0;
            this.groupCodeC1ComboBox.Tag = null;
            this.groupCodeC1ComboBox.TextDetached = true;
            this.groupCodeC1ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectIndexChanged);
            this.groupCodeC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.groupCodeC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.groupCodeC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.groupCodeC1ComboBox.Validated += new System.EventHandler(this.ComboBoxValidated);
            // 
            // groupCodeNameC1TextBox
            // 
            this.groupCodeNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.groupCodeNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupCodeNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.groupCodeNameC1TextBox.Enabled = false;
            this.groupCodeNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupCodeNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.groupCodeNameC1TextBox.Location = new System.Drawing.Point(209, 30);
            this.groupCodeNameC1TextBox.MaxLength = 12;
            this.groupCodeNameC1TextBox.Name = "groupCodeNameC1TextBox";
            this.groupCodeNameC1TextBox.Size = new System.Drawing.Size(171, 21);
            this.groupCodeNameC1TextBox.TabIndex = 445;
            this.groupCodeNameC1TextBox.TabStop = false;
            this.groupCodeNameC1TextBox.Tag = null;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(27, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 446;
            this.label8.Text = "組立部門";
            // 
            // dateC1DateEdit
            // 
            this.dateC1DateEdit.AllowSpinLoop = false;
            this.dateC1DateEdit.AutoSize = false;
            this.dateC1DateEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dateC1DateEdit.Calendar.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            this.dateC1DateEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dateC1DateEdit.DisplayFormat.CustomFormat = "yyyy/MM/dd";
            this.dateC1DateEdit.DisplayFormat.EmptyAsNull = false;
            this.dateC1DateEdit.DisplayFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.CustomFormat;
            this.dateC1DateEdit.DisplayFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)(((C1.Win.C1Input.FormatInfoInheritFlags.TrimStart | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.dateC1DateEdit.EditFormat.CustomFormat = "yyyy/MM/dd";
            this.dateC1DateEdit.EditFormat.EmptyAsNull = false;
            this.dateC1DateEdit.EditFormat.FormatType = C1.Win.C1Input.FormatTypeEnum.CustomFormat;
            this.dateC1DateEdit.EditFormat.Inherit = ((C1.Win.C1Input.FormatInfoInheritFlags)((((C1.Win.C1Input.FormatInfoInheritFlags.NullText | C1.Win.C1Input.FormatInfoInheritFlags.TrimStart) 
            | C1.Win.C1Input.FormatInfoInheritFlags.TrimEnd) 
            | C1.Win.C1Input.FormatInfoInheritFlags.CalendarType)));
            this.dateC1DateEdit.EmptyAsNull = true;
            this.dateC1DateEdit.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateC1DateEdit.FormatType = C1.Win.C1Input.FormatTypeEnum.CustomFormat;
            this.dateC1DateEdit.ImagePadding = new System.Windows.Forms.Padding(0);
            this.dateC1DateEdit.Location = new System.Drawing.Point(117, 159);
            this.dateC1DateEdit.Name = "dateC1DateEdit";
            this.dateC1DateEdit.Size = new System.Drawing.Size(149, 23);
            this.dateC1DateEdit.TabIndex = 2;
            this.dateC1DateEdit.Tag = null;
            this.dateC1DateEdit.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.DropDown;
            this.dateC1DateEdit.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.dateC1DateEdit.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.label4.Location = new System.Drawing.Point(27, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 195;
            this.label4.Text = "日付";
            // 
            // F224_IOCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F224_IOCheckList";
            this.Text = "F224_IOCheckList";
            this.Load += new System.EventHandler(this.F224_IOCheckList_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupCodeNameC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateC1DateEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button onlineInputPersonBt;
        private System.Windows.Forms.Button onlineInputStencilBt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1Input.C1ComboBox groupCodeC1ComboBox;
        private C1.Win.C1Input.C1TextBox groupCodeNameC1TextBox;
        private System.Windows.Forms.Label label8;
        private C1.Win.Calendar.C1DateEdit dateC1DateEdit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button accountsPayableButton;
        private System.Windows.Forms.Button inputOrderButton;
        private C1.Win.C1Input.C1ComboBox userCodeC1ComboBox;
        private C1.Win.C1Input.C1TextBox userNameC1TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button inputOrderButton2;
    }
}