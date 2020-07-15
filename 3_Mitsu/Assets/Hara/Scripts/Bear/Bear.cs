using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    private SpriteRenderer bearSprite = null;

    public Vector3 TargetPos { set; private get; } = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BearMove();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void BearInit()
    {
        bearSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void BearMove()
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

        if(isActve == false) { return; }

        // 現在座標を取得
        Vector3 now = transform.position;
        now.z = 0;

        // 移動先の座標を取得
        Vector3 target = TargetPos;
        target.z = 0;

        // 移動方向を算出
        Vector3 direction = (now - target).normalized;

        // 移動する方向を向く
        if(direction != Vector3.zero)
        {
            
        }
    }
}
