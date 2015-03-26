using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ADReNA_API.NeuralNetwork;
using ADReNA_API.Data;
using System.ComponentModel;
using System.Windows.Threading;
using QuickGraph;
using GraphSharp.Controls;

namespace ADReNA.View
{

    public enum MainWindowState { NoANN, RawANN, LoadedTrainingSet, TrainingANN, TrainedANN };

    public partial class MainWindow : Window
    {
        public static INeuralNetwork RNA;

        public static DataSet TrainingSet;

        private System.ComponentModel.BackgroundWorker backgroundWorker;

        private string LastSaved = null;

        UIElement graphUI;

        private MainWindowState state;
        public MainWindowState State
        {
            get
            {
                return state;
            }
            set
            {
                switch (value)
                {
                    case MainWindowState.NoANN:
                        btnSave.IsEnabled = false;
                        btnSaveAs.IsEnabled = false;
                        btnMenuTrain.IsEnabled = false;
                        btnSaveTrainingSet.IsEnabled = false;
                        btnNewTrainingSet.IsEnabled = false;
                        btnLoadTrainingSet.IsEnabled = false;
                        btnSaveLearn.IsEnabled = false;
                        btnLoadLearn.IsEnabled = false;
                        btnNewSet.IsEnabled = false;
                        btnLoadSet.IsEnabled = false;
                        btnEditSet.IsEnabled = false;
                        btnTrain.IsEnabled = false;
                        btnRecognize.IsEnabled = false;
                        txtInputPattern.IsEnabled = false;
                        txtLearningRate.IsEnabled = true;
                        txtNeighborhoodRadius.IsEnabled = true;
                        txtErrorRate.IsEnabled = true;
                        txtIterationNumber.IsEnabled = true;
                        menu.IsEnabled = true;
                        txtResult.Text = "";
                        labelIterationCount.Visibility = System.Windows.Visibility.Collapsed;
                        ShowOrHidePanels(value);
                        ClearGraph();
                        break;
                    case MainWindowState.RawANN:
                        btnSave.IsEnabled = true;
                        btnSaveAs.IsEnabled = true;
                        btnMenuTrain.IsEnabled = false;
                        btnSaveTrainingSet.IsEnabled = false;
                        btnNewTrainingSet.IsEnabled = true;
                        btnLoadTrainingSet.IsEnabled = true;
                        btnSaveLearn.IsEnabled = false;
                        btnLoadLearn.IsEnabled = true;
                        btnNewSet.IsEnabled = true;
                        btnLoadSet.IsEnabled = true;
                        btnEditSet.IsEnabled = false;
                        btnTrain.IsEnabled = false;
                        btnRecognize.IsEnabled = false;
                        txtInputPattern.IsEnabled = false;
                        txtLearningRate.IsEnabled = true;
                        txtNeighborhoodRadius.IsEnabled = true;
                        txtErrorRate.IsEnabled = true;
                        txtIterationNumber.IsEnabled = true;
                        menu.IsEnabled = true;
                        txtResult.Text = "";
                        labelIterationCount.Visibility = System.Windows.Visibility.Collapsed;
                        ShowOrHidePanels(value);
                        CreateGraphFromANN();
                        break;
                    case MainWindowState.LoadedTrainingSet:
                        btnSave.IsEnabled = true;
                        btnSaveAs.IsEnabled = true;
                        btnMenuTrain.IsEnabled = true;
                        btnSaveTrainingSet.IsEnabled = true;
                        btnNewTrainingSet.IsEnabled = true;
                        btnLoadTrainingSet.IsEnabled = true;
                        btnSaveLearn.IsEnabled = false;
                        btnLoadLearn.IsEnabled = true;
                        btnNewSet.IsEnabled = true;
                        btnLoadSet.IsEnabled = true;
                        btnEditSet.IsEnabled = true;
                        btnTrain.IsEnabled = true;
                        btnRecognize.IsEnabled = false;
                        txtInputPattern.IsEnabled = false;
                        txtLearningRate.IsEnabled = true;
                        txtNeighborhoodRadius.IsEnabled = true;
                        txtErrorRate.IsEnabled = true;
                        txtIterationNumber.IsEnabled = true;
                        menu.IsEnabled = true;
                        txtResult.Text = "";
                        ShowOrHidePanels(value);
                        break;
                    case MainWindowState.TrainingANN:
                        btnSave.IsEnabled = false;
                        btnSaveAs.IsEnabled = false;
                        btnMenuTrain.IsEnabled = false;
                        btnSaveTrainingSet.IsEnabled = false;
                        btnNewTrainingSet.IsEnabled = false;
                        btnLoadTrainingSet.IsEnabled = false;
                        btnSaveLearn.IsEnabled = false;
                        btnLoadLearn.IsEnabled = false;
                        btnNewSet.IsEnabled = false;
                        btnLoadSet.IsEnabled = false;
                        btnEditSet.IsEnabled = false;
                        btnTrain.IsEnabled = false;
                        btnRecognize.IsEnabled = false;
                        txtInputPattern.IsEnabled = false;
                        txtLearningRate.IsEnabled = false;
                        txtNeighborhoodRadius.IsEnabled = false;
                        txtErrorRate.IsEnabled = false;
                        txtIterationNumber.IsEnabled = false;
                        menu.IsEnabled = false;
                        txtResult.Text = "";
                        labelIterationCount.Visibility = System.Windows.Visibility.Collapsed;
                        ShowOrHidePanels(value);
                        ClearGraph();
                        break;
                    case MainWindowState.TrainedANN:
                        btnSave.IsEnabled = true;
                        btnSaveAs.IsEnabled = true;
                        btnMenuTrain.IsEnabled = true;
                        btnSaveTrainingSet.IsEnabled = true;
                        btnNewTrainingSet.IsEnabled = true;
                        btnLoadTrainingSet.IsEnabled = true;
                        btnSaveLearn.IsEnabled = true;
                        btnLoadLearn.IsEnabled = true;
                        btnNewSet.IsEnabled = true;
                        btnLoadSet.IsEnabled = true;
                        btnEditSet.IsEnabled = true;
                        btnTrain.IsEnabled = true;
                        btnRecognize.IsEnabled = true;
                        txtInputPattern.IsEnabled = true;
                        txtLearningRate.IsEnabled = true;
                        txtNeighborhoodRadius.IsEnabled = true;
                        txtErrorRate.IsEnabled = true;
                        txtIterationNumber.IsEnabled = true;
                        menu.IsEnabled = true;
                        ShowOrHidePanels(value);
                        DrawGraph();
                        break;
                }

                state = value;
            }

        }

