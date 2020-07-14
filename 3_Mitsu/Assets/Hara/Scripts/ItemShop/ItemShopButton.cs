using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopButton : MonoBehaviour
{
    [SerializeField, Tooltip("商品名のテキスト")] private Text itemNameText = null;
    [SerializeField, Tooltip("商品画像")] private Image itemImage = null;
    [SerializeField, Tooltip("商品の値段テキスト")] private Text itemPriceText = null;
    public Button ShopButton { private set; get; } = null;

    /// <summary>
    /// ショップの購入ボタンを設定する
    /// </summary>
    /// <param name="itemName">商品名</param>
    /// <param name="itemSprite">商品画像</param>
    /// <param name="itemPrice">商品の値段</param>
    /// <param name="buttonAction">購入処理</param>
    public void SetButtonAction(string itemName, Sprite itemSprite, int itemPrice, UnityEngine.Events.UnityAction buttonAction)
    {
        if(itemName != null)
        {
            // 商品名をセット
            itemNameText.text = itemName;
        }

        if(itemImage != null)
        {
            // 商品画像をセット
            itemImage.sprite = itemSprite;
        }

        if(itemPriceText != null)
        {
            // 値段をセット
            if(itemPrice <= 0)
            {
                itemPriceText.text = "無料";
            }
            else
            {
                itemPriceText.text = itemPrice.ToString() + " 円";
            }
        }

        // 購入処理をセット
        ShopButton = GetComponent<Button>();
        ShopButton.onClick.AddListener(buttonAction);
    }
}
