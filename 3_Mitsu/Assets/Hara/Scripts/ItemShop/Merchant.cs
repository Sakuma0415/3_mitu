using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    private bool hitPlayer = false;

    [SerializeField, Tooltip("アイテムショップ")] private ItemShop shop = null;

    // Start is called before the first frame update
    void Start()
    {
        MercantInit();
    }

    private void Update()
    {
        // 蜂の巣を採取できる状態かチェック
        bool input = hitPlayer;
        bool isPlay;

        try
        {
            isPlay = GameStatus.Instance.gameMode == GameStatus.GameMode.Play;
        }
        catch
        {
            isPlay = true;
        }

        if (input && isPlay)
        {
            Debug.Log("アイテム買えるよ");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowItemShop();
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void MercantInit()
    {
        if(shop != null)
        {
            shop.ShopInit();
        }
    }

    /// <summary>
    /// アイテムショップにアクセスする
    /// </summary>
    private void ShowItemShop()
    {
        if(shop == null) { return; }
        shop.OpenShop();
    }

    /// <summary>
    /// 商人とプレイヤーが接触しているとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitPlayer = true;
    }

    /// <summary>
    /// 商人とプレイヤーが離れたとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        hitPlayer = false;
    }
}
