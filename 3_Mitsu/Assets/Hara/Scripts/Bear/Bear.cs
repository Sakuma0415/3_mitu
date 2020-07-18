﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    [SerializeField, Tooltip("熊のSpriteRenderer")]
    private SpriteRenderer bearSprite = null;

    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;
    public Vector3 SpawnPos { set { spawnPos = value; } }

    [SerializeField]
    private Vector3 targetPos = Vector3.zero;
    public Vector3 TargetPos { set { targetPos = value; } }

    public float BearSpeed { set; private get; } = 1.0f;

    [SerializeField]
    private bool isCanMove = false;
    public bool IsCanMove { set { isCanMove = value; } }

    public enum BearAnimationMode
    {
        Spawn,
        Remove
    }
    private BearAnimationMode bearAnimeMode = BearAnimationMode.Spawn;
    public BearAnimationMode BearAnimeMode { set { bearAnimeMode = value; } }

    private float bearAnimeTimer = 0;
    public float BearAnimeTimer { set { bearAnimeTimer = value; } }

    private float animeDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        // 移動処理
        BearMove();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void BearInit()
    {
        bearAnimeTimer = 0;
        bearAnimeMode = BearAnimationMode.Spawn;
        isCanMove = true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void BearMove()
    {
        

        // 移動する処理
        if (isCanMove == true)
        {
            if(bearSprite != null)
            {
                float rate = bearAnimeTimer / animeDuration;
                if(bearAnimeMode == BearAnimationMode.Spawn)
                {
                    if(bearAnimeTimer < animeDuration)
                    {
                        bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, rate);
                    }
                    else
                    {
                        bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 1.0f);
                    }
                    
                }
                else
                {
                    if (bearAnimeTimer < animeDuration)
                    {
                        bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 1.0f - rate);
                    }
                    else
                    {
                        bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 0f);
                        gameObject.SetActive(false);
                    }

                    targetPos = spawnPos;
                }
                bearAnimeTimer += Time.deltaTime;
            }

            // 現在座標を取得
            Vector2 now = transform.position;

            // 移動先の座標を取得
            Vector2 target = new Vector2(targetPos.x, targetPos.y);

            // 移動方向の角度を取得
            Vector2 diff = target - now;
            float radian = Mathf.Atan2(diff.y, diff.x);
            float degree = radian * Mathf.Rad2Deg;

            // 移動する方向を向く
            if (diff != Vector2.zero)
            {
                transform.rotation = Quaternion.Euler(0, 0, degree);
            }
            transform.position = Vector2.MoveTowards(now, target, BearSpeed * Time.deltaTime);
        }
    }
}