        public MainWindow()
        {
            InitializeComponent();
            State = MainWindowState.NoANN;
            InitializeBackgroundWorker();
        }

        #region private
        private bool ValidateDouble(string txt)
        {
            try
            {
                Convert.ToDouble(txt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateInt(string txt)
        {
            try
            {
                Convert.ToInt32(txt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DrawGraph()
        {
            for (int x = 0; x < 2; x++)
            {
                ClearGraph();
                graphUI = MainPanelBorder.Child;
                MainPanelBorder.Child = null;
                CreateGraphFromANN();
                MainPanelBorder.Child = graphUI;
            }
        }

        private void CreateGraphFromANN()
        {
            BidirectionalGraph<object, IEdge<object>> g = new BidirectionalGraph<object, IEdge<object>>();

            if (RNA is Kohonen)
            {
                int inputN = ((Kohonen)RNA).GetInputLayerSize();
                for(int x = 0; x < inputN ; x++)
                {
                    g.AddVertex("Input-" + (x+1).ToString());
                }

                KohonenNeuron[,] layer = ((Kohonen)RNA).GetCompetitiveLayer();
                for (int i = 0; i < ((Kohonen)RNA).GetCompetitiveLayerLength(); i++)
                {
                    for (int j = 0; j < ((Kohonen)RNA).GetCompetitiveLayerLength(); j++)
                    {
                        KohonenNeuron neuron = layer[i, j];
                        g.AddVertex(neuron);
                        for (int x2 = 0; x2 < inputN; x2++)
                        {
                            g.AddEdge(new Edge<object>("Input-" + (x2 + 1).ToString(), neuron));
                        }
                    }
                }

            }
            else //is Backpropagation
            {
                List<BackpropagationLayer> layers = ((Backpropagation)RNA).GetLayers();
                foreach (BackpropagationLayer layer in layers)
                {
                    foreach(BackpropagationNeuron neuron in layer.neurons)
                    {
                        g.AddVertex(neuron);
                    }
                }
                foreach (BackpropagationLayer layer in layers)
                {
                    foreach (BackpropagationNeuron neuron in layer.neurons)
                    {
                        foreach (BackpropagationConnection conn in neuron.listConnection)
                        {
                            g.AddEdge(new Edge<object>(conn.neuron, neuron));
                        }
                    }
                }
            }

            graphLayout.Graph = g;
            graphLayout.LayoutMode = LayoutMode.Automatic;
            graphLayout.LayoutAlgorithmType = "Tree";
        }

        private void ClearRNAOutputs()
        {
            if (RNA is Backpropagation)
            {
                List<BackpropagationLayer> layers = ((Backpropagation)RNA).GetLayers();
                foreach (BackpropagationLayer layer in layers)
                    foreach (BackpropagationNeuron neuron in layer.neurons)
                        neuron.valuePattern = 0;
                DrawGraph();
            }
        }

        private void ClearGraph()
        {
            graphLayout.Graph = null;
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RNA.Learn(TrainingSet);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowLoading(false);

            if (e.Error != null)
            {
                State = MainWindowState.LoadedTrainingSet;
                MessageBox.Show("O seguinte erro ocorreu: " + e.Error.Message);
            }
            else
            {
                State = MainWindowState.TrainedANN;
                ClearRNAOutputs();

                if (RNA is Kohonen)
                {
                    MessageBox.Show("RNA Treinada com Sucesso!");
                }
                else //is Backpropagation
                {
                    labelIterationCount.Visibility = System.Windows.Visibility.Visible;
                    labelIterationCount.Content = "Número de iterações feitas: " + ((Backpropagation)RNA).GetIterationNumber().ToString();
                    MessageBox.Show("RNA Treinada com Sucesso em " + ((Backpropagation)RNA).GetIterationNumber().ToString() + " iterações!");
                }
            }


        }

        private void ShowLoading(bool load)
        {
            if (load)
            {
                LoadingControl.Control.LoadingAnimation loading = new LoadingControl.Control.LoadingAnimation();
                loading.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                loading.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                graphUI = MainPanelBorder.Child;
                MainPanelBorder.Child = loading;
            }
            else
            {
                MainPanelBorder.Child = graphUI;
            }
        }

        private void ShowOrHidePanels(MainWindowState _state)
        {
            switch (_state)
            {
                case MainWindowState.NoANN:
                    MainPanel.Visibility = Visibility.Collapsed;
                    UpperPanel.Visibility = Visibility.Collapsed;
                    LowerPanel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    MainPanel.Visibility = Visibility.Visible;
                    UpperPanel.Visibility = Visibility.Visible;
                    LowerPanel.Visibility = Visibility.Visible;
                    if (RNA is Kohonen)
                    {
                        this.Title = "ADReNA - Rede Neural de Kohonen";
                        labelNetworkType.Content = "Kohonen";
                    }
                    else // is backpropagation
                    {
                        this.Title = "ADReNA - Rede Neural de Backpropagation";
                        labelNetworkType.Content = "Backpropagation";
                    }
                    break;
            }
        }

        private void NewRNA()
        {
            State = MainWindowState.RawANN;
            TrainingSet = null;
            LastSaved = null;

            if (RNA is Kohonen)
            {
                lblErrorRate.Visibility = Visibility.Collapsed;
                txtErrorRate.Visibility = Visibility.Collapsed;
                lblNeighborhoodRadius.Visibility = Visibility.Visible;
                txtNeighborhoodRadius.Visibility = Visibility.Visible;

                txtNeighborhoodRadius.Text = Convert.ToString(((Kohonen)RNA).GetNeighborhoodRadius());
                txtLearningRate.Text = Convert.ToString(((Kohonen)RNA).GetLearningRate());
                txtIterationNumber.Text = Convert.ToString(((Kohonen)RNA).GetIterationNumber());
            }
            else // is Backpropagation
            {
                lblErrorRate.Visibility = Visibility.Visible;
                txtErrorRate.Visibility = Visibility.Visible;
                lblNeighborhoodRadius.Visibility = Visibility.Collapsed;
                txtNeighborhoodRadius.Visibility = Visibility.Collapsed;

                txtErrorRate.Text = Convert.ToString(((Backpropagation)RNA).GetErrorRate());
                txtLearningRate.Text = Convert.ToString(((Backpropagation)RNA).GetLearningRate());
                txtIterationNumber.Text = Convert.ToString(((Backpropagation)RNA).GetMaxIterationNumber());
            }
        }

        private NewTrainingSet NewTrainingSetScreen()
        {
            View.NewTrainingSet w = new View.NewTrainingSet();
            if (RNA is Kohonen)
            {
                w.SetSupervisedLearning(false);
                w.SetInputNumber(((Kohonen)RNA).GetInputLayerSize());
                w.SetOutputNumber(0);
            }
            else // is Backpropagation
            {
                w.SetSupervisedLearning(true);
                w.SetInputNumber(((Backpropagation)RNA).GetInputLayerSize());
                w.SetOutputNumber(((Backpropagation)RNA).GetOutputLayerSize());
            }

            return w;
        }

        private bool IsPatternRight()
        {
            string[] neuronsList = txtInputPattern.Text.Split(',');
            if (neuronsList.Count() != (RNA is Kohonen ? ((Kohonen)RNA).GetInputLayerSize() : ((Backpropagation)RNA).GetInputLayerSize()))
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
            return true;
        }

        private double[] GetInputPattern()
        {
            double[] ret = new double[(RNA is Kohonen ? ((Kohonen)RNA).GetInputLayerSize() : ((Backpropagation)RNA).GetInputLayerSize())];
            int x = 0;
            string[] neuronsList = txtInputPattern.Text.Split(',');
            foreach (string neuron in neuronsList)
            {
                ret[x] = Convert.ToDouble(neuron.Replace(".", ","));
                x++;
            }

            return ret;
        }
        #endregion

        #region Handlers
        private void Click_Edge(object sender, RoutedEventArgs e)
        {
            if (RNA is Backpropagation)
            {
                BackpropagationNeuron source = (BackpropagationNeuron)((Edge<object>)(((EdgeControl)sender).Edge)).Source;
                BackpropagationNeuron target = (BackpropagationNeuron)((Edge<object>)(((EdgeControl)sender).Edge)).Target;
                foreach (BackpropagationConnection conn in target.listConnection)
                {
                    if (conn.neuron == source)
                    {
                        graphText.Content = "Peso: " + conn.valueWeight;
                        break;
                    }
                }
                
            }
            else // is Kohonen
            {
                string source = (string)((Edge<object>)(((EdgeControl)sender).Edge)).Source;
                KohonenNeuron target = (KohonenNeuron)((Edge<object>)(((EdgeControl)sender).Edge)).Target;
                string[] sA = source.Split('-');
                int wIndex = (Convert.ToInt32(sA[1]) - 1);
                graphText.Content = "Peso: " + target.weights[wIndex];
            }
            
        }

        private void Click_Vertex(object sender, RoutedEventArgs e)
        {
            if (RNA is Backpropagation)
            {
                object content = ((ContentPresenter)((Border)sender).Child).Content;
                graphText.Content = "Saída:" + (string)content.ToString();
            }
            else // is Kohonen
            {
                object content = ((ContentPresenter)((Border)sender).Child).Content;
                graphText.Content = (string)content.ToString();
            }
        }

        private void btnExit_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (btnNew.IsEnabled)
            {
                if (State != MainWindowState.NoANN)
                {
                    string messageBoxText = "Você tem certeza que deseja criar uma nova RNA?\nFazendo isto você perderá toda informação não salva da RNA atual.";
                    string caption = "Criar nova RNA";
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    MessageBoxImage icon = MessageBoxImage.Warning;

                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            INeuralNetwork lastRNA = RNA;

                            View.NewRNA w = new View.NewRNA();
                            w.ShowDialog();

                            if (RNA != null && lastRNA != RNA)
                                NewRNA();
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    View.NewRNA w = new View.NewRNA();
                    w.ShowDialog();

                    if (RNA != null)
                        NewRNA();
                }
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (btnSave.IsEnabled)
            {
                if (LastSaved == null)
                    btnSaveAs_Click(sender, e);
                else
                {
                    ADReNA_API.Util.Export.NeuralNetworkStructure(RNA, LastSaved);
                    MessageBox.Show("Rede Neural Artificial salva com sucesso!");
                }
            }
        }

        private void btnKohonenSample_Click(object sender, RoutedEventArgs e)
        {
            ExemploKohonen.MainWindow w = new ExemploKohonen.MainWindow();
            w.ShowDialog();
        }

        private void btnBackpropagationSample_Click(object sender, RoutedEventArgs e)
        {
            ExemploBackpropagation.MainWindow w = new ExemploBackpropagation.MainWindow();
            w.ShowDialog();
        }

        private void btnSobre_Click(object sender, RoutedEventArgs e)
        {
            View.About w = new View.About();
            w.ShowDialog();
        }

        private void btnNewSetTraining_Click(object sender, RoutedEventArgs e)
        {
            View.NewTrainingSet w = NewTrainingSetScreen();
            w.ShowDialog();

            if (TrainingSet != null && State == MainWindowState.RawANN)
                State = MainWindowState.LoadedTrainingSet;

        }

        private void btnEditTrainingSet_Click(object sender, RoutedEventArgs e)
        {
            View.NewTrainingSet w = NewTrainingSetScreen();
            w.SetTrainingSet(TrainingSet);
            w.ShowDialog();
            if (TrainingSet != null && State == MainWindowState.RawANN)
                State = MainWindowState.LoadedTrainingSet;
        }

        private void btnTrain_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading(true);
            State = MainWindowState.TrainingANN;
            backgroundWorker.RunWorkerAsync();
        }

        private void btnRecognize_Click(object sender, RoutedEventArgs e)
        {
            if (IsPatternRight())
            {
                if (RNA is Backpropagation)
                {
                    double[] ret = RNA.Recognize(GetInputPattern());
                    DrawGraph();
                    txtResult.Text = "";
                    string linebreak = "";
                    string comma = "";
                    foreach (double r in ret)
                    {
                        txtResult.Text += comma + linebreak + r.ToString();
                        linebreak = "\n";
                        comma = ", ";
                    }
                }
                else // is Kohonen
                {
                    KohonenNeuron neuron = ((Kohonen)RNA).RecognizeWinnerNeuron(GetInputPattern());
                    txtResult.Text = "Neurônio Vencedor: " + neuron.ToString() + "\n\n";
                    txtResult.Text += "Pesos:";
                    int x = 1;
                    foreach (double w in neuron.weights)
                    {
                        txtResult.Text += "\nInput-"+x.ToString()+": " + w.ToString();
                        x++;
                    }
                }
            }
            else
            {
                MessageBox.Show("Padrão de entrada com formato incorreto!");
                txtInputPattern.Focus();
            }
        }

        private void btnSaveTrainingSet_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Treinamento";
            dlg.DefaultExt = ".json";
            dlg.Filter = "Arquivos JSON (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                ADReNA_API.Util.Export.DataSet(TrainingSet, filename);
                MessageBox.Show("Conjunto de treinamento salvo com sucesso!");
            }
        }

        private void btnLoadTrainingSet_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Treinamento";
            dlg.DefaultExt = ".json";
            dlg.Filter = "Arquivos JSON (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                bool importable = false;
                bool error = true;
                DataSet imported = new DataSet();

                try
                {
                    imported = ADReNA_API.Util.Import.DataSet(filename);
                    error = false;
                }
                catch
                {
                    MessageBox.Show("Impossível carregar arquivo!");
                }

                if (!error)
                {
                    if (RNA is Kohonen)
                    {
                        if (imported.inputSize == ((Kohonen)RNA).inputLayerSize && imported.outputSize == 0)
                            importable = true;
                    }
                    else
                    {
                        if (imported.inputSize == ((Backpropagation)RNA).inputLayerSize && imported.outputSize == ((Backpropagation)RNA).outputLayerSize)
                            importable = true;
                    }

                    if (importable)
                    {
                        TrainingSet = imported;
                        if (State == MainWindowState.RawANN)
                            State = MainWindowState.LoadedTrainingSet;
                        MessageBox.Show("Conjunto de treinamento carregado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível importar o conjunto de treinamento! Conjuntos de treinamento devem ser compatíveis com a RNA em questão.");
                    }
                }

            }
        }

        private void btnSaveLearn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Aprendizado";
            dlg.DefaultExt = ".json";
            dlg.Filter = "Arquivos JSON (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                ADReNA_API.Util.Export.KnowledgeBase(RNA, filename);
                MessageBox.Show("Aprendizado da RNA salvo com sucesso!");
            }
        }

        private void btnLoadLearn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Aprendizado";
            dlg.DefaultExt = ".json";
            dlg.Filter = "Arquivos JSON (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                bool error = true;

                try
                {
                    ADReNA_API.Util.Import.KnowledgeBase(RNA, filename);
                    error = false;
                }
                catch
                {
                    MessageBox.Show("Não foi possível importar o aprendizado da RNA! O aprendizao deve ser compatível com a RNA em questão.");
                }

                if (!error)
                {
                    State = MainWindowState.TrainedANN;
                    if (TrainingSet == null)
                    {
                        if (RNA is Kohonen)
                        {
                            TrainingSet = new DataSet(((Kohonen)RNA).GetInputLayerSize());
                        }
                        else if (RNA is Backpropagation)
                        {
                            TrainingSet = new DataSet(((Backpropagation)RNA).GetInputLayerSize(), ((Backpropagation)RNA).GetOutputLayerSize());
                        }
                    }
                    labelIterationCount.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show("Aprendizado carregado com sucesso!");
                }

            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            bool canLoad = false;
            if (State != MainWindowState.NoANN)
            {
                string messageBoxText = "Você tem certeza que deseja carregar uma nova RNA?\nFazendo isto você perderá toda informação não salva da RNA atual.";
                string caption = "Carregar nova RNA";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        canLoad = true;
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                canLoad = true;
            }

