using ADReNA_API.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADReNA.View
{
    /// <summary>
    /// Interaction logic for NewTrainingSet.xaml
    /// </summary>
    public partial class NewTrainingSet : Window
    {
        private int inputNumber, outputNumber;

        public void SetSupervisedLearning(bool supervised)
        {
            if (supervised)
            {
                lblSupervisionedLearn.Content = "Sim";
                PatternDataGrid.ItemsSource = new List<SupervisedPattern>();
            }
            else
            {
                lblSupervisionedLearn.Content = "Não";
                PatternDataGrid.ItemsSource = new List<UnsupervisedPattern>();
            }
        }

        public void SetInputNumber(int input)
        {
            inputNumber = input;
            lblInputNumber.Content = Convert.ToString(input);
        }

        public void SetOutputNumber(int output)
        {
            outputNumber = output;
            if (output != 0)
                lblOutputNumber.Content = Convert.ToString(output);
            else
                lblOutputNumber.Content = "-";
        }

        public NewTrainingSet()
        {
            InitializeComponent();
        }

        public void SetTrainingSet(DataSet ds)
        {
            SetInputNumber(ds.GetInputSize());
            SetOutputNumber(ds.GetOutputSize());

            if (outputNumber == 0) //UnsupervisedPattern
            {
                List<UnsupervisedPattern> list = new List<UnsupervisedPattern>();
                foreach (DataSetObject obj in ds.GetList())
                {
                    string input = "";
                    string inputComma = "";
                    foreach (double inputValue in obj.GetInput())
                    {
                        input = input + inputComma + inputValue;
                        inputComma = ",";
                    }
                    list.Add(new UnsupervisedPattern() { Entrada = input });
                }
                PatternDataGrid.ItemsSource = list;
            }
            else //SupervisedPattern
            {
                List<SupervisedPattern> list = new List<SupervisedPattern>();
                foreach (DataSetObject obj in ds.GetList())
                {
                    string output = "";
                    string outputComma = "";
                    foreach (double outputValue in obj.GetTargetOutput())
                    {
                        output = output + outputComma + outputValue;
                        outputComma = ",";
                    }

                    string input = "";
                    string inputComma = "";
                    foreach (double inputValue in obj.GetInput())
                    {
                        input = input + inputComma + inputValue;
                        inputComma = ",";
                    }
                    list.Add(new SupervisedPattern() { Entrada = input , Saída = output});
                }
                PatternDataGrid.ItemsSource = list;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var patterns = PatternDataGrid.ItemsSource;
            if (patterns is List<SupervisedPattern>)
            {
                List<SupervisedPattern> listPatterns = (List<SupervisedPattern>)patterns;
                if (listPatterns.Count == 0)
                {
                    ShowError1();
                }
                else
                {
                    if (IsOk(listPatterns))
                    {
                        MainWindow.TrainingSet = GetTrainingSet(listPatterns);
                        this.Close();
                    }
                    else
                    {
                        ShowError2();
                    }
                }
            }
            else //is List<UnsupervisedPattern>
            {
                List<UnsupervisedPattern> listPatterns = (List<UnsupervisedPattern>)patterns;
                if (listPatterns.Count == 0)
                {
                    ShowError1();
                }
                else
                {
                    if (IsOk(listPatterns))
                    {
                        MainWindow.TrainingSet = GetTrainingSet(listPatterns);
                        this.Close();
                    }
                    else
                    {
                        ShowError2();
                    }
                }
            }
        }

        private DataSet GetTrainingSet(List<SupervisedPattern> list)
        {
            DataSet ds = new DataSet(inputNumber, outputNumber);
            foreach (SupervisedPattern p in list)
            {
                if (p != null && p.Entrada != null && p.Entrada != "" && p.Saída != null && p.Saída != "")
                {
                    double[] outputValues = new double[outputNumber];
                    int y = 0;
                    string[] outputNeuronsList = p.Saída.Split(',');
                    foreach (string neuron in outputNeuronsList)
                    {
                        outputValues[y] = Convert.ToDouble(neuron.Replace(".",","));
                        y++;
                    }

                    double[] inputValues = new double[inputNumber];
                    int x = 0;
                    string[] neuronsList = p.Entrada.Split(',');
                    foreach (string neuron in neuronsList)
                    {
                        inputValues[x] = Convert.ToDouble(neuron.Replace(".", ","));
                        x++;
                    }

                    ds.Add(new DataSetObject(inputValues, outputValues));
                }
            }
            return ds;
        }

        private DataSet GetTrainingSet(List<UnsupervisedPattern> list)
        {
            DataSet ds = new DataSet(inputNumber);
            foreach (UnsupervisedPattern p in list)
            {
                if (p != null && p.Entrada != null && p.Entrada != "")
                {
                    double[] values = new double[inputNumber];
                    int x = 0;
                    string[] neuronsList = p.Entrada.Split(',');
                    foreach (string neuron in neuronsList)
                    {
                        values[x] = Convert.ToDouble(neuron.Replace(".", ","));
                        x++;
                    }
                    ds.Add(new DataSetObject(values));
                }
            }
            return ds;
        }
        

        private bool IsOk(List<UnsupervisedPattern> list)
        {
            foreach (UnsupervisedPattern p in list)
            {
                if (p != null && p.Entrada != null && p.Entrada != "")
                {
                    string[] neuronsList = p.Entrada.Split(',');
                    if (neuronsList.Count() != inputNumber)
                        return false;
                    foreach (string neuron in neuronsList)
                    {
                        try
                        {
                            Convert.ToDouble(neuron.Replace(".", ","));
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool IsOk(List<SupervisedPattern> list)
        {
            foreach (SupervisedPattern p in list)
            {
                if (p != null && p.Entrada != null && p.Entrada != "" && p.Saída != null && p.Saída != "")
                {
                    string[] outputNeuronsList = p.Saída.Split(',');
                    if (outputNeuronsList.Count() != outputNumber)
                        return false;
                    foreach (string neuron in outputNeuronsList)
                    {
                        try
                        {
                            Convert.ToDouble(neuron.Replace(".", ","));
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    string[] inputNeuronsList = p.Entrada.Split(',');
                    if (inputNeuronsList.Count() != inputNumber)
                        return false;
                    foreach (string neuron in inputNeuronsList)
                    {
                        try
                        {
                            Convert.ToDouble(neuron.Replace(".", ","));
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                if (p != null && p.Entrada != null && p.Entrada != "" && (p.Saída == null || p.Saída == "")
                    || p != null && p.Saída != null && p.Saída != "" && (p.Entrada == null || p.Entrada == ""))
                {
                    return false;
                }
            }
            return true;
        }
        

        private void ShowError1()
        {
            MessageBox.Show("Nenhum padrão de treinamento informado.");
        }

        private void ShowError2()
        {
            MessageBox.Show("Existe incosistência no formato dos padrões de treinamento");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public class SupervisedPattern
        {
            public string Entrada { get; set; }
            public string Saída { get; set; }
        }

        public class UnsupervisedPattern
        {
            public string Entrada { get; set; }
        }

    }
}
