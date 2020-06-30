using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///ゲームの進行度を管理するクラス
/// </summary>
public class Progress : MonoBehaviour
{

    //Instance
    static public Progress Instance;
    //進行度
    public int Degree;

    //アクションシーン開始時の進行度の初期化
    private void Awake()
    {
        Instance = new Progress();
        Instance.Degree = 0;
    }
    
}
