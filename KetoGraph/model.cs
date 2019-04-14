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
    //Faço uma classe personalisada para poder mandar informação 
    //através dos eventos. Neste caso vou usar uma matriz bidimensional
    //onde possa guardar os valores recolhidos do ficheiro CSV. 
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

    public class Brain
    {
        // Com C# 2.0 podemos usar a versão genérica sem precisar de delegado personalizado.
        // Uso por tanto o formato public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        public event EventHandler<MatrixEventArgs> OnInfoIn;

        //Método de acesso ao ficheiro, recolha da informação 
        public string[,] ReadCSV(string[,] args)
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
                    j = 0;
                    index++;
                }
            }
            InfoIn(new MatrixEventArgs(reg1));
            return reg1;
        }
        //InfoIN informa a quem quiser subscrever que a informação já está carregada
        protected virtual void InfoIn(MatrixEventArgs e)
        {
            OnInfoIn?.Invoke(this, e);
        }
    }
}
