using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using KetoGraph.Model;


namespace KetoGraph.View
{
    interface ICarga
    {
        string[,] ReadCSV(string[,] args, string ficheiro);
        string OpenBox();
        event EventHandler<MatrixEventArgs> OnInfoIn;
    }

    public partial class Botoes : Form
    {
        ICarga leitura = new Brain();
        bool desenhado = false;

        public Botoes()
        {
            InitializeComponent();
        }

        //Clicar no botao Load vai buscar o ficheiro e chama Loader, input do user
        private void ClicarLoad(object sender, EventArgs e)
        {
            string[,] a = new string[100, 3];
            string ficheiro;

            if (desenhado == false)
            {
                try
                {
                    ficheiro = leitura.OpenBox();
                    if (ficheiro != "")
                    {
                        Loader(a, ficheiro);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Os dados já foram carregados");
            }

        }
        //Evento associado ao click do botão sair
        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        //O Loader chama um método dentro do Model usando a interface ICarga 
        public string[,] Loader(string[,] args, string ficheiro)
        {
            string[,] a = new string[100, 3];
            leitura.OnInfoIn += Print;
            a = leitura.ReadCSV(a, ficheiro);
            return a;
        }

        //Print imprime as linhas
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
            desenhado = true;
        }
    }
}