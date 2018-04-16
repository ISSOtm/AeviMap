namespace AeviMap
{
    partial class mapCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mapCreator));
            this.createButton = new System.Windows.Forms.Button();
            this.mapXBox = new System.Windows.Forms.NumericUpDown();
            this.mapDGrp = new System.Windows.Forms.GroupBox();
            this.mapHeight = new System.Windows.Forms.Label();
            this.mapWidth = new System.Windows.Forms.Label();
            this.mapYBox = new System.Windows.Forms.NumericUpDown();
            this.mapDataGrp = new System.Windows.Forms.GroupBox();
            this.FillerBlockID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.FillerBlockPreview = new System.Windows.Forms.PictureBox();
            this.mapNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tilesetPrev = new System.Windows.Forms.PictureBox();
            this.tilesetSelLab = new System.Windows.Forms.Label();
            this.tilesetSel = new System.Windows.Forms.ComboBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapXBox)).BeginInit();
            this.mapDGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapYBox)).BeginInit();
            this.mapDataGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).BeginInit();
            this.SuspendLayout();
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(333, 318);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 6;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.CreateMap);
            // 
            // mapXBox
            // 
            this.mapXBox.Location = new System.Drawing.Point(60, 31);
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
            this.mapDGrp.Controls.Add(this.mapHeight);
            this.mapDGrp.Controls.Add(this.mapWidth);
            this.mapDGrp.Controls.Add(this.mapYBox);
            this.mapDGrp.Controls.Add(this.mapXBox);
            this.mapDGrp.Location = new System.Drawing.Point(12, 12);
            this.mapDGrp.Name = "mapDGrp";
            this.mapDGrp.Size = new System.Drawing.Size(396, 96);
            this.mapDGrp.TabIndex = 2;
            this.mapDGrp.TabStop = false;
            this.mapDGrp.Text = "Map Dimensions";
            // 
            // mapHeight
            // 
            this.mapHeight.AutoSize = true;
            this.mapHeight.Location = new System.Drawing.Point(217, 33);
            this.mapHeight.Name = "mapHeight";
            this.mapHeight.Size = new System.Drawing.Size(38, 13);
            this.mapHeight.TabIndex = 5;
            this.mapHeight.Text = "Height";
            // 
            // mapWidth
            // 
            this.mapWidth.AutoSize = true;
            this.mapWidth.Location = new System.Drawing.Point(19, 33);
            this.mapWidth.Name = "mapWidth";
            this.mapWidth.Size = new System.Drawing.Size(35, 13);
            this.mapWidth.TabIndex = 4;
            this.mapWidth.Text = "Width";
            // 
            // mapYBox
            // 
            this.mapYBox.Location = new System.Drawing.Point(261, 31);
            this.mapYBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mapYBox.Name = "mapYBox";
            this.mapYBox.Size = new System.Drawing.Size(120, 20);
            this.mapYBox.TabIndex = 2;
            this.mapYBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // mapDataGrp
            // 
            this.mapDataGrp.Controls.Add(this.FillerBlockID);
            this.mapDataGrp.Controls.Add(this.label2);
            this.mapDataGrp.Controls.Add(this.FillerBlockPreview);
            this.mapDataGrp.Controls.Add(this.mapNameBox);
            this.mapDataGrp.Controls.Add(this.label1);
            this.mapDataGrp.Controls.Add(this.tilesetPrev);
            this.mapDataGrp.Controls.Add(this.tilesetSelLab);
            this.mapDataGrp.Controls.Add(this.tilesetSel);
            this.mapDataGrp.Location = new System.Drawing.Point(12, 114);
            this.mapDataGrp.Name = "mapDataGrp";
            this.mapDataGrp.Size = new System.Drawing.Size(396, 198);
            this.mapDataGrp.TabIndex = 3;
            this.mapDataGrp.TabStop = false;
            this.mapDataGrp.Text = "Map Data";
            // 
            // FillerBlockID
            // 
            this.FillerBlockID.Location = new System.Drawing.Point(82, 121);
            this.FillerBlockID.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.FillerBlockID.Name = "FillerBlockID";
            this.FillerBlockID.Size = new System.Drawing.Size(120, 20);
            this.FillerBlockID.TabIndex = 5;
            this.FillerBlockID.ValueChanged += new System.EventHandler(this.updateBlockPreview);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Filler block";
            // 
            // FillerBlockPreview
            // 
            this.FillerBlockPreview.Location = new System.Drawing.Point(221, 99);
            this.FillerBlockPreview.Name = "FillerBlockPreview";
            this.FillerBlockPreview.Size = new System.Drawing.Size(64, 64);
            this.FillerBlockPreview.TabIndex = 8;
            this.FillerBlockPreview.TabStop = false;
            // 
            // mapNameBox
            // 
            this.mapNameBox.Location = new System.Drawing.Point(82, 33);
            this.mapNameBox.Name = "mapNameBox";
            this.mapNameBox.Size = new System.Drawing.Size(120, 20);
            this.mapNameBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // tilesetPrev
            // 
            this.tilesetPrev.Image = global::AeviMap.tilesetDB.unknown_tileset;
            this.tilesetPrev.Location = new System.Drawing.Point(221, 33);
            this.tilesetPrev.Name = "tilesetPrev";
            this.tilesetPrev.Size = new System.Drawing.Size(160, 60);
            this.tilesetPrev.TabIndex = 4;
            this.tilesetPrev.TabStop = false;
            // 
            // tilesetSelLab
            // 
            this.tilesetSelLab.AutoSize = true;
            this.tilesetSelLab.Location = new System.Drawing.Point(19, 70);
            this.tilesetSelLab.Name = "tilesetSelLab";
            this.tilesetSelLab.Size = new System.Drawing.Size(38, 13);
            this.tilesetSelLab.TabIndex = 2;
            this.tilesetSelLab.Text = "Tileset";
            // 
            // tilesetSel
            // 
            this.tilesetSel.FormattingEnabled = true;
            this.tilesetSel.Items.AddRange(new object[] {
            "Outdoor",
            "Beach",
            "Indoor",
            "Ruins"});
            this.tilesetSel.Location = new System.Drawing.Point(82, 67);
            this.tilesetSel.Name = "tilesetSel";
            this.tilesetSel.Size = new System.Drawing.Size(120, 21);
            this.tilesetSel.TabIndex = 4;
            this.tilesetSel.Text = "Select Tileset";
            this.tilesetSel.SelectedIndexChanged += new System.EventHandler(this.updateTilesetPreview);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(252, 318);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 0;
            this.cancelBtn.TabStop = false;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // mapCreator
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(420, 353);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.mapDataGrp);
            this.Controls.Add(this.mapDGrp);
            this.Controls.Add(this.createButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mapCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Map";
            ((System.ComponentModel.ISupportInitialize)(this.mapXBox)).EndInit();
            this.mapDGrp.ResumeLayout(false);
            this.mapDGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapYBox)).EndInit();
            this.mapDataGrp.ResumeLayout(false);
            this.mapDataGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FillerBlockPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.NumericUpDown mapXBox;
        private System.Windows.Forms.GroupBox mapDGrp;
        private System.Windows.Forms.Label mapHeight;
        private System.Windows.Forms.Label mapWidth;
        private System.Windows.Forms.NumericUpDown mapYBox;
        private System.Windows.Forms.GroupBox mapDataGrp;
        private System.Windows.Forms.PictureBox tilesetPrev;
        private System.Windows.Forms.Label tilesetSelLab;
        private System.Windows.Forms.ComboBox tilesetSel;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.TextBox mapNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown FillerBlockID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox FillerBlockPreview;
    }
}