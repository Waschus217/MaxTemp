using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MaxTemp
{
    public partial class MainWindow : Window
    {
        public bool isDarkMode = false;
        private LoadingScreen loadingScreen;
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }

        private List<string> helpfulTips = new List<string>
        {
            "Wusstest du, dass Menschen mit \nNussalergie keine Nüsse essen?",
            "Wenn zwei Menschen immer \ndasselbe denken, \nist einer von ihnen überflüssig.",
            "Es gibt immer zwei Meinungen:\nMeine und die Falsche.",
            "Die meisten Probleme \nlösen sich von alleine.\nMan darf sie nur nicht dabei stören.",
            "Wenn deine Chaya \nClubbesitzer kennt. \nDann ist sie eine Thot",
            "Versuche nicht, \nAlpha wütend zu machen."
        };
        
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            if (loadingScreen == null)
            {
                loadingScreen = new LoadingScreen();
                loadingScreen.Owner = this;
            }

            loadingScreen.Loading();
            loadingScreen.Show();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int index = random.Next(helpfulTips.Count);
            loadingScreen.helpfulTipsListBox.Items[0] = helpfulTips[index];
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

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime selectedDate = calendar.SelectedDate ?? DateTime.Now;

                // Führe die Berechnungen für das ausgewählte Datum durch
                CalculateStatisticsForDate(selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Auswerten: {ex.Message}");
            }
        }

        private void CalculateStatisticsForDate(DateTime selectedDate)
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
                            DateTime currentDate = DateTime.ParseExact(values[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            if (currentDate.Date == selectedDate.Date)
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
                UpdateUI(() =>
                {
                    txtHighestTemperature.Text = highestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                    txtLowestTemperature.Text = lowestTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                    txtAverageTemperature.Text = averageTemperature.ToString("F2", CultureInfo.InvariantCulture) + " C°";
                    txtMostFrequentSensorHigh.Text = $"{mostFrequentSensorHigh} - Häufigkeit: {highestTemperatureFrequency}";
                    txtMostFrequentSensorLow.Text = $"{mostFrequentSensorLow} - Häufigkeit: {lowestTemperatureFrequency}";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Auswerten: {ex.Message}");
            }
        }

        private void UpdateUI(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        private void changeMode(bool isDarkMode)
        {
            isDarkMode = !isDarkMode;
            Brush foregroundColor = isDarkMode ? Brushes.White : Brushes.Black;
            Brush colorForCalender = isDarkMode ? Brushes.White : Brushes.Black;
            if (isDarkMode)
            {
                btnChangeMode.Content = "Light";
                this.Background = Brushes.Black;
                btnChangeMode.Foreground = Brushes.White;
                ChangeAllTextForeground(this, Brushes.White);
                calendar.Foreground = colorForCalender;
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
            calendar.Foreground = colorForCalender;
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

