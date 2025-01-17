using System.ComponentModel;

namespace SparkLauncher.Common.Models {
    public class AppUpdaterEventArgs(AppUpdateStatus updateStatus, Version updateVersion, DateTime updateDate, Uri updateUri, string changeLog) : EventArgs {
        public AppUpdateStatus UpdateStatus { get; } = updateStatus;
        public Version UpdateVersion { get; } = updateVersion;
        public Uri UpdateUri { get; } = updateUri;
        public DateTime UpdateDate { get; } = updateDate;
        public string ChangeLog { get; } = changeLog;
    }

    public enum AppUpdateStatus {
        [Description("Software is up-to-date.")]
        Uptodate,
        [Description("Update available.")]
        Available,
        [Description("Installed software version higher than whats available online.")]
        Invalid,
        [Description("Update not checked yet.")]
        Notchecked,
        [Description("Update check failed.")]
        Error,
    }
}
