using ADReNA_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public class KohonenNeuron
    {
        public double[] weights;

        public double[] GetWeights()
        {
            return this.weights;
        }

        public int i, j;

        public KohonenNeuron()
        {
        }

        public KohonenNeuron(int inputLayerSize)
        {
            weights = new double[inputLayerSize];
        }

        public KohonenNeuron(int inputLayerSize, int i, int j)
        {
            this.i = i;
            this.j = j;
            weights = new double[inputLayerSize];
        }

        /*
         * Assume valores aleatórios para os neurônios
         */
        public void RandomizeWeights(int maximumWeight)
        {
            for(int x = 0; x < weights.Length; x++)
                weights[x] = Util.Randomizer.NextDouble() * maximumWeight;
        }

        /*
         * Calcula distância euclidiana do padrão de entrada
         */
        public double GetEuclideanDistance(DataSetObject pattern)
        {
            double dist = 0;

            if (pattern.GetInputLenght() != weights.Length)
            {
                throw new Exception("Incorrect data format!");
            }
            else
            {
                double[] inputWeights = pattern.GetInput();

                if (weights.Length != inputWeights.Length)
                {
                    throw new Exception("Incorrect data format!");
                }
                else
                {
                    for (int x = 0; x < weights.Length; x++)
                        dist += ((inputWeights[x] - weights[x]) * (inputWeights[x] - weights[x]));
                }
            }
            return dist;
        }

        /*
         * Dado um padrão de entrada e uma variável de aprendizado, atualiza os pesos do neurônio
         */
        public void UpdateWeights(DataSetObject pattern, double learningRate)
        {
            if (pattern.GetInputLenght() != weights.Length)
            {
                throw new Exception("Incorrect data format!");
            }
            else
            {
                double[] inputWeights = pattern.GetInput();

                for (int i = 0; i < inputWeights.Length; i++)
                {
                    weights[i] += learningRate * (inputWeights[i] - weights[i]);
                }
            }
        }

        public override string ToString()
        {
            return "{" + i.ToString() + "," + j.ToString() + "}";
        }
    }
}
