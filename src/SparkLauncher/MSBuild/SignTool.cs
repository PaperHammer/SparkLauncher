using Microsoft.Build.Framework;
using SparkLauncher.Common;
using SparkLauncher.Security;
using Task = Microsoft.Build.Utilities.Task;

// MSB4062 未能从程序集 SparkLauncher.dll 加载任务“SparkLauncher.MSBuild.SignTool”。
// Could not load file or assembly 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. 系统找不到指定的文件。

namespace SparkLauncher.MSBuild {
    //public class SignTool : Task {
    //    // 属性用于接收MSBuild传递的参数
    //    [Required]
    //    public string KeyOutputDir { get; set; } = string.Empty;

    //    [Required]
    //    public ITaskItem[] Files { get; set; } = [];

    //    // 执行任务的方法
    //    public override bool Execute() {
    //        try {
    //            GenerateKeyResponse? generateResponse = GenerateKeys();
    //            string[] files = Files?.Select(item => item.ItemSpec).ToArray() ?? [];
    //            _ = SignFiles(files, generateResponse.PrivateKeyFilePath, KeyOutputDir);

    //            return true;
    //        }
    //        catch (Exception ex) {
    //            Log.LogErrorFromException(ex);
    //            return false;
    //        }
    //    }

    //    private GenerateKeyResponse? GenerateKeys() {
    //        return SecurityMain.Run(CommandType.Generate, new GenerateKeyParams() {
    //            KeyOutputDir = this.KeyOutputDir,
    //        }) as GenerateKeyResponse;
    //    }

    //    private static SignResponse? SignFiles(string[] files, string privateKeyFilePath, string signatureOutputDir) {
    //        return SecurityMain.Run(CommandType.Sign, new SignParams() {
    //            FilePaths = files,
    //            PrivateKeyFilePath = privateKeyFilePath,
    //            SignatureOutputDir = signatureOutputDir
    //        }) as SignResponse;
    //    }
    //}
}
