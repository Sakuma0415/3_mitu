using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class okane : MonoBehaviour
{
    [SerializeField]
    Text text;
    void Update()
    {
        text.text = ItemList.Instance.okane .ToString ()+ " 円";
    }
}
