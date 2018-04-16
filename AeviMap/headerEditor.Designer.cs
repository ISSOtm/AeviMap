namespace AeviMap
{
    partial class headerEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(headerEditor));
            this.applyButton = new System.Windows.Forms.Button();
            this.mapXBox = new System.Windows.Forms.NumericUpDown();
            this.mapDGrp = new System.Windows.Forms.GroupBox();
            this.FillerBlockID = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FillerBlockPreview = new System.Windows.Forms.PictureBox();
            this.mapYOrigin = new System.Windows.Forms.NumericUpDown();
            this.mapXOrigin = new System.Windows.Forms.NumericUpDown();
            this.mapYLab = new System.Windows.Forms.Label();
            this.mapXLab = new System.Windows.Forms.Label();
            this.mapYBox = new System.Windows.Forms.NumericUpDown();
            this.mapDataGrp = new System.Windows.Forms.GroupBox();
            this.tilesetPrev = new System.Windows.Forms.PictureBox();
            this.songSelLab = new System.Windows.Forms.Label();
            this.tilesetSelLab = new System.Windows.Forms.Label();
            this.songSel = new System.Windows.Forms.ComboBox();
            this.tilesetSel = new System.Windows.Forms.ComboBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapXBox)).BeginInit();
            this.mapDGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapYOrigin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapXOrigin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapYBox)).BeginInit();
            this.mapDataGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).BeginInit();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(333, 382);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 5;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyModifications);
            // 
            // mapXBox
            // 
            this.mapXBox.Location = new System.Drawing.Point(60, 60);
            this.mapXBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.mapXBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mapXBox.Name = "mapXBox";
            this.mapXBox.Size = new System.Drawing.Size(120, 20);
            this.mapXBox.TabIndex = 1;
            this.mapXBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // mapDGrp
            // 
            this.mapDGrp.Controls.Add(this.FillerBlockID);
            this.mapDGrp.Controls.Add(this.label1);
            this.mapDGrp.Controls.Add(this.label3);
            this.mapDGrp.Controls.Add(this.label2);
            this.mapDGrp.Controls.Add(this.FillerBlockPreview);
            this.mapDGrp.Controls.Add(this.mapYOrigin);
            this.mapDGrp.Controls.Add(this.mapXOrigin);
            this.mapDGrp.Controls.Add(this.mapYLab);
            this.mapDGrp.Controls.Add(this.mapXLab);
            this.mapDGrp.Controls.Add(this.mapYBox);
            this.mapDGrp.Controls.Add(this.mapXBox);
            this.mapDGrp.Location = new System.Drawing.Point(12, 11);
            this.mapDGrp.Name = "mapDGrp";
            this.mapDGrp.Size = new System.Drawing.Size(396, 181);
            this.mapDGrp.TabIndex = 6;
            this.mapDGrp.TabStop = false;
            this.mapDGrp.Text = "Map Dimensions";
            // 
            // FillerBlockID
            // 
            this.FillerBlockID.Location = new System.Drawing.Point(122, 118);
            this.FillerBlockID.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.FillerBlockID.Name = "FillerBlockID";
            this.FillerBlockID.Size = new System.Drawing.Size(120, 20);
            this.FillerBlockID.TabIndex = 10;
            this.FillerBlockID.ValueChanged += new System.EventHandler(this.updateBlockPreview);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Y origin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Filler block";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "X origin";
            // 
            // FillerBlockPreview
            // 
            this.FillerBlockPreview.Location = new System.Drawing.Point(261, 96);
            this.FillerBlockPreview.Name = "FillerBlockPreview";
            this.FillerBlockPreview.Size = new System.Drawing.Size(64, 64);
            this.FillerBlockPreview.TabIndex = 11;
            this.FillerBlockPreview.TabStop = false;
            // 
            // mapYOrigin
            // 
            this.mapYOrigin.Location = new System.Drawing.Point(261, 25);
            this.mapYOrigin.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.mapYOrigin.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.mapYOrigin.Name = "mapYOrigin";
            this.mapYOrigin.Size = new System.Drawing.Size(120, 20);
            this.mapYOrigin.TabIndex = 7;
            // 
            // mapXOrigin
            // 
            this.mapXOrigin.Location = new System.Drawing.Point(60, 25);
            this.mapXOrigin.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.mapXOrigin.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.mapXOrigin.Name = "mapXOrigin";
            this.mapXOrigin.Size = new System.Drawing.Size(120, 20);
            this.mapXOrigin.TabIndex = 6;
            // 
            // mapYLab
            // 
            this.mapYLab.AutoSize = true;
            this.mapYLab.Location = new System.Drawing.Point(217, 62);
            this.mapYLab.Name = "mapYLab";
            this.mapYLab.Size = new System.Drawing.Size(38, 13);
            this.mapYLab.TabIndex = 5;
            this.mapYLab.Text = "Height";
            // 
            // mapXLab
            // 
            this.mapXLab.AutoSize = true;
            this.mapXLab.Location = new System.Drawing.Point(19, 62);
            this.mapXLab.Name = "mapXLab";
            this.mapXLab.Size = new System.Drawing.Size(35, 13);
            this.mapXLab.TabIndex = 4;
            this.mapXLab.Text = "Width";
            // 
            // mapYBox
            // 
            this.mapYBox.Location = new System.Drawing.Point(261, 60);
            this.mapYBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.mapYBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mapYBox.Name = "mapYBox";
            this.mapYBox.Size = new System.Drawing.Size(120, 20);
            this.mapYBox.TabIndex = 3;
            this.mapYBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // mapDataGrp
            // 
            this.mapDataGrp.Controls.Add(this.tilesetPrev);
            this.mapDataGrp.Controls.Add(this.songSelLab);
            this.mapDataGrp.Controls.Add(this.tilesetSelLab);
            this.mapDataGrp.Controls.Add(this.songSel);
            this.mapDataGrp.Controls.Add(this.tilesetSel);
            this.mapDataGrp.Location = new System.Drawing.Point(12, 198);
            this.mapDataGrp.Name = "mapDataGrp";
            this.mapDataGrp.Size = new System.Drawing.Size(396, 178);
            this.mapDataGrp.TabIndex = 7;
            this.mapDataGrp.TabStop = false;
            this.mapDataGrp.Text = "Map Data";
            // 
            // tilesetPrev
            // 
            this.tilesetPrev.Image = global::AeviMap.tilesetDB.unknown_tileset;
            this.tilesetPrev.Location = new System.Drawing.Point(221, 19);
            this.tilesetPrev.Name = "tilesetPrev";
            this.tilesetPrev.Size = new System.Drawing.Size(160, 60);
            this.tilesetPrev.TabIndex = 4;
            this.tilesetPrev.TabStop = false;
            // 
            // songSelLab
            // 
            this.songSelLab.AutoSize = true;
            this.songSelLab.Location = new System.Drawing.Point(18, 98);
            this.songSelLab.Name = "songSelLab";
            this.songSelLab.Size = new System.Drawing.Size(32, 13);
            this.songSelLab.TabIndex = 3;
            this.songSelLab.Text = "Song";
            // 
            // tilesetSelLab
            // 
            this.tilesetSelLab.AutoSize = true;
            this.tilesetSelLab.Location = new System.Drawing.Point(18, 40);
            this.tilesetSelLab.Name = "tilesetSelLab";
            this.tilesetSelLab.Size = new System.Drawing.Size(38, 13);
            this.tilesetSelLab.TabIndex = 2;
            this.tilesetSelLab.Text = "Tileset";
            // 
            // songSel
            // 
            this.songSel.FormattingEnabled = true;
            this.songSel.Location = new System.Drawing.Point(73, 98);
            this.songSel.Name = "songSel";
            this.songSel.Size = new System.Drawing.Size(121, 21);
            this.songSel.TabIndex = 1;
            this.songSel.Text = "Select Song";
            // 
            // tilesetSel
            // 
            this.tilesetSel.FormattingEnabled = true;
            this.tilesetSel.Location = new System.Drawing.Point(73, 37);
            this.tilesetSel.Name = "tilesetSel";
            this.tilesetSel.Size = new System.Drawing.Size(121, 21);
            this.tilesetSel.TabIndex = 0;
            this.tilesetSel.Text = "Select Tileset";
            this.tilesetSel.SelectedIndexChanged += new System.EventHandler(this.updateTilesetPreview);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(252, 382);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // headerEditor
            // 
            this.AcceptButton = this.applyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 417);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.mapDGrp);
            this.Controls.Add(this.mapDataGrp);
            this.Controls.Add(this.cancelBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "headerEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Header";
            this.Click += new System.EventHandler(this.ApplyModifications);
            ((System.ComponentModel.ISupportInitialize)(this.mapXBox)).EndInit();
            this.mapDGrp.ResumeLayout(false);
            this.mapDGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapYOrigin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapXOrigin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapYBox)).EndInit();
            this.mapDataGrp.ResumeLayout(false);
            this.mapDataGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.NumericUpDown mapXBox;
        private System.Windows.Forms.GroupBox mapDGrp;
        private System.Windows.Forms.Label mapYLab;
        private System.Windows.Forms.Label mapXLab;
        private System.Windows.Forms.NumericUpDown mapYBox;
        private System.Windows.Forms.GroupBox mapDataGrp;
        private System.Windows.Forms.PictureBox tilesetPrev;
        private System.Windows.Forms.Label songSelLab;
        private System.Windows.Forms.Label tilesetSelLab;
        private System.Windows.Forms.ComboBox songSel;
        private System.Windows.Forms.ComboBox tilesetSel;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown mapYOrigin;
        private System.Windows.Forms.NumericUpDown mapXOrigin;
        private System.Windows.Forms.NumericUpDown FillerBlockID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox FillerBlockPreview;
    }
}