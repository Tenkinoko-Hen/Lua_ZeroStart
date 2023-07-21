using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 监听者模式
/// 从Manger这个方法找到所有的管理器，可以调用所有管理器的方法
/// </summary>
public class Manager : MonoBehaviour
{
    private static ResourceManager _resource;
    public static ResourceManager Resource
    {
        get { return _resource; }
    }

    private void Awake()
    {
        //实例化
        _resource = this.gameObject.AddComponent<ResourceManager>();
    }
}
