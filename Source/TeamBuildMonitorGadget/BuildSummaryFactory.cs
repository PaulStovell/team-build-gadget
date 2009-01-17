using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.Build.Proxy;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class BuildSummaryFactory {
        private List<BuildSummary> _summaries = new List<BuildSummary>();

        public BuildSummary GetBuildSummary(BuildData buildData) {
            BuildSummary result = null;

            foreach (BuildSummary summary in _summaries) {
                if (summary.BuildUri == buildData.BuildUri) {
                    result = summary;
                    break;
                }
            }

            if (result == null) {
                result = new BuildSummary();
                _summaries.Add(result);
            }

            result.BuildDate = buildData.StartTime;
            result.BuildNumber = buildData.BuildNumber;
            result.BuildStatus = buildData.BuildStatus;
            result.BuildUri = buildData.BuildUri;
            result.DropLocation = buildData.DropLocation;

            return result;
        }
    }
}
