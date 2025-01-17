#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Wesley.Tool
{
    public class RenameTool : EditorWindow
    {
        #region members
        private Vector2 m_scrollPos;
        private string m_styleName = string.Empty;
        private string m_splitSymbol = string.Empty;
        private string m_insertFrontName = string.Empty;
        private string m_originalName = string.Empty;
        private string m_SecondName = string.Empty;
        private string m_insertBackName = string.Empty;
        private string m_previewName = string.Empty;
        private string m_startNum_str = "0";
        private int m_startNum_int = 0;
        #endregion
        #region get window
        [MenuItem("Wesley/Rename Tool")]
        private static void OpenWindow()
        {
            GetWindow<RenameTool>("Rename Tool").Show();
        }
        #endregion
        #region render the window
        private void OnGUI()
        {
            m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);
            {

                GUILayout.BeginVertical();
                {
                    #region text input

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Split Symbol : ");
                        m_splitSymbol = GUILayout.TextField(m_splitSymbol, GUILayout.Width(30));

                        GUILayout.Label("Style Name : ");
                        m_styleName = GUILayout.TextField(m_styleName, GUILayout.Width(70));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Front Name: ");
                        m_insertFrontName = GUILayout.TextField(m_insertFrontName, GUILayout.Width(70));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Original Name: ");
                        m_originalName = GUILayout.TextField(m_originalName, GUILayout.Width(70));
                    }
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Back Name: ");
                        m_insertBackName = GUILayout.TextField(m_insertBackName, GUILayout.Width(70));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Second Name: ");
                        m_SecondName = GUILayout.TextField(m_SecondName, GUILayout.Width(70));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Start Number: ");
                        m_startNum_str = GUILayout.TextField(m_startNum_str, GUILayout.Width(60));
                        try
                        {
                            m_startNum_int = int.Parse(m_startNum_str);
                        }
                        catch
                        {

                        }
                    }
                    GUILayout.EndHorizontal();



                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Preview:");
                        GUILayout.Label(m_previewName);

                    }
                    GUILayout.EndHorizontal();




                    #endregion
                    #region button
                    bool hasObject = (Selection.objects.Length > 0);
                    GUI.enabled = hasObject;

                    GUILayout.FlexibleSpace();
                    if (!hasObject)
                    {
                        GUI.color = Color.red;
                        GUILayout.Button("No Selected Objects!");
                        GUI.color = Color.white;

                    }
                    else
                    {
                    }
                    if (GUILayout.Button("Preview"))
                    {
                        Preview();
                    }
                    if (GUILayout.Button("Rename"))
                    {
                        Rename();

                    }
                    #endregion
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
        #endregion
        #region Rename Function
        void Preview()
        {
            if (!docked) return;
            string insertFront = m_insertFrontName.Trim();//去除头尾空白字符串
            string insertBack = m_insertBackName.Trim();//去除头尾空白字符串
            int index = m_startNum_int;

            if ((name + index) != string.Empty)//若名字不为空
            {

                foreach (Object o in Selection.objects)
                {
                    bool doubleSplit = Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[1].Value != string.Empty | m_SecondName != string.Empty;
                    string styleName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[1].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[1].Value;

                    string originalName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[3].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[3].Value;
                    string secondName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[5].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[5].Value;


                    m_previewName = "";
                    m_previewName += m_styleName != string.Empty ? m_styleName : styleName;
                    m_previewName += m_splitSymbol != string.Empty ? m_splitSymbol : "";
                    m_previewName += insertFront ?? "";
                    m_previewName += m_originalName != string.Empty ? m_originalName : originalName;
                    m_previewName += insertBack ?? "";
                    m_previewName += doubleSplit ? m_splitSymbol : "";
                    m_previewName += doubleSplit & m_SecondName != string.Empty ? m_SecondName : secondName;
                    m_previewName += m_startNum_str != string.Empty ? index.ToString() : "";

                    index++;
                }

            }
        }
        private void Rename()
        {
            string insertFront = m_insertFrontName.Trim();//去除头尾空白字符串
            string insertBack = m_insertBackName.Trim();//去除头尾空白字符串


            int index = m_startNum_int;
            if ((name + index) != string.Empty)//若名字不为空
            {
                bool isAssetsObject = false;//flag, 是否是assets文件夹的资源

                foreach (Object o in Selection.objects)
                {
                    bool doubleSplit = Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[1].Value != string.Empty | m_SecondName != string.Empty;
                    string styleName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[1].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[1].Value;

                    string originalName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[3].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[3].Value;
                    string secondName = doubleSplit ?
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)({m_splitSymbol})(.*)").Groups[5].Value :
                        Regex.Match(o.name, @$"(.*)({m_splitSymbol})(.*)").Groups[5].Value;

                    string path_g = AssetDatabase.GetAssetPath(o);//获得选中物的路径
                    //查看路径后缀
                    if (Path.GetExtension(path_g) != "")//若后缀不为空, 则为assets文件夹物体
                    {
                        if (name.Length >= 2 && name.Substring(0, 2) == "m_")// m_ 开头会被吞
                        {
                            //用 M_ 修正
                            string temp_name = name.Remove(0, 1);
                            name = temp_name.Insert(0, "M");
                        }

                        o.name = "";
                        o.name += m_styleName != string.Empty ? m_styleName : styleName;
                        o.name += m_splitSymbol != string.Empty ? m_splitSymbol : "";
                        o.name += insertFront ?? "";
                        o.name += m_originalName != string.Empty ? m_originalName : originalName;
                        o.name += insertBack ?? "";
                        o.name += doubleSplit ? m_splitSymbol : "";
                        o.name += doubleSplit & m_SecondName != string.Empty ? m_SecondName : secondName;
                        o.name += m_startNum_str != string.Empty ? index.ToString() : "";
                        AssetDatabase.RenameAsset(path_g, o.name);//改名API
                        if (!isAssetsObject)
                        {
                            isAssetsObject = true;//修改flag
                        }
                    }
                    else//后缀为空, 是场景物体
                    {
                        o.name = "";
                        o.name += m_styleName != string.Empty ? m_styleName : styleName;
                        o.name += m_splitSymbol != string.Empty ? m_splitSymbol : "";
                        o.name += insertFront ?? "";
                        o.name += m_originalName != string.Empty ? m_originalName : originalName;
                        o.name += insertBack ?? "";
                        o.name += doubleSplit ? m_splitSymbol : "";
                        o.name += doubleSplit & m_SecondName != string.Empty ? m_SecondName : secondName;
                        o.name += m_startNum_str != string.Empty ? index.ToString() : "";


                    }
                    index++;
                    Undo.RegisterFullObjectHierarchyUndo(o, "Rename");
                }
                if (isAssetsObject)//若是assets文件夹资源, 则刷新assets
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
        #endregion
    }
}
#endif