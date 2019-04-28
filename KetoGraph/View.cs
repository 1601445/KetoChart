using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
using KetoGraph.Model;


namespace KetoGraph.View
{
    public partial class Botoes : Form
    {
        static Brain b;

        public Botoes()
        {
            InitializeComponent();
        }

        //Temos 2 botões: Load e Sair(Fechar)
        //Quando clicar no Load é chamado Loader, input do user
        private void ClicarLoad(object sender, EventArgs e)
        {
            b = new Brain();
            string[,] a = new string[100, 3];

            try
            {
                Loader(a, b);
            }
            catch (IOException ex)
            {
                if (ex is FileNotFoundException) MessageBox.Show(ex.Message);
            }
        }
        //Evento associado ao click do botão sair
        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        //O Loader chama um método dentro do Model que vai recolher os dados do ficheiro
        //e passá-los à matriz bidimensional para processar a informação
        //Também subscreve Print a OnInfoIn
        public string[,] Loader(string[,] args, Brain pub)
        {
            string[,] a = new string[100, 3];

            pub.OnInfoIn += Print;
            a = pub.ReadCSV(a);
            return a;
        }

        public void Print(object sender, MatrixEventArgs e)
        {
            int d;
            int m;
            int yy;
            int i = 0;

            PointPairList glicoXY = new PointPairList();
            PointPairList ketoXY = new PointPairList();
            PointPairList glicoKXY = new PointPairList();
            PointPairList racioXY = new PointPairList();

            while (e.Matrix[i, 0] != null)
            {
                var data = e.Matrix[i, 0].Split('/');

                d = Convert.ToInt32(data[0]);
                m = Convert.ToInt32(data[1]);
                yy = Convert.ToInt32(data[2]);

                double x = (double)new XDate(d, m, yy);
                double y1 = Convert.ToDouble(e.Matrix[i, 1]);
                double y2 = Convert.ToDouble(e.Matrix[i, 2]) / 10;
                double y3 = y1 / 18.0194805;
                double racio = y3 / y2;

                glicoXY.Add(x, y1);
                ketoXY.Add(x, (y2));
                glicoKXY.Add(x, y3);
                racioXY.Add(x, racio);

                i++;
            }
            //zedGraphControl1.GraphPane.AddCurve("Glicose(mg/dl)", glicoXY, Color.Green);
            zedGraphControl1.GraphPane.AddCurve("Keto(mmol/L)", ketoXY, Color.Red);
            zedGraphControl1.GraphPane.AddCurve("Glicose(mmol/L)", glicoKXY, Color.Blue);
            zedGraphControl1.GraphPane.AddCurve("Racio G:K", racioXY, Color.Yellow);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Refresh();
        }
    }
}
