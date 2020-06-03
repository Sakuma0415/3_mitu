using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeControl : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度"), Range(1, 5)] private float speed = 1.0f;

    private Vector3 spawnPos = Vector3.zero;
    private Vector3 nowPos = Vector3.zero;

    /// <summary>
    /// プレイヤーを追従する
    /// </summary>
    public bool MoveToPlayer { set; private get; } = false;

    /// <summary>
    /// 移動座標の設定
    /// </summary>
    public Vector3 MovePos { set; private get; } = Vector3.zero;

    private GameObject[] beeObject = null;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        MovePos = MouseToWorld();
        SetMoveFlag();

        BeeMove();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        beeObject = new GameObject[transform.childCount];
        for(int i = 0; i < beeObject.Length; i++)
        {
            beeObject[i] = transform.GetChild(i).gameObject;
        }

        // スポーン地点を保存
        spawnPos = new Vector3(transform.position.x, transform.position.y, 0);

        MoveToPlayer = true;
    }

    /// <summary>
    /// 蜂の移動関数
    /// </summary>
    private void BeeMove()
    {
        // 現在の位置情報を更新
        nowPos = new Vector3(transform.position.x, transform.position.y, 0);

        // 移動先の座標を設定する
        Vector3 target = MoveToPlayer ? new Vector3(MovePos.x, MovePos.y, 0) : spawnPos;

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
    /// テスト用の関数
    /// </summary>
    private void SetMoveFlag()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToPlayer = !MoveToPlayer;
        }
    }
}
