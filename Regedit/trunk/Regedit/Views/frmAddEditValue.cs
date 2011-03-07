using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsCE.Forms;
using Regedit.Presenters;

namespace Regedit.Views
{
    public partial class frmAddEditValue : Form
    {
        #region Ctor

        public frmAddEditValue()
        {
            InitializeComponent();
            this.Presenter = new AddValuePresenter(this);
        }
        #endregion

        #region Properties

        public AddValuePresenter Presenter { get; private set;}

        private static frmAddEditValue instance;
        public static frmAddEditValue Instance
        {
            get
            {
                if (instance == null)
                    instance = new frmAddEditValue();
                return instance;
            }
        }

        private bool closedByUser;

        public string CurrentKeyPath
        {
            get; 
            private set;
        }

        public bool IsEditing { get; private set; }

        public string LastEditedValueName { get; private set; }

        #endregion

        #region Event Listeners

        private void onCmbxValueTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Text = string.Empty;
            
            AdaptUI();
        }

        private void onMnuSaveClick(object sender, EventArgs e)
        {
            closedByUser = true;
            // Saving the key.
            SaveValue();
            this.DialogResult = DialogResult.Yes;
            //this.Close();
        }

        private void onMnuCancelClick(object sender, EventArgs e)
        {
            closedByUser = true;
            this.DialogResult = DialogResult.Cancel;
            // this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!closedByUser)
            {
                // verify if some changes have been made to the data
                RegistryKey currentKey = RegistryUtils.OpenKeyFromPath(this.CurrentKeyPath, true);
                object oldData = currentKey.GetValue(txtName.Text);
                if (this.Presenter.CurrentData.ToString() == oldData.ToString())
                {
                    base.OnClosing(e);
                    return;
                }
                // Show a confimation message
                DialogResult result = MessageBox.Show("Do you want to save this value?", "Save", MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    SaveValue();
                    this.DialogResult = DialogResult.Yes;
                }
                else
                    this.DialogResult = DialogResult.Cancel;
            }
            closedByUser = false;
            base.OnClosing(e);
        }

        private bool nonNumberEntered = false;

