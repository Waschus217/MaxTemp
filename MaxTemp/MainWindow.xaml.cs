using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Diese Routine (EventHandler des Buttons Auswerten) liest die Werte
        /// zeilenweise aus der Datei temps.csv aus, merkt sich den höchsten Wert
        /// und gibt diesen auf der Oberfläche aus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "temps.csv";
                double highestTemperature = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if(values.Length >= 3)
                        {
                            double temperature = double.Parse(values[2], CultureInfo.InvariantCulture);

                            if (temperature > highestTemperature)
                            {
                                highestTemperature = temperature;
                            }
                        }
                    }
                }
                txtHighestTemperature.Text = highestTemperature.ToString(CultureInfo.InvariantCulture) + " C°";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Auswerten: {ex.Message}");
            }
        }

        private void btnBeenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
