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
    //プレイヤーの移動速度の減少亮
    public float down = 0;
    //プレイヤーの最高速度になるまでの時間
    public float speadmaximumTime = 0;
    //プレイヤーのトランスフォーム
    public Transform PlayerTransform;

    //Private
    //プレイヤーのオブジェクト
    GameObject playerObject;
    [SerializeField]
    HoneyMaster honeyMaster;


    private void Start()
    {
        Init();
    }

    //起動時処理
    private void Init()
    {
        playerObject = Instantiate(playerPrefab);
        Camera.main.GetComponent<CameraMove>().SeeSet(playerObject);
        playerObject.GetComponent<PlayerMove>().playerStatus = this;
        PlayerTransform = playerObject.GetComponent<Transform>();
        ItemList.Instance.playerPos = playerObject.transform;
    }

    void Update()
    {
        if(honeyMaster.IsHitPlayer)
        {
            
            for(int i=0;i<ItemList.Instance.itemList.Length; i++)
            {
                if (ItemList.Instance.itemList[i]!=-1) {
                    if (ItemList.Instance.itemDataList[ItemList.Instance.itemList[i]].itemName == "ハチの巣")
                    {
                        ItemList.Instance.ItemLost(i);
                    }
                }
            }
            playerObject.transform.position = new Vector3(0, 0, 0);
        }
    }
}
