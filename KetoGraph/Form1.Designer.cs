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

namespace KetoGraph
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.IsShowPointValues = false;
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 12);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.PointValueFormat = "G";
            this.zedGraphControl1.Size = new System.Drawing.Size(661, 390);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(679, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.load);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(679, 53);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 35);
            this.button2.TabIndex = 2;
            this.button2.Text = "Sair";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Sair);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

        public string[,] readCVS(string[,] args)
        {
            int index = 0;
            int j = 0;
            string[,] reg1 = new string[100, 3];
            List<string> D = new List<string>();
            List<string> G = new List<string>();
            List<string> K = new List<string>();
            using (var leitor = new StreamReader(@"C:/data.csv"))
            {
                while (!leitor.EndOfStream)
                {
                    var linha = leitor.ReadLine();
                    var valores = linha.Split(';');

                    D.Add(valores[0]);
                    G.Add(valores[1]);
                    K.Add(valores[2]);

                    if (j == 0)
                    {
                        reg1[index, j] = D[index];
                        j = 1; ;
                    }
                    if (j == 1)
                    {
                        reg1[index, j] = G[index];
                        j = 2;
                    }
                    if (j == 2)
                    {
                        reg1[index, j] = K[index];
                    }

                    //MessageBox.Show($"Valores da tabela ");
                    j = 0;
                    index++;
                }
            }
            return reg1;
        }

        public void graficator(string[,] args)
        {
            int d;
            int m;
            int yy;
            int i = 0;

            PointPairList glicoXY = new PointPairList();
            PointPairList ketoXY = new PointPairList();
            PointPairList glicoKXY = new PointPairList();
            PointPairList racioXY = new PointPairList();

            while (args[i, 0] != null)
            {
                var data = args[i, 0].Split('/');

                d = Convert.ToInt32(data[0]);
                m = Convert.ToInt32(data[1]);
                yy = Convert.ToInt32(data[2]);

                double x = (double)new XDate(d, m, yy);
                double y1 = Convert.ToDouble(args[i, 1]);
                double y2 = Convert.ToDouble(args[i, 2]) / 10;
                double y3 = y1 / 18.0194805;
                double racio = y3 / y2;

                glicoXY.Add(x, y1);
                ketoXY.Add(x, (y2));
                glicoKXY.Add(x, y3);
                racioXY.Add(x, racio);

                //MessageBox.Show($"Valores da tabela {x} | {y1} | {y2} | {y3}");
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

