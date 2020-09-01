using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField, Tooltip("弾が当たるレイヤー")] private LayerMask targetLayer;
    private SpriteRenderer targetSprite = null;

    /// <summary>
    /// 銃の使用許可フラグ
    /// </summary>
    public bool IsCanUseGun { set; private get; } = false;

    private bool activeFlag = true;

    // 弾丸に当たったオブジェクトを保存しておく変数
    public GameObject HitObject { private set; get; } = null;


    // Start is called before the first frame update
    void Start()
    {
        targetSprite = GetComponent<SpriteRenderer>();
        IsCanUseGun = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlay;
        try
        {
            isPlay = GameStatus.Instance.gameMode == GameStatus.GameMode.Play;
        }
        catch
        {
            isPlay = true;
        }

        if(IsCanUseGun && isPlay)
        {
            activeFlag = true;
        }
        else
        {
            activeFlag = false;
        }

        SetTarget();
    }

    /// <summary>
    /// マウス座標をワールド座標に変換する
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
    /// 弾が当たったオブジェクトの情報を初期化する
    /// </summary>
    public void ResetHitObject()
    {
        if(HitObject != null) { HitObject = null; }
    }

    /// <summary>
    /// マウスに合わせて照準を動かす処理
    /// </summary>
    private void SetTarget()
    {
        if (activeFlag)
        {
            // マウスカーソルの設定
            Cursor.visible = false;

            // 照準アイコンの表示
            targetSprite.enabled = true;

            // 照準アイコンをマウスの座標に合わせる
            transform.position = MouseToWorld();

            // 弾丸を発射
            if (Input.GetMouseButtonDown(0))
            {
                Ray bulletRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2D = Physics2D.Raycast(bulletRay.origin, bulletRay.direction, 200, targetLayer);

                if (hit2D)
                {
                    HitObject = hit2D.transform.gameObject;
                }
                else
                {
                    ResetHitObject();
                }
            }
        }
        else
        {
            // マウスカーソルの設定を解除
            Cursor.visible = true;

            // 照準アイコンの非表示
            targetSprite.enabled = false;

            // 当たったオブジェクト情報をリセットする
            ResetHitObject();
        }
    }
}
