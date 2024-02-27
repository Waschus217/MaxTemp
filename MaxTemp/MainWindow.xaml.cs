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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "temps.csv";
                double highestTemperature = double.MinValue; // Set to minimum possible value
                double lowestTemperature = double.MaxValue; // Set to maximum possible value
                double totalTemperature = 0;
                int temperatureCount = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length >= 3)
                        {
                            double temperature = double.Parse(values[2], CultureInfo.InvariantCulture);

                            if (temperature > highestTemperature)
                            {
                                highestTemperature = temperature;
                            }

                            if (temperature < lowestTemperature)
                            {
                                lowestTemperature = temperature;
                            }

                            totalTemperature += temperature;
                            temperatureCount++;
                        }
                    }
                }

                double averageTemperature = totalTemperature / temperatureCount;
                txtHighestTemperature.Text = highestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                txtLowestTemperature.Text = lowestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                txtAverageTemperature.Text = averageTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
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