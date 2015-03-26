using ADReNA_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public class Kohonen : INeuralNetwork
    {
        #region Attributes
        public int inputLayerSize, competitiveNeuronLength, iterationNumber, neighborhoodRadius, maximumWeightRange;
        public double learningRate;
        public KohonenNeuron[,] competitiveLayer;
        #endregion

        #region public Methods
        /*
         * Método de aprendizado da Rede
         */
        public void Learn(DataSet trainingSet)
        {
            if (trainingSet.GetOutputSize() != 0 || trainingSet.GetInputSize() != this.inputLayerSize)
                throw new Exception("Incorrect data format!");
            else
            {
                List<DataSetObject> dataset = trainingSet.GetList();

                //Para cada iteração
                for (int itN = 0; itN < iterationNumber; itN++)
                {
                    //Para cada entrada
                    for (int inputN = 0; inputN < trainingSet.Length(); inputN++)
                    {
                        //Calcula neurônio vencedor
                        KohonenNeuron neuro = CalculateWinnerNeuron(dataset[inputN]);

                        //Atualiza pesos do neurônio vencedor e de seus vizinhos
                        List<KohonenNeuron> winners = GetNeighbors(neuro);
                        for (int x = 0; x < winners.Count; x++)
                            winners[x].UpdateWeights(dataset[inputN], learningRate);
                    }
                }
            }
        }

        /*
         * Método de aprendizado da Rede, enviando alguns parâmetros de configuração do aprendizado
         */
        public void Learn(DataSet trainingSet, int iterationNumber, int learningRate, int neighborhoodRadius)
        {
            this.iterationNumber = iterationNumber;
            this.learningRate = learningRate;
            this.neighborhoodRadius = neighborhoodRadius;
            Learn(trainingSet);
        }

        public double[] Recognize(double[] input)
        {
            if (input.Count() != this.inputLayerSize)
                throw new Exception("Incorrect data format!");
            else
            {
                DataSetObject obj = new DataSetObject(input);
                //Calcula neurônio vencedor
                return CalculateWinnerNeuron(obj).GetWeights();
            }
        }

        public KohonenNeuron RecognizeWinnerNeuron(double[] input)
        {
            if (input.Count() != this.inputLayerSize)
                throw new Exception("Incorrect data format!");
            else
            {
                DataSetObject obj = new DataSetObject(input);
                //Calcula neurônio vencedor
                return CalculateWinnerNeuron(obj);
            }
        }

        /*
         * Cria uma instância de uma rede Kohonen
         */
        public Kohonen(int inputLayerSize, int competitiveNeuronLength)
        {
            BuildNewKohonen(inputLayerSize, competitiveNeuronLength, competitiveNeuronLength);
        }

        /*
         * Cria uma instância de uma rede Kohonen
         */
        public Kohonen(int inputLayerSize, int competitiveNeuronLength, int maximumWeightRange)
        {
            BuildNewKohonen(inputLayerSize, competitiveNeuronLength, maximumWeightRange);
        }

        /*
         * Retorna o intervalo de peso máximo escolhido na instanciação da rede
         */
        public int GetMaximumWeightRange()
        {
            return this.maximumWeightRange;
        }

        /*
         * Retorna todos os neurônios da camada competitiva
         */
        public KohonenNeuron[,] GetCompetitiveLayer()
        {
            return this.competitiveLayer;
        }

        /*
         * Define todos os neurônios da camada competitiva
         */
        public void SetCompetitiveLayer(KohonenNeuron[,] cLayers)
        {
            if (cLayers.GetLength(0) == this.competitiveLayer.GetLength(0) && cLayers.GetLength(1) == this.competitiveLayer.GetLength(1))
                this.competitiveLayer = cLayers;
            else
                throw new Exception("Incorrect data format!");
        }

        /*
         * Retorna o tamanho da camada competitiva (onde NxN)
         */
        public int GetCompetitiveLayerLength()
        {
            return this.competitiveNeuronLength;
        }

        /*
         * Retorna o tamanho da camada de entrada
         */
        public int GetInputLayerSize()
        {
            return this.inputLayerSize;
        }

        /*
         * Define o fator de aprendizagem
         */
        public void SetLearningRate(double rate)
        {
            learningRate = rate;
        }

        /*
         * Retorna o fator de aprendizagem
         */
        public double GetLearningRate()
        {
            return learningRate;
        }

        /*
         * Define o raio da vizinhança
         */
        public void SetNeighborhoodRadius(int radius)
        {
            neighborhoodRadius = radius;
        }

        /*
         * Retorna o raio da vizinhança
         */
        public int GetNeighborhoodRadius()
        {
            return neighborhoodRadius;
        }

        /*
         * Define o número de iterações
         */
        public void SetIterationNumber(int number)
        {
            iterationNumber = number;
        }

        /*
         * Retorna o número de iterações
         */
        public int GetIterationNumber()
        {
            return iterationNumber;
        }

        #endregion

        #region private Methods
        private void BuildNewKohonen(int inputLayerSize, int competitiveNeuronLength, int maximumWeightRange)
        {
            this.maximumWeightRange = maximumWeightRange;
            iterationNumber = 20 * inputLayerSize * competitiveNeuronLength; 
            learningRate = 0.2; 
            neighborhoodRadius = 1;

            this.inputLayerSize = inputLayerSize;
            this.competitiveNeuronLength = competitiveNeuronLength;
            competitiveLayer = new KohonenNeuron[competitiveNeuronLength, competitiveNeuronLength];
            for (int i = 0; i < competitiveNeuronLength; i++)
                for (int j = 0; j < competitiveNeuronLength; j++)
                    (competitiveLayer[i, j] = new KohonenNeuron(inputLayerSize, i, j)).RandomizeWeights(maximumWeightRange);
        }

        /*
         * Dado um padrão de entrada, calcula o neurônio vencedor
         */
        private KohonenNeuron CalculateWinnerNeuron(DataSetObject pattern)
        {
            double smallestDist = -1;
            KohonenNeuron winner = new KohonenNeuron(inputLayerSize);

            //Para cada neurônio da camada competitiva
            for(int i = 0; i < competitiveNeuronLength; i ++)
            {
                for(int j = 0; j < competitiveNeuronLength; j ++)
                {
                    KohonenNeuron neuron = competitiveLayer[i,j];

                    //Calcula a distância euclidiana
                    double dist = neuron.GetEuclideanDistance(pattern);
                    //Primeiro neurônio sempre é o primeiro vencedor
                    if (smallestDist == -1)
                    {
                        smallestDist = dist;
                        winner = neuron;
                    }
                    //Se a distância for a menor que a distância do último vencedor, temos um novo vencedor 
                    if (dist < smallestDist)
                    {
                        smallestDist = dist;
                        winner = neuron;
                    }
                }
            }

            return winner;
        }

        /*
         * Retorna todos os neurônios no raio de vizinhança do vencedor
         */
        private List<KohonenNeuron> GetNeighbors(KohonenNeuron neuron)
        {
            List<KohonenNeuron> list = new List<KohonenNeuron>();

            int[] index = FindIndex(neuron);

            int X = index[0];
            int Y = index[1];

            for (int i = X - neighborhoodRadius; i <= X + neighborhoodRadius; i++)
            {
                for (int j = Y - neighborhoodRadius; j <= Y + neighborhoodRadius; j++)
                {
                    if (i < competitiveNeuronLength && i >= 0 && j < competitiveNeuronLength && j >= 0)
                    {
                        list.Add(competitiveLayer[i, j]);
                    }
                }
            }

            return list;
        }

        private int[] FindIndex(KohonenNeuron neuron)
        {
            int[] IJ = new int[2];
            for (int i = 0; i < competitiveLayer.GetLength(0); i++)
            {
                for (int j = 0; j < competitiveLayer.GetLength(1); j++)
                {
                    if (neuron == competitiveLayer[i, j])
                    {
                        IJ[0] = i;
                        IJ[1] = j;
                    }
                }
            }
            return IJ;
        }
        #endregion

    }
}
