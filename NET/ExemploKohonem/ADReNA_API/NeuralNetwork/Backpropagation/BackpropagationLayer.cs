using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public class BackpropagationLayer
    {
        #region Attributes
        public List<BackpropagationNeuron> neurons;
        #endregion

        #region private Methods
        #endregion

        #region public Methods
        public BackpropagationLayer()
        {
            neurons = new List<BackpropagationNeuron>();
        }

        public BackpropagationLayer(int layerSize)
        {
            neurons = new List<BackpropagationNeuron>();
            for (int x = 0; x < layerSize; x++)
                neurons.Add(new BackpropagationNeuron());
        }

        public BackpropagationLayer(int layerSize, BackpropagationLayer fatherLayer)
        {
            neurons = new List<BackpropagationNeuron>();
            for (int x = 0; x < layerSize; x++)
            {
                BackpropagationNeuron neuron = new BackpropagationNeuron();
                neurons.Add(neuron);

                foreach (BackpropagationNeuron fatherNeuron in fatherLayer.neurons)
                {
                    neuron.listConnection.Add(new BackpropagationConnection(fatherNeuron));
                }
            }
        }
        #endregion
    }
}
