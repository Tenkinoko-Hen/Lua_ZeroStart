using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameStart : MonoBehaviour
{
    public GameMode gameMode;
    // Start is called before the first frame update
    void Awake()
    {
        AppCounst.GameMode = gameMode;
        DontDestroyOnLoad(this);
    }
    
}
