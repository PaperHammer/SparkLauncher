using SparkLauncher.Common.Models;

namespace SparkLauncher.Common.Interfaces
{
    public interface IAppUpdater
    {
        event EventHandler<AppUpdaterEventArgs> UpdateChecked;

        string LastCheckChangelog { get; }
        DateTime LastCheckTime { get; }
        Uri LastCheckUri { get; }
        Version LastCheckVersion { get; }
        AppUpdateStatus Status { get; }

        Task<AppUpdateStatus> CheckUpdate(int fetchDelay = 45000);
        Task<(Uri, Version, string)> GetLatestRelease(bool isBeta);
        void Start();
        void Stop();
    }
}
