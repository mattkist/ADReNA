package ADReNA_API.Data;

import java.util.ArrayList;

public class DataSet {

	public ArrayList<DataSetObject> data;
	public int inputSize, outputSize;

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

    private void BuildNewDataSet(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        data = new ArrayList<DataSetObject>();
    }


    /*
     * Adiciona um dado ao conjunto de dados
     * Retorna um erro se o dado tiver um formato incorreto
     */
    public void Add(DataSetObject obj) throws Exception
    {
        if (obj.GetInputLenght() == inputSize && obj.GetTargetOutputLenght() == outputSize)
            data.add(obj);
        else
            throw new Exception("Incorrect data format!");
    }

    /*
     * Retorna a lista de padrões
     */
    public ArrayList<DataSetObject> GetList()
    {
        return data;
    }

    /*
     * Retorna a quantidade de padrões
     */
    public int Length()
    {
        return data.size();
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

}
