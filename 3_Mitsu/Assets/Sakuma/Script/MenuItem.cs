using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    [SerializeField]
    Image[] ListObj;

    [SerializeField]
    Sprite NoneImage;

    [SerializeField]
    Sprite  dontOpen;


    void Update()
    {
        for(int i=0;i< ListObj.Length; i++)
        {
            if(ItemList.Instance.itemList .Length > i)
            {
                if (ItemList.Instance.itemList[i] == -1)
                {
                    ListObj[i].sprite = NoneImage;
                }
                else
                {
                    ListObj[i].sprite = ItemList.Instance.itemDataList[ItemList.Instance.itemList[i]].itemSprite; 
                }
            }
            else
            {
                ListObj[i].sprite = dontOpen;
            }
        }
    }
}
