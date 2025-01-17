namespace SparkLauncher.Common {
    public static class Constants {
        public static class Filds {
            public static string PrivateKeyFileName { get; } = "private_key.txt";
            public static string PublicKeyFileName { get; } = "public_key.txt";
        }

        public static class ModuleName {
            public static string WinUI_UIComponent { get; } = "SparkLauncher.WinUI.UIComponent";
        }

        public static class I18n {
            public static string Tasks_SelfCheck { get; } = "Tasks_SelfCheck";
            public static string Tasks_CheckUpdate { get; } = "Tasks_CheckUpdate";
            public static string Tasks_Download { get; } = "Tasks_Download"; 
            public static string Tasks_Verification { get; } = "Tasks_Verification";
            public static string Tasks_Callback { get; } = "Tasks_Callback";
        }

        public static class ApplicationType {
            public static bool IsTestBuild { get; } = false;
            public static bool IsMSIX { get; } = new DesktopBridge.Helpers().IsRunningAsUwp();
        }
    }
}
