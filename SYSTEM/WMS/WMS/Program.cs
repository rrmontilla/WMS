using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static WMS_security loginfrm;
        public static WMS_Main mainfrm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            loginfrm = new WMS_security();
            mainfrm = new WMS_Main();
            Application.Run(loginfrm);
        }
    }
}
