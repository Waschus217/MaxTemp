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
using System.Windows.Threading;

namespace MaxTemp
{
    public partial class LoadingScreen : Window
    {
        public bool isDarkMode = false;
        private ResultWindow resultWindow;
        private MainWindow mainWindow;
        private DispatcherTimer timer;

        public LoadingScreen()
        {
            InitializeComponent();
        }

        public void Loading()
        {
            if (resultWindow == null)
            {
                resultWindow = new ResultWindow();
            }

            StartTimer();
            resultWindow.Auswerten();
        }

        public void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += TimerTicker;
            timer.Start();
        }
        private void TimerTicker(object sender, EventArgs e)
        {
            timer.Stop();
            Close();
            resultWindow.Show();
        }
    }
}
