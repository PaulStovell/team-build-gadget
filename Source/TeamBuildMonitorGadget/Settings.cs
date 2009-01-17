using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Deployment.Application;
using System.Xml.Serialization;
using System.Collections;
using System.Web;
using System.Reflection;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class Settings {
        private string _settingsFileName;
        private Dictionary<string, string> _innerSettings;

        private Settings(string fileName) {
            _settingsFileName = fileName;

            _innerSettings = new Dictionary<string,string>();
            if (File.Exists(_settingsFileName)) {
                string[] lines = File.ReadAllLines(_settingsFileName);
                foreach (string line in lines) {
                    string[] keyValuePair = line.Split('=');
                    string key = HttpUtility.UrlDecode(keyValuePair[0]);
                    string value = HttpUtility.UrlDecode(keyValuePair[1]);
                    _innerSettings[key] = value;
                }
            }
        }

        public string TeamFoundationServerUri {
            get { return GetSetting("TeamFoundationServerUri", "http://myserver:8080"); }
            set { SetSetting("TeamFoundationServerUri", value); }
        }

        public string TeamProjectName {
            get { return GetSetting("TeamProjectName", "MyProjectName"); }
            set { SetSetting("TeamProjectName", value); }
        }

        public string TeamBuildTypeName {
            get { return GetSetting("TeamBuildTypeName", "MyBuildName"); }
            set { SetSetting("TeamBuildTypeName", value); }
        }

        public string FriendlyName {
            get { return GetSetting("FriendlyName", "My project\r\nMy favourite build"); }
            set { SetSetting("FriendlyName", value); }
        }

        public string SettingsFileName {
            get { return _settingsFileName; }
        }

        private string GetSetting(string settingName, string defaultValue) {
            string result = defaultValue;
            if (_innerSettings.ContainsKey(settingName)) {
                result = _innerSettings[settingName];
            }
            return result;
        }

        private void SetSetting(string settingName, string value) {
            _innerSettings[settingName] = value;
        }

        public void Save() {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, string> kvp in _innerSettings) {
                lines.Add(HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value));
            }
            File.WriteAllLines(_settingsFileName, lines.ToArray());
        }

        public static Settings GetSettings() {
            string startDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("file:///", ""));
            string filename = Path.Combine(startDirectory, "PaulStovell.NET\\TeamBuildMonitor");
            if (!Directory.Exists(filename)) {
                Directory.CreateDirectory(filename);
            }
            filename = Path.Combine(filename, "Settings.xml");
            return new Settings(filename);
        }
    }
}
