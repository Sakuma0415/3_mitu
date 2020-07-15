using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{
    private SpriteRenderer bearSprite = null;

    public Vector3 SpawnPos { set; private get; } = Vector3.zero;

    public Vector3 TargetPos { set; private get; } = Vector3.zero;

    public bool IsArrived { set; private get; } = false;

    public float BearSpeed { set; private get; } = 1.0f;

    private Coroutine coroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        BearInit();
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
        if(bearSprite == null) { bearSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>(); }

        // 熊を停止状態にする
        IsArrived = true;
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
        Vector2 now = new Vector2(transform.position.x, transform.position.y);

        // 移動先の座標を取得
        Vector2 target = new Vector2(TargetPos.x, TargetPos.y);

        // 移動方向の角度を取得
        Vector2 diff = target - now;
        float radian = Mathf.Atan2(diff.y, diff.x);
        float degree = radian * Mathf.Rad2Deg;

        // 移動する方向を向く
        if(diff != Vector2.zero)
        {
            transform.rotation = Quaternion.Euler(0, 0, degree);
        }

        // 移動する処理
        if (IsArrived == false)
        {
            transform.position = Vector2.MoveTowards(now, target, BearSpeed * Time.deltaTime);
        }
    }

    public void SpawnBear()
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(SpawnAnimation());
    }

    /// <summary>
    /// 熊のスポーン時のアニメーション
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnAnimation()
    {
        IsArrived = true;

        if(bearSprite != null)
        {
            float time = 0;
            float duration = 1.0f;

            while (time < duration)
            {
                float rate = time / duration;
                bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, rate);
                time += Time.deltaTime;
                yield return null;
            }
            bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 1.0f);
        }

        IsArrived = false;
        coroutine = null;
    }

    /// <summary>
    /// 熊が森に帰る(退散する)処理
    /// </summary>
    /// <returns></returns>
    private IEnumerable ReturnAnimation()
    {
        IsArrived = true;
        TargetPos = SpawnPos;
        Vector2 dir = (transform.position - TargetPos).normalized;

        if (bearSprite != null)
        {
            float time = 0;
            float duration = 3.0f;

            while (time < duration)
            {
                float rate = time / duration;
                bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 1.0f - rate);
                transform.position = dir * BearSpeed * Time.deltaTime;
                time += Time.deltaTime;
                yield return null;
            }
            bearSprite.color = new Color(bearSprite.color.r, bearSprite.color.g, bearSprite.color.b, 0);
        }

        coroutine = null;

        gameObject.SetActive(false);
    }
}