        private void onTxtValueKeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.Presenter.IsStringValue)
                return;
            // Initialize the flag to false.
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                // Determine whether the keystroke is a backspace.
                if (e.KeyChar != '\b'       // Backspace
                    && e.KeyChar != '\r'    // Enter
                    )
                {
                    if (this.optHexadecimal.Checked)
                    {
                        char enteredChar = char.ToUpper(e.KeyChar);
                        if (enteredChar < 'A' || enteredChar > 'F')
                            nonNumberEntered = true;
                    }
                    else
                        nonNumberEntered = true;
                }
            } 
            e.Handled = nonNumberEntered;
        }

        #endregion

        #region Methods

        private void SaveValue()
        {
            try
            {
                RegistryValueKind kind = this.Presenter.SelectedValueKind;            // Value's kind
                object value = this.Presenter.CurrentData;                      // Value's Data
                RegistryKey currentKey = RegistryUtils.OpenKeyFromPath(this.CurrentKeyPath, true);
                if (this.IsEditing) // Removing the old value.
                    currentKey.DeleteValue(this.LastEditedValueName);
                currentKey.SetValue(txtName.Text, value, kind);
                currentKey.Flush();
                currentKey.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                MessageBoxDefaultButton.Button1);                
            }
        }

        public void ShowDialog(string keyPath)
        {
            this.Text = "Add New Value";
            this.txtPath.Text = frmMain.Instance.txtPath.Text;
            this.cmbxValueType.SelectedIndex = 0;
            this.txtName.Text = string.Empty;
            this.txtValue.Text = string.Empty;
            this.optDecimal.Checked = true;
            this.txtName.Focus();
            this.CurrentKeyPath = keyPath;
            this.ShowDialog();
        }

        public void ShowDialogForEditRename(string keyPath, string valueName, bool isEdit)
        {
            if (isEdit)
                this.Text = "Edit Value";
            else // Rename
            {
                this.Text = "Rename Value";
                this.txtValue.Enabled = false;
                this.cmbxValueType.Enabled = false;
                this.optDecimal.Enabled = false;
                this.optHexadecimal.Enabled = false;
            }
            this.txtPath.Text = frmMain.Instance.txtPath.Text;
            this.LastEditedValueName = valueName;
            this.IsEditing = true;
            this.CurrentKeyPath = keyPath;
            this.optDecimal.Checked = true;

            RegistryKey parrentKey = RegistryUtils.OpenKeyFromPath(keyPath, false);
            object valueData = parrentKey.GetValue(valueName);
            RegistryValueKind valueKind = parrentKey.GetValueKind(valueName);
            parrentKey.Close();

            // Value Name.
            this.txtName.Text = valueName;

            // Value Kind / Value Data.
            switch (valueKind)
            {
                case RegistryValueKind.String:
                    this.cmbxValueType.SelectedIndex = 0; // BUG FIXED: cmbxValueType's SelectedIndex property must be set before showing data contents
                    this.txtValue.Text = valueData.ToString();
                    break;
                case RegistryValueKind.Binary: // Array of bytes.
                    this.cmbxValueType.SelectedIndex = 1;
                    byte[] bytes = valueData as byte[];
                    foreach (var b in bytes)
                    {
                        this.txtValue.Text += b.ToString() + "\r\n";
                    }
                    break;
                case RegistryValueKind.DWord:
                    this.cmbxValueType.SelectedIndex = 2;
                    this.txtValue.Text = valueData.ToString();
                    break;
                case RegistryValueKind.QWord:
                    this.cmbxValueType.SelectedIndex = 3;
                    this.txtValue.Text = valueData.ToString();
                    break;
                case RegistryValueKind.MultiString: // Array of strings
                    this.cmbxValueType.SelectedIndex = 4;
                    string[] lines = valueData as string[];
                    foreach (var line in lines)
                    {
                        this.txtValue.Text += line + "\r\n";
                    }
                    break;
                case RegistryValueKind.ExpandString:
                    this.cmbxValueType.SelectedIndex = 5;
                    this.txtValue.Text = valueData.ToString();
                    break;
                default: // Unknown Type
                    this.cmbxValueType.SelectedIndex = 0;
                    this.txtValue.Text = valueData.ToString();
                    break;
            }

            this.txtName.Focus();
            this.txtName.SelectAll();
            this.ShowDialog();
            // Default Values
            this.IsEditing = false;
            this.LastEditedValueName = txtName.Text;
            this.txtValue.Enabled = true;
            this.cmbxValueType.Enabled = true;
            this.optDecimal.Enabled = true;
            this.optHexadecimal.Enabled = true;

        }

        private void AdaptUI()
        {
            // Identifying the selected value
            switch (cmbxValueType.SelectedIndex)
            {
                case 0:            // 0 String
                    this.txtValue.Multiline = false;
                    this.txtValue.ScrollBars = ScrollBars.None;
                    this.txtValue.Height = 21;
                    // Input Method
                    this.lblInputMethod.Visible = false;
                    this.optDecimal.Visible = false;
                    this.optHexadecimal.Visible = false;
                    break;
                case 1:            // 1 Binary
                    this.txtValue.Multiline = true;
                    this.txtValue.ScrollBars = ScrollBars.Vertical;
                    this.txtValue.Height = 80;
                    // Input Method
                    this.lblInputMethod.Top = 182;
                    this.lblInputMethod.Visible = true;
                    this.optDecimal.Top = 204;
                    this.optDecimal.Visible = true;
                    this.optHexadecimal.Top = this.optDecimal.Top;
                    this.optHexadecimal.Visible = true;
                    
                    break;
                case 2:            // 2 DWord (32-bit)
                    this.txtValue.Multiline = false;
                    this.txtValue.ScrollBars = ScrollBars.None;
                    this.txtValue.Height = 21;
                    // Input Method
                    this.lblInputMethod.Top = 125;
                    this.lblInputMethod.Visible = true;
                    this.optDecimal.Top = 147;
                    this.optDecimal.Visible = true;
                    this.optHexadecimal.Top = this.optDecimal.Top;
                    this.optHexadecimal.Visible = true;
                    break;
                case 3:            // 3 QWord (64-bit)
                    this.txtValue.Multiline = false;
                    this.txtValue.ScrollBars = ScrollBars.None;
                    this.txtValue.Height = 21;
                    // Input Method
                    this.lblInputMethod.Top = 125;
                    this.lblInputMethod.Visible = true;
                    this.optDecimal.Top = 147;
                    this.optDecimal.Visible = true;
                    this.optHexadecimal.Top = this.optDecimal.Top;
                    this.optHexadecimal.Visible = true;
                    break;
                case 4:            // 4 Multi-String
                    this.txtValue.Multiline = true;
                    this.txtValue.ScrollBars = ScrollBars.Vertical;
                    this.txtValue.Height = 115;
                    // Input Method
                    this.lblInputMethod.Visible = false;
                    this.optDecimal.Visible = false;
                    this.optHexadecimal.Visible = false;
                    break;
                case 5:            // 5 Expandable String
                    this.txtValue.Multiline = false;
                    this.txtValue.ScrollBars = ScrollBars.None;
                    this.txtValue.Height = 21;
                    // Input Method
                    this.lblInputMethod.Visible = false;
                    this.optDecimal.Visible = false;
                    this.optHexadecimal.Visible = false;
                    break;
            }
        }

        #endregion
    }
}