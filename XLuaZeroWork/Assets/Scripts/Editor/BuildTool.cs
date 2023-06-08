using System.Collections;
using System.Collections.Generic;
using System.IO;
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
       
        //�ռ�������ļ���Ϣ
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        //�����ļ�·��������ƥ���ַ���������ѡ��
        string[] files = Directory.GetFiles(PathUtill.BuildResourcesPath, "*", SearchOption.AllDirectories);
        //ɸѡ��meta�ļ�
        for (int i = 0; i < files.Length; i++)
        {
     
            if (files[i].EndsWith(".meta"))
                continue;
       
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileNmae = PathUtill.GetStandardPath(files[i]);
            Debug.Log("file:" + fileNmae);
            string assetName = PathUtill.GetUnityPath(files[i]);
            assetBundle.assetNames = new string[] { assetName };
            string bundleName = fileNmae.Replace(PathUtill.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);
        }
        if (Directory.Exists(PathUtill.BuildOutPath))
            Directory.Delete(PathUtill.BuildOutPath, true);
        Directory.CreateDirectory(PathUtill.BuildOutPath);

        //���·����Build���б��趨�����飩,ѹ����ʽ�������Ŀ��ƽ̨��
        BuildPipeline.BuildAssetBundles(PathUtill.BuildOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);


    }
}
