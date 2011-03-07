using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Regedit.Presenters;
using Microsoft.Win32;
using System.Security.Permissions;

namespace Regedit.Views
{
    public partial class frmMain : Form
    {
        #region ctor
        public frmMain()
        {
            InitializeComponent();
            // Data Presenter
            this.Presenter = new KeysValuesPresenter(this);

            // Adapting UI Components
            this.trvKeys.Width = this.Width - 2;
            this.trvKeys.Left = 1;
            this.trvKeys.Top = 1;
            this.trvKeys.Height = (int)(0.6 * this.Height);

            this.lstValues.Height = this.Height - (this.trvKeys.Height + this.txtPath.Height + 4);
            this.lstValues.Width = this.Width - 2;
            this.lstValues.Left = 1;
            this.lstValues.Top = this.trvKeys.Top + this.trvKeys.Height + 1;

            this.txtPath.Top = this.lstValues.Top + this.lstValues.Height + 1;
            this.txtPath.Width = this.Width - 2;
            this.txtPath.Left = 1;

            originalKeysTreeViewHeight = this.trvKeys.Height;
            originalValuesListViewHeight = this.lstValues.Height;
            originalValuesListViewTop = this.lstValues.Top;

            // There is a bug in the .NET CF Windows Forms classes that supports scrolling, they don't show the
            //      scroll bars when items are added for the first time. we need to resize them manually
            //      to show the hidden scroll bars.
            this.onMnuViewMenuItemClicked(mnuViewValues, EventArgs.Empty);
            this.onMnuViewMenuItemClicked(mnuViewKeys, EventArgs.Empty);
            this.onMnuViewMenuItemClicked(mnuViewBoth, EventArgs.Empty);
        }
        #endregion

        #region Properties

        private static frmMain instance;
        public static frmMain Instance
        {
            get 
            { 
                if (instance == null)
                    instance = new frmMain();
                return instance;
            }
        }

        public KeysValuesPresenter Presenter { get; private set; }
        private int originalKeysTreeViewHeight;
        private int originalValuesListViewHeight;
        private int originalValuesListViewTop;
        #endregion

        #region Events Listeners
        private void onMnuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void onMnuEditPopup(object sender, EventArgs e)
        {
            bool selected = Presenter.IsKeyValueSelected;
            mnuDelete.Enabled = selected;
            mnuRename.Enabled = selected;
            mnuEditValue.Enabled = this.lstValues.SelectedIndices.Count != 0;
        }

        private void onCmnuEditPopup(object sender, EventArgs e)
        {
            bool selected = this.Presenter.IsKeyValueSelected;
            cmnuDelete.Enabled = selected;
            cmnuRename.Enabled = selected;
            cmnuEditValue.Enabled = this.lstValues.SelectedIndices.Count != 0;
        }


