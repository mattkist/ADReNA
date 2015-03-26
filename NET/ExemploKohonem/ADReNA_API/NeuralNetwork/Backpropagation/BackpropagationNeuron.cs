using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.NeuralNetwork
{
    public class BackpropagationNeuron
    {
        #region Attributes
        public double valuePattern; //{0 or 1}
        public List<BackpropagationConnection> listConnection;
        public double valueError;
        #endregion

        #region private Methods
        #endregion

        #region public Methods
        public BackpropagationNeuron()
        {
            valuePattern = 0;
            listConnection = new List<BackpropagationConnection>();
            valueError = 0;
        }

        public BackpropagationNeuron(int valuePattern)
        {
            SetValuePattern(valuePattern);
            listConnection = new List<BackpropagationConnection>();
            valueError = 0;
        }

        public void SetValuePattern(double value)
        {
            this.valuePattern = value;    
        }

        public double GetValuePattern()
        {
            return this.valuePattern;
        }
        #endregion

        public override string ToString()
        {
            return valuePattern.ToString();
        }
    }
}
