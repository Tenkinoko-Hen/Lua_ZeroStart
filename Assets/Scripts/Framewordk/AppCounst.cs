using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum GameMode {
    EditorMode,
    PackgeBundle,
    UpdateMode,
}


public  class AppCounst
{
    public const string BundleExtension = ".ab";
    public const string FileListName = "filelist.txt";

    public static GameMode GameMode = GameMode.EditorMode;

    public const string ResourcesUrl = "http://192.168.4.101/AssetBundles";
}

