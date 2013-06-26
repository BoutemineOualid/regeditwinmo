using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Regedit.Views;

namespace Regedit.Presenters
{
    public class AddValuePresenter
    {
        #region ctor
        public AddValuePresenter(frmAddEditValue window)
        {
            this.Window = window;
            StartListeners();
        }
        #endregion

        #region Methods
        private void StartListeners()
        {
            // txtValue's Events
            this.Window.txtValue.KeyUp += onValueKeyUp;
            // OptDecimal
            this.Window.optDecimal.CheckedChanged += onOptCheckedChanged;
        }

        private void SwitchDecimalBase(bool isHexa)
        {
            if (IsStringValue)
                return;
            if (string.IsNullOrEmpty(this.Window.txtValue.Text))
                return;
            string format = string.Empty;
            NumberStyles numberStyle = !isHexa ? NumberStyles.HexNumber : NumberStyles.Number;
            if (isHexa)
                format = "{0:X}";
            else
                format = "{0:D}";

            string buffer = string.Empty;
            if (this.Window.cmbxValueType.SelectedIndex.Equals(1)) // Binary
            {
                string[] lines = this.Window.txtValue.Text.Split(new[] { '\n' });

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    long value = long.Parse(line, numberStyle);
                    buffer += string.Format(format, value) + "\r\n";
                }
            }
            else // DWord, QWord
            {
                long value = long.Parse(this.Window.txtValue.Text, numberStyle);
                buffer = string.Format(format, value);
            }
            this.Window.txtValue.Text = buffer;
        }

        private bool ValidateValue(long maxValue)
        {
            bool validationResult = true;
            //int selectionStartIndex = 0;
            //int selectionLength = 0;

            string[] lines = this.Window.txtValue.Text.Split('\n');
            
            // Identifying the current line
            string currentLine = string.Empty;
            int currentCursorPos = this.Window.txtValue.SelectionStart;
            
            // Calculating the number of chars before the current pos
            int charsCounter = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                //selectionStartIndex = charsCounter;
                //selectionLength = lines[i].Length;
                charsCounter += lines[i].Length;
                if (lines.Length > 1 && i < lines.Length - 1) // 2 lines or more and not the last line which does not contain the \n char
                    charsCounter++; // Counting the \n char that was removed.
                if (charsCounter >= currentCursorPos) // Ok this is the wanted line.
                {
                    //if (lines.Length > 1 && i < lines.Length - 1) // 2 lines or more and not the last line which does not contain the \n char
                    //    currentLine = lines[i+1];
                    //else
                    currentLine = lines[i];
                    break;
                }
            }

            try
            {
                if (string.IsNullOrEmpty(currentLine)) // If no available data, get out
                    return true;
                long value = long.Parse(currentLine, this.Window.optHexadecimal.Checked ? NumberStyles.HexNumber : NumberStyles.Number);

                if (value > maxValue)
                    validationResult = false;
            }
            catch
            {
                validationResult = false;
            }

