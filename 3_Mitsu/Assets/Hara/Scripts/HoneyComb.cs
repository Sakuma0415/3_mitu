using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyComb : MonoBehaviour
{
    [SerializeField, Tooltip("蜂のPrefab")] private GameObject beeObject = null;
    [SerializeField, Tooltip("蜂の巣のSprite")] private SpriteRenderer honeySprite = null;
    private BeeControl bee = null;
    private SpriteRenderer beeSprite = null;
    private Coroutine coroutine = null;
    private Color baseHoneyColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 蜂に移動座標のデータを渡す
        SetData();


        // テストキー
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetHoney();
            Spawn();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        // 蜂の巣のベースカラーを取得
        if(honeySprite != null)
        {
            baseHoneyColor = honeySprite.color;
        }
    }

    /// <summary>
    /// 蜂の巣を取得する処理
    /// </summary>
    public void GetHoney()
    {
        // 蜂の巣を非表示にする
        if(honeySprite != null)
        {
            honeySprite.color = new Color(baseHoneyColor.r, baseHoneyColor.g, baseHoneyColor.b, 0);
        }

        // プレイヤー側にデータを渡す

    }

    /// <summary>
    /// 蜂を呼び出す関数
    /// </summary>
    public void Spawn()
    {
        // 蜂のインスタンスを作成
        if(beeObject != null && bee == null)
        {
            bee = Instantiate(beeObject).GetComponent<BeeControl>();

            if(beeSprite == null)
            {
                beeSprite = bee.transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
        }

        // 蜂の生成アニメーションを実行
        if(coroutine == null)
        {
            coroutine = StartCoroutine(BeeAnimation());
        }
    }

    /// <summary>
    /// テスト用の関数
    /// </summary>
    /// <returns></returns>
    private Vector3 MouseToWorld()
    {
        Vector3 mouse = Input.mousePosition;
        Camera main = Camera.main;
        mouse.z = 10;
        Vector3 pos = main.ScreenToWorldPoint(mouse);
        return pos;
    }

    /// <summary>
    /// 蜂に引き渡す情報
    /// </summary>
    private void SetData()
    {
        if(bee == null) { return; }
        bee.MovePos = MouseToWorld();
    }

    /// <summary>
    /// 蜂の生成アニメーション
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeeAnimation()
    {
        if(bee == null || beeSprite == null) { yield break; }

        // アニメーション開始前に初期化しておく
        bee.StartMove = false;
        bee.transform.position = transform.position;
        Color baseColor = beeSprite.color;
        beeSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);

        // 1秒間に上方向に移動しながらフェードインを実行する
        float time = 0;
        float duration = 1.0f;
        Vector3 start = bee.transform.position;
        Vector3 target = bee.transform.position + Vector3.up * 1.0f;
        while(time < duration)
        {
            float diff = time / duration;
            bee.transform.position = Vector3.Lerp(start, target, diff);
            beeSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * diff);
            time += Time.deltaTime;
            yield return null;
        }

        // アニメーション実行時間を経過したら正規化させる
        bee.transform.position = target;
        beeSprite.color = baseColor;

        // 0.5秒遅延処理を挟む
        time = 0;
        duration = 0.5f;
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 蜂の初期化処理を実行
        bee.Init();

        // コルーチン完了処理
        coroutine = null;
    }
}
