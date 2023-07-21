using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UObject = UnityEngine.Object;

/// <summary>
/// 1.解析版本文件 保存文件信息
/// 2.获取文件信息
/// 3.异步加载bundle
/// 4.检查依赖
/// 5.加载资源
/// 6.完成后回调
/// </summary>
public class ResourceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }
    //存放Bandle信息的集合
    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();
    //存放Bundle资源的集合
    //private Dictionary<string, BundleData> m_AssetBundles = new Dictionary<string, BundleData>();
    /// <summary>
    /// 解析版本文件
    /// </summary>
   public void ParseVersionFile()
    {
        //拿到版本文件路径
        string url = Path.Combine(PathUtill.BundleResourcePath, AppCounst.FileListName);
        string[] data = File.ReadAllLines(url);

        //解析文件信息
        for (int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];
            //list特性 带动态扩容
            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);

        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">完成回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName,Action<UObject> action = null)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtill.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;
        if(dependences !=null && dependences.Count > 0)
        {
            for (int i = 0; i < dependences.Count; i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;


        AssetBundleRequest bundleRequest= request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        //新颖的判空
        action?.Invoke(bundleRequest?.asset);
    }



#if UNITY_EDITOR
    /// <summary>
    /// 编辑器环境加载资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="action"></param>
    void EditorLoadAsset(string assetName, Action<UObject> action = null)
    {
        Debug.Log("this is EditorLoadAsset");
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UObject));
        if (obj == null)
            Debug.LogError("assets name is not exist:" + assetName);
        action?.Invoke(obj);
    }
#endif

    private void LoadAsset(string assetName, Action<UObject> action)
    {
#if UNITY_EDITOR
        if (AppCounst.GameMode == GameMode.EditorMode)
            EditorLoadAsset(assetName, action);
        else
#endif
            StartCoroutine(LoadBundleAsync(assetName, action));
    }

    //卸载

     //加载各类资源方法
    public void LoadUI(string assetName,Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtill.GetUIPath(assetName), action);
    }

    public void LoadMusic(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtill.GetMusicPath(assetName), action);
    }

    public void LoadSound(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtill.GetSoundPath(assetName), action);
    }

    public void LoadEffect(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtill.GetEffectPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtill.GetScenePath(assetName), action);
    }

  
}
