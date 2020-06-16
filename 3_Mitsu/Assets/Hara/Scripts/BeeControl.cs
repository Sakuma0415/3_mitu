using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeControl : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度"), Range(1, 5)] private float speed = 1.0f;
    private Vector3 nowPos = Vector3.zero;

    /// <summary>
    /// 移動座標の設定
    /// </summary>
    public Vector3 MovePos { set; private get; } = Vector3.zero;

    /// <summary>
    /// 移動処理の実行を管理するフラグ
    /// </summary>
    public bool StartMove { set; private get; } = false;

    // Update is called once per frame
    void Update()
    {
        BeeMove();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        StartMove = true;
    }

    /// <summary>
    /// 蜂の移動関数
    /// </summary>
    private void BeeMove()
    {
        if(StartMove == false) { return; }

        // 現在の位置情報を更新
        nowPos = new Vector3(transform.position.x, transform.position.y, 0);

        // 移動先の座標を設定する
        Vector3 target = new Vector3(MovePos.x, MovePos.y, 0);

        // 移動方向によって向きを変える
        if (nowPos != target)
        {
            if ((target - nowPos).normalized.x > 0)
            {
                // 反転
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            
            if((target - nowPos).normalized.x < 0)
            {
                // 正方向
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }

        // 移動処理
        transform.position = Vector3.MoveTowards(nowPos, target, speed * Time.deltaTime);
    }
}
