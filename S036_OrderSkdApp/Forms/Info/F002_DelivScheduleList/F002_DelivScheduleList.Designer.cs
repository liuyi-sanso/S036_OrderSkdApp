namespace S036_OrderSkdApp
{
    partial class F002_DelivScheduleList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F002_DelivScheduleList));
            this.c1TrueDBGrid = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // c1TrueDBGrid
            // 
            this.c1TrueDBGrid.AllowFilter = false;
            this.c1TrueDBGrid.AllowSort = false;
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
            this.c1TrueDBGrid.LinesPerRow = 2;
            this.c1TrueDBGrid.Location = new System.Drawing.Point(0, 50);
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
            this.c1TrueDBGrid.Size = new System.Drawing.Size(1182, 649);
            this.c1TrueDBGrid.TabIndex = 5;
            this.c1TrueDBGrid.TabStop = false;
            this.c1TrueDBGrid.UseCompatibleTextRendering = false;
            this.c1TrueDBGrid.VisualStyle = C1.Win.C1TrueDBGrid.VisualStyle.System;
            this.c1TrueDBGrid.FetchCellStyle += new C1.Win.C1TrueDBGrid.FetchCellStyleEventHandler(this.c1TrueDBGrid_FetchCellStyle);
            this.c1TrueDBGrid.PropBag = resources.GetString("c1TrueDBGrid.PropBag");
            // 
            // F002_DelivScheduleList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.c1TrueDBGrid);
            this.Name = "F002_DelivScheduleList";
            this.Text = "F002_DelivScheduleList";
            this.Load += new System.EventHandler(this.F002_DelivScheduleList_Load);
            this.SizeChanged += new System.EventHandler(this.F002_DelivScheduleList_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.c1TrueDBGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid c1TrueDBGrid;
    }
}