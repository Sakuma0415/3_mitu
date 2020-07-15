using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMaster : MonoBehaviour
{
    [SerializeField, Tooltip("熊のPrefab")] private Bear bearPrefab = null;
    private Bear[] bears = null;

    [SerializeField, Header("熊の生成上限数"), Range(1, 10)] private int maxBear = 5;
    [SerializeField, Header("熊の移動速度"), Range(1.0f, 5.0f)] private float bearSpeed = 1.0f;
    [SerializeField, Header("熊の基本スポーン間隔"), Range(0f, 20.0f)] private float spawnTime = 10.0f;
    [SerializeField, Header("熊のスポーンレベル"), Range(0, 5)] private int spawnLevel = 1;
    [SerializeField, Header("熊が向かう場所(座標)")] private Vector3 bearTargetPos = Vector3.zero;
    [SerializeField, Header("有効範囲"), Range(0f, 5.0f)] private float targetArea = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        BearMasterInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void BearMasterInit()
    {
        CreateBear();
    }

    /// <summary>
    /// 熊を生成する処理
    /// </summary>
    private void CreateBear()
    {
        bears = new Bear[maxBear];
        for(int i = 0; i < bears.Length; i++)
        {
            bears[i] = Instantiate(bearPrefab);
            bears[i].BearInit();
            bears[i].gameObject.SetActive(false);
        }
    }
}
