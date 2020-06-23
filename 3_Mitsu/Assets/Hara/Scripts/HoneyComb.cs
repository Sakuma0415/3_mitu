using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyComb : MonoBehaviour
{
    private bool hitPlayer = false;

    /// <summary>
    /// 蜂の生成を許可するフラグ
    /// </summary>
    public bool SpawnFlag { set; get; } = false;

    /// <summary>
    /// 蜂の巣の設置個所ID
    /// </summary>
    public int LocationID { set; get; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // 蜂の巣を採取できる状態かチェック
        bool input = hitPlayer;

        if (input)
        {
            Debug.Log("蜂の巣とれるよ！");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetHoney();
            }
        }
    }

    /// <summary>
    /// 蜂の巣を取得する処理
    /// </summary>
    public void GetHoney()
    {
        // 蜂の巣を非表示にする
        gameObject.SetActive(false);

        // 蜂の呼び出しフラグをONにする
        SpawnFlag = true;

        // プレイヤー側にデータを渡す

    }

    /// <summary>
    /// 蜂の巣を生成する（表示する）
    /// </summary>
    public void SetHoney()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 蜂の巣とプレイヤーが接触しているとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitPlayer = true;
    }

    /// <summary>
    /// 蜂の巣とプレイヤーが離れたとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        hitPlayer = false;
    }
}
