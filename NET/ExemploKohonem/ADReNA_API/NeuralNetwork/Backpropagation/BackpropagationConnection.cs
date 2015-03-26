using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public class BackpropagationConnection
    {
        #region Attributes
        public BackpropagationNeuron neuron;
        public double valueWeight; //{peso da ligação}
        public double deltaWeight;
        #endregion

        #region private Methods
        #endregion

        #region public Methods
        public BackpropagationConnection(BackpropagationNeuron neuron)
        {
            this.neuron = neuron;
            this.valueWeight = (Util.Randomizer.NextDouble(1)-0.5); //RANDON VALUE BETWEEN [-0.5 AND +0.5]
            this.deltaWeight = 0;
        }
        #endregion
    }
}
