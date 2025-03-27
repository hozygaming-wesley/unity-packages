using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScriptEditorSwicherSetting", menuName = "ScriptEditorSwicher/ScriptEditorSwicherSetting", order = 0)]

public class ScriptEditorSwicherSetting : ScriptableObject
{
    public string VisualStudioCode;
    public string VisualStudio;

    // 製作一個按鈕在 inspector 上，用來自動尋找 Visual Studio 的安裝路徑
    [ContextMenu("Find Visual Studio Path")]
    public List<string> FindVisualStudioPath() {
        List<string> visualStudioPaths = VisualStudioFinder.GetVisualStudioPaths();
        Debug.Log("Visual Studio Paths:");
        foreach (var path in visualStudioPaths) {
            Debug.Log(path);
        }
        return visualStudioPaths;
    }
    // 製作一個按鈕在 inspector 上，用來自動尋找 Visual Studio Code 的安裝路徑
    [ContextMenu("Find Visual Studio Code Path")]
    public void FindVisualStudioCodePath() {
        List<string> visualStudioCodePaths = VisualStudioCodeFinder.GetVisualStudioCodePaths();
        Debug.Log("Visual Studio Code Paths:");
        foreach (var path in visualStudioCodePaths) {
            Debug.Log(path);
        }
        if (visualStudioCodePaths.Count > 0) {
            VisualStudioCode = visualStudioCodePaths[0];
        }
    }

}
