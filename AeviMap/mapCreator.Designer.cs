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
            this.cBtn = new System.Windows.Forms.Button();
            this.mapXBox = new System.Windows.Forms.NumericUpDown();
            this.mapDGrp = new System.Windows.Forms.GroupBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.mapYBox)).BeginInit();
            this.mapDataGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).BeginInit();
            this.SuspendLayout();
            // 
            // cBtn
            // 
            this.cBtn.Location = new System.Drawing.Point(333, 231);
            this.cBtn.Name = "cBtn";
            this.cBtn.Size = new System.Drawing.Size(75, 23);
            this.cBtn.TabIndex = 0;
            this.cBtn.Text = "Create";
            this.cBtn.UseVisualStyleBackColor = true;
            // 
            // mapXBox
            // 
            this.mapXBox.Location = new System.Drawing.Point(75, 31);
            this.mapXBox.Name = "mapXBox";
            this.mapXBox.Size = new System.Drawing.Size(120, 20);
            this.mapXBox.TabIndex = 1;
            // 
            // mapDGrp
            // 
            this.mapDGrp.Controls.Add(this.mapYLab);
            this.mapDGrp.Controls.Add(this.mapXLab);
            this.mapDGrp.Controls.Add(this.mapYBox);
            this.mapDGrp.Controls.Add(this.mapXBox);
            this.mapDGrp.Location = new System.Drawing.Point(12, 12);
            this.mapDGrp.Name = "mapDGrp";
            this.mapDGrp.Size = new System.Drawing.Size(396, 96);
            this.mapDGrp.TabIndex = 2;
            this.mapDGrp.TabStop = false;
            this.mapDGrp.Text = "Map Dimensions";
            // 
            // mapYLab
            // 
            this.mapYLab.AutoSize = true;
            this.mapYLab.Location = new System.Drawing.Point(19, 59);
            this.mapYLab.Name = "mapYLab";
            this.mapYLab.Size = new System.Drawing.Size(50, 13);
            this.mapYLab.TabIndex = 5;
            this.mapYLab.Text = "Y Length";
            // 
            // mapXLab
            // 
            this.mapXLab.AutoSize = true;
            this.mapXLab.Location = new System.Drawing.Point(19, 33);
            this.mapXLab.Name = "mapXLab";
            this.mapXLab.Size = new System.Drawing.Size(50, 13);
            this.mapXLab.TabIndex = 4;
            this.mapXLab.Text = "X Length";
            // 
            // mapYBox
            // 
            this.mapYBox.Location = new System.Drawing.Point(75, 57);
            this.mapYBox.Name = "mapYBox";
            this.mapYBox.Size = new System.Drawing.Size(120, 20);
            this.mapYBox.TabIndex = 3;
            // 
            // mapDataGrp
            // 
            this.mapDataGrp.Controls.Add(this.tilesetPrev);
            this.mapDataGrp.Controls.Add(this.songSelLab);
            this.mapDataGrp.Controls.Add(this.tilesetSelLab);
            this.mapDataGrp.Controls.Add(this.songSel);
            this.mapDataGrp.Controls.Add(this.tilesetSel);
            this.mapDataGrp.Location = new System.Drawing.Point(12, 114);
            this.mapDataGrp.Name = "mapDataGrp";
            this.mapDataGrp.Size = new System.Drawing.Size(396, 111);
            this.mapDataGrp.TabIndex = 3;
            this.mapDataGrp.TabStop = false;
            this.mapDataGrp.Text = "Map Data";
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
            // songSelLab
            // 
            this.songSelLab.AutoSize = true;
            this.songSelLab.Location = new System.Drawing.Point(19, 63);
            this.songSelLab.Name = "songSelLab";
            this.songSelLab.Size = new System.Drawing.Size(32, 13);
            this.songSelLab.TabIndex = 3;
            this.songSelLab.Text = "Song";
            // 
            // tilesetSelLab
            // 
            this.tilesetSelLab.AutoSize = true;
            this.tilesetSelLab.Location = new System.Drawing.Point(19, 36);
            this.tilesetSelLab.Name = "tilesetSelLab";
            this.tilesetSelLab.Size = new System.Drawing.Size(38, 13);
            this.tilesetSelLab.TabIndex = 2;
            this.tilesetSelLab.Text = "Tileset";
            // 
            // songSel
            // 
            this.songSel.FormattingEnabled = true;
            this.songSel.Location = new System.Drawing.Point(74, 63);
            this.songSel.Name = "songSel";
            this.songSel.Size = new System.Drawing.Size(121, 21);
            this.songSel.TabIndex = 1;
            this.songSel.Text = "Select Song";
            // 
            // tilesetSel
            // 
            this.tilesetSel.FormattingEnabled = true;
            this.tilesetSel.Items.AddRange(new object[] {
            "Outdoor",
            "Beach",
            "Indoor",
            "Ruins"});
            this.tilesetSel.Location = new System.Drawing.Point(74, 33);
            this.tilesetSel.Name = "tilesetSel";
            this.tilesetSel.Size = new System.Drawing.Size(121, 21);
            this.tilesetSel.TabIndex = 0;
            this.tilesetSel.Text = "Select Tileset";
            this.tilesetSel.SelectedIndexChanged += new System.EventHandler(this.updatePreview);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(252, 231);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // mapCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 264);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.mapDataGrp);
            this.Controls.Add(this.mapDGrp);
            this.Controls.Add(this.cBtn);
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
            ((System.ComponentModel.ISupportInitialize)(this.tilesetPrev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cBtn;
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
    }
}