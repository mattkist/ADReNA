package ADReNA_API.Backpropagation;

import ADReNA_API.Util.Randomizer;

public class BackpropagationConnection {

    public BackpropagationNeuron neuron;
    public double valueWeight; //{peso da ligação}
    public double deltaWeight;

    public BackpropagationConnection(BackpropagationNeuron neuron)
    {
        this.neuron = neuron;
        this.valueWeight = (Randomizer.NextDouble(1)-0.5); //RANDON VALUE BETWEEN [-0.5 AND +0.5]
        this.deltaWeight = 0;
    }

}
