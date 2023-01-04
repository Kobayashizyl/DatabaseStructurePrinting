using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Database
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string lj = Application.StartupPath + "\\license.key";
            Stimulsoft.Base.StiLicense.LoadFromFile(lj);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
