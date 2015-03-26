using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADReNA_API.Data
{
    public class DataSet
    {
        #region Attributes
        public List<DataSetObject> data;
        public int inputSize, outputSize;
        #endregion

        #region Builders
        /*
         * Cria uma instância de um conjunto de dados nulo
         */
        public DataSet()
        {
            BuildNewDataSet(0, 0);
        }

        /*
         * Cria uma instância de um novo conjunto de dados
         */
        public DataSet(int inputSize)
        {
            BuildNewDataSet(inputSize, 0);
        }

        /*
         * Cria uma instância de um novo conjunto de dados
         */
        public DataSet(int inputSize, int outputSize)
        {
            BuildNewDataSet(inputSize, outputSize);
        }
        #endregion

        #region private Methods
        private void BuildNewDataSet(int inputSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            data = new List<DataSetObject>();
        }
        #endregion

        #region public Methods
        /*
         * Adiciona um dado ao conjunto de dados
         * Retorna um erro se o dado tiver um formato incorreto
         */
        public void Add(DataSetObject obj)
        {
            if (obj.GetInputLenght() == inputSize && obj.GetTargetOutputLenght() == outputSize)
                data.Add(obj);
            else
                throw new Exception("Incorrect data format!");
        }

        /*
         * Retorna a lista de padrões
         */
        public List<DataSetObject> GetList()
        {
            return data;
        }

        /*
         * Retorna a quantidade de padrões
         */
        public int Length()
        {
            return data.Count;
        }

        /*
         * Retorna o tamanho dos padroes de entrada
         */
        public int GetInputSize()
        {
            return this.inputSize;
        }

        /*
         * Retorna o tamanho dos padroes de saida
         */
        public int GetOutputSize()
        {
            return this.outputSize;
        }

        #endregion

    }
}
