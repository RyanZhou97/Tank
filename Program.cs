﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace My90Tank
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mapchose());
            Application.Run(new start());
            Application.Run(new Form1());
        }
    }
}
