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

namespace KetoGraph.Model
{

    //Classe personalisada para poder mandar informação 
    //através dos eventos.
    public class MatrixEventArgs : EventArgs
    {
        public MatrixEventArgs(string[,] s)
        {
            msg = s;
        }
        private string[,] msg;
        public string[,] Matrix
        {
            get { return msg; }
            set { msg = value; }
        }
    }

    public class Brain : ICarga
    {
        // Com C# 2.0 podemos usar a versão genérica sem precisar de delegado personalizado.
        // Uso por tanto o formato public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        public event EventHandler<MatrixEventArgs> OnInfoIn;

        //Janela para selecionar o ficheiro que vamos carregar
        public string OpenBox()
        {
            string ficheiro;
            OpenFileDialog abrirF = new OpenFileDialog
            {
                InitialDirectory = @"C:",
                Title = "Carregar Ficheiro",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (abrirF.ShowDialog() == DialogResult.OK)
            {
                ficheiro = abrirF.FileName;
            }
            else { ficheiro = ""; }
            return ficheiro;
        }

        //Método de acesso ao ficheiro e recolha de informação 
        public string[,] ReadCSV(string[,] args, string ficheiro)
        {
            int index = 0;
            int j = 0;
            string[,] reg1 = new string[100, 3];
            List<string> D = new List<string>();
            List<string> G = new List<string>();
            List<string> K = new List<string>();
            using (var leitor = new StreamReader(ficheiro))
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
                    j = 0;
                    index++;
                }
            }
            InfoIn(new MatrixEventArgs(reg1));
            return reg1;
        }

        //InfoIN informa a quem quiser subscrever --> a informação já está carregada
        protected virtual void InfoIn(MatrixEventArgs e)
        {
            OnInfoIn?.Invoke(this, e);
        }
    }
}

