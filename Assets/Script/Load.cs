using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class Load : MonoBehaviour
{
    public Canvas Canvas;
    private GameObject panelPrefab;
    GameObject gameobject;
    Dictionary<string,AssetBundle> abs=new Dictionary<string, AssetBundle>();
    string loadABName= "prfb.unity";
    string loadObjName = "Image.prefab";
    AssetBundle loadAB;
    private AssetBundle loadAB1;
    private AssetBundle loadAB2;

    void LoadAB()
    {
        var assetBundle = AssetBundle.LoadFromFile("AssetBundles/AssetBundles");
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("assetBundleManifest");
        assetBundle.Unload(false);
        assetBundle = null;
        if (manifest == null)
        {
            Debug.Log("无");

        }
        var abNames = manifest.GetAllDependencies(loadABName);
        for (int i = 0; i < abNames.Length; i++)
        {
            Debug.Log("包名：" + abNames[i]);
            if (!abs.ContainsKey(abNames[i]))
            {
                abs.Add(abNames[i], AssetBundle.LoadFromFile("AssetBundles/" + abNames[i]));
            }
        }
        if (!abs.ContainsKey(loadABName))
        {
            loadAB = AssetBundle.LoadFromFile("AssetBundles/" + loadABName);
            abs.Add(loadABName, loadAB);
        }
        panelPrefab = loadAB.LoadAsset<GameObject>(loadObjName);
    }

    void Create()
    {
        gameobject = Instantiate(panelPrefab, Canvas.transform);
    }

    void GetObj()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDestroyObject();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //loadAB.Unload(true);
            //loadAB = null;

            UnLoadBundle();
           
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            UnLoadUnusedAsset();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadAB();
            Create();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            loadAB1 = AssetBundle.LoadFromFile("AssetBundles/adf.unity");
            loadAB2 = AssetBundle.LoadFromFile("AssetBundles/qr.unity");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoadSprite();
        }
    }

    private void OnDestroyObject()
    {
        Destroy(gameobject);
        try
        {
            UnLoadBundle();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        try
        {
            UnLoadUnusedAsset();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void UnLoadBundle()//看资源加载频率决定是否卸载资源（及卸载时机） 个别包仍被场景引用（报错）try
    {
        foreach (var item in abs)
        {
            item.Value.Unload(false);//卸载包（true 包含已load的，false 不包含）
        }
        abs.Clear();
        gameobject = null;
        //AssetBundle.UnloadAllAssetBundles(false);//卸载为已load的包 效率低
    }

    void UnLoadUnusedAsset()
    {
        Resources.UnloadUnusedAssets();//卸载未引用的资源 调了Bundle.Unload(true);才调这个
        //Resources.UnloadAsset(loadAB);//卸载指定资源（除了gameobject compoment）
    }

    void LoadSprite()
    {
        SpriteAtlas a = Resources.Load<SpriteAtlas>("New Sprite Atlas");
       Sprite sprite= a.GetSprite("jia2");
        gameobject.GetComponent<Image>().sprite = sprite;
    }
}
