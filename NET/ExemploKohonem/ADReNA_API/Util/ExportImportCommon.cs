using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.Util
{
    public static class ExportImportCommon
    {
        public enum AnnType { Backpropagation, Kohonen }

        public class CommonStructure
        {
            public AnnType type;
            public Nullable<int> inputLayerSize, competitiveNeuronLength, iterationNumber, neighborhoodRadius, maximumWeightRange, outputLayerSize;
            public Nullable<double> learningRate, error;
            public int[] hiddenLayerSizes;
        }

    }
}
