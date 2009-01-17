using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.TeamFoundation.Build.Proxy;
using System.ComponentModel;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using System.Net;
using Microsoft.TeamFoundation.Build.Common;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class BuildStoreDriver : DependencyObject {
        private BuildStoreWorker _backgroundWorker;

        public BuildStoreDriver() {

        }

        public static readonly DependencyProperty TeamFoundationServerUriProperty = DependencyProperty.Register("TeamFoundationServerUri", typeof(string), typeof(BuildStoreDriver), new UIPropertyMetadata(null, RecreateBackgroundWorker));
        public static readonly DependencyProperty TeamProjectNameProperty = DependencyProperty.Register("TeamProjectName", typeof(string), typeof(BuildStoreDriver), new UIPropertyMetadata(null, RecreateBackgroundWorker));
        public static readonly DependencyProperty TeamBuildTypeNameProperty = DependencyProperty.Register("TeamBuildTypeName", typeof(string), typeof(BuildStoreDriver), new UIPropertyMetadata(null, RecreateBackgroundWorker));
        public static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.Register("IsUpdating", typeof(bool), typeof(BuildStoreDriver), new UIPropertyMetadata(false));
        public static readonly DependencyProperty LastExceptionProperty = DependencyProperty.Register("LastException", typeof(Exception), typeof(BuildStoreDriver), new UIPropertyMetadata(null));
        private static readonly DependencyPropertyKey PreviousBuildsPropertyKey = DependencyProperty.RegisterReadOnly("PreviousBuilds", typeof(ObservableCollection<BuildSummary>), typeof(BuildStoreDriver), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PreviousBuildsProperty = PreviousBuildsPropertyKey.DependencyProperty;

        public ObservableCollection<BuildSummary> PreviousBuilds {
            get { return (ObservableCollection<BuildSummary>)GetValue(PreviousBuildsProperty); }
            private set { SetValue(PreviousBuildsPropertyKey, value); }
        }

        public string TeamFoundationServerUri {
            get { return (string)GetValue(TeamFoundationServerUriProperty); }
            set { SetValue(TeamFoundationServerUriProperty, value); }
        }

        public string TeamProjectName {
            get { return (string)GetValue(TeamProjectNameProperty); }
            set { SetValue(TeamProjectNameProperty, value); }
        }

        public string TeamBuildTypeName {
            get { return (string)GetValue(TeamBuildTypeNameProperty); }
            set { SetValue(TeamBuildTypeNameProperty, value); }
        }

        public Exception LastException {
            get { return (Exception)GetValue(LastExceptionProperty); }
            set { SetValue(LastExceptionProperty, value); }
        }

        private static void RecreateBackgroundWorker(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            BuildStoreDriver driver = dependencyObject as BuildStoreDriver;
            if (driver != null) {
                driver.PreviousBuilds = new ObservableCollection<BuildSummary>();
                if (driver._backgroundWorker != null) {
                    driver._backgroundWorker.CancelAsync();
                }
                driver._backgroundWorker = new BuildStoreWorker(driver.TeamFoundationServerUri, driver.TeamProjectName, driver.TeamBuildTypeName, driver.PreviousBuilds);
                driver._backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                    delegate(object sender, RunWorkerCompletedEventArgs workedCompletedEventArgs) {
                        if (workedCompletedEventArgs.Error != null) {
                            driver.LastException = workedCompletedEventArgs.Error;
                        }
                    });
            }
        }

        public void Monitor() {
            VerifyAccess();

            if (!_backgroundWorker.IsBusy) {
                this.LastException = null;
                _backgroundWorker.RunWorkerAsync();
            }
        }
    }
}
