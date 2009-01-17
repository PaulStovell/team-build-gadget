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
using System.Windows.Shapes;

namespace PaulStovell.TeamBuildMonitorGadget {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : System.Windows.Window {
        public SettingsWindow() {
            InitializeComponent();

            this.Settings = Settings.GetSettings();
        }

        /// <summary>
        /// DependencyProperty for the Settings property.
        /// </summary>
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register("Settings", typeof(Settings), typeof(SettingsWindow), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the settings shown on the dialog. 
        /// </summary>
        public Settings Settings {
            get { return (Settings)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        public void SaveButton_Clicked(object sender, RoutedEventArgs e) {
            this.Settings.Save();
            this.Close();
        }

        /// <summary>
        /// Called when the Cancel button is clicked.
        /// </summary>
        public void CancelButton_Clicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}