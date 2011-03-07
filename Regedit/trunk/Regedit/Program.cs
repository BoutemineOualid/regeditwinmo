using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Regedit.Views;

namespace Regedit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(frmMain.Instance);
        }
    }
}