        private void onMnuRenameClick(object sender, EventArgs e)
        {
            try
            {
                if (!this.lstValues.SelectedIndices.Count.Equals(0)) // Value
                {
                    string keyPath = this.lstValues.Tag.ToString();
                    frmAddEditValue.Instance.ShowDialogForEditRename(keyPath, this.lstValues.Items[this.lstValues.SelectedIndices[0]].SubItems[0].Text, false);
                    // Refreshing the shown content
                    Presenter.RefreshValues(keyPath);
                }
                else // Key
                {
                    frmRenameAddKey.Instance.ShowDialogForRename(this.trvKeys.SelectedNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void onMnuDeleteClick(object sender, EventArgs e)
        {
            DeleteSelectedKeyValue();
        }

        private void DeleteSelectedKeyValue()
        {
            // Getting the selection's type
            try
            {
                if (!this.lstValues.SelectedIndices.Count.Equals(0)) // Value
                {
                    DialogResult result = MessageBox.Show("Are you sure that you want to delete the selected value?", "Confirmation",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result != DialogResult.Yes)
                        return;
                    string keyPath = this.lstValues.Tag.ToString();
                    RegistryKey key = RegistryUtils.OpenKeyFromPath(keyPath, true);
                    string valueName = this.lstValues.Items[this.lstValues.SelectedIndices[0]].SubItems[0].Text;
                    key.DeleteValue(valueName);
                    key.Flush();
                    key.Close();
                    // Refreshing the shown content
                    Presenter.RefreshValues(keyPath);
                }
                else // Key
                {
                    DialogResult result = MessageBox.Show("Are you sure that you want to delete the selected key?", "Confirmation",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result != DialogResult.Yes)
                        return;
                    string keyPath = this.trvKeys.SelectedNode.FullPath;
                    string selectedValueName = keyPath.Substring(keyPath.LastIndexOf("\\") + 1);

                    string parrentKeyPath = this.trvKeys.SelectedNode.Parent.FullPath;
                    RegistryKey parrentKey = RegistryUtils.OpenKeyFromPath(parrentKeyPath, true);

                    parrentKey.DeleteSubKeyTree(selectedValueName);
                    parrentKey.Flush();
                    parrentKey.Close();
                    // Refreshing the shown content
                    Presenter.RefreshKeys(this.trvKeys.SelectedNode.Parent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                MessageBoxDefaultButton.Button1);
            }
        }

        private void onMnuViewMenuItemClicked(object sender, EventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            AdaptUIComponents(menu);
        }

        private void AdaptUIComponents(MenuItem menu)
        {
            if (menu == mnuViewBoth)
            {
                mnuViewBoth.Checked = true;
                mnuViewKeys.Checked = false;
                mnuViewValues.Checked = false;

                trvKeys.Visible = true;
                trvKeys.Height = originalKeysTreeViewHeight;

                lstValues.Visible = true;
                lstValues.Top = originalValuesListViewTop;
                lstValues.Height = originalValuesListViewHeight;

            }
            else if (menu == mnuViewKeys)
            {
                mnuViewBoth.Checked = false;
                mnuViewKeys.Checked = true;
                mnuViewValues.Checked = false;

                trvKeys.Visible = true;
                lstValues.Visible = false;

                this.trvKeys.Height = this.Height - (txtPath.Height + 3);
            }
            else if (menu == mnuViewValues)
            {
                mnuViewBoth.Checked = false;
                mnuViewKeys.Checked = false;
                mnuViewValues.Checked = true;

                trvKeys.Visible = false;
                lstValues.Visible = true;

                lstValues.Top = 1;
                lstValues.Height = this.Height - (txtPath.Height + 3);
            }
        }

        private void onMnuNewValueClick(object sender, EventArgs e)
        {
            // Show the new window
            frmAddEditValue.Instance.ShowDialog(this.trvKeys.SelectedNode.FullPath);
            // Verify if the user has add a new key
            if (frmAddEditValue.Instance.DialogResult != DialogResult.Cancel)
                this.Presenter.RefreshValues(this.trvKeys.SelectedNode.FullPath);
        }

        private void onTrvKeysBeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 0; // Collapsed
            e.Node.SelectedImageIndex = 0;
        }

        private void onTrvKeysBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 1; // Expanded
            e.Node.SelectedImageIndex = 1;
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
            base.OnClosed(e);
        }

        private void onMnuEditValueClick(object sender, EventArgs e)
        {
            
            string valueName = lstValues.Items[lstValues.SelectedIndices[0]].SubItems[0].Text;
            string keyPath = this.lstValues.Tag.ToString();

            frmAddEditValue.Instance.txtPath.Text = this.txtPath.Text;
            frmAddEditValue.Instance.ShowDialogForEditRename(keyPath, valueName, true);
            
            if (frmAddEditValue.Instance.DialogResult != DialogResult.Cancel)
                this.Presenter.RefreshValues(this.trvKeys.SelectedNode.FullPath);
        }

        private void onTrvKeysAfterSelect(object sender, TreeViewEventArgs e)
        {
            // Adapting the contextual menu
            if (this.cmnuEdit.MenuItems.Contains(cmnuEditValue))
                cmnuEdit.MenuItems.Remove(this.cmnuEditValue);
            if (!cmnuEdit.MenuItems.Contains(this.cmnuAdd))
            {
                cmnuEdit.MenuItems.Clear();
                cmnuEdit.MenuItems.Add(this.cmnuAdd);
                this.cmnuEdit.MenuItems.Add(this.cmnuDelete);
                this.cmnuEdit.MenuItems.Add(this.cmnuRename);
            }
            this.trvKeys.ContextMenu = cmnuEdit;
        }


        
        private void onLstValuesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstValues.SelectedIndices.Count > 0)
            {
                if (!cmnuEdit.MenuItems.Contains(cmnuEditValue))
                    cmnuEdit.MenuItems.Add(this.cmnuEditValue);
                if (cmnuEdit.MenuItems.Contains(this.cmnuAdd))
                    cmnuEdit.MenuItems.Remove(this.cmnuAdd);
                this.lstValues.ContextMenu = cmnuEdit;
            }
            else
            {
                this.lstValues.ContextMenu = null;
                if (!cmnuEdit.MenuItems.Contains(this.cmnuAdd))
                {
                    cmnuEdit.MenuItems.Clear();
                    this.cmnuEdit.MenuItems.Add(this.cmnuAdd);
                    this.cmnuEdit.MenuItems.Add(this.cmnuDelete);
                    this.cmnuEdit.MenuItems.Add(this.cmnuRename);
                    this.cmnuEdit.MenuItems.Add(this.cmnuEditValue);
                }
            }
        }

        private void onMnuNewKeyClick(object sender, EventArgs e)
        {
            frmRenameAddKey.Instance.ShowDialog(this.trvKeys.SelectedNode);
        }
        
        #endregion

        #region Methods
        private void addTestKeyValues()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Pete");
                key.SetValue("Binary Value", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, RegistryValueKind.Binary);
                key.SetValue("DWord Value", 32, RegistryValueKind.DWord);
                key.SetValue("ExpandString Value", "this is a test", RegistryValueKind.ExpandString);
                key.SetValue("MultiString Value", new[] { "string 1, string 2, string 3, string 4" }, RegistryValueKind.MultiString);
                key.SetValue("QWord Value", 64, RegistryValueKind.QWord);
                key.SetValue("String Value", "this is a test", RegistryValueKind.String);
                key.SetValue("Unknown Type Value", new object(), RegistryValueKind.Unknown);
                key.Flush();

                RegistryKey subkey1 = key.CreateSubKey("Sub Key 1");
                subkey1.SetValue("Binary Value", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, RegistryValueKind.Binary);
                subkey1.SetValue("DWord Value", 32, RegistryValueKind.DWord);
                subkey1.Flush();
                subkey1.Close();

                RegistryKey subkey2 = key.CreateSubKey("Sub Key 2");
                subkey2.SetValue("ExpandString Value", "this is a test", RegistryValueKind.ExpandString);
                subkey2.SetValue("MultiString Value", new[] { "string 1, string 2, string 3, string 4" }, RegistryValueKind.MultiString);
                subkey2.Flush();
                subkey2.Close();

                RegistryKey subkey3 = key.CreateSubKey("Sub Key 3");
                subkey3.SetValue("QWord Value", 64, RegistryValueKind.QWord);
                subkey3.SetValue("String Value", "this is a test", RegistryValueKind.String);
                subkey3.SetValue("Unknown Type Value", new object(), RegistryValueKind.Unknown);
                subkey3.Flush();
                subkey3.Close();
                key.Close();
            }
            catch
            {
            }
        }

        #endregion
    }
}