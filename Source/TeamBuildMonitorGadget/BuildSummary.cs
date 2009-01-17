using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class BuildSummary : INotifyPropertyChanged, IComparable<BuildSummary> {
        private string _buildNumber;
        private DateTime _buildDate;
        private string _buildStatus;
        private string _buildUri;
        private string _dropLocation;

        public static readonly PropertyChangedEventArgs BuildNumberPropertyChangedEventArgs = new PropertyChangedEventArgs("BuildNumber");
        public static readonly PropertyChangedEventArgs BuildDatePropertyChangedEventArgs = new PropertyChangedEventArgs("BuildDate");
        public static readonly PropertyChangedEventArgs BuildStatusPropertyChangedEventArgs = new PropertyChangedEventArgs("BuildStatus");
        public static readonly PropertyChangedEventArgs DropLocationPropertyChangedEventArgs = new PropertyChangedEventArgs("DropLocation");

        public string BuildUri {
            get { return _buildUri; }
            internal set { _buildUri = value; }
        }

        public string BuildNumber {
            get { return _buildNumber; }
            set {
                _buildNumber = value;
                OnPropertyChanged(BuildNumberPropertyChangedEventArgs);
            }
        }

        public DateTime BuildDate {
            get { return _buildDate; }
            set {
                _buildDate = value;
                OnPropertyChanged(BuildDatePropertyChangedEventArgs);
            }
        }

        public string BuildStatus {
            get { return _buildStatus; }
            set {
                _buildStatus = value;
                OnPropertyChanged(BuildStatusPropertyChangedEventArgs);
            }
        }

        public string DropLocation {
            get { return _dropLocation; }
            set {
                _dropLocation = value;
                OnPropertyChanged(DropLocationPropertyChangedEventArgs);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, e);
            }
        }

        #region IComparable<BuildSummary> Members

        public int CompareTo(BuildSummary other) {
            return other.BuildDate.CompareTo(this.BuildDate);
        }

        #endregion
    }
}
