namespace AeviMap
{
    partial class MapEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadROMItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CloseAeviMapItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editMapDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.NoMapHereItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutAeviMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMapButton = new System.Windows.Forms.Button();
            this.mapRendererPanel = new System.Windows.Forms.Panel();
            this.mapRenderer = new System.Windows.Forms.PictureBox();
            this.openROMDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMapDialog = new System.Windows.Forms.SaveFileDialog();
            this.blockPickerPanel = new System.Windows.Forms.Panel();
            this.blockPicker = new System.Windows.Forms.PictureBox();
            this.openMapDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectMapName = new System.Windows.Forms.ComboBox();
            this.MapIDLabel = new System.Windows.Forms.Label();
            this.importBLKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar.SuspendLayout();
            this.mapRendererPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapRenderer)).BeginInit();
            this.blockPickerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blockPicker)).BeginInit();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.MapMenu,
            this.HelpMenu});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(767, 24);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadROMItem,
            this.CloseAeviMapItem,
            this.toolStripSeparator1,
            this.newMapItem,
            this.importBLKToolStripMenuItem,
            this.saveMapItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // LoadROMItem
            // 
            this.LoadROMItem.Name = "LoadROMItem";
            this.LoadROMItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.LoadROMItem.Size = new System.Drawing.Size(182, 22);
            this.LoadROMItem.Text = "Load ROM...";
            this.LoadROMItem.ToolTipText = "Load Aevilia GB ROM to edit";
            this.LoadROMItem.Click += new System.EventHandler(this.OpenROM);
            // 
            // newMapItem
            // 
            this.newMapItem.Name = "newMapItem";
            this.newMapItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMapItem.Size = new System.Drawing.Size(182, 22);
            this.newMapItem.Text = "New Map...";
            this.newMapItem.Click += new System.EventHandler(this.NewMap);
            // 
            // saveMapItem
            // 
            this.saveMapItem.Name = "saveMapItem";
            this.saveMapItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.saveMapItem.Size = new System.Drawing.Size(182, 22);
            this.saveMapItem.Text = "Export BLK...";
            this.saveMapItem.ToolTipText = "Save the current map to a .blk file";
            this.saveMapItem.Click += new System.EventHandler(this.SaveMap);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // CloseAeviMapItem
            // 
            this.CloseAeviMapItem.Name = "CloseAeviMapItem";
            this.CloseAeviMapItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.CloseAeviMapItem.Size = new System.Drawing.Size(182, 22);
            this.CloseAeviMapItem.Text = "Close";
            this.CloseAeviMapItem.Click += new System.EventHandler(this.closeApp);
            // 
            // MapMenu
            // 
            this.MapMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editMapDataToolStripMenuItem,
            this.toolStripSeparator2,
            this.NoMapHereItem});
            this.MapMenu.Name = "MapMenu";
            this.MapMenu.Size = new System.Drawing.Size(43, 20);
            this.MapMenu.Text = "Map";
            // 
            // editMapDataToolStripMenuItem
            // 
            this.editMapDataToolStripMenuItem.Name = "editMapDataToolStripMenuItem";
            this.editMapDataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.editMapDataToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.editMapDataToolStripMenuItem.Text = "Edit Map Header...";
            this.editMapDataToolStripMenuItem.Click += new System.EventHandler(this.EditMap);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(211, 6);
            // 
            // NoMapHereItem
            // 
            this.NoMapHereItem.Enabled = false;
            this.NoMapHereItem.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.NoMapHereItem.Name = "NoMapHereItem";
            this.NoMapHereItem.Size = new System.Drawing.Size(214, 22);
            this.NoMapHereItem.Text = "No map here";
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutAeviMapToolStripMenuItem});
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(44, 20);
            this.HelpMenu.Text = "Help";
            // 
            // aboutAeviMapToolStripMenuItem
            // 
            this.aboutAeviMapToolStripMenuItem.Name = "aboutAeviMapToolStripMenuItem";
            this.aboutAeviMapToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutAeviMapToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.aboutAeviMapToolStripMenuItem.Text = "About AeviMap";
            this.aboutAeviMapToolStripMenuItem.Click += new System.EventHandler(this.aboutAeviMapToolStripMenuItem_Click);
            // 
            // loadMapButton
            // 
            this.loadMapButton.Location = new System.Drawing.Point(139, 26);
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(75, 23);
            this.loadMapButton.TabIndex = 5;
            this.loadMapButton.Text = "Load Map";
            this.loadMapButton.UseVisualStyleBackColor = true;
            this.loadMapButton.Click += new System.EventHandler(this.LoadROMMap);
            // 
            // mapRendererPanel
            // 
            this.mapRendererPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapRendererPanel.AutoScroll = true;
            this.mapRendererPanel.Controls.Add(this.mapRenderer);
            this.mapRendererPanel.Location = new System.Drawing.Point(12, 56);
            this.mapRendererPanel.Name = "mapRendererPanel";
            this.mapRendererPanel.Size = new System.Drawing.Size(692, 526);
            this.mapRendererPanel.TabIndex = 6;
            // 
            // mapRenderer
            // 
            this.mapRenderer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.mapRenderer.Location = new System.Drawing.Point(0, 0);
            this.mapRenderer.Name = "mapRenderer";
            this.mapRenderer.Size = new System.Drawing.Size(692, 526);
            this.mapRenderer.TabIndex = 2;
            this.mapRenderer.TabStop = false;
            this.mapRenderer.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderMap);
            this.mapRenderer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ClickMapRenderer);
            this.mapRenderer.MouseEnter += new System.EventHandler(this.MouseEnterMapRenderer);
            this.mapRenderer.MouseLeave += new System.EventHandler(this.MouseLeaveMapRenderer);
            this.mapRenderer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveMapRenderer);
            // 
            // openROMDialog
            // 
            this.openROMDialog.DefaultExt = "gbc";
            this.openROMDialog.Filter = "GBC ROMs (*.gbc)|*.gbc|All files|*.*";
            this.openROMDialog.Title = "Load ROM";
            // 
            // saveMapDialog
            // 
            this.saveMapDialog.DefaultExt = "blk";
            this.saveMapDialog.Filter = "Map block files (*.blk)|*.blk|All files|*.*";
            this.saveMapDialog.Title = "Save map";
            // 
            // blockPickerPanel
            // 
            this.blockPickerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blockPickerPanel.AutoScroll = true;
            this.blockPickerPanel.Controls.Add(this.blockPicker);
            this.blockPickerPanel.Location = new System.Drawing.Point(710, 56);
            this.blockPickerPanel.Name = "blockPickerPanel";
            this.blockPickerPanel.Size = new System.Drawing.Size(45, 526);
            this.blockPickerPanel.TabIndex = 7;
            // 
            // blockPicker
            // 
            this.blockPicker.BackColor = System.Drawing.SystemColors.ControlDark;
            this.blockPicker.Location = new System.Drawing.Point(0, 0);
            this.blockPicker.Name = "blockPicker";
            this.blockPicker.Size = new System.Drawing.Size(16, 526);
            this.blockPicker.TabIndex = 0;
            this.blockPicker.TabStop = false;
            this.blockPicker.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderBlockPicker);
            this.blockPicker.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ClickBlockPicker);
            this.blockPicker.MouseEnter += new System.EventHandler(this.MouseEnterBlockPicker);
            this.blockPicker.MouseLeave += new System.EventHandler(this.MouseLeaveBlockPicker);
            this.blockPicker.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveBlockPicker);
            // 
            // openMapDialog
            // 
            this.openMapDialog.DefaultExt = "blk";
            this.openMapDialog.Filter = "Map block files (*.blk)|*.blk|All files|*.*";
            this.openMapDialog.Title = "Load map from file";
            // 
            // selectMapName
            // 
            this.selectMapName.FormattingEnabled = true;
            this.selectMapName.Location = new System.Drawing.Point(12, 27);
            this.selectMapName.Name = "selectMapName";
            this.selectMapName.Size = new System.Drawing.Size(121, 21);
            this.selectMapName.TabIndex = 8;
            this.selectMapName.Text = "Select Map";
            // 
            // MapIDLabel
            // 
            this.MapIDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MapIDLabel.AutoSize = true;
            this.MapIDLabel.Location = new System.Drawing.Point(684, 30);
            this.MapIDLabel.Name = "MapIDLabel";
            this.MapIDLabel.Size = new System.Drawing.Size(71, 13);
            this.MapIDLabel.TabIndex = 9;
            this.MapIDLabel.Text = "Map ID : N/A";
            this.MapIDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // importBLKToolStripMenuItem
            // 
            this.importBLKToolStripMenuItem.Name = "importBLKToolStripMenuItem";
            this.importBLKToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importBLKToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.importBLKToolStripMenuItem.Text = "Import BLK...";
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 594);
            this.Controls.Add(this.MapIDLabel);
            this.Controls.Add(this.selectMapName);
            this.Controls.Add(this.blockPickerPanel);
            this.Controls.Add(this.mapRendererPanel);
            this.Controls.Add(this.loadMapButton);
            this.Controls.Add(this.menuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuBar;
            this.Name = "MapEditor";
            this.Text = "AeviMap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfirmClose);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.mapRendererPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapRenderer)).EndInit();
            this.blockPickerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.blockPicker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem LoadROMItem;
        private System.Windows.Forms.ToolStripMenuItem CloseAeviMapItem;
        private System.Windows.Forms.PictureBox mapRenderer;
        private System.Windows.Forms.Button loadMapButton;
        private System.Windows.Forms.Panel mapRendererPanel;
        private System.Windows.Forms.OpenFileDialog openROMDialog;
        private System.Windows.Forms.ToolStripMenuItem saveMapItem;
        private System.Windows.Forms.SaveFileDialog saveMapDialog;
        private System.Windows.Forms.Panel blockPickerPanel;
        private System.Windows.Forms.PictureBox blockPicker;
        private System.Windows.Forms.OpenFileDialog openMapDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem aboutAeviMapToolStripMenuItem;
        private System.Windows.Forms.ComboBox selectMapName;
        private System.Windows.Forms.Label MapIDLabel;
        private System.Windows.Forms.ToolStripMenuItem newMapItem;
        private System.Windows.Forms.ToolStripMenuItem MapMenu;
        private System.Windows.Forms.ToolStripMenuItem editMapDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem NoMapHereItem;
        private System.Windows.Forms.ToolStripMenuItem importBLKToolStripMenuItem;
    }
}

