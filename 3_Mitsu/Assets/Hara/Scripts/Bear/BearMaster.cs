﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMaster : MonoBehaviour
{
    [SerializeField, Tooltip("熊のPrefab")] private Bear bearPrefab = null;
    [SerializeField, Tooltip("銃のオブジェクト")] private Gun gun = null;
    private BearUI bearUI = null;
    private Bear[] bears = null;
    private bool uiFlag = false;
    
    private enum BearState
    {
        Standby,
        Start,
        Arrived,
        Return,
        Death
    }
    private BearState[] bearStates = null;

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
        CheckBearState();
        LevelCheck();
        SpawnBear();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void BearMasterInit()
    {
        CreateBear();
        bearUI = GetComponent<BearUI>();
        bearUI.Init();
    }

    /// <summary>
    /// 熊を生成する処理
    /// </summary>
    private void CreateBear()
    {
        bears = new Bear[maxBear];
        bearStates = new BearState[bears.Length];
        for (int i = 0; i < bears.Length; i++)
        {
            bears[i] = Instantiate(bearPrefab);
            bears[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bearStates.Length; i++)
        {
            bearStates[i] = BearState.Standby;
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

        // 熊を生成できる状態かチェック
        int count = 0;
        foreach(var state in bearStates)
        {
            if(state == BearState.Standby)
            {
                break;
            }
            count++;
        }

        bool isCanSpawn = count < bearStates.Length && spawnLevel > 0;
        if (isCanSpawn)
        {
            float spawnDuration = spawnTime - spawnTime * (0.2f * ((spawnLevel > 5 ? 5 : spawnLevel) - 1));
            if (spawnDuration < 0) { spawnDuration = 0; }

            if (timer < spawnDuration)
            {
                timer += Time.deltaTime;
                return;
            }
            timer = 0;

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
            bears[count].BearSpeed = bearSpeed;
            bearStates[count] = BearState.Start;
            bears[count].BearInit();
            bears[count].gameObject.SetActive(true);
        }
        else
        {
            timer = 0;
        }
    }

    /// <summary>
    /// 熊のステータスをチェック
    /// </summary>
    private void CheckBearState()
    {
        
        for(int i = 0; i < bears.Length; i++)
        {
            // 熊が製造機にたどり着いたかをチェック
            if (Vector2.Distance(bears[i].transform.position, bearTargetPos) < targetArea && bearStates[i] == BearState.Start)
            {
                bears[i].IsCanMove = false;
                bearStates[i] = BearState.Arrived;
            }

            // 非アクティブの熊がいるかをチェック
            if (bears[i].gameObject.activeSelf == false)
            {
                bearStates[i] = BearState.Standby;
            }

            // 銃に当たった熊を検知し、撃退処理を実行させる
            if (gun != null && gun.HitObject != null)
            {
                if (gun.HitObject == bears[i].gameObject && (bearStates[i] == BearState.Start || bearStates[i] == BearState.Arrived))
                {
                    bearStates[i] = BearState.Death;
                    bears[i].IsDeath = true;
                    bears[i].DeathAnimation();
                    gun.ResetHitObject();
                }
            }

            // Endフラグを検知したら熊を退去(非表示)させる
            if(bears[i].IsEnd)
            {
                bearStates[i] = BearState.Standby;
                bears[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 拠点に到達している熊の数を取得
    /// </summary>
    /// <returns></returns>
    public int ArrivedCount()
    {
        int count = 0;
        foreach(var state in bearStates)
        {
            if(state == BearState.Arrived)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// スポーンレベルをチェックしてUIの表示を行う
    /// </summary>
    private void LevelCheck()
    {
        if(bearUI == null) { return; }

        if(spawnLevel > 0 && uiFlag == false)
        {
            uiFlag = true;
            bearUI.BearUIAction();
        }

        if(spawnLevel < 1 && uiFlag)
        {
            uiFlag = false;
        }
    }
}
