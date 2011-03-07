namespace Regedit.Views
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuViewKeys = new System.Windows.Forms.MenuItem();
            this.mnuViewValues = new System.Windows.Forms.MenuItem();
            this.mnuViewBoth = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuAdd = new System.Windows.Forms.MenuItem();
            this.mnuNewKey = new System.Windows.Forms.MenuItem();
            this.mnuNewValue = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.mnuRename = new System.Windows.Forms.MenuItem();
            this.mnuEditValue = new System.Windows.Forms.MenuItem();
            this.trvKeys = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList();
            this.lstValues = new System.Windows.Forms.ListView();
            this.clmnValueName = new System.Windows.Forms.ColumnHeader();
            this.clmnValue = new System.Windows.Forms.ColumnHeader();
            this.clmnValueType = new System.Windows.Forms.ColumnHeader();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.cmnuEdit = new System.Windows.Forms.ContextMenu();
            this.cmnuAdd = new System.Windows.Forms.MenuItem();
            this.cmnuNewKey = new System.Windows.Forms.MenuItem();
            this.cmnuNewValue = new System.Windows.Forms.MenuItem();
            this.cmnuDelete = new System.Windows.Forms.MenuItem();
            this.cmnuRename = new System.Windows.Forms.MenuItem();
            this.cmnuEditValue = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.mnuOptions);
            this.mainMenu.MenuItems.Add(this.mnuEdit);
            // 
            // mnuOptions
            // 
            this.mnuOptions.MenuItems.Add(this.mnuView);
            this.mnuOptions.MenuItems.Add(this.mnuExit);
            this.mnuOptions.Text = "Options";
            // 
            // mnuView
            // 
            this.mnuView.MenuItems.Add(this.mnuViewKeys);
            this.mnuView.MenuItems.Add(this.mnuViewValues);
            this.mnuView.MenuItems.Add(this.mnuViewBoth);
            this.mnuView.Text = "View";
            // 
            // mnuViewKeys
            // 
            this.mnuViewKeys.Text = "Keys";
            this.mnuViewKeys.Click += new System.EventHandler(this.onMnuViewMenuItemClicked);
            // 
            // mnuViewValues
            // 
            this.mnuViewValues.Text = "Values";
            this.mnuViewValues.Click += new System.EventHandler(this.onMnuViewMenuItemClicked);
            // 
            // mnuViewBoth
            // 
            this.mnuViewBoth.Checked = true;
            this.mnuViewBoth.Text = "Both";
            this.mnuViewBoth.Click += new System.EventHandler(this.onMnuViewMenuItemClicked);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.onMnuExitClick);
            // 
            // mnuEdit
            // 
            this.mnuEdit.MenuItems.Add(this.mnuAdd);
            this.mnuEdit.MenuItems.Add(this.mnuDelete);
            this.mnuEdit.MenuItems.Add(this.mnuRename);
            this.mnuEdit.MenuItems.Add(this.mnuEditValue);
            this.mnuEdit.Text = "Edit";
            this.mnuEdit.Popup += new System.EventHandler(this.onMnuEditPopup);
            // 
            // mnuAdd
            // 
            this.mnuAdd.MenuItems.Add(this.mnuNewKey);
            this.mnuAdd.MenuItems.Add(this.mnuNewValue);
            this.mnuAdd.Text = "Add";
            // 
            // mnuNewKey
            // 
            this.mnuNewKey.Text = "New Key";
            this.mnuNewKey.Click += new System.EventHandler(this.onMnuNewKeyClick);
            // 
            // mnuNewValue
            // 
            this.mnuNewValue.Text = "New Value";
            this.mnuNewValue.Click += new System.EventHandler(this.onMnuNewValueClick);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.onMnuDeleteClick);
            // 
            // mnuRename
            // 
            this.mnuRename.Text = "Rename";
            this.mnuRename.Click += new System.EventHandler(this.onMnuRenameClick);
            // 
            // mnuEditValue
            // 
            this.mnuEditValue.Text = "Edit Value";
            this.mnuEditValue.Click += new System.EventHandler(this.onMnuEditValueClick);
            // 
            // trvKeys
            // 
            this.trvKeys.ImageIndex = 0;
            this.trvKeys.ImageList = this.imageList;
            this.trvKeys.Location = new System.Drawing.Point(0, 0);
            this.trvKeys.Name = "trvKeys";
            this.trvKeys.SelectedImageIndex = 0;
            this.trvKeys.Size = new System.Drawing.Size(257, 159);
            this.trvKeys.TabIndex = 1;
            this.trvKeys.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.onTrvKeysBeforeExpand);
            this.trvKeys.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.onTrvKeysBeforeCollapse);
            this.trvKeys.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.onTrvKeysAfterSelect);
            this.imageList.Images.Clear();
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource1"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource2"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource3"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource4"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource5"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource6"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource7"))));
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource8"))));
            // 
            // lstValues
            // 
            this.lstValues.Columns.Add(this.clmnValueName);
            this.lstValues.Columns.Add(this.clmnValue);
            this.lstValues.Columns.Add(this.clmnValueType);
            this.lstValues.Location = new System.Drawing.Point(0, 159);
            this.lstValues.Name = "lstValues";
            this.lstValues.Size = new System.Drawing.Size(257, 104);
            this.lstValues.SmallImageList = this.imageList;
            this.lstValues.TabIndex = 2;
            this.lstValues.View = System.Windows.Forms.View.Details;
            this.lstValues.SelectedIndexChanged += new System.EventHandler(this.onLstValuesSelectedIndexChanged);
            // 
            // clmnValueName
            // 
            this.clmnValueName.Text = "Name";
            this.clmnValueName.Width = 86;
            // 
            // clmnValue
            // 
            this.clmnValue.Text = "Value";
            this.clmnValue.Width = 105;
            // 
            // clmnValueType
            // 
            this.clmnValueType.Text = "Type";
            this.clmnValueType.Width = 60;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(0, 264);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(257, 21);
            this.txtPath.TabIndex = 3;
            this.txtPath.WordWrap = false;
            // 
            // cmnuEdit
            // 
            this.cmnuEdit.MenuItems.Add(this.cmnuAdd);
            this.cmnuEdit.MenuItems.Add(this.cmnuDelete);
            this.cmnuEdit.MenuItems.Add(this.cmnuRename);
            this.cmnuEdit.MenuItems.Add(this.cmnuEditValue);
            this.cmnuEdit.Popup += new System.EventHandler(this.onCmnuEditPopup);
            // 
            // cmnuAdd
            // 
            this.cmnuAdd.MenuItems.Add(this.cmnuNewKey);
            this.cmnuAdd.MenuItems.Add(this.cmnuNewValue);
            this.cmnuAdd.Text = "Add";
            // 
            // cmnuNewKey
            // 
            this.cmnuNewKey.Text = "New Key";
            this.cmnuNewKey.Click += new System.EventHandler(this.onMnuNewKeyClick);
            // 
            // cmnuNewValue
            // 
            this.cmnuNewValue.Text = "New Value";
            this.cmnuNewValue.Click += new System.EventHandler(this.onMnuNewValueClick);
            // 
            // cmnuDelete
            // 
            this.cmnuDelete.Text = "Delete";
            this.cmnuDelete.Click += new System.EventHandler(this.onMnuDeleteClick);
            // 
            // cmnuRename
            // 
            this.cmnuRename.Text = "Rename";
            this.cmnuRename.Click += new System.EventHandler(this.onMnuRenameClick);
            // 
            // cmnuEditValue
            // 
            this.cmnuEditValue.Text = "Edit Value";
            this.cmnuEditValue.Click += new System.EventHandler(this.onMnuEditValueClick);
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(257, 285);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lstValues);
            this.Controls.Add(this.trvKeys);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Registry Editor";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView trvKeys;
        public System.Windows.Forms.ListView lstValues;
        public System.Windows.Forms.ColumnHeader clmnValueName;
        public System.Windows.Forms.ColumnHeader clmnValue;
        public System.Windows.Forms.ColumnHeader clmnValueType;
        public System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.MenuItem mnuEdit;
        public System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem mnuRename;
        private System.Windows.Forms.MenuItem mnuDelete;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuViewKeys;
        private System.Windows.Forms.MenuItem mnuViewValues;
        private System.Windows.Forms.MenuItem mnuViewBoth;
        private System.Windows.Forms.MenuItem mnuAdd;
        public System.Windows.Forms.ContextMenu cmnuEdit;
        private System.Windows.Forms.MenuItem cmnuAdd;
        private System.Windows.Forms.MenuItem cmnuDelete;
        private System.Windows.Forms.MenuItem cmnuRename;
        private System.Windows.Forms.MenuItem mnuNewKey;
        private System.Windows.Forms.MenuItem mnuNewValue;
        private System.Windows.Forms.MenuItem cmnuNewKey;
        private System.Windows.Forms.MenuItem cmnuNewValue;
        private System.Windows.Forms.MenuItem mnuEditValue;
        private System.Windows.Forms.MenuItem cmnuEditValue;
        private System.Windows.Forms.ImageList imageList;

    }
}

