using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態を管理するクラス
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [Header("設定項目")]

    //プレイヤーのプレハブ
    [SerializeField]
    GameObject playerPrefab;
    //プレイヤーの移動速度
    public float spead=0;
    //プレイヤーの最高速度になるまでの時間
    public float speadmaximumTime = 0;


    //Private
    //プレイヤーのオブジェクト
    GameObject playerObject;



    private void Start()
    {
        Init();
    }

    //起動時処理
    private void Init()
    {
        playerObject = Instantiate(playerPrefab);
        playerObject.GetComponent<PlayerMove>().playerStatus = this;
    }

    void Update()
    {
        
    }
}
