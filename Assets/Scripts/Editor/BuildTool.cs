using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    [MenuItem("Tools/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }
    [MenuItem("Tools/Build iPhone Bundle")]
    static void BundleIOSBuild()
    {
        Build(BuildTarget.iOS);
    }
   
    static void Build(BuildTarget target)
    {
       
        //收集打包的文件信息
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        //文件信息列表
        List<string> bundleInfos = new List<string>();
        //搜索文件路径，搜索匹配字符串，搜索选项
        string[] files = Directory.GetFiles(PathUtill.BuildResourcesPath, "*", SearchOption.AllDirectories);
        //筛选非meta文件
        for (int i = 0; i < files.Length; i++)
        {
     
            if (files[i].EndsWith(".meta"))
                continue;
       
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtill.GetStandardPath(files[i]);
            Debug.Log("file:" + fileName);
            string assetName = PathUtill.GetUnityPath(fileName);
            assetBundle.assetNames = new string[] { assetName };
            string bundleName = fileName.Replace(PathUtill.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);

            //添加文件和依赖信息
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName+".ab";

            if (dependenceInfo.Count > 0)
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

            bundleInfos.Add(bundleInfo);
        }
      

        if (Directory.Exists(PathUtill.BuildOutPath))
            Directory.Delete(PathUtill.BuildOutPath, true);
        Directory.CreateDirectory(PathUtill.BuildOutPath);

        //输出路径，Build的列表（需定义数组）,压缩格式，打包的目标平台）
        BuildPipeline.BuildAssetBundles(PathUtill.BuildOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);

        File.WriteAllLines(PathUtill.BuildOutPath + "/" + AppCounst.FileListName, bundleInfos);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
