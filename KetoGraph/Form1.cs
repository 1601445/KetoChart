using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KetoGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void load(object sender, EventArgs e)
        {
            string[,] args = new string[100, 3];

            args = readCVS((string[,])args);

            graficator((string[,])args);
        }

        private void Sair(object sender, EventArgs e)
        {
            Close();
        }
    }
}
