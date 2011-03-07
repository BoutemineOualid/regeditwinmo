using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Regedit.Views
{
    public partial class frmRenameAddKey : Form
    {
        #region ctor
        public frmRenameAddKey()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        private static frmRenameAddKey instance;
        public static frmRenameAddKey Instance
        {
            get
            {
                if (instance == null)
                    instance = new frmRenameAddKey();
                return instance;
            }
        }

        public TreeNode CurrentNode { get; set; }

        private string currentKeyPath;
        public string CurrentKeyPath
        {
            get
            {
                return currentKeyPath;
            }
            private set
            {
                currentKeyPath = value;
            }
        }

        private string  parrentKeyPath;
        public string ParrentKeyPath
        {
            get
            {
                return parrentKeyPath;
            }

            private set
            {
                parrentKeyPath = value; 
            }
        }

        public List<string> SameLevelKeyNames
        {
            get; set;
        }      

        public List<string> SubKeyNames
        {
            get; set;
        }

        public bool IsForRename { get; set; }

        #endregion

        #region Methods

        private void PrepareLocalData(TreeNode editedNode)
        {
            this.CurrentKeyPath = editedNode.FullPath;
            RegistryKey currentKey = RegistryUtils.OpenKeyFromPath(this.CurrentKeyPath, false);
            this.SubKeyNames = new List<string>(currentKey.GetSubKeyNames());
            currentKey.Close();

            if (editedNode.Parent != null)
            {
                this.ParrentKeyPath = editedNode.Parent.FullPath;
                RegistryKey parrentKey = RegistryUtils.OpenKeyFromPath(this.ParrentKeyPath, false);
                this.SameLevelKeyNames = new List<string>(parrentKey.GetSubKeyNames());
                parrentKey.Close();
            }
            else
                this.SameLevelKeyNames = new List<string>();
            
            this.CurrentNode = editedNode;
            this.txtPath.Text = frmMain.Instance.txtPath.Text;
        }

        public void ShowDialog(TreeNode editedNode)
        {
            if (editedNode == null)
                return;

            PrepareLocalData(editedNode);
            this.IsForRename = false;

            this.Text = "Add New Key";
            this.txtName.Focus();
            this.ShowDialog();
        }

        public void ShowDialogForRename(TreeNode editedNode)
        {
            if (editedNode == null)
                return;
            
            PrepareLocalData(editedNode);
            this.IsForRename = true;

            this.Text = "Rename Key";
            this.txtName.Text = editedNode.Text;
            this.txtName.SelectAll();
            this.txtName.Focus();
            this.ShowDialog();
        }

        private void SaveChanges()
        {
            // Identifying the correct action to be done
            if (IsForRename)
            {
                 // If no changes have been made to the name, do nothing);
                 if (!this.CurrentNode.Text.Equals(this.txtName.Text))
                 {
                     RegistryKey parentKey = RegistryUtils.OpenKeyFromPath(this.ParrentKeyPath, true);
                     
                     // Renaming it
                     RegistryUtils.RenameSubKey(parentKey, this.CurrentNode.Text, this.txtName.Text);
                     parentKey.Close();
                     
                     // Updating UI
                     this.CurrentNode.Text = this.txtName.Text;
                 }
                // the opened key will be closed by the rename method
            }
            else
            {
                // Adding a new Key
                RegistryKey currentKey = RegistryUtils.OpenKeyFromPath(currentKeyPath, true);
                currentKey.CreateSubKey(txtName.Text).Close();
                currentKey.Flush();
                currentKey.Close();
                
                // Updating UI
                frmMain.Instance.Presenter.RefreshKeys(this.CurrentNode);
                // Selecting the node of the new currently added key
                TreeNode lastAddedNode = null;
                foreach (TreeNode node in this.CurrentNode.Nodes)
                {
                    if (node.Text.Equals(this.txtName.Text))
                    {
                        lastAddedNode = node;
                        break;
                    }
                }
                this.CurrentNode.Expand();
                frmMain.Instance.trvKeys.SelectedNode = lastAddedNode;
            }
        }

        #endregion

        #region Event Listeners
        private void onTxtNameTextChanged(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (IsForRename)
            {
                // Check if the currently typed name is currently used by another 
                // key in the same level of the currently renamed key.
                if (this.SameLevelKeyNames.Count(keyName => keyName.Equals(this.txtName.Text) && 
                                                            !keyName.Equals(this.CurrentNode.Text)) > 0)
                {
                    lblError.Text = "This name is currently in use by another key";
                }
            }
            else
            {
                // Check if the currently typed name is currently used by another 
                // sub key.
                if (this.SubKeyNames.Count(keyName => keyName.Equals(this.txtName.Text)) > 0)
                    lblError.Text = "This name is currently in use by another key";
            }
        }

        private bool closedByUserFlag;
        private void onMnuOkClick(object sender, EventArgs e)
        {
            closedByUserFlag = true;
            // Checking if there is some errors
            if (string.IsNullOrEmpty(lblError.Text))
            {
                try
                {
                    SaveChanges();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                    MessageBoxDefaultButton.Button1);
                }
            }
            else // Errors have been reported
            {
                MessageBox.Show(lblError.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                MessageBoxDefaultButton.Button1);
                txtName.SelectAll();
            }
        }

        private void onMnuCancelClick(object sender, EventArgs e)
        {
            closedByUserFlag = true;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!closedByUserFlag)
            {
                DialogResult result = MessageBox.Show("Would you like to save this changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    // If some errors exist.)
                    if (!string.IsNullOrEmpty(lblError.Text))
                        e.Cancel = true; // Cancels this event
                    // Saving Changes
                    onMnuOkClick(this, EventArgs.Empty);
                }
            }
            closedByUserFlag = false;
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsForRename)
                this.txtName.Text = string.Empty;
            base.OnLoad(e);
        }

        #endregion
    }
}