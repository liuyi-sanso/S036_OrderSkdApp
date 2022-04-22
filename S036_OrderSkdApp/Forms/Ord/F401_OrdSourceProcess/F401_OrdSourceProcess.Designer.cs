namespace S036_OrderSkdApp
{
    partial class F401_OrdSourceProcess
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
            this.confirmButton = new System.Windows.Forms.Button();
            this.maintButton = new System.Windows.Forms.Button();
            this.searchC1ComboBox = new C1.Win.C1Input.C1ComboBox();
            this.completFlagLabel = new System.Windows.Forms.Label();
            this.excelButton = new System.Windows.Forms.Button();
            this.searchCodeLabel = new System.Windows.Forms.Label();
            this.searchCodeC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.searchNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchC1ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCodeC1TextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchNameC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.confirmButton);
            this.panel1.Controls.Add(this.maintButton);
            this.panel1.Controls.Add(this.searchC1ComboBox);
            this.panel1.Controls.Add(this.completFlagLabel);
            this.panel1.Controls.Add(this.excelButton);
            this.panel1.Controls.Add(this.searchCodeLabel);
            this.panel1.Controls.Add(this.searchCodeC1TextBox);
            this.panel1.Controls.Add(this.searchNameC1TextBox);
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 515);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // confirmButton
            // 
            this.confirmButton.AutoSize = true;
            this.confirmButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.confirmButton.Location = new System.Drawing.Point(127, 249);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(253, 33);
            this.confirmButton.TabIndex = 4;
            this.confirmButton.TabStop = false;
            this.confirmButton.Text = "確定";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // maintButton
            // 
            this.maintButton.AutoSize = true;
            this.maintButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.maintButton.Location = new System.Drawing.Point(127, 195);
            this.maintButton.Name = "maintButton";
            this.maintButton.Size = new System.Drawing.Size(253, 33);
            this.maintButton.TabIndex = 3;
            this.maintButton.TabStop = false;
            this.maintButton.Text = "W発注日程マスタ　メンテナンス";
            this.maintButton.UseVisualStyleBackColor = true;
            this.maintButton.Click += new System.EventHandler(this.maintButton_Click);
            // 
            // searchC1ComboBox
            // 
            this.searchC1ComboBox.AllowSpinLoop = false;
            this.searchC1ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.searchC1ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.searchC1ComboBox.BorderColor = System.Drawing.Color.Red;
            this.searchC1ComboBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchC1ComboBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchC1ComboBox.DisabledForeColor = System.Drawing.Color.Black;
            this.searchC1ComboBox.DropDownWidth = -1;
            this.searchC1ComboBox.ErrorInfo.ShowErrorMessage = false;
            this.searchC1ComboBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchC1ComboBox.GapHeight = 0;
            this.searchC1ComboBox.ImagePadding = new System.Windows.Forms.Padding(0);
            this.searchC1ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.searchC1ComboBox.ItemsDisplayMember = "";
            this.searchC1ComboBox.ItemsValueMember = "";
            this.searchC1ComboBox.Label = this.completFlagLabel;
            this.searchC1ComboBox.Location = new System.Drawing.Point(193, 24);
            this.searchC1ComboBox.MaxLength = 6;
            this.searchC1ComboBox.Name = "searchC1ComboBox";
            this.searchC1ComboBox.Size = new System.Drawing.Size(135, 21);
            this.searchC1ComboBox.TabIndex = 0;
            this.searchC1ComboBox.Tag = null;
            this.searchC1ComboBox.TextDetached = true;
            this.searchC1ComboBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.searchC1ComboBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.searchC1ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            // 
            // completFlagLabel
            // 
            this.completFlagLabel.AutoSize = true;
            this.completFlagLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.completFlagLabel.Location = new System.Drawing.Point(124, 27);
            this.completFlagLabel.Name = "completFlagLabel";
            this.completFlagLabel.Size = new System.Drawing.Size(64, 16);
            this.completFlagLabel.TabIndex = 589;
            this.completFlagLabel.Text = "絞り込み";
            // 
            // excelButton
            // 
            this.excelButton.AutoSize = true;
            this.excelButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.excelButton.Location = new System.Drawing.Point(127, 142);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(253, 33);
            this.excelButton.TabIndex = 2;
            this.excelButton.TabStop = false;
            this.excelButton.Text = "Ｗ発注日程マスタ　Excel";
            this.excelButton.UseVisualStyleBackColor = true;
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // searchCodeLabel
            // 
            this.searchCodeLabel.AutoSize = true;
            this.searchCodeLabel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchCodeLabel.Location = new System.Drawing.Point(124, 77);
            this.searchCodeLabel.Name = "searchCodeLabel";
            this.searchCodeLabel.Size = new System.Drawing.Size(44, 16);
            this.searchCodeLabel.TabIndex = 440;
            this.searchCodeLabel.Text = "コード";
            // 
            // searchCodeC1TextBox
            // 
            this.searchCodeC1TextBox.BorderColor = System.Drawing.Color.Red;
            this.searchCodeC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchCodeC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.searchCodeC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchCodeC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.searchCodeC1TextBox.Label = this.searchCodeLabel;
            this.searchCodeC1TextBox.Location = new System.Drawing.Point(193, 74);
            this.searchCodeC1TextBox.MaxLength = 10;
            this.searchCodeC1TextBox.Name = "searchCodeC1TextBox";
            this.searchCodeC1TextBox.Size = new System.Drawing.Size(110, 21);
            this.searchCodeC1TextBox.TabIndex = 1;
            this.searchCodeC1TextBox.Tag = null;
            this.searchCodeC1TextBox.TextDetached = true;
            this.searchCodeC1TextBox.Enter += new System.EventHandler(this.ChangeCtlBkColEnter);
            this.searchCodeC1TextBox.Leave += new System.EventHandler(this.ChangeCtlBkColLeave);
            this.searchCodeC1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.searchCodeC1TextBox_Validating);
            // 
            // searchNameC1TextBox
            // 
            this.searchNameC1TextBox.BackColor = System.Drawing.Color.PeachPuff;
            this.searchNameC1TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.searchNameC1TextBox.Enabled = false;
            this.searchNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.searchNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.searchNameC1TextBox.Location = new System.Drawing.Point(306, 74);
            this.searchNameC1TextBox.MaxLength = 12;
            this.searchNameC1TextBox.Name = "searchNameC1TextBox";
            this.searchNameC1TextBox.Size = new System.Drawing.Size(200, 21);
            this.searchNameC1TextBox.TabIndex = 442;
            this.searchNameC1TextBox.TabStop = false;
            this.searchNameC1TextBox.Tag = null;
            // 
            // F401_OrdSourceProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.panel1);
            this.Name = "F401_OrdSourceProcess";
            this.Text = "F401_OrdSourceProcess";
            this.Load += new System.EventHandler(this.F401_OrdSourceProcess_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchC1ComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCodeC1TextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchNameC1TextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button maintButton;
        private C1.Win.C1Input.C1ComboBox searchC1ComboBox;
        private System.Windows.Forms.Label completFlagLabel;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Label searchCodeLabel;
        public C1.Win.C1Input.C1TextBox searchCodeC1TextBox;
        public C1.Win.C1Input.C1TextBox searchNameC1TextBox;
    }
}