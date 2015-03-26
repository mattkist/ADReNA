using ADReNA_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public interface INeuralNetwork
    {
       void Learn(DataSet trainingSet);
       double[] Recognize(double[] input);
    }
}
