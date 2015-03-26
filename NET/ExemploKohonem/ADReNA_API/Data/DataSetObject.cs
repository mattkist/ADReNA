using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.Data
{
    public class DataSetObject
    {
        #region Attributes
        public double[] input, targetOutput;
        #endregion

        #region Builders
        /*
         * Cria uma instância de um um novo dado de entrada nulo
         */
        public DataSetObject()
        {
            BuildNewDataSetObject(new double[0], new double[0]);
        }

        /*
         * Cria uma instância de um um novo dado de entrada
         */
        public DataSetObject(double[] input, double[] targetOutput)
        {
            BuildNewDataSetObject(input, targetOutput);
        }

        /*
         * Cria uma instância de um novo dado de entrada
         */
        public DataSetObject(double[] input)
        {
            BuildNewDataSetObject(input, new double[0]);
        }
        #endregion

        #region private Methods
        private void BuildNewDataSetObject(double[] input, double[] targetOutput)
        {
            this.SetInput(input);
            this.SetTargetOutput(targetOutput);
        }
        #endregion

        #region public Methods
        /*
         * Retorna o padrão de entrada
         */
        public double[] GetInput()
        {
            return input;
        }

        /*
         * Retorna o padrão desejado de saída
         */
        public double[] GetTargetOutput()
        {
            return targetOutput;
        }

        /*
         * Insere o padrão de entrada
         */
        public void SetInput(double[] input)
        {
            this.input = input;
        }

        /*
         * Insere o padrão desejado de saída
         */
        public void SetTargetOutput(double[] targetOutput)
        {
            this.targetOutput = targetOutput;
        }

        /*
         * Retorna o tamanho do padrão de entrada
         */
        public int GetInputLenght()
        {
            return input.Length;
        }

        /*
         * Retorna o tamanho do padrão desejado de saída
         */
        public int GetTargetOutputLenght()
        {
            return targetOutput.Length;
        }
        #endregion
    }
}
