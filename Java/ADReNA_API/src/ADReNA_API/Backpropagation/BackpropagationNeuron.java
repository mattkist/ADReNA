package ADReNA_API.Backpropagation;

import java.util.ArrayList;

public class BackpropagationNeuron {

     public double valuePattern; //{0 or 1}
     public ArrayList<BackpropagationConnection> listConnection;
     public double valueError;

     public BackpropagationNeuron()
     {
         valuePattern = 0;
         listConnection = new ArrayList<BackpropagationConnection>();
         valueError = 0;
     }

     public BackpropagationNeuron(int valuePattern)
     {
         SetValuePattern(valuePattern);
         listConnection = new ArrayList<BackpropagationConnection>();
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
     
     public String ToString()
     {
         return String.valueOf(valuePattern);
     }
}
