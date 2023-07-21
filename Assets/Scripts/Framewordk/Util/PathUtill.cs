using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtill 
{
    //��Ŀ¼
    public static readonly string AssetsPath = Application.dataPath;

    //��Ҫ��Bundle��Ŀ¼
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";

    //Bundle���Ŀ¼
    public static readonly string BuildOutPath = Application.streamingAssetsPath;

    //ֻ��Ŀ¼
    public static readonly string ReadPath = Application.streamingAssetsPath;

    //�ɶ�дĿ¼
    public static readonly string ReadWritePath = Application.persistentDataPath;

    //bundle��Դ·��
    public static string BundleResourcePath {
        get {
            if (AppCounst.GameMode == GameMode.UpdateMode)
                return ReadWritePath;
            return ReadPath;
        }
    }


    /// <summary>
    /// ��ȡUnity�����·��
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Substring(path.IndexOf("Assets"));
    }
    /// <summary>
    /// ��ȡ��׼·��
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        return path.Trim().Replace("\\", "/");
    }

    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScripts/{0}.bytes", name);
    }

    public static string GetUIPath(string name)
    {
        return string.Format("Assets/BuildResources/UI/prerfabs/{0}.prefab", name);
    }

    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Music/{0}", name);
    }

    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Sound/{0}", name);
    }

    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/BuildResources/Effect/prerfabs/{0}.prefab", name);
    }

    public static string GeSpritePath(string name)
    {
        return string.Format("Assets/BuildResources/Sprites/{0}", name);
    }

    public static string GetScenePath(string name)
    {
        return string.Format("Assets/BuildResources/Scenes/{0}.unity", name);
    }
}
