using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeControl : MonoBehaviour
{
    /// <summary>
    /// 蜂の移動速度
    /// </summary>
    public float Speed { set; private get; } = 1.0f;

    private Vector3 nowPos = Vector3.zero;

    private Coroutine coroutine = null;
    private SpriteRenderer spriteRenderer = null;
    private BeeHit beeHit = null;

    public bool HitFlag { private set; get; } = false;

    /// <summary>
    /// 移動座標の設定
    /// </summary>
    public Vector3 MovePos { set; private get; } = Vector3.zero;

    private bool moveFlag = false;

    public bool Chase { private set; get; } = false;

    // Update is called once per frame
    void Update()
    {
        BeeMove();

        GetPlayerHitInfo();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // 初回時の必要なコンポーネントを取得
        if(spriteRenderer == null)
        {
            spriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }

        if(beeHit == null)
        {
            beeHit = transform.GetChild(0).gameObject.GetComponent<BeeHit>();
        }

        Chase = false;
        HitFlag = false;
    }

    /// <summary>
    /// 蜂の移動関数
    /// </summary>
    private void BeeMove()
    {
        if(moveFlag == false) { return; }

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
        transform.position = Vector3.MoveTowards(nowPos, target, (Chase ? Speed : Speed * 0.5f) * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーと蜂が接触したかをチェックする
    /// </summary>
    private void GetPlayerHitInfo()
    {
        if(beeHit != null)
        {
            if (Chase == false || moveFlag == false) { beeHit.PlayerHit = false; }
            HitFlag = Chase && moveFlag && beeHit.PlayerHit;
        }
        else
        {
            HitFlag = false;
        }
    }

    /// <summary>
    /// 蜂の生成アニメーションを再生
    /// </summary>
    public void SpawnAnimation()
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(BeeAnimation());
    }

    /// <summary>
    /// 蜂の生成アニメーション
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeeAnimation()
    {
        if (spriteRenderer == null) { yield break; }

        // アニメーション開始前に初期化しておく
        moveFlag = false;
        Color baseColor = spriteRenderer.color;
        spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);

        // 1秒間に上方向に移動しながらフェードインを実行する
        float time = 0;
        float duration = 1.0f;
        Vector3 start = transform.position;
        Vector3 target = transform.position + Vector3.up * 1.0f;
        while (time < duration)
        {
            float diff = time / duration;
            transform.position = Vector3.Lerp(start, target, diff);
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * diff);
            time += Time.deltaTime;
            yield return null;
        }

        // アニメーション実行時間を経過したら正規化させる
        transform.position = target;
        spriteRenderer.color = baseColor;

        // 0.5秒遅延処理を挟む
        time = 0;
        duration = 0.5f;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 蜂の初期化処理を実行
        moveFlag = true;

        // コルーチン完了処理
        coroutine = null;
    }

    /// <summary>
    /// 蜂の視界内に入ったら追跡を開始
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Chase = true;
    }

    /// <summary>
    /// 蜂の視界外に行ったら追跡を終了
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        Chase = false;
    }
}
