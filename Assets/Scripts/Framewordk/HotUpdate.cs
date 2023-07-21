using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UObject = UnityEngine.Object;

public class HotUpdate : MonoBehaviour
{
    byte[] m_ReadPathFileListData;
    byte[] m_ServerFileListData;
    internal class DownFileInfo
    {
        public string url;
        public string fileName;
        public DownloadHandler fileData;
    }
    /// <summary>
    /// 下载单个文件
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
  IEnumerator DownLoadFile(DownFileInfo info,Action<DownFileInfo> Complete)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(info.url);
        yield return webRequest.SendWebRequest();
        //if (webRequest.isHttpError || webRequest.isNetworkError)
        //{
        //    Debug.LogError("下载文件出错:" + info.url);
        //    yield break;
        //}
        info.fileData = webRequest.downloadHandler;
        Complete?.Invoke(info);
        webRequest.Dispose();
    }

    IEnumerator DownLoadFile(List<DownFileInfo> infos, Action<DownFileInfo> Complete,Action DownLoadAllComplete)
    {
        foreach (DownFileInfo info in infos)
        {
            yield return DownLoadFile(info, Complete);
        }
        DownLoadAllComplete?.Invoke();
    }

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <returns></returns>
    private List<DownFileInfo> GetFileList(string fileData,string path)
    {
        string content = fileData.Trim().Replace("\r", "");
        string[] files = content.Split('\n');
        List<DownFileInfo> downFileInfos = new List<DownFileInfo>(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            string[] info = files[i].Split('|');
            DownFileInfo fileInfo = new DownFileInfo();
            fileInfo.fileName = info[1];
            fileInfo.url = Path.Combine(path, info[1]);
            downFileInfos.Add(fileInfo);
        }
        return downFileInfos;
    }

    private void Start()
    {
        if (IsFirstInstall())
        {
            ReleaseResources();
        }
        else
        {
            CheckUpdate();
        }
    }

    private void CheckUpdate()
    {
        string url = Path.Combine(AppCounst.ResourcesUrl, AppCounst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info, OnDownLoadServerFileListComplete));

    }



    //释放资源
    private void ReleaseResources()
    {
        string url = Path.Combine(PathUtill.ReadPath, AppCounst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info,OnDownLoadReadPathFileListComplete));
    }

    private void OnDownLoadReadPathFileListComplete(DownFileInfo file)
    {
        m_ReadPathFileListData = file.fileData.data;
        List<DownFileInfo> fileInfos = GetFileList(file.fileData.text, PathUtill.ReadPath);
        StartCoroutine(DownLoadFile(fileInfos, OnReleaseFileComplete, OnReleaseAllFileComplete));
    }

    private void OnReleaseAllFileComplete()
    {
        FileUtill.WriteFile(Path.Combine(PathUtill.ReadWritePath, AppCounst.FileListName), m_ReadPathFileListData);
        CheckUpdate();
    }

    private void OnReleaseFileComplete(DownFileInfo fileInfo)
    {
        Debug.Log("OnReleaseFileComplete:" + fileInfo.url);
        string writeFile = Path.Combine(PathUtill.ReadWritePath, fileInfo.fileName);
        FileUtill.WriteFile(writeFile, fileInfo.fileData.data);
    }

    //判断是否初次安装
    private bool IsFirstInstall()
    {
        //判断只读目录是否存在版本文件
        bool isExistsReadPath = FileUtill.IsExists(Path.Combine(PathUtill.ReadPath, AppCounst.FileListName));

        //判断可读写目录是否存在版本文件
        bool isExistsReadWritePath = FileUtill.IsExists(Path.Combine(PathUtill.ReadWritePath, AppCounst.FileListName));

        return isExistsReadPath && !isExistsReadWritePath;
    }

    private void OnDownLoadServerFileListComplete(DownFileInfo file)
    {
        m_ServerFileListData = file.fileData.data;
        List<DownFileInfo> fileInfos = GetFileList(file.fileData.text, AppCounst.ResourcesUrl);
        List<DownFileInfo> downListFiles = new List<DownFileInfo>();

        for (int i = 0; i < fileInfos.Count; i++)
        {
            string localFile = Path.Combine(PathUtill.ReadWritePath, fileInfos[i].fileName);
            if (!FileUtill.IsExists(localFile))
            {
                fileInfos[i].url = Path.Combine(AppCounst.ResourcesUrl, fileInfos[i].fileName);
                downListFiles.Add(fileInfos[i]);
            }
        }
        if (downListFiles.Count > 0)
            StartCoroutine(DownLoadFile(fileInfos, OnUpdateFileComplete, OnUpdateAllFileComplete));
        else
            EnterGame();
    }



    private void OnUpdateAllFileComplete()
    {
        FileUtill.WriteFile(Path.Combine(PathUtill.ReadWritePath, AppCounst.FileListName), m_ServerFileListData);
        EnterGame();
    }

    private void OnUpdateFileComplete(DownFileInfo fileInfo)
    {
        Debug.Log("OnReleaseFileComplete:" + fileInfo.url);

        string writeFile = Path.Combine(PathUtill.ReadWritePath, fileInfo.fileName);
        FileUtill.WriteFile(writeFile, fileInfo.fileData.data);
    }

    private void EnterGame()
    {
        Manager.Resource.ParseVersionFile();
       Manager.Resource.LoadUI("TestUI", Oncomplete);
    }


    private void Oncomplete(UObject obj)
    {
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}
