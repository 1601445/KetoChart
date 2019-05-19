using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetoGraph.Model;

namespace KetoGraph
{
    interface ICarga
    {
        string[,] ReadCSV(string[,] args, string ficheiro);
        string OpenBox();
        event EventHandler<MatrixEventArgs> OnInfoIn;
    }
}
