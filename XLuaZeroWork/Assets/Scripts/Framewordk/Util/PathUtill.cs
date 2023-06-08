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
}