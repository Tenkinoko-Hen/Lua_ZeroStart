using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Loadab : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
      
        AssetBundleCreateRequest requestFab = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prerfabs/testui.prefab.ab");
        yield return requestFab;

        AssetBundleCreateRequest requestPic = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prerfabs/res/login/bg.jpg.ab");
        yield return requestPic;
        AssetBundleCreateRequest requestPic2 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prerfabs/res/login/test.png.ab");
        yield return requestPic2;


        AssetBundleRequest bundleRequestFab = requestFab.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/prerfabs/TestUI.prefab");
        yield return bundleRequestFab;

        GameObject go = Instantiate(bundleRequestFab.asset) as GameObject;
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
