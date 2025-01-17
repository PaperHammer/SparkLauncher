using SparkLauncher.Common;

namespace SparkLauncher.Tasks.Presets {
    public class CheckUpdate : TaskHandler {
        public override string TaskName => Constants.I18n.Tasks_CheckUpdate;

        public CheckUpdate(TaskRequest? request = null) {
            Request = request ?? new();
        }

        public override void Handle() {
            HandleAsync().GetAwaiter().GetResult();
        }

        public override async Task HandleAsync() {
            try {
                Status = RuntimeStatus.Pending;
                await Task.Delay(1000);

                Request ??= new CheckUpdateRequest();
                if (Request is not CheckUpdateRequest privateArgs) throw new ArgumentException();
                AddMsg(TaskMsgType.Info, "CheckUpdate running...");

                
                AddMsg(TaskMsgType.Info, "CheckUpdate done.");
            }
            catch (Exception ex) {
                Status = RuntimeStatus.Failed;
                AddMsg(TaskMsgType.Error, $"Exception-{ex.Message}");
                if (Request == null || !Request.IgnoreError) {
                    throw;
                }
                AddMsg(TaskMsgType.Warn, $"CheckUpdate IgnoreError.");
            }
            finally {
                // 避免覆盖 Failed
                if (Status == RuntimeStatus.Pending) {
                    Status = RuntimeStatus.Succeeded;
                }
                await Task.Delay(500);
            }

        }
    }

    public class CheckUpdateRequest : TaskRequest {
        public string TargetDirectory { get; set; } = string.Empty;
    }
}
