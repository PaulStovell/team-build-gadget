using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Build.Proxy;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Windows;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class BuildStoreWorker : BackgroundWorker {
        private string _teamFoundationServerUri;
        private string _teamProjectName;
        private string _teamBuildTypeName;
        private BuildSummaryFactory _summaryFactory;
        private ObservableCollection<BuildSummary> _buildSummaries;
		
        public BuildStoreWorker(string teamFoundationServerUri, string teamProjectName, string teamBuildTypeName, ObservableCollection<BuildSummary> buildSummaries) {
            _teamFoundationServerUri = teamFoundationServerUri;
            _teamProjectName = teamProjectName;
            _teamBuildTypeName = teamBuildTypeName;
            _buildSummaries = buildSummaries;
            _summaryFactory = new BuildSummaryFactory();

            this.WorkerSupportsCancellation = true;
            this.WorkerReportsProgress = true;
            this.DoWork += new DoWorkEventHandler(BuildStoreWorker_DoWork);
            this.ProgressChanged += new ProgressChangedEventHandler(BuildStoreWorker_ProgressChanged);
        }

        private void BuildStoreWorker_DoWork(object sender, DoWorkEventArgs e) {
            
            TeamFoundationServer server = new TeamFoundationServer(_teamFoundationServerUri, CredentialCache.DefaultCredentials);
            server.EnsureAuthenticated();

            while (!this.CancellationPending) {
                BuildStore store = (BuildStore)server.GetService(typeof(BuildStore));
                BuildData[] builds = store.GetListOfBuilds(_teamProjectName, _teamBuildTypeName);
                this.ReportProgress(0, builds);
                Thread.Sleep(30000);
            }
        }

        private void BuildStoreWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            BuildData[] results = (BuildData[])e.UserState;

            List<BuildSummary> summaries = new List<BuildSummary>();
            foreach (BuildData buildData in results) {
                BuildSummary summary = _summaryFactory.GetBuildSummary(buildData);
                summaries.Add(summary);
            }
            summaries.Sort();

            _buildSummaries.Clear();
            for (int i = 0; i < summaries.Count && i < 4; i++) {
                _buildSummaries.Add(summaries[i]);
            }
        
        }
    }
}
