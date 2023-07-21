using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

class FileUtill
{
    /// <summary>
    /// 检测文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        FileInfo file = new FileInfo(path);
        return file.Exists;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteFile(string path,byte[] data)
    {
        //获取标注路径
        path = PathUtill.GetStandardPath(path);
        //文件夹的路径
        string dir = path.Substring(0, path.LastIndexOf("/"));
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
        }
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }

        }
        catch(IOException e)
        {
            Debug.LogError(e.Message);
        }
    }
 }


