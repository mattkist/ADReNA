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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using ADReNA_API.NeuralNetwork;

namespace ADReNA.View
{
    /// <summary>
    /// Interaction logic for NewRNA.xaml
    /// </summary>
    public partial class NewRNA : Window
    {
        public NewRNA()
        {
            InitializeComponent();
            cbModelo.SelectedIndex = 0;
        }

        private void cbModelo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbModelo.SelectedIndex)
            {
                case 0:
                    KohonenGrid.Visibility = Visibility.Collapsed;
                    BackpropagationGrid.Visibility = Visibility.Visible;
                    CheckButtonCreate();
                    break;
                case 1:
                    KohonenGrid.Visibility = Visibility.Visible;
                    BackpropagationGrid.Visibility = Visibility.Collapsed;
                    CheckButtonCreate();
                    break;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HiddenLayerValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9|,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            switch (cbModelo.SelectedIndex)
            {
                case 0:
                    if (txtHiddenLayer.Text != null && txtHiddenLayer.Text != "")
                    {
                        MainWindow.RNA = new Backpropagation(Convert.ToInt32(txtBackpropagationInputLayerSize.Text), Convert.ToInt32(txtOutputLayerSize.Text), GetHiddenLayer());
                    }
                    else
                    {
                        MainWindow.RNA = new Backpropagation(Convert.ToInt32(txtBackpropagationInputLayerSize.Text), Convert.ToInt32(txtOutputLayerSize.Text));
                    }
                    break;
                case 1:
                    MainWindow.RNA = new Kohonen(Convert.ToInt32(txtKohonenInputLayerSize.Text), Convert.ToInt32(txtCompetitiveLayerSize.Text), Convert.ToInt32(txtMaximumWeightRange.Text));
                    break;
            }

            this.Close();
        }

        private void anyText_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckButtonCreate();
        }

        private void CheckButtonCreate()
        {
            bool enabled = true;

            switch (cbModelo.SelectedIndex)
            {
                case 0:
                    if (txtBackpropagationInputLayerSize.Text == "" || txtBackpropagationInputLayerSize.Text == null)
                        enabled = false;
                    if (txtOutputLayerSize.Text == "" || txtOutputLayerSize.Text == null)
                        enabled = false;
                    if(!HiddenLayerOK())
                        enabled = false;
                    break;
                case 1:
                    if (txtCompetitiveLayerSize.Text == "" || txtCompetitiveLayerSize.Text == null)
                        enabled = false;
                    if (txtKohonenInputLayerSize.Text == "" || txtKohonenInputLayerSize.Text == null)
                        enabled = false;
                    if (txtMaximumWeightRange.Text == "" || txtMaximumWeightRange.Text == null)
                        enabled = false;
                    break;
            }

            btnCreate.IsEnabled = enabled;
        }

        private bool HiddenLayerOK()
        {
            if (txtHiddenLayer.Text != null && txtHiddenLayer.Text != "")
            {
                string[] layers = txtHiddenLayer.Text.Split(',');

                foreach (string layer in layers)
                {
                    if (layer == null || layer == "")
                        return false;
                    try
                    {
                        Convert.ToInt32(layer);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[] GetHiddenLayer()
        {
            string[] layers = txtHiddenLayer.Text.Split(',');
            int[] hiddenLayers = new int[layers.Length];

            int x = 0;
            foreach (string layer in layers)
            {
                hiddenLayers[x] = Convert.ToInt32(layer);
                x++;
            }

            return hiddenLayers;
        }

        private void KeyUpTextBox(object sender, KeyEventArgs e)
        {
            CheckButtonCreate();
        }
    }
}