            return validationResult;
        }

        #endregion

        #region Event Listeners
        private void onValueKeyUp(object sender, KeyEventArgs e)
        {
            if (!IsStringValue)
            {
                long maxValue = 0;
                switch (SelectedValueKind)
                {
                    case RegistryValueKind.Binary:
                        maxValue = byte.MaxValue;
                        break;
                    case RegistryValueKind.DWord:
                        maxValue = int.MaxValue;
                        break;
                    case RegistryValueKind.QWord:
                        maxValue = long.MaxValue;
                        break;
                }
                bool validationResult = ValidateValue(maxValue);
                if (!validationResult) // Removing the char
                {
                    // Selecting the line containing the wrong value.
                    //this.Window.txtValue.Select(selectionStartIndex, selectionLength);

                    if (e.KeyCode != Keys.Back) // if this event was fired by the last call to sendkeys, don't show the error message again.
                    {
                        // Show an error message.
                        MessageBox.Show("Invalid value. Max accepted value is " + maxValue.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                                        MessageBoxDefaultButton.Button1);
                    }
                    SendKeys.Send("{BACKSPACE}");
                }
            }
        }

        private void onOptCheckedChanged(object sender, EventArgs e)
        {
            SwitchDecimalBase(this.Window.optHexadecimal.Checked);
        }

        #endregion

        #region Properties
        public frmAddEditValue Window { get; private set; }

        public object CurrentData
        {
            get
            {
                switch (this.Window.cmbxValueType.SelectedIndex)
                {
                    case 0: // 0 String
                        return this.Window.txtValue.Text;
                    case 1: // 1 Binary
                        List<string> binaryDataLines = new List<string>(this.Window.txtValue.Text.Split(new[] { '\n' }));
                        foreach (var binaryDataLine in binaryDataLines)
                        {
                            if (string.IsNullOrEmpty(binaryDataLine)) // Removing empty lines
                                binaryDataLines.Remove(binaryDataLine);
                        }
                        byte[] binaryValues = new byte[binaryDataLines.Count];
                        for (int i = 0; i < binaryDataLines.Count; i++)
                        {
                            try
                            {
                                binaryValues[i] = byte.Parse(binaryDataLines[i],
                                                             this.Window.optDecimal.Checked
                                                                 ? NumberStyles.Number
                                                                 : NumberStyles.HexNumber);
                            }
                            catch
                            {
                            }
                        }
                        return binaryValues;
                    case 2: // 2 DWord (32-bit)
                        try
                        {
                            return int.Parse(this.Window.txtValue.Text,
                                             this.Window.optHexadecimal.Checked
                                                 ? NumberStyles.HexNumber
                                                 : NumberStyles.Number);
                        }
                        catch 
                        {
                            return 0;
                        }
                    case 3: // 3 QWord (64-bit)
                        try
                        {
                            return long.Parse(this.Window.txtValue.Text,
                                             this.Window.optHexadecimal.Checked
                                                 ? NumberStyles.HexNumber
                                                 : NumberStyles.Number);
                        }
                        catch
                        {
                            return 0L;
                        }
                    case 4: // 4 Multi-String
                        string[] stringLines = this.Window.txtValue.Text.Split(new[] { '\n' });
                        for (int i = 0; i < stringLines.Length; i++)
                        {
                            if (!stringLines[i].Contains("\r"))
                                continue;
                            stringLines[i] = stringLines[i].Remove(stringLines[i].Length - 1, 1); // Removing the \r char
                        }
                        return stringLines;
                    case 5: // 5 Expandable String
                        return this.Window.txtValue.Text;
                    default: // Unknown
                        return this.Window.txtValue.Text;
                }
            }
        }

        public RegistryValueKind SelectedValueKind
        {
            get
            {

                switch (this.Window.cmbxValueType.SelectedIndex)
                {
                    case 0: // 0 String
                        return RegistryValueKind.String;
                    case 1: // 1 Binary
                        return RegistryValueKind.Binary;
                    case 2: // 2 DWord (32-bit)
                        return RegistryValueKind.DWord;
                    case 3: // 3 QWord (64-bit)
                        return RegistryValueKind.QWord;
                    case 4: // 4 Multi-String
                        return RegistryValueKind.MultiString;
                    case 5: // 5 Expandable String
                        return RegistryValueKind.ExpandString;
                    default: // 6 Unknown
                        return RegistryValueKind.Unknown;
                }
            }
        }

        public bool IsStringValue
        {
            get
            {
                // Identifying the selected value
                switch (this.Window.cmbxValueType.SelectedIndex)
                {
                    case 0:            // 0 String
                        return true;
                    case 1:            // 1 Binary
                        return false;
                    case 2:            // 2 DWord (32-bit)
                        return false;
                    case 3:            // 3 QWord (64-bit)
                        return false;
                    case 4:            // 4 Multi-String
                        return true;
                    case 5:            // 5 Expandable String
                        return true;
                    default:            // Unknown
                        return true;
                }
            }
        }
        #endregion
    }
}
