package ADReNA_API.Util;

import ADReNA_API.Util.ExportImportCommon.AnnType;

public class CommonStructure
{
    public AnnType type;
    public Integer inputLayerSize, competitiveNeuronLength, iterationNumber, neighborhoodRadius, maximumWeightRange, outputLayerSize;
    public Double learningRate, error;
    public Integer[] hiddenLayerSizes;
}
