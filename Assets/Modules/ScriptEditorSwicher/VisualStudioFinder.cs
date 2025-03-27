using System.Collections.Generic;
using System.IO;

public static class VisualStudioFinder {
    public static List<string> GetVisualStudioPaths() {
        List<string> visualStudioPaths = new List<string>();

        // 預設安裝路徑
        string[] defaultPaths = {
            @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe",
            @"C:\Program Files\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe",
            @"C:\Program Files\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe",
            @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe",
        };

        foreach (var path in defaultPaths) {
            if (File.Exists(path)) {
                visualStudioPaths.Add(path);
            }
        }

        return visualStudioPaths;
    }
}
