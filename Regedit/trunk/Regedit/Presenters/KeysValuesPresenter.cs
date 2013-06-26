using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Regedit.Views;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Regedit.Presenters
{
    public class KeysValuesPresenter
    {
        public KeysValuesPresenter(frmMain window)
        {
            this.Window = window;
            StartListners();
        }

        #region Methods
        public frmMain Window { get; private set; }

        #endregion

        #region Events Listners
        
        void onWindowLoaded(object sender, EventArgs e)
        {
            this.Window.trvKeys.PathSeparator = @"\";
            AddKey(this.Window.trvKeys.Nodes, Registry.ClassesRoot);
            AddKey(this.Window.trvKeys.Nodes, Registry.CurrentUser);
            AddKey(this.Window.trvKeys.Nodes, Registry.LocalMachine);
            AddKey(this.Window.trvKeys.Nodes, Registry.Users);
        }

        void onNodeBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            RefreshKeys(e.Node);
        }

        void onNodeAfterSelected(object sender, TreeViewEventArgs e)
        {
            this.Window.txtPath.Text = @"\" + e.Node.FullPath;
            RefreshValues(e.Node.FullPath);
        }

        #endregion 
        
        #region PInvoke Declarations
        private const int IDC_WAIT = 32514;
        private const int IDC_ARROW = 32512;

        [DllImport("coredll.dll")]
        public static extern int SetCursor(IntPtr hCursor);

        [DllImport("coredll.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        #endregion

        #region Methods

        private void StartListners()
        {
            // Main Window
            this.Window.Load += onWindowLoaded;

            // Tree View
            this.Window.trvKeys.BeforeExpand += onNodeBeforeExpand;
            this.Window.trvKeys.AfterSelect += onNodeAfterSelected;
        }

        public void RefreshValues(string keyPath)
        {
            if (string.IsNullOrEmpty(keyPath))
                return;
            RegistryKey key = RegistryUtils.OpenKeyFromPath(keyPath, false);
            if (key == null)
                return;

            // Populating values 
            this.Window.lstValues.Items.Clear();
            foreach (string valueName in key.GetValueNames())
            {
                object value = key.GetValue(valueName);
                RegistryValueKind valueKind = key.GetValueKind(valueName);
                string formattedValue = FormatValue(value, valueKind);
                ListViewItem item = new ListViewItem(new[] { valueName, formattedValue, valueKind.ToString() });
                item.Tag = valueName;
                this.Window.lstValues.Items.Add(item);
                item.ImageIndex = GetImageIndex(valueKind);
                // This allows us to focus the updated item (Update done by the user) in the list view 
                if (valueName == frmAddEditValue.Instance.LastEditedValueName)
                {
                    item.Selected = true;
                    item.Focused = true;
                }
            }
            // Saving a reference to the source key of these values.
            this.Window.lstValues.Tag = keyPath;
            key.Close(); // Free resources
            
            // Setting the columns
            if (this.Window.lstValues.Items.Count > 0)
            {
                foreach (ColumnHeader column in this.Window.lstValues.Columns)
                {
                    column.Width = -1;
                }
            }
            else
            {
                this.Window.clmnValueName.Width = 86;
                this.Window.clmnValue.Width = 105;
                this.Window.clmnValueType.Width = 60;
            }
        }

        public void RefreshKeys(TreeNode node)
        {
            if (node == null)
                return;
            RegistryKey registryKey = RegistryUtils.OpenKeyFromPath(node.FullPath, false);
            if (registryKey == null)
                return;

            string[] subKeysNames = registryKey.GetSubKeyNames();

            showWaitingCursor(true);

            if (node.Nodes.Count == 1 && string.IsNullOrEmpty(node.Nodes[0].Text)) // Lazy loading goes in here.
            {
                if (string.IsNullOrEmpty(node.Nodes[0].Text))
                {
                    this.Window.trvKeys.BeginUpdate();
                    node.Nodes.Clear();
                    foreach (string keyName in subKeysNames)
                    {
                        RegistryKey key = registryKey.OpenSubKey(keyName, true);
                        AddKey(node.Nodes, key);
                        key.Close();
                    }
                    this.Window.trvKeys.EndUpdate();
                }
            }
            else // The node contains some sub nodes, we check if there are any new changes.
            {
                // Removing obsolete keys (This refers to renamed keys)
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    if (!subKeysNames.Contains(node.Nodes[i].Text)) // Obsolete node, removing it
                        node.Nodes.RemoveAt(i);
                }

                if (subKeysNames.Length > node.Nodes.Count)                // some keys have been added
                {

                    // Adding newly added keys to the nodes collection
                    for (int i = 0; i < subKeysNames.Length; i++)
                    {
                        if (i >= node.Nodes.Count || !node.Nodes[i].Text.Equals(subKeysNames[i])) // Ok the key must be added here.
                        {
                            RegistryKey key = registryKey.OpenSubKey(subKeysNames[i], true);
                            InsertKey(node.Nodes, key, i);
                            key.Close();
                        }
                    }
                }
                else // some keys have been removed/renamed
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        string nodesText = node.Nodes[i].Text;
                        if (i >= subKeysNames.Length || !nodesText.Equals(subKeysNames[i])) // The key must be added here.
                        {
                            node.Nodes.RemoveAt(i);
                        }
                    }
                }
            }
            showWaitingCursor(false);
            registryKey.Close();// Free resoruces
        }

        private void showWaitingCursor(bool show)
        {
            if (show)
                SetCursor(LoadCursor(IntPtr.Zero, IDC_WAIT));
            else
                SetCursor(LoadCursor(IntPtr.Zero, IDC_ARROW)); 
        }

        private void AddKey(TreeNodeCollection rootSubNodesCollection, RegistryKey key)
        {
            if (rootSubNodesCollection == null || key == null)
                return;
            // Getting the name of the key by removing the path elements
            string[] keyPathComponents = key.Name.Split(new [] {'\\'});
            string name = keyPathComponents[keyPathComponents.Length - 1];
            TreeNode node = rootSubNodesCollection.Add(name); // Adding a new node to the collection.
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            if (key.SubKeyCount != 0)
                node.Nodes.Add(string.Empty); // Showing the + Sign next to the node
        }

        private void InsertKey(TreeNodeCollection rootSubNodesCollection, RegistryKey key, int index)
        {
            if (rootSubNodesCollection == null || key == null)
                return;
            // Getting the name of the key by removing the path elements
            string[] keyPathElements = key.Name.Split(new[] {'\\'});
            string name = keyPathElements[keyPathElements.Length - 1];
            TreeNode node = new TreeNode(name);
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            rootSubNodesCollection.Insert(index, node); // Adding a new node to the collection.
            if (key.SubKeyCount != 0)
                node.Nodes.Insert(index, new TreeNode(string.Empty)); // Showing the + Sign next to the node
        }

        private int GetImageIndex(RegistryValueKind kind)
        {
            switch (kind)
            {
                case RegistryValueKind.Binary:
                    return 3;
                case RegistryValueKind.DWord:
                    return 4;
                case RegistryValueKind.ExpandString:
                    return 7;
                case RegistryValueKind.MultiString:
                    return 6;
                case RegistryValueKind.QWord:
                    return 5;
                case RegistryValueKind.String:
                    return 2;
                default: // Unknown
                    return 8;
            }
        }
        
        private string FormatValue(object value, RegistryValueKind kind)
        {
            if (value == null)
                return string.Empty;
            Type obj = value.GetType();

            string formattedValue = value.ToString();
            switch (kind)
            {
                case RegistryValueKind.Binary: // Byte array
                    {

                        formattedValue = string.Empty;
                        byte[] byteValues = value as byte[];
                        foreach (byte byteValue in byteValues)
                        {
                            formattedValue += string.Format("{0:X2} ", byteValue);
                        }
                        break;
                    }
                case RegistryValueKind.DWord:
                    {
                        formattedValue = string.Format("{0}(0x{1:X8})", value, value);
                        break;
                    }
                case RegistryValueKind.QWord:
                    {
                        formattedValue = string.Format("{0}(0x{1:X16})", value, value);
                        break;
                    }
                case RegistryValueKind.MultiString:
                    {
                        string[] subValues = value as string[];
                        formattedValue = string.Empty;
                        foreach (string subValue in subValues)
                        {
                            if (string.IsNullOrEmpty(subValue))
                                continue;
                            formattedValue += subValue + ";";
                        }
                        formattedValue = formattedValue.Substring(0, formattedValue.Length - 1);
                        break;
                    }
            }
            return formattedValue;
        }

        public bool IsKeyValueSelected
        {
            get
            {
                // Don't allow deletion of Root keys like HKEY_CURRENT_USER.
                var selected = (this.Window.trvKeys.SelectedNode != null) &&
                                !this.Window.trvKeys.SelectedNode.Text.Equals(Registry.ClassesRoot.Name) &&
                                !this.Window.trvKeys.SelectedNode.Text.Equals(Registry.CurrentUser.Name) &&
                                !this.Window.trvKeys.SelectedNode.Text.Equals(Registry.LocalMachine.Name) &&
                                !this.Window.trvKeys.SelectedNode.Text.Equals(Registry.Users.Name);
                selected |= !this.Window.lstValues.SelectedIndices.Count.Equals(0);

                return selected;
            }
        }
        #endregion
    }
}