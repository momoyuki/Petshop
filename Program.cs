﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Petshop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            FrmIndex fLogin = new FrmIndex();
            if (fLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
        }
    }
}
