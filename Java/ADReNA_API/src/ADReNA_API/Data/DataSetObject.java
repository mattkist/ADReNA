package ADReNA_API.Data;

public class DataSetObject {

	public double[] input, targetOutput;
	
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
    
    private void BuildNewDataSetObject(double[] input, double[] targetOutput)
    {
        this.SetInput(input);
        this.SetTargetOutput(targetOutput);
    }
    
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
        return input.length;
    }

    /*
     * Retorna o tamanho do padrão desejado de saída
     */
    public int GetTargetOutputLenght()
    {
        return targetOutput.length;
    }
}
