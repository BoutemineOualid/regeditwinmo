namespace Regedit.Views
{
    partial class frmAddEditValue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

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
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.mnuCancel = new System.Windows.Forms.MenuItem();
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbxValueType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.optDecimal = new System.Windows.Forms.RadioButton();
            this.optHexadecimal = new System.Windows.Forms.RadioButton();
            this.lblInputMethod = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.mnuCancel);
            this.mainMenu.MenuItems.Add(this.mnuSave);
            // 
            // mnuCancel
            // 
            this.mnuCancel.Text = "Cancel";
            this.mnuCancel.Click += new System.EventHandler(this.onMnuCancelClick);
            // 
            // mnuSave
            // 
            this.mnuSave.Text = "Save";
            this.mnuSave.Click += new System.EventHandler(this.onMnuSaveClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 18);
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(58, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(169, 21);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.Text = "Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbxValueType
            // 
            this.cmbxValueType.Items.Add("String");
            this.cmbxValueType.Items.Add("Binary");
            this.cmbxValueType.Items.Add("DWord (32-bit)");
            this.cmbxValueType.Items.Add("QWord (64-bit)");
            this.cmbxValueType.Items.Add("Multi-String");
            this.cmbxValueType.Items.Add("Expandable String");
            this.cmbxValueType.Location = new System.Drawing.Point(57, 40);
            this.cmbxValueType.Name = "cmbxValueType";
            this.cmbxValueType.Size = new System.Drawing.Size(170, 22);
            this.cmbxValueType.TabIndex = 3;
            this.cmbxValueType.SelectedIndexChanged += new System.EventHandler(this.onCmbxValueTypeSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.Text = "Value:";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(57, 72);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtValue.Size = new System.Drawing.Size(168, 80);
            this.txtValue.TabIndex = 5;
            this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onTxtValueKeyPress);
            // 
            // optDecimal
            // 
            this.optDecimal.Checked = true;
            this.optDecimal.Location = new System.Drawing.Point(19, 204);
            this.optDecimal.Name = "optDecimal";
            this.optDecimal.Size = new System.Drawing.Size(100, 20);
            this.optDecimal.TabIndex = 9;
            this.optDecimal.Text = "Decimal";
            this.optDecimal.Visible = false;
            // 
            // optHexadecimal
            // 
            this.optHexadecimal.Location = new System.Drawing.Point(125, 204);
            this.optHexadecimal.Name = "optHexadecimal";
            this.optHexadecimal.Size = new System.Drawing.Size(100, 20);
            this.optHexadecimal.TabIndex = 10;
            this.optHexadecimal.TabStop = false;
            this.optHexadecimal.Text = "Hexadecimal";
            this.optHexadecimal.Visible = false;
            // 
            // lblInputMethod
            // 
            this.lblInputMethod.Location = new System.Drawing.Point(19, 181);
            this.lblInputMethod.Name = "lblInputMethod";
            this.lblInputMethod.Size = new System.Drawing.Size(89, 19);
            this.lblInputMethod.Text = "Input Method";
            this.lblInputMethod.Visible = false;
            // 
            // txtPath
            // 
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPath.Location = new System.Drawing.Point(0, 247);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(240, 21);
            this.txtPath.TabIndex = 14;
            this.txtPath.WordWrap = false;
            // 
            // frmAddEditValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblInputMethod);
            this.Controls.Add(this.optHexadecimal);
            this.Controls.Add(this.optDecimal);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbxValueType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "frmAddEditValue";
            this.Text = "Add Value";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuItem mnuCancel;
        private System.Windows.Forms.MenuItem mnuSave;
        public System.Windows.Forms.TextBox txtPath;
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.ComboBox cmbxValueType;
        public System.Windows.Forms.TextBox txtValue;
        public System.Windows.Forms.RadioButton optDecimal;
        public System.Windows.Forms.RadioButton optHexadecimal;
        public System.Windows.Forms.Label lblInputMethod;
    }
}