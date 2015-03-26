using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADReNA_API.NeuralNetwork;
using ADReNA_API.Data;

namespace ADReNA_API.Util
{
    public static class Export
    {

        /*
        * Serializa um conjunto de padrões para treinamento
        */
        public static void DataSet(DataSet set, string file)
        {
            string json = JsonConvert.SerializeObject(set);
            WriteFile(json, file);
        }

        /*
        * Serializa a base de conhecimento de uma RNA em um arquivo
        */
        public static void KnowledgeBase(INeuralNetwork net, string file)
        {
            string json = "";

            if (net is Kohonen)
            {
                json = JsonConvert.SerializeObject(((Kohonen)net).GetCompetitiveLayer());
            }
            else //Backpropagation
            {
                json = JsonConvert.SerializeObject(((Backpropagation)net).GetLayers());
            }

            WriteFile(json, file);
        }

        /*
         * Serializa a estrutura da rede de uma RNA em um arquivo
         */
        public static void NeuralNetworkStructure(INeuralNetwork net, string file)
        {
            string json = "";
            if (net is Kohonen)
            {
                ADReNA_API.Util.ExportImportCommon.CommonStructure structure = new ExportImportCommon.CommonStructure();
                structure.type = ExportImportCommon.AnnType.Kohonen;
                structure.inputLayerSize =  ((Kohonen)net).GetInputLayerSize();
                structure.competitiveNeuronLength = ((Kohonen)net).GetCompetitiveLayerLength();
                structure.maximumWeightRange = ((Kohonen)net).GetMaximumWeightRange();
                structure.iterationNumber = ((Kohonen)net).GetIterationNumber();
                structure.learningRate = ((Kohonen)net).GetLearningRate();
                structure.neighborhoodRadius = ((Kohonen)net).GetNeighborhoodRadius();

                json = JsonConvert.SerializeObject(structure);
            }
            else //Backpropagation
            {
                ADReNA_API.Util.ExportImportCommon.CommonStructure structure = new ExportImportCommon.CommonStructure();
                structure.type = ExportImportCommon.AnnType.Backpropagation;
                structure.inputLayerSize = ((Backpropagation)net).GetInputLayerSize();
                structure.outputLayerSize = ((Backpropagation)net).GetOutputLayerSize();
                structure.hiddenLayerSizes = ((Backpropagation)net).GetHiddenLayerSizes();
                structure.error = ((Backpropagation)net).GetErrorRate();
                structure.iterationNumber = ((Backpropagation)net).GetMaxIterationNumber();
                structure.learningRate = ((Backpropagation)net).GetLearningRate();

                json = JsonConvert.SerializeObject(structure);
            }

            WriteFile(json, file);
        }

        private static void WriteFile(string json, string file)
        {
            System.IO.StreamWriter sWriter = new System.IO.StreamWriter(file);
            sWriter.WriteLine(json);
            sWriter.Close();
        }
        
    }
}
