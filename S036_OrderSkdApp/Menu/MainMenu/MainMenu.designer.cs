namespace S036_OrderSkdApp
{
    partial class MainMenu
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
            C1.Win.C1Tile.PanelElement panelElement2 = new C1.Win.C1Tile.PanelElement();
            C1.Win.C1Tile.ImageElement imageElement2 = new C1.Win.C1Tile.ImageElement();
            C1.Win.C1Tile.TextElement textElement2 = new C1.Win.C1Tile.TextElement();
            this.label_main_title = new System.Windows.Forms.Label();
            this.tileControlMainMenu = new C1.Win.C1Tile.C1TileControl();
            this.group1 = new C1.Win.C1Tile.Group();
            this.tile1 = new C1.Win.C1Tile.Tile();
            this.tile2 = new C1.Win.C1Tile.Tile();
            this.tile3 = new C1.Win.C1Tile.Tile();
            this.panel1 = new System.Windows.Forms.Panel();
            this.developLabel = new System.Windows.Forms.Label();
            this.B_Back = new System.Windows.Forms.Button();
            this.B_Close = new System.Windows.Forms.Button();
            this.panelButtom = new System.Windows.Forms.Panel();
            this.L_Message = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelButtom.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_main_title
            // 
            this.label_main_title.AutoSize = true;
            this.label_main_title.Font = new System.Drawing.Font("メイリオ", 24F);
            this.label_main_title.Location = new System.Drawing.Point(29, 54);
            this.label_main_title.Name = "label_main_title";
            this.label_main_title.Size = new System.Drawing.Size(244, 48);
            this.label_main_title.TabIndex = 5;
            this.label_main_title.Text = "部品調達システム";
            // 
            // tileControlMainMenu
            // 
            this.tileControlMainMenu.AllowChecking = true;
            this.tileControlMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileControlMainMenu.CellHeight = 125;
            this.tileControlMainMenu.CellWidth = 125;
            // 
            // 
            // 
            panelElement2.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            panelElement2.Children.Add(imageElement2);
            panelElement2.Children.Add(textElement2);
            panelElement2.Margin = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.tileControlMainMenu.DefaultTemplate.Elements.Add(panelElement2);
            this.tileControlMainMenu.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tileControlMainMenu.Groups.Add(this.group1);
            this.tileControlMainMenu.Location = new System.Drawing.Point(-2, 98);
            this.tileControlMainMenu.Name = "tileControlMainMenu";
            this.tileControlMainMenu.Orientation = C1.Win.C1Tile.LayoutOrientation.Vertical;
            this.tileControlMainMenu.Padding = new System.Windows.Forms.Padding(0);
            this.tileControlMainMenu.Size = new System.Drawing.Size(688, 548);
            this.tileControlMainMenu.TabIndex = 4;
            // 
            // group1
            // 
            this.group1.Name = "group1";
            this.group1.Text = "グループ１";
            this.group1.Tiles.Add(this.tile1);
            this.group1.Tiles.Add(this.tile2);
            this.group1.Tiles.Add(this.tile3);
            // 
            // tile1
            // 
            this.tile1.BackColor = System.Drawing.Color.LightCoral;
            this.tile1.Name = "tile1";
            this.tile1.Text = "タイル１";
            // 
            // tile2
            // 
            this.tile2.BackColor = System.Drawing.Color.Teal;
            this.tile2.Name = "tile2";
            this.tile2.Text = "タイル２";
            // 
            // tile3
            // 
            this.tile3.BackColor = System.Drawing.Color.SteelBlue;
            this.tile3.ImageKey = "(なし)";
            this.tile3.Name = "tile3";
            this.tile3.Text = "タイル３";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.developLabel);
            this.panel1.Controls.Add(this.B_Back);
            this.panel1.Controls.Add(this.B_Close);
            this.panel1.Location = new System.Drawing.Point(-2, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(688, 45);
            this.panel1.TabIndex = 6;
            // 
            // developLabel
            // 
            this.developLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.developLabel.AutoSize = true;
            this.developLabel.BackColor = System.Drawing.Color.Silver;
            this.developLabel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.developLabel.ForeColor = System.Drawing.Color.Red;
            this.developLabel.Location = new System.Drawing.Point(15, 9);
            this.developLabel.Name = "developLabel";
            this.developLabel.Size = new System.Drawing.Size(113, 24);
            this.developLabel.TabIndex = 18;
            this.developLabel.Text = "SSERV33";
            this.developLabel.Visible = false;
            // 
            // B_Back
            // 
            this.B_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.B_Back.Location = new System.Drawing.Point(462, 4);
            this.B_Back.Name = "B_Back";
            this.B_Back.Size = new System.Drawing.Size(103, 38);
            this.B_Back.TabIndex = 1;
            this.B_Back.Text = "戻る";
            this.B_Back.UseVisualStyleBackColor = false;
            // 
            // B_Close
            // 
            this.B_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.B_Close.Location = new System.Drawing.Point(571, 4);
            this.B_Close.Name = "B_Close";
            this.B_Close.Size = new System.Drawing.Size(103, 38);
            this.B_Close.TabIndex = 0;
            this.B_Close.Text = "閉じる";
            this.B_Close.UseVisualStyleBackColor = false;
            this.B_Close.Click += new System.EventHandler(this.B_Close_Click);
            // 
            // panelButtom
            // 
            this.panelButtom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtom.BackColor = System.Drawing.Color.DimGray;
            this.panelButtom.Controls.Add(this.L_Message);
            this.panelButtom.Location = new System.Drawing.Point(-2, 663);
            this.panelButtom.Name = "panelButtom";
            this.panelButtom.Size = new System.Drawing.Size(688, 40);
            this.panelButtom.TabIndex = 7;
            // 
            // L_Message
            // 
            this.L_Message.AutoSize = true;
            this.L_Message.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.L_Message.ForeColor = System.Drawing.Color.White;
            this.L_Message.Location = new System.Drawing.Point(15, 7);
            this.L_Message.Name = "L_Message";
            this.L_Message.Size = new System.Drawing.Size(105, 24);
            this.L_Message.TabIndex = 0;
            this.L_Message.Text = "L_MESSAGE";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 701);
            this.Controls.Add(this.panelButtom);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label_main_title);
            this.Controls.Add(this.tileControlMainMenu);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenu_FormClosed);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelButtom.ResumeLayout(false);
            this.panelButtom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_main_title;
        private C1.Win.C1Tile.C1TileControl tileControlMainMenu;
        private C1.Win.C1Tile.Group group1;
        private C1.Win.C1Tile.Tile tile1;
        private C1.Win.C1Tile.Tile tile2;
        private C1.Win.C1Tile.Tile tile3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button B_Back;
        private System.Windows.Forms.Button B_Close;
        private System.Windows.Forms.Panel panelButtom;
        private System.Windows.Forms.Label L_Message;
        protected System.Windows.Forms.Label developLabel;
    }
}