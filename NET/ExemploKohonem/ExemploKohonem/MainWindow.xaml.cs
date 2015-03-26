using ADReNA_API.Data;
using ADReNA_API.NeuralNetwork;
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

namespace ExemploKohonen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* Conjunto de padrões de entrada para o treinamento da rede */
        private DataSet conjuntoDeTreinamento;

        /* Rede Neural */
        INeuralNetwork rna;

        public MainWindow()
        {
            InitializeComponent();

            /* Instanciando um conjunto de treinamento de duas dimensões (X e Y) */
            conjuntoDeTreinamento = new DataSet(2);

            /* Instanciando uma rede neural de Kohonen com:
             * - 2 neurônios na camada de entrada (X e Y) 
             * - 10x10 neurônios na camada de competitiva
             * - pesos dos neurônios entre 0-300 (tamanho do canvas)
             */
            rna = new Kohonen(2, 10, 300);
            DesenhaRNA();
        }

        /*
         * Método responsável por inserir o ponto clicado como padrão de entrada
         * no conjunto de treinamento
         */
        private void AddPadrao(Point pontoCLick)
        {
            conjuntoDeTreinamento.Add(new DataSetObject(new double[] {pontoCLick.X, pontoCLick.Y}));
        }

        /*
         * Método responsável por desenhar ponto na tela
         */
        private void Desenha(Point pontoCLick)
        {
            Polygon ponto = new Polygon();
            ponto.Points.Add(new Point(pontoCLick.X - 1, pontoCLick.Y + 1));
            ponto.Points.Add(new Point(pontoCLick.X + 1, pontoCLick.Y + 1));
            ponto.Points.Add(new Point(pontoCLick.X + 1, pontoCLick.Y - 1));
            ponto.Points.Add(new Point(pontoCLick.X - 1, pontoCLick.Y - 1));

            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;
            ponto.Fill = redBrush;
            ponto.Stroke = redBrush;

            inputSurface.Children.Add(ponto);
        }

        /*
         * Método responsável por desenhar a camada competitiva da RNA de Kohonen
         */
        private void DesenhaRNA()
        {
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Blue;

            KohonenNeuron[,] camadaCompetitiva = ((Kohonen)(rna)).GetCompetitiveLayer();

            for (int i = 0; i < ((Kohonen)(rna)).GetCompetitiveLayerLength(); i++)
            {
                for (int j = 0; j < ((Kohonen)(rna)).GetCompetitiveLayerLength() - 1; j++)
                {
                    Line linha = new Line();
                    linha.X1 = camadaCompetitiva[i, j].GetWeights()[0];
                    linha.Y1 = camadaCompetitiva[i, j].GetWeights()[1];
                    linha.X2 = camadaCompetitiva[i, j + 1].GetWeights()[0];
                    linha.Y2 = camadaCompetitiva[i, j + 1].GetWeights()[1];
                    linha.Stroke = blueBrush;
                    inputSurface.Children.Add(linha);
                }
            }

            for (int i = 0; i < ((Kohonen)(rna)).GetCompetitiveLayerLength() - 1; i++)
            {
                for (int j = 0; j < ((Kohonen)(rna)).GetCompetitiveLayerLength(); j++)
                {
                    Line linha = new Line();
                    linha.X1 = camadaCompetitiva[i, j].GetWeights()[0];
                    linha.Y1 = camadaCompetitiva[i, j].GetWeights()[1];
                    linha.X2 = camadaCompetitiva[i + 1, j].GetWeights()[0];
                    linha.Y2 = camadaCompetitiva[i + 1, j].GetWeights()[1];
                    linha.Stroke = blueBrush;
                    inputSurface.Children.Add(linha);
                }
            }
        }

        private void LimpaLinhasRNA()
        {
            //Limpando as linhas desenhadas pela RNA
            UIElement[] elementosClone = new UIElement[inputSurface.Children.Count];
            inputSurface.Children.CopyTo(elementosClone, 0);
            foreach (UIElement child in elementosClone)
                if (child is Line)
                    inputSurface.Children.Remove(child);
        }

        #region Eventos
        /* Evento de clique no Canvas */
        private void inputSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                Point pontoCLick = e.GetPosition(inputSurface);

                AddPadrao(pontoCLick);
                Desenha(pontoCLick);
            }
        }

        /* Evento de clique em Limpar */
        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            UIElement[] elementosClone = new UIElement[inputSurface.Children.Count];
            inputSurface.Children.CopyTo(elementosClone, 0);
            foreach (UIElement child in elementosClone)
                if (child is Polygon)
                    inputSurface.Children.Remove(child);

            conjuntoDeTreinamento = new DataSet(2);
        }

        /* Evento de clique em Reiniciar */
        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            LimpaLinhasRNA();
            rna = new Kohonen(2, 10, 300);
            DesenhaRNA();
        }

        /* Evento de clique em Aprender */
        private void btnAprender_Click(object sender, RoutedEventArgs e)
        {
            rna.Learn(conjuntoDeTreinamento);
            LimpaLinhasRNA();
            DesenhaRNA();
        }
        #endregion

    }
}
