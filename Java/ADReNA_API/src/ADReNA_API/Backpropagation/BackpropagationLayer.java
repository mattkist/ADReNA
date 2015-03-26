package ADReNA_API.Backpropagation;

import java.util.ArrayList;

public class BackpropagationLayer {

    public ArrayList<BackpropagationNeuron> neurons;

    public BackpropagationLayer()
    {
        neurons = new ArrayList<BackpropagationNeuron>();
    }

    public BackpropagationLayer(int layerSize)
    {
        neurons = new ArrayList<BackpropagationNeuron>();
        for (int x = 0; x < layerSize; x++)
            neurons.add(new BackpropagationNeuron());
    }

    public BackpropagationLayer(int layerSize, BackpropagationLayer fatherLayer)
    {
        neurons = new ArrayList<BackpropagationNeuron>();
        for (int x = 0; x < layerSize; x++)
        {
            BackpropagationNeuron neuron = new BackpropagationNeuron();
            neurons.add(neuron);

            for (BackpropagationNeuron fatherNeuron : fatherLayer.neurons)
            {
                neuron.listConnection.add(new BackpropagationConnection(fatherNeuron));
            }
        }
    }

}
