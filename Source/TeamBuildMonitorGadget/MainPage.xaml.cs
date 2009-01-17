using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Diagnostics;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace PaulStovell.TeamBuildMonitorGadget {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage() {
            InitializeComponent();

            LoadSettings();
        }

        public static readonly DependencyProperty BuildStoreDriverProperty = DependencyProperty.Register("BuildStoreDriver", typeof(BuildStoreDriver), typeof(MainPage), new UIPropertyMetadata(null));
        public static readonly DependencyProperty FriendlyNameProperty = DependencyProperty.Register("FriendlyName", typeof(string), typeof(MainPage), new UIPropertyMetadata(null));

        public BuildStoreDriver BuildStoreDriver {
            get { return (BuildStoreDriver)GetValue(BuildStoreDriverProperty); }
            set { SetValue(BuildStoreDriverProperty, value); }
        }

        public string FriendlyName {
            get { return (string)GetValue(FriendlyNameProperty); }
            set { SetValue(FriendlyNameProperty, value); }
        }

        private void BuildItem_Clicked(object sender, RoutedEventArgs e) {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null) {
                BuildSummary summary = element.Tag as BuildSummary;
                if (summary != null && summary.DropLocation != null && summary.DropLocation.Length > 0) {
                    Process.Start(summary.DropLocation);
                }
            }
        }

        private void ExceptionDetailsLink_Clicked(object sender, RoutedEventArgs e) {
            if (BuildStoreDriver.LastException != null) {
                MessageBox.Show(BuildStoreDriver.LastException.ToString());
            }
        }

        private void TryAgainLink_Clicked(object sender, RoutedEventArgs e) {
            this.BuildStoreDriver.Monitor();
        }

        private void SettingsLink_Clicked(object sender, RoutedEventArgs e) {
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
            LoadSettings();
        }

        private void LoadSettings() {
            Settings settings = Settings.GetSettings();
            this.FriendlyName = settings.FriendlyName;

            BuildStoreDriver driver = new BuildStoreDriver();
            driver.TeamFoundationServerUri = settings.TeamFoundationServerUri;
            driver.TeamProjectName = settings.TeamProjectName;
            driver.TeamBuildTypeName = settings.TeamBuildTypeName;
            driver.Monitor();

            this.BuildStoreDriver = driver;
        }
    }
}