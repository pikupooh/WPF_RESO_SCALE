using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ResolutionCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double targetWidthPercentage = 0.25; // Set your desired percentage here
        private double targetHeightPercentage = 0.70;
        private static double thisDpiWidthFactor;
        private static double thisDpiHeightFactor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterDisplaySettingsChangedEvent();
            OnDisplaySettingsChanged(sender, e);
        }

        private void UpdateWindowSize()
        {
            //double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            //double targetWidth = screenWidth * targetWidthPercentage;
            //double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            //double targetHeight = screenHeight * targetHeightPercentage;

            CalculateDpiFactors();

            System.Diagnostics.Debug.WriteLine(SystemParameters.PrimaryScreenHeight + " " + SystemParameters.PrimaryScreenWidth);
            double ScreenHeight = SystemParameters.PrimaryScreenHeight * thisDpiHeightFactor;
            double ScreenWidth = SystemParameters.PrimaryScreenWidth * thisDpiWidthFactor;

            // Set the window width to the target percentage of the screen width
            this.Width = ScreenWidth * targetWidthPercentage;
            this.Height = ScreenHeight * targetHeightPercentage;

            // You may adjust the height if needed based on your UI design
            // Example: this.Height = targetHeight;
        }

        private void RegisterDisplaySettingsChangedEvent()
        {
            //SystemEvents.DisplaySettingsChanged += test1;
            SystemEvents.DisplaySettingsChanging += OnDisplaySettingsChanged;
        }

        private void UnregisterDisplaySettingsChangedEvent()
        {
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(UpdateWindowSize));
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            UnregisterDisplaySettingsChangedEvent(); // Unsubscribe from the event
        }

        private static void CalculateDpiFactors()
        {
            Window MainWindow = Application.Current.MainWindow;
            PresentationSource MainWindowPresentationSource = PresentationSource.FromVisual(MainWindow);
            Matrix m = MainWindowPresentationSource.CompositionTarget.TransformToDevice;
            thisDpiWidthFactor = m.M11;
            thisDpiHeightFactor = m.M22;
        }

        private void onclick(object sender, RoutedEventArgs e)
        {
            var point = Mouse.GetPosition(null);
            System.Diagnostics.Debug.WriteLine(point.X + " " + point.Y);
        }
    }
}
