using System.IO;
using UnityEditor;
using UnityEngine;

public class AB : EditorWindow
{
    private bool open;

    [MenuItem("卡进度/修改循环状态")]
    static void AB_Built()
    {
        string dir = "AssetBundles"; //相对路径
        if (!Directory.Exists(dir))   //判断是否存在
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //AB window = (AB)GetWindow(typeof(AB), true, "AB");
        //window.Show();
        
    }

    private void OnGUI()
    {
        //open = GUILayout.Button("开", GUILayout.ExpandWidth(true));
        //if (open)
        //{

        //}
      
    }
}
