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
        public bool isDarkMode = false;
        private LoadingScreen loadingScreen;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            if (loadingScreen == null)
            {
                loadingScreen = new LoadingScreen();
                loadingScreen.Owner = this;
            }

            loadingScreen.Loading();
            loadingScreen.Show();
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

