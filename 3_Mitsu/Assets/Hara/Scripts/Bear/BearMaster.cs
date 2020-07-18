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
    [SerializeField, Header("熊のスポーンレベル"), Range(0, 5)] public int spawnLevel = 1;
    [SerializeField, Header("熊が向かう場所(座標)")] private Vector3 bearTargetPos = Vector3.zero;
    [SerializeField, Header("有効範囲"), Range(0f, 5.0f)] private float targetArea = 1.0f;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        BearMasterInit();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBear();
        CheckBearState();
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

    /// <summary>
    /// 熊をスポーンさせる処理
    /// </summary>
    private void SpawnBear()
    {
        // ゲームモードがプレイ中かチェック
        bool isActve;
        try
        {
            isActve = GameStatus.Instance.gameMode == GameStatus.GameMode.Play;
        }
        catch
        {
            isActve = true;
        }

        if (isActve == false) { return; }

        float spawnDuration;

        // スポーンレベルに応じたスポーン間隔を設定、0ならばスポーンはしない
        if(spawnLevel < 1)
        {
            timer = 0;
            return;
        }
        else
        {
            spawnDuration = spawnTime - spawnTime * (0.2f * ((spawnLevel > 5 ? 5 : spawnLevel) - 1));
            if(spawnDuration < 0) { spawnDuration = 0; }
        }

        if(timer < spawnDuration)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;

        // 非アクティブな熊がいるかチェック
        int count = 0;
        foreach(var bear in bears)
        {
            if(bear.gameObject.activeSelf == false)
            {
                break;
            }
            count++;
        }
        if (count < bears.Length)
        {
            // スポーン位置の設定
            float angle = Random.Range(0, 360);
            float radius = Random.Range(8.0f, 10.0f);
            float rad = angle * Mathf.Deg2Rad;
            float px = Mathf.Cos(rad) * radius + bearTargetPos.x;
            float py = Mathf.Sin(rad) * radius + bearTargetPos.y;
            Vector2 spawnPos = new Vector2(px, py);

            bears[count].transform.position = spawnPos;
            bears[count].SpawnPos = spawnPos;
            bears[count].TargetPos = bearTargetPos;
            bears[count].gameObject.SetActive(true);
            bears[count].SpawnBear();
        }
    }

    /// <summary>
    /// 熊が製造機に着いたかをチェック
    /// </summary>
    private void CheckBearState()
    {
        foreach(var bear in bears)
        {
            if(Vector2.Distance(bear.transform.position, bearTargetPos) < targetArea)
            {
                bear.IsArrived = true;
            }
        }
    }
}
