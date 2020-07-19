using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField, Tooltip("弾が当たるレイヤー")] private LayerMask targetLayer;
    private SpriteRenderer targetSprite = null;

    [SerializeField, Header("残弾数")] private int bullet = 10;
    public int Bullet { set { bullet = value; } get { return bullet; } }

    private bool isCanUseGun = true;

    private bool activeFlag = true;

    // 弾丸に当たったオブジェクトを保存しておく変数
    public GameObject HitObject { private set; get; } = null;


    // Start is called before the first frame update
    void Start()
    {
        targetSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            isCanUseGun = GameStatus.Instance.gameMode == GameStatus.GameMode.Play;
        }
        catch
        {
            isCanUseGun = true;
        }

        if(isCanUseGun)
        {
            if (Input.GetKeyDown(KeyCode.E)) { activeFlag = !activeFlag; }
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
        bullet = Mathf.Max(0, bullet);

        if (activeFlag)
        {
            // マウスカーソルの設定
            Cursor.visible = false;

            // 照準アイコンの表示
            targetSprite.enabled = true;

            // 照準アイコンをマウスの座標に合わせる
            transform.position = MouseToWorld();

            // 弾丸を発射
            if (Input.GetMouseButtonDown(0) && bullet > 0)
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

                bullet--;
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
