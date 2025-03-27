using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class VisualStudioCodeFinder {
    public static List<string> GetVisualStudioCodePaths() {
        List<string> visualStudioCodePaths = new List<string>();
        // 預設安裝路徑
        var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        Debug.Log(appdata);
        //C:\Users\cips6\AppData\Local\Programs\Microsoft VS Code

        string[] defaultPaths = {
            $@"{appdata}\Programs\Microsoft VS Code\Code.exe",
        };
        foreach (var path in defaultPaths) {
            if (File.Exists(path)) {
                visualStudioCodePaths.Add(path);
            }
        }
        return visualStudioCodePaths;
    }
}