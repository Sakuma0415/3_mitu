using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// アイテムのステータス
/// </summary>

[CreateAssetMenu(menuName = "Scriptable/ItemData")]
public class ItemData : ScriptableObject
{
    //名前
    public string itemName;
    //画像
    public Sprite itemSprite;
    //値段
    public float price;
}
