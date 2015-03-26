using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADReNA_API.NeuralNetwork;
using Newtonsoft.Json;
using ADReNA_API.Data;

namespace ADReNA_API.Util
{
    public static class Import
    {
        /*
        * Recupera um conjunto de padrões para treinamento
        */
        public static DataSet DataSet(string file)
        {
            string json = ReadFile(file);
            return JsonConvert.DeserializeObject<DataSet>(json);
        }

        /*
         * Recupera a base de conhecimento de uma RNA a partir de um arquivo
         */
        public static void KnowledgeBase(INeuralNetwork net, string file)
        {
            string json = ReadFile(file);

            if (net is Kohonen)
            {
                ((Kohonen)net).SetCompetitiveLayer(JsonConvert.DeserializeObject<KohonenNeuron[,]>(json));
            }
            else //Backpropagation
            {
                ((Backpropagation)net).SetLayers(JsonConvert.DeserializeObject<List<BackpropagationLayer>>(json));
            }
        }

        /*
         * Recupera a estrutura de uma RNA a partir de um arquivo
         */
        public static INeuralNetwork NeuralNetworkStructure(string file)
        {
            string json = ReadFile(file);
            ADReNA_API.Util.ExportImportCommon.CommonStructure stru = JsonConvert.DeserializeObject<ADReNA_API.Util.ExportImportCommon.CommonStructure>(json);
            INeuralNetwork ann = null;
            switch (stru.type)
            {
                case ExportImportCommon.AnnType.Backpropagation:
                    ann = new Backpropagation(stru.inputLayerSize.Value, stru.outputLayerSize.Value, stru.hiddenLayerSizes);
                    ((Backpropagation)ann).SetErrorRate(stru.error.Value);
                    ((Backpropagation)ann).SetMaxIterationNumber(stru.iterationNumber.Value);
                    ((Backpropagation)ann).SetLearningRate(stru.learningRate.Value);
                    break;
                case ExportImportCommon.AnnType.Kohonen:
                    ann = new Kohonen(stru.inputLayerSize.Value, stru.competitiveNeuronLength.Value, stru.maximumWeightRange.Value);
                    ((Kohonen)ann).SetIterationNumber(stru.iterationNumber.Value);
                    ((Kohonen)ann).SetLearningRate(stru.learningRate.Value);
                    ((Kohonen)ann).SetNeighborhoodRadius(stru.neighborhoodRadius.Value);
                    break;
            }

            return ann;
        }

        private static string ReadFile(string file)
        {
            System.IO.StreamReader sReader = new System.IO.StreamReader(file);
            string json = sReader.ReadToEnd();
            sReader.Close();
            return json;
        }

    }
}
