using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHoneyComb : MonoBehaviour
{
    private bool hitPlayer = false;

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
        bool isItemFull;
        try
        {
            isItemFull = ItemList.Instance.noSpace;
        }
        catch
        {
            isItemFull = false;
        }

        // アイテムが満帆なら処理を終了
        if (isItemFull) { return; }

        // 蜂の巣を非表示にする
        gameObject.SetActive(false);

        // プレイヤー側にデータを渡す
        ItemList.Instance.ItemGet("ハチの巣");
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
