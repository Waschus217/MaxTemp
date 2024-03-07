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
        private bool isDarkMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "temps.csv";
                double highestTemperature = double.MinValue;
                double lowestTemperature = double.MaxValue;
                double totalTemperature = 0;
                int temperatureCount = 0;
                Dictionary<string, Dictionary<double, int>> sensorTemperatureFrequency = new Dictionary<string, Dictionary<double, int>>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length >= 3)
                        {
                            string sensorName = values[0];
                            double temperature = double.Parse(values[2], CultureInfo.InvariantCulture);

                            if (!sensorTemperatureFrequency.ContainsKey(sensorName))
                            {
                                sensorTemperatureFrequency[sensorName] = new Dictionary<double, int>();
                            }

                            if (sensorTemperatureFrequency[sensorName].ContainsKey(temperature))
                            {
                                sensorTemperatureFrequency[sensorName][temperature]++;
                            }
                            else
                            {
                                sensorTemperatureFrequency[sensorName][temperature] = 1;
                            }
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

                string mostFrequentSensorHigh = "";
                double highestTemperatureFrequency = 0;

                foreach (var entry in sensorTemperatureFrequency)
                {
                    var maxFrequencyEntry = entry.Value.Aggregate((x, y) => x.Value > y.Value ? x : y);

                    if (maxFrequencyEntry.Value > highestTemperatureFrequency)
                    {
                        highestTemperatureFrequency = maxFrequencyEntry.Value;
                        mostFrequentSensorHigh = entry.Key;
                    }
                }

                string mostFrequentSensorLow = "";
                double lowestTemperatureFrequency = double.MaxValue;

                foreach (var entry in sensorTemperatureFrequency)
                {
                    var minFrequencyEntry = entry.Value.Aggregate((x, y) => x.Value < y.Value ? x : y);

                    if (minFrequencyEntry.Value < lowestTemperatureFrequency)
                    {
                        lowestTemperatureFrequency = minFrequencyEntry.Value;
                        mostFrequentSensorLow = entry.Key;
                    }
                }

                double averageTemperature = totalTemperature / temperatureCount;
                txtHighestTemperature.Text = highestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                txtLowestTemperature.Text = lowestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                txtAverageTemperature.Text = averageTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                txtMostFrequentSensorHigh.Text = $"{mostFrequentSensorHigh} - Häufigkeit: {highestTemperatureFrequency}";
                txtMostFrequentSensorLow.Text = $"{mostFrequentSensorLow} - Häufigkeit: {lowestTemperatureFrequency}";
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

        private void btnChangeMode_Click(object sender, RoutedEventArgs e)
        {
            isDarkMode = !isDarkMode;
            changeMode(isDarkMode);
        }

        private void changeMode(bool isDarkMode)
        {
            isDarkMode = !isDarkMode;
            Brush foregroundColor = isDarkMode ? Brushes.White : Brushes.Black;
            if (isDarkMode)
            {
                btnChangeMode.Content = "Light";
                this.Background = Brushes.Black;
                btnChangeMode.Foreground = Brushes.White;
                ChangeAllTextForeground(this, Brushes.White);
            }
            else
            {
                btnChangeMode.Content = "Dark";
                this.Background = Brushes.White;
                btnChangeMode.Foreground = Brushes.Black;
                ChangeAllTextForeground(this, Brushes.Black);
            }

            txtHighestTemperature.Foreground = foregroundColor;
            txtLowestTemperature.Foreground = foregroundColor;
            txtAverageTemperature.Foreground = foregroundColor;
            txtMostFrequentSensorHigh.Foreground = foregroundColor;
            txtMostFrequentSensorLow.Foreground = foregroundColor;
        }

        private void ChangeAllTextForeground(DependencyObject parent, Brush brush)
        {
            if (parent == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Control control)
                {
                    control.Foreground = brush;
                }

                ChangeAllTextForeground(child, brush);
            }
        }
    }
}
