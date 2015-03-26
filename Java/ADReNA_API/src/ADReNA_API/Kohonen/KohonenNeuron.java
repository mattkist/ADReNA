package ADReNA_API.Kohonen;

import ADReNA_API.Data.*;
import ADReNA_API.Util.*;

public class KohonenNeuron {

	public double[] weights;

    public double[] GetWeights()
    {
        return this.weights;
    }
    
    public int i, j;

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
        for(int x = 0; x < weights.length; x++)
            weights[x] = Randomizer.NextDouble() * maximumWeight;
    }

    /*
     * Calcula distância euclidiana do padrão de entrada
     */
    public double GetEuclideanDistance(DataSetObject pattern) throws Exception
    {
        double dist = 0;

        if (pattern.GetInputLenght() != weights.length)
        {
            throw new Exception("Incorrect data format!");
        }
        else
        {
            double[] inputWeights = pattern.GetInput();

            if (weights.length != inputWeights.length)
            {
                throw new Exception("Incorrect data format!");
            }
            else
            {
                for (int x = 0; x < weights.length; x++)
                    dist += ((inputWeights[x] - weights[x]) * (inputWeights[x] - weights[x]));
            }
        }
        return dist;
    }

    /*
     * Dado um padrão de entrada e uma variável de aprendizado, atualiza os pesos do neurônio
     */
    public void UpdateWeights(DataSetObject pattern, double learningRate) throws Exception
    {
        if (pattern.GetInputLenght() != weights.length)
        {
            throw new Exception("Incorrect data format!");
        }
        else
        {
            double[] inputWeights = pattern.GetInput();

            for (int i = 0; i < inputWeights.length; i++)
            {
                weights[i] += learningRate * (inputWeights[i] - weights[i]);
            }
        }
    }
    
    public String ToString()
    {
        return "{" + String.valueOf(i)+ "," + String.valueOf(j) + "}";
    }
}
