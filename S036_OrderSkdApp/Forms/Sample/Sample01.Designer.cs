namespace S036_OrderSkdApp
{
    partial class Sample01
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
            this.components = new System.ComponentModel.Container();
            this.c1SuperTooltip1 = new C1.Win.C1SuperTooltip.C1SuperTooltip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.productNameC1TextBox = new C1.Win.C1Input.C1TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.F901Button = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.productNameC1TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // c1SuperTooltip1
            // 
            this.c1SuperTooltip1.AutoPopDelay = 30000;
            this.c1SuperTooltip1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c1SuperTooltip1.IsBalloon = true;
            this.c1SuperTooltip1.Opacity = 0.75D;
            this.c1SuperTooltip1.RightToLeft = System.Windows.Forms.RightToLeft.Inherit;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(181, 432);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 53);
            this.button1.TabIndex = 0;
            this.button1.Text = "サンプルボタン";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // productNameC1TextBox
            // 
            this.productNameC1TextBox.BackColor = System.Drawing.Color.White;
            this.productNameC1TextBox.BorderColor = System.Drawing.Color.Black;
            this.productNameC1TextBox.DisabledForeColor = System.Drawing.Color.Black;
            this.productNameC1TextBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.productNameC1TextBox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.productNameC1TextBox.Location = new System.Drawing.Point(239, 289);
            this.productNameC1TextBox.MaxLength = 20;
            this.productNameC1TextBox.Name = "productNameC1TextBox";
            this.productNameC1TextBox.Size = new System.Drawing.Size(210, 21);
            this.productNameC1TextBox.TabIndex = 4;
            this.productNameC1TextBox.Tag = null;
            this.productNameC1TextBox.TextDetached = true;
            this.productNameC1TextBox.Value = "a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(178, 291);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "機種名";
            // 
            // F901Button
            // 
            this.F901Button.Location = new System.Drawing.Point(459, 288);
            this.F901Button.Name = "F901Button";
            this.F901Button.Size = new System.Drawing.Size(72, 23);
            this.F901Button.TabIndex = 6;
            this.F901Button.Text = "検索";
            this.F901Button.UseVisualStyleBackColor = true;
            this.F901Button.Click += new System.EventHandler(this.F901Button_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(617, 273);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 53);
            this.button2.TabIndex = 7;
            this.button2.Text = "現品票発行ボタン";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Sample01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.F901Button);
            this.Controls.Add(this.productNameC1TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Name = "Sample01";
            this.Text = "Sample01";
            this.Load += new System.EventHandler(this.Sample01_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productNameC1TextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1SuperTooltip.C1SuperTooltip c1SuperTooltip1;
        private System.Windows.Forms.Button button1;
        private C1.Win.C1Input.C1TextBox productNameC1TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button F901Button;
        private System.Windows.Forms.Button button2;
    }
}