﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using KetoGraph.Model;
using KetoGraph.View;


namespace KetoGraph
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Brain pub = new Brain();
            Botoes sub = new Botoes();

            Application.Run(new Botoes());
        }
    }
}