package ADReNA_API.NeuralNetwork;

import java.util.ArrayList;

import ADReNA_API.Backpropagation.*;
import ADReNA_API.Data.*;

public class Backpropagation implements INeuralNetwork {

    public double ETA;
    public double Error;
    public int maxIterationNumber, iterationNumber;
    public int inputLayerSize, outputLayerSize;
    public int[] hiddenLayerSizes;
    public ArrayList<BackpropagationLayer> layers;

    private void ApplyPattern(double[] pattern)
    {
        BackpropagationLayer inputLayer = layers.get(0);
        for (int x = 0; x < inputLayer.neurons.size(); x++)
            inputLayer.neurons.get(x).SetValuePattern(pattern[x]);
    }

    private void DoPropagation()
    {
        //CALCULA A SOMA DE CADA NEURÔNIO DE CADA CAMADA, EXCETO A DE ENTRADA.
        for (int x = 1; x < layers.size(); x++)
        {
            CalculateSum(layers.get(x));
        }
    }

    private void CalculateSum(BackpropagationLayer layer)
    {
        for (int x = 0; x < layer.neurons.size(); x++ )
        {
            BackpropagationNeuron neuron = layer.neurons.get(x);

            //CALCULA A SOMA PARA CADA NEURONIO DA CAMADA.
            //PARA ISSO, APURA CADA RELAÇÃO DE SUA CONEXAO (BACKWARD).
            double sum = 0;
            for (int y = 0; y < neuron.listConnection.size(); y++)
            {
                BackpropagationConnection connection = neuron.listConnection.get(y);
                // Formula: (E x*w)
                sum += (connection.neuron.GetValuePattern() * connection.valueWeight);
            }

            //APLICA A FUNÇÃO DE TRANSFERÊNCIA.
            neuron.SetValuePattern(TransferFunction(sum));
        }
    }

    private double TransferFunction(double value)
    {
        //Formula: (1 / 1 + e -input)
        return (1 / (1 + (Math.pow(Math.E, -value))));
    }

    private double CalculateStopError(double[] outPattern)
    {
        double result = 0;
        BackpropagationLayer outLayer = layers.get(layers.size()-1);

        for (int x = 0; x < outPattern.length; x++)
        {
        	//double value = outPattern[x];
            //Formula: erro = 1/2 E (valor desejado - valor obtido)2
            result += (outPattern[x] - outLayer.neurons.get(x).GetValuePattern()) * (outPattern[x] - outLayer.neurons.get(x).GetValuePattern());
        }

        return result/2;
    }

    private void CalculateErrorOut(double[] outPattern)
    {
        BackpropagationLayer outLayer = layers.get(layers.size() - 1);

        for (int x = 0; x < outPattern.length; x++)
        {
            BackpropagationNeuron neuronOut = outLayer.neurons.get(x);
            //Formula: Erro_saída[j] = (valor_desejado – valor_obtido)* valor_obtido * (1 – (valor_obtido))
            neuronOut.valueError = (outPattern[x] -  neuronOut.GetValuePattern()) * neuronOut.GetValuePattern() * (1 - neuronOut.GetValuePattern());
        }
    }

    
    private void DoBackPropagation(double[] outPattern)
    {
        //CALCULA ERRO NA CAMADA DE SAÍDA!
        CalculateErrorOut(outPattern);

        //Calcula o erro nas camadas intermediárias
        //ErrorA = Output A (1 - Output A)(ErrorB WAB + ErrorC WAC) 
        //VEM PROPAGANDO DE TRÁS PARA FRENTE. COMEÇA NA PENÚLTIMA CAMADA E VAI ATÉ A PRIMEIRA
        for (int x = layers.size() - 2; x >= 0; x--)
        {
            BackpropagationLayer layerL = layers.get(x);
            BackpropagationLayer layerR = layers.get(x + 1);

            //PARA CADA NEURÔNIO DA CAMADA DA ESQUERDA
            for(int y = 0; y < layerL.neurons.size(); y++)
            {
                BackpropagationNeuron neuronL = layerL.neurons.get(y);
                neuronL.valueError = 0;

                //CALCULA O ERRO
                double sum = 0;
                for (int z = 0; z < layerR.neurons.size(); z++)
                {
                    //para cada neurônio da camada da direita, pega ele e encontra a conexão entre os 2
                    BackpropagationNeuron neuronR = layerR.neurons.get(z);
                    for (int c = 0; c < neuronR.listConnection.size(); c++)
                    {
                        BackpropagationConnection connection = neuronR.listConnection.get(c);
                        if (connection.neuron == neuronL)
                        {
                            //acumula a soma do  (erro do neuronio da direita * peso da ligacao)
                            sum += neuronR.valueError * connection.valueWeight;
                            //e
                            //atualiza peso da conexão
                            //WAB = WAB + (ErrorB x OutputA) 
                            connection.valueWeight = connection.valueWeight + (neuronR.valueError * neuronL.valuePattern);
                        }
                    }
                }
                //ErrorA = Output A (1 - Output A)(ErrorB WAB + ErrorC WAC) 
                neuronL.valueError = neuronL.valuePattern * (1 - neuronL.valuePattern) * sum;
            }
        }
    }

    private double[] GetDoubleArrayOutput()
    {
        BackpropagationLayer outLayer = layers.get(layers.size() - 1);
        double[] result = new double[outLayer.neurons.size()];

        for (int x = 0; x < outLayer.neurons.size(); x++)
        {
            BackpropagationNeuron neuron = outLayer.neurons.get(x);
            result[x] = neuron.GetValuePattern();
        }
        return result;
    }
    
    private void BuildBackpropagation(int inputLayerSize, int outputLayerSize, int[] hiddenLayerSizes)
    {
    	this.ETA = 0.3; 
        this.Error = 0.005; 
        this.maxIterationNumber = 10000; 

        this.inputLayerSize = inputLayerSize;
        this.outputLayerSize = outputLayerSize;
        this.hiddenLayerSizes = hiddenLayerSizes;
        layers = new ArrayList<BackpropagationLayer>();
        BackpropagationLayer inputLayer = new BackpropagationLayer(inputLayerSize);
        layers.add(inputLayer);
        BackpropagationLayer last = inputLayer;
        for (int layerSize : hiddenLayerSizes)
        {
            BackpropagationLayer hiddenLayer = new BackpropagationLayer(layerSize, last);
            layers.add(hiddenLayer);
            last = hiddenLayer;
        }

        layers.add(new BackpropagationLayer(outputLayerSize, last));
    }

    /*
     * Método de aprendizado da Rede
     */
    public void Learn(DataSet trainingSet) throws Exception
    {
        if (trainingSet.GetOutputSize() !=  this.outputLayerSize || trainingSet.GetInputSize() != this.inputLayerSize)
            throw new Exception("Incorrect data format!");
        else
        {
            boolean learned = false;
            ArrayList<DataSetObject> patterns = trainingSet.GetList();
            for (int it = 1; learned == false && it <= maxIterationNumber; it++)
            {
                //VAI APRESENTANDO OS PADRÕES, DE FORMA ALTERNADA, ATÉ QUE TODOS ATINJAM O ERRO REQUERIDO.
                int allPatternOK = 0;
                for (int p = 0; p < patterns.size(); p++)
                {
                    DataSetObject pattern = patterns.get(p);

                    //INJETA O PADRÃO NA ENTRADA DA REDE.
                    ApplyPattern(pattern.GetInput());

                    //PROPAGA
                    DoPropagation();

                    if (CalculateStopError(pattern.GetTargetOutput()) > this.Error)
                        DoBackPropagation(pattern.GetTargetOutput());
                    else
                        allPatternOK++;

                    if (allPatternOK == patterns.size())
                        learned = true;
                    
                    iterationNumber = it;
                }
            }
        }
    }

    public double[] Recognize(double[] input) throws Exception
    {
        if (input.length != this.inputLayerSize)
            throw new Exception("Incorrect data format!");
        else
        {
            //INJETA O PADRÃO NA ENTRADA DA REDE.
            ApplyPattern(input);

            //PROPAGA O SINAL PELA REDE E VE O QUE SAI NO RESULTADO.
            DoPropagation();
            return GetDoubleArrayOutput();
        }
    }

    public Backpropagation(int inputLayerSize, int outputLayerSize)
    {
        int[] hiddenLayerSizes = { ((int)Math.round((double)(inputLayerSize / 4)) > 2 ? (int)Math.round((double)(inputLayerSize / 4)) : 2) };

        BuildBackpropagation(inputLayerSize, outputLayerSize, hiddenLayerSizes);
    }

    public Backpropagation(int inputLayerSize, int outputLayerSize, int[] hiddenLayerSizes)
    {
        BuildBackpropagation(inputLayerSize, outputLayerSize, hiddenLayerSizes);
    }
    
    /*
     * Retorna a quantidade de camadas ocultas e quantos neurônios em cada
     */
     public int[] GetHiddenLayerSizes()
     {
         return hiddenLayerSizes;
     }

     /*
      * Retorna todas as camadas da rede
      */
     public ArrayList<BackpropagationLayer> GetLayers()
     {
         return layers;
     }

     /*
      * Define todas as camadas da rede
      */
     public void SetLayers(ArrayList<BackpropagationLayer> layers) throws Exception
     {
         boolean incorrectDataFormat = false;

         if (layers.size() == this.layers.size())
         {
             for (int x = 0; x < layers.size(); x++)
                 if (layers.get(x).neurons.size() != this.layers.get(x).neurons.size())
                     incorrectDataFormat = true;
         }
         else
             incorrectDataFormat = true;

         if (incorrectDataFormat)
             throw new Exception("Incorrect data format!");
         else
         {
             this.layers = layers;
             //Redefinindo conexões
             BackpropagationLayer lastLayer = null;
             for(BackpropagationLayer layer : layers)
             {
                 for (BackpropagationNeuron neuron : layer.neurons)
                 {
                     int connNumber = 0;
                     for (BackpropagationConnection conn : neuron.listConnection)
                     {
                         conn.neuron = lastLayer.neurons.get(connNumber);
                         connNumber++;
                     }
                 }
                 lastLayer = layer;
             }
         }
     }
     
    /*
     * Define o número máximo de iterações
     */
    public void SetMaxIterationNumber(int number)
    {
        maxIterationNumber = number;
    }

    /*
     * Retorna o número máximo de iterações
     */
    public int GetMaxIterationNumber()
    {
        return maxIterationNumber;
    }
    
    /*
     * Retorna o número de iterações que foram necessárias para a rede convergir
     */
     public int GetIterationNumber()
     {
         return iterationNumber;
     }

    /*
     * Define o fator de aprendizagem
     */
    public void SetLearningRate(double rate)
    {
        ETA = rate;
    }

    /*
     * Retorna o fator de aprendizagem
     */
    public double GetLearningRate()
    {
        return ETA;
    }

    /*
     * Define o fator de erro
     */
    public void SetErrorRate(double rate)
    {
        Error = rate;
    }

    /*
     * Retorna o fator de erro
     */
    public double GetErrorRate()
    {
        return Error;
    }
    
    /*
     * Retorna o tamanho da camada de entrada
     */
    public int GetInputLayerSize()
    {
        return inputLayerSize;
    }

    /*
     * Retorna o tamanho da camada de saída
     */
    public int GetOutputLayerSize()
    {
        return outputLayerSize;
    }
}
