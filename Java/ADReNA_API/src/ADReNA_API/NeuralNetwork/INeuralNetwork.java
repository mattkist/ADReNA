package ADReNA_API.NeuralNetwork;

import ADReNA_API.Data.DataSet;

public interface INeuralNetwork
{
   void Learn(DataSet trainingSet) throws Exception;
   double[] Recognize(double[] input) throws Exception;
}
