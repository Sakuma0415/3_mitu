using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{

    [SerializeField, Tooltip("戻るボタン")] private Button exitButton = null;
    [SerializeField, Tooltip("所持金を表示するテキスト")] private Text moneyText = null;
    [SerializeField, Tooltip("ショップボタンを格納しているオブジェクト")] private GameObject itemList = null;
    private ItemShopButton[] shopButton = null;

    [SerializeField, Header("販売商品リスト")] private ItemData[] itemDatas = null;

    // 所持金の情報を扱う変数
    private int money = 0;

    private Coroutine coroutine = null;
    private bool isConnectItemList = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void ShopInit()
    {
        // 戻るボタンに処理を割り当てる
        exitButton.onClick.AddListener(() => CloseShop());

        // ショップボタンを取得する
        if(itemList != null)
        {
            shopButton = new ItemShopButton[itemList.transform.childCount];
            for(int i = 0; i < shopButton.Length; i++)
            {
                int num = i;
                shopButton[num] = itemList.transform.GetChild(num).gameObject.GetComponent<ItemShopButton>();

                if(num >= itemDatas.Length || itemDatas == null)
                {
                    // 商品数を超えた場合はボタンを非表示にする
                    shopButton[num].gameObject.SetActive(false);
                }
                else
                {
                    // 商品情報を取得する
                    string itemName = itemDatas[num].itemName;
                    Sprite itemSprite = itemDatas[num].itemSprite;
                    int itemPrice = itemDatas[num].price;
                    if(itemPrice < 0) { itemPrice = 0; }

                    // ボタン処理を実装する
                    shopButton[num].SetButtonAction(itemName, itemSprite, itemPrice, () => ItemBuy(itemName, itemPrice, true, shopButton[num]));
                }
            }
        }

        // お店を非表示にしておく
        gameObject.SetActive(false);
    }

    /// <summary>
    /// アイテムショップを開く
    /// </summary>
    public void OpenShop()
    {
        if (gameObject.activeSelf) { return; }

        // ショップの表示
        gameObject.SetActive(true);

        // ゲームモードをPauseにする
        GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Pause);

        // 所持金の情報を取得する
        try
        {
            money = ItemList.Instance.okane;
            isConnectItemList = true;
            if(money < 0)
            {
                money = 0;
            }
        }
        catch
        {
            money = 0;
            isConnectItemList = false;
        }
        moneyText.text = money.ToString() + " 円";
    }

    /// <summary>
    /// アイテムショップを閉じる
    /// </summary>
    public void CloseShop()
    {
        if(gameObject.activeSelf == false) { return; }

        // ショップの非表示
        gameObject.SetActive(false);

        // ゲームモードをPlayにする
        GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Play);

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        // 所持金をスコアに反映する
        if (isConnectItemList)
        {
            ItemList.Instance.okane = money;
        }
    }

    /// <summary>
    /// 購入処理
    /// </summary>
    /// <param name="itemName">商品名</param>
    /// <param name="price">値段</param>
    /// <param name="onlyOnceBuy">一回しか購入できないならtrue</param>
    /// <param name="shopButton">制御するボタン</param>
    public void ItemBuy(string itemName, int price, bool onlyOnceBuy, ItemShopButton shopButton)
    {
        // 所持金チェック
        if(Pay(price) == false || ItemList.Instance.noSpace) { return; }
        
        if (onlyOnceBuy)
        {
            // 一度購入したらボタンを押せないようにする
            shopButton.ShopButton.interactable = false;
        }

        // アイテムをインベントリに追加する処理
        ItemList.Instance.ItemGet(itemName);
    }

    /// <summary>
    /// 支払いできるかチェックする
    /// </summary>
    /// <param name="price">支払金額</param>
    /// <returns></returns>
    private bool Pay(int price)
    {
        // 金額の計算を実行
        int result = money - price;

        if(result < 0 || coroutine != null) { return false; }

        // 支払いのアニメーションを実行
        coroutine = StartCoroutine(PayAnimation(money, result, 1.0f));
        money = result;

        return true;
    }

    /// <summary>
    /// 支払いアニメーションのコルーチン処理
    /// </summary>
    /// <param name="start">元の金額</param>
    /// <param name="end">購入後の金額</param>
    /// <param name="duration">アニメーション時間</param>
    /// <returns></returns>
    private IEnumerator PayAnimation(int start, int end, float duration)
    {
        if(moneyText != null)
        {
            if(start != end)
            {
                float time = 0;

                while (time < duration)
                {
                    float rate = time / duration;
                    int diff = start - (int)(Mathf.Abs(start - end) * rate);
                    moneyText.text = diff.ToString() + " 円";
                    time += Time.deltaTime;
                    yield return null;
                }
            }

            moneyText.text = end.ToString() + " 円";
        }

        coroutine = null;
    }
}
