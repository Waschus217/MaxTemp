using System;
using System.Globalization;
using System.IO;
using System.Windows;

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
