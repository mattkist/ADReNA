package ADReNA_API.Util;

import java.io.PrintWriter;

import com.google.gson.Gson;

import ADReNA_API.NeuralNetwork.INeuralNetwork;
import ADReNA_API.NeuralNetwork.Kohonen;
import ADReNA_API.NeuralNetwork.Backpropagation;

public class Export {
	/*
     * Serializa um conjunto de padrões para treinamento
     */
     public static void DataSet(ADReNA_API.Data.DataSet set, String file) throws Exception
     {
    	 Gson gson = new Gson();
    	 String json = gson.toJson(set);
    	 WriteFile(json, file);
     }

     /*
     * Serializa a base de conhecimento de uma RNA em um arquivo
     */
     public static void KnowledgeBase(INeuralNetwork net, String file) throws Exception
     {
    	 Gson gson = new Gson();
    	 String json = "";
    	 
    	 if(net instanceof Kohonen)
    	 {
    		 json = gson.toJson(((Kohonen)net).GetCompetitiveLayer());
    	 }
    	 else //Backpropagation
    	 {
    		 json = gson.toJson(((Backpropagation)net).GetLayers());
    	 }
    	 
    	 WriteFile(json, file);
     }

     /*
      * Serializa a estrutura da rede de uma RNA em um arquivo
      */
     public static void NeuralNetworkStructure(INeuralNetwork net, String file) throws Exception
     {
    	 Gson gson = new Gson();
    	 String json = "";
    	 CommonStructure structure = new CommonStructure();
    	 
    	 if(net instanceof Kohonen)
    	 {
             structure.type = ExportImportCommon.AnnType.Kohonen;
             structure.inputLayerSize =  ((Kohonen)net).GetInputLayerSize();
             structure.competitiveNeuronLength = ((Kohonen)net).GetCompetitiveLayerLength();
             structure.maximumWeightRange = ((Kohonen)net).GetMaximumWeightRange();
             structure.iterationNumber = ((Kohonen)net).GetIterationNumber();
             structure.learningRate = ((Kohonen)net).GetLearningRate();
             structure.neighborhoodRadius = ((Kohonen)net).GetNeighborhoodRadius();
             
             json = gson.toJson(structure);
    	 }
    	 else //Backpropagation
    	 {
    		 structure.type = ExportImportCommon.AnnType.Backpropagation;
             structure.inputLayerSize = ((Backpropagation)net).GetInputLayerSize();
             structure.outputLayerSize = ((Backpropagation)net).GetOutputLayerSize();
             int[] ihlayers = ((Backpropagation)net).GetHiddenLayerSizes();
             Integer[] Ihlayers = new Integer[ihlayers.length];
             for(int x = 0; x < ihlayers.length; x++)
    		 {
            	 Ihlayers[x] = ihlayers[x];
    		 }
             structure.hiddenLayerSizes = Ihlayers;
             structure.error = ((Backpropagation)net).GetErrorRate();
             structure.iterationNumber = ((Backpropagation)net).GetMaxIterationNumber();
             structure.learningRate = ((Backpropagation)net).GetLearningRate();
    		 
    		 json = gson.toJson(structure);
    	 }
    	 
    	 WriteFile(json, file);
     }

     private static void WriteFile(String json, String file) throws Exception
     {
    	 PrintWriter writer = new PrintWriter(file, "UTF-8");
    	 writer.print(json);
    	 writer.close();
     }
}