            if (canLoad)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "RNA";
                dlg.DefaultExt = ".json";
                dlg.Filter = "Arquivos JSON (.json)|*.json";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    string filename = dlg.FileName;
                    bool error = true;
                    INeuralNetwork imported = null;

                    try
                    {
                        imported = ADReNA_API.Util.Import.NeuralNetworkStructure(filename);
                        error = false;
                    }
                    catch
                    {
                        MessageBox.Show("Impossível carregar arquivo!");
                    }

                    if (!error)
                    {
                        RNA = imported;
                        NewRNA();
                        MessageBox.Show("Rede Neural Artificial carregada com sucesso!");
                    }

                }
            }
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "RNA";
            dlg.DefaultExt = ".json";
            dlg.Filter = "Arquivos JSON (.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                ADReNA_API.Util.Export.NeuralNetworkStructure(RNA, filename);
                LastSaved = filename;
                MessageBox.Show("Rede Neural Artificial salva com sucesso!");
            }
        }

        private void txtLearningRate_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ValidateDouble(txtLearningRate.Text))
            {
                if (RNA is Kohonen)
                {
                    ((Kohonen)RNA).SetLearningRate(Convert.ToDouble(txtLearningRate.Text));
                }
                else if (RNA is Backpropagation)
                {
                    ((Backpropagation)RNA).SetLearningRate(Convert.ToDouble(txtLearningRate.Text));
                }
            }
            else
            {
                MessageBox.Show("Formato Incorreto!");
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
                {
                    txtLearningRate.Focus();
                    Keyboard.Focus(txtLearningRate);
                }));
            }

        }

        private void txtErrorRate_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ValidateDouble(txtErrorRate.Text))
            {
                ((Backpropagation)RNA).SetErrorRate(Convert.ToDouble(txtErrorRate.Text));
            }
            else
            {
                MessageBox.Show("Formato Incorreto!");
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
                {
                    txtErrorRate.Focus();
                    Keyboard.Focus(txtErrorRate);
                }));
            }

        }

        private void txtNeighborhoodRadius_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ValidateInt(txtNeighborhoodRadius.Text))
            {
                ((Kohonen)RNA).SetNeighborhoodRadius(Convert.ToInt32(txtNeighborhoodRadius.Text));
            }
            else
            {
                MessageBox.Show("Formato Incorreto!");
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
                {
                    txtNeighborhoodRadius.Focus();
                    Keyboard.Focus(txtNeighborhoodRadius);
                }));
            }

        }

        private void txtIterationNumber_LostFocus(object sender, RoutedEventArgs e)
        {

            if (ValidateInt(txtIterationNumber.Text))
            {
                if (RNA is Kohonen)
                {
                    ((Kohonen)RNA).SetIterationNumber(Convert.ToInt32(txtIterationNumber.Text));
                }
                else if (RNA is Backpropagation)
                {
                    ((Backpropagation)RNA).SetMaxIterationNumber(Convert.ToInt32(txtIterationNumber.Text));
                }
            }
            else
            {
                MessageBox.Show("Formato Incorreto!");
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
                {
                    txtIterationNumber.Focus();
                    Keyboard.Focus(txtIterationNumber);
                }));
            }

        }
        #endregion
    }
}