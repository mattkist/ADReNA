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

namespace ExemploBackpropagation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int[] camadasIntermediarias = { 16 };
        private static int camadaEntrada = 8 * 8;
        private static int camadaSaida = 2; 
        Backpropagation rna;
        ADReNA_API.Data.DataSet treinamento;

        double[,] entradaTreinamento = new double[8, 8];
        double[,] entradaReconhecimento = new double[8, 8];
        List<double[,]> patterns = new List<double[,]>();

        public enum EstadoTela {Limpa, ComAlgunsPadroes, ComTodosPadroes, Aprendida}

        private EstadoTela estado;
        private EstadoTela Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                switch (value)
                {
                    case EstadoTela.Limpa:
                        btnAprender.IsEnabled = false;
                        btnLimpar.IsEnabled = false;
                        tabReconhecimento.IsEnabled = false;
                        btnInserir.IsEnabled = true;
                        break;
                    case EstadoTela.ComAlgunsPadroes:
                        btnAprender.IsEnabled = true;
                        btnLimpar.IsEnabled = true;
                        tabReconhecimento.IsEnabled = false;
                        btnInserir.IsEnabled = true;
                        break;
                    case EstadoTela.ComTodosPadroes:
                        btnAprender.IsEnabled = true;
                        btnLimpar.IsEnabled = true;
                        tabReconhecimento.IsEnabled = false;
                        btnInserir.IsEnabled = false;
                        break;
                    case EstadoTela.Aprendida:
                        btnAprender.IsEnabled = false;
                        btnLimpar.IsEnabled = false;
                        tabReconhecimento.IsEnabled = true;
                        btnInserir.IsEnabled = false;
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.estado = EstadoTela.Limpa;
            ConfiguraNovaRNA();
        }

        #region Eventos
        /* Evento de clique no Canvas de aprendizado*/
        private void inputSurface_MouseDown(object sender, MouseEventArgs e)
        {
            EventoDesenhar(sender, e, inputSurface);
        }

        /* Evento de arrastar o mouse no Canvas de aprendizado */
        private void inputSurface_MouseMove(object sender, MouseEventArgs e)
        {
            EventoDesenhar(sender, e, inputSurface);
        }

        /* Evento de clique no Canvas de reconhecimento */
        private void inputSurfaceReconhecimento_MouseDown(object sender, MouseEventArgs e)
        {
            EventoDesenhar(sender, e, inputSurfaceReconhecimento);
        }

        /* Evento de arrastar o mouse no Canvas de reconhecimento */
        private void inputSurfaceReconhecimento_MouseMove(object sender, MouseEventArgs e)
        {
            EventoDesenhar(sender, e, inputSurfaceReconhecimento);
        }

        /* Evento de clique em Inserir Padrão */
        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmpty(entradaTreinamento))
            {
                double[,] entradaClone = (double[,])entradaTreinamento.Clone();
                patterns.Add(entradaClone);
                DesenhaPattern(patterns.Count, entradaClone);
                entradaTreinamento = new double[8, 8];
                LimpaDesenho(inputSurface);

                switch (patterns.Count)
                {
                    case 1:
                        Estado = EstadoTela.ComAlgunsPadroes;
                        break;
                    case 2:
                        Estado = EstadoTela.ComAlgunsPadroes;
                        break;
                    case 3:
                        Estado = EstadoTela.ComTodosPadroes;
                        break;
                }
            }
        }

        /* Evento de arrastar o mouse no Canvas */
        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            patterns.Clear();
            LimparPatterns();
            entradaTreinamento = new double[8, 8];
            LimpaDesenho(inputSurface);
            Estado = EstadoTela.Limpa;
        }

        /* Evento de clique em Aprender */
        private void btnAprender_Click(object sender, RoutedEventArgs e)
        {
            MontaConjuntoTreinamento();
            rna.Learn(treinamento);

            Estado = EstadoTela.Aprendida;
            LimpaDesenho(patternReconhecido);
            tabs.SelectedItem = tabReconhecimento;
        }

         /* Evento de clique em Reconhecer */
        private void btnReconhecer_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmpty(entradaReconhecimento))
            {
                LimpaDesenho(patternReconhecido);

                double[] pattern = SerializarPattern(entradaReconhecimento);
                double[] resultado = rna.Recognize(pattern);

                if (Convert.ToInt32(resultado[0]) == 0 && Convert.ToInt32(resultado[1]) == 0)
                {
                    DesenhaPattern(4, patterns[0]);
                }
                else if (Convert.ToInt32(resultado[0]) == 0 && Convert.ToInt32(resultado[1]) == 1)
                {
                    DesenhaPattern(4, patterns[1]);
                }
                else if (Convert.ToInt32(resultado[0]) == 1 && Convert.ToInt32(resultado[1]) == 0)
                {
                    DesenhaPattern(4, patterns[2]);
                }  

            }
        }

         /* Evento de clique em Reiniciar */
        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            ConfiguraNovaRNA();

            entradaReconhecimento = new double[8, 8];
            LimpaDesenho(inputSurfaceReconhecimento);
            btnLimpar_Click(sender, e);
            tabs.SelectedItem = tabAprendizado;
        }
        #endregion

        #region private Methods

        private void MontaConjuntoTreinamento()
        {
            for (int x = 0; x < patterns.Count; x++)
            {
                double[,] pattern = patterns[x];
                double[] patternS = SerializarPattern(pattern);
                ADReNA_API.Data.DataSetObject  p = null;
                switch (x)
                {
                    case 0:
                        p = new ADReNA_API.Data.DataSetObject(patternS, new double[] { 0, 0 });
                        break;
                    case 1:
                        p = new ADReNA_API.Data.DataSetObject(patternS, new double[] { 0, 1 });
                        break;
                    case 2:
                        p = new ADReNA_API.Data.DataSetObject(patternS, new double[] { 1, 0 });
                        break;
                } 

                treinamento.Add(p);
            }
        }

        private double[] SerializarPattern(double[,] pattern)
        {
            double[] retorno = new double[pattern.GetLength(0) * pattern.GetLength(1)];
            int z = 0;
            for (int x = 0; x < pattern.GetLength(0); x++)
            {
                for (int y = 0; y < pattern.GetLength(1); y++)
                {
                    retorno[z] = pattern[x, y];
                    z += 1;
                }
            }
            return retorno;
        }

        private void ConfiguraNovaRNA()
        {
            /* inicialização da rede backpropagation 
            * e do conjunto de treinamentos (ADReNA)*/
            rna = new Backpropagation(camadaEntrada, camadaSaida, camadasIntermediarias);
            treinamento = new ADReNA_API.Data.DataSet(camadaEntrada, camadaSaida);
            /**/

            /*Configuração da rede Backpropagation */
            rna.ETA = 0.3; //taxa de aprendizado inicial 
            rna.Error = 0.005; //erro aceitável
            rna.maxIterationNumber = 10000; //número de iterações
            /**/
        }

        private void EventoDesenhar(object sender, MouseEventArgs e, Canvas tela)
        {
            double[,] entrada = null;
            if (tela == inputSurface)
            {
                entrada = entradaTreinamento;
            }
            else
            {
                entrada = entradaReconhecimento;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pontoCLick = e.GetPosition(tela);
                Point coordenadaGrid = CoordenadaGrid(pontoCLick);
                if (IsNovoPonto(coordenadaGrid, entrada))
                {
                    AddPonto(coordenadaGrid, entrada);
                    DesenhaQuadrado(tela, coordenadaGrid, Colors.Black);
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                Point pontoCLick = e.GetPosition(tela);
                Point coordenadaGrid = CoordenadaGrid(pontoCLick);
                if (!IsNovoPonto(coordenadaGrid, entrada))
                {
                    RemovePonto(coordenadaGrid, entrada);
                    DesenhaQuadrado(tela, coordenadaGrid, Colors.WhiteSmoke);
                }
            }
        }

        private Point CoordenadaGrid(Point ponto)
        {
            Point coordenada = new Point();
            coordenada.X = (int)(ponto.X / 15);
            coordenada.Y = (int)(ponto.Y / 15);
            if (coordenada.X >= 8)
                coordenada.X = 7;
            if (coordenada.Y >= 8)
                coordenada.Y = 7;
            return coordenada;
        }

        private bool IsNovoPonto(Point ponto, double[,] entrada)
        {
            double valor = entrada[(int)ponto.X, (int)ponto.Y];
            return valor == 0;
        }

        private void AddPonto(Point ponto, double[,] entrada)
        {
            entrada[(int)ponto.X, (int)ponto.Y] = 1;
        }

        private void RemovePonto(Point ponto, double[,] entrada)
        {
            entrada[(int)ponto.X, (int)ponto.Y] = 0;
        }

        private void DesenhaQuadrado(Canvas tela, Point ponto, Color cor)
        {
            Polygon poligono = new Polygon();
            poligono.Points.Add(new Point(ponto.X*15, ponto.Y*15));
            poligono.Points.Add(new Point((ponto.X + 1)*15 , ponto.Y*15));
            poligono.Points.Add(new Point((ponto.X + 1) * 15, (ponto.Y + 1) * 15));
            poligono.Points.Add(new Point(ponto.X*15, (ponto.Y + 1) * 15));

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = cor;
            poligono.Fill = brush;

            tela.Children.Add(poligono);
        }

        private void DesenhaPattern(int patternIndex, double[,] pattern)
        {
            Canvas tela =  new Canvas();
            switch(patternIndex)
            {
                case 1:
                    tela = surfacePattern1;
                    break;
                 case 2:
                    tela = surfacePattern2;
                    break;
                 case 3:
                    tela = surfacePattern3;
                    break;
                 case 4:
                    tela = patternReconhecido;
                    break;
            }
            for(int x = 0; x < 8; x++)
                for (int y = 0; y <8; y++)
                    if (pattern[x,y] == 1)
                        DesenhaQuadrado(tela, new Point(x,y), Colors.Black);
        }

        private void LimparPatterns()
        {
            LimpaDesenho(surfacePattern1);
            LimpaDesenho(surfacePattern2);
            LimpaDesenho(surfacePattern3);
        }

        private void LimpaDesenho(Canvas tela)
        {
            for(int x = 0; x < 8; x++)
                for (int y = 0; y <8; y++)
                    DesenhaQuadrado(tela, new Point(x,y), Colors.WhiteSmoke);
        }

        private bool IsEmpty(double[,] entrada)
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    if (entrada[x, y] == 1)
                        return false;
            return true;
        }
        #endregion
    }
}
