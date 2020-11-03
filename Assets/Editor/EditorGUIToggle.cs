using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorGUIToggle : EditorWindow
{
    bool open = true;
    bool close = false;
    string path = "";
    private Vector2 ve2;
    Dictionary<string, Item> other = new Dictionary<string, Item>();

    [MenuItem("Live2D/修改循环状态")]
    static void Init()
    {
        EditorGUIToggle window = (EditorGUIToggle)GetWindow(typeof(EditorGUIToggle), true, "Live2D动画循环批量修改器");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("批量选择：");
        open = GUILayout.Button("开",GUILayout.ExpandWidth(true));
        close = GUILayout.Button("关", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        if (open)
        {
            SetBool(true);
        }
        if (close)
        {
            SetBool(false);
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Live2D动画文件");
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
        path = EditorGUI.TextField(rect, path);
        GUILayout.EndHorizontal();
        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内  
        if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect.Contains(Event.current.mousePosition))
        {
            //改变鼠标的外表  
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                path = DragAndDrop.paths[0];
            }
        }

        if (Event.current.type != EventType.DragUpdated && Event.current.type != EventType.DragExited)
        {
            CreateList(path);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("修改动画循环状态"))
            {
                SetLoopValue();
                //this.Close();
            }
        }
        GUILayout.EndVertical();

    }

    private void SetLoopValue()
    {
        foreach (var item in other)
        {
            AnimationClip clip = item.Value.animationClip;
            AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
            clipSetting.loopTime = item.Value.ison;
            AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
        }
    }

    void CreateList(string fullPath)
    {
        if (fullPath == "") return;
        //获取指定路径下面的所有资源文件  
        if (System.IO.Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            ve2 = GUILayout.BeginScrollView(ve2);
            Debug.Log("fullPath:" + fullPath);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".anim"))
                {
                    GUILayout.BeginHorizontal();
         
                    if (!other.ContainsKey(files[i].Name))
                    {
                        other.Add(files[i].Name, new Item { ison = true });
                    }
                    //Debug.Log(files[i].Name+":"+ other[files[i].Name].ison);
                    other[files[i].Name].ison = EditorGUILayout.Toggle("", other[files[i].Name].ison);

                    AnimationClip animation = AssetDatabase.LoadAssetAtPath(Path.Combine(fullPath, files[i].Name), typeof(AnimationClip)) as AnimationClip;
                    other[files[i].Name].animationClip = animation;
                    AnimationClip targetGo = animation;
                    targetGo = (AnimationClip)EditorGUILayout.ObjectField("", targetGo, typeof(AnimationClip), false);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
    }

    void SetBool(bool _switch)
    {
        foreach (var item in other)
        {
            if (item.Value.ison == _switch) continue;
            item.Value.ison = _switch;
        }
    }
}


class Item
{
    public bool ison;
    public AnimationClip animationClip;
}