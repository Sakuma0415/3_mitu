using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// アイテムを管理するクラス
/// </summary>
public class ItemList : MonoBehaviour
{
    static public ItemList Instance;

    public ItemData[] itemDataList;

    public int[] itemList ;

    private void Awake()
    {
        itemList = new int[6];
        for (int i = 0; i < 6; i++)
        {
            itemList[i] = -1;
        }
        

        Instance = this;
    }

    public void ItemGet(string name)
    {
        int itemNum = -1;
        for(int i=0;i< itemDataList.Length; i++)
        {
            if(itemDataList[i].itemName  == name)
            {
                itemNum = i;
            }
        }
        if(itemNum == -1)
        {
            Debug.Log("名前が検索できませんでした");
            return;
        }

        for(int i = 0; i < itemList.Length; i++)
        {

            if (itemList[i] == -1)
            {
                itemList[i] = itemNum;
                return;
            }

        }

        Debug.Log("アイテム欄がいっぱいです");

    }

    public void ItemLost(int num)
    {
        itemList[num] = -1;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ItemList.Instance.ItemGet("ハチの巣");
        }
    }
}
