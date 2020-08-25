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

    public int itemCont = 0;

    public bool noSpace=false;

    public int okane = 0;

    public bool shoes = false;

    public Transform playerPos;
    [SerializeField]
    HoneyMaster honeyMaster;

    float angle = 0;

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

                switch (name)
                {
                    case "俊足シューズ":
                        shoes = true;
                        break ;
                }
                return;
            }

        }

        Debug.Log("アイテム欄がいっぱいです");

    }

    public void ItemLost(int num)
    {

        switch (itemDataList[itemList[num]].itemName)
        {
            case "ハチの巣":
                angle += 60;
                Vector3[] lists = new Vector3[1];
                lists[0] = playerPos.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -0.1f) * 1.3f;
                honeyMaster.DropHoneyComb(lists);
                break ;
            case "俊足シューズ":
                shoes = false;
                break ;
        }
        itemList[num] = -1;

    }

    public void ListPlus()
    {
        if(itemList.Length < 10)
        {
            int[] nextList = new int[10];
            for(int i=0;i< nextList.Length; i++)
            {
                if(i< itemList.Length)
                {
                    nextList[i] = itemList[i];
                }
                else
                {
                    nextList[i] = -1;
                }
            }
            
            itemList = nextList;
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.K))
        {
            ListPlus();
        }

        int cont = 0;
        for (int i = 0; i < Instance.itemList.Length; i++)
        {

            if (Instance.itemList[i] != -1)
            {
                cont++;
            }

        }
        Instance.itemCont = cont;
        Instance.noSpace = (cont == Instance.itemList.Length);

        
    }
}
