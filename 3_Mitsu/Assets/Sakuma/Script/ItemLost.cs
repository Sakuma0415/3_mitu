using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLost : MonoBehaviour
{
    public int lostNum=-1;
    public ItemBox itemBox;
    [SerializeField]
    RectTransform rectTransform;
    public void lost()
    {
        ItemList.Instance.ItemLost(lostNum);
        itemBox.botb = false;
        Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var screenPoint = Input.mousePosition;
            if (Mathf.Abs(rectTransform.position.x - screenPoint.x) > 110 || Mathf.Abs(rectTransform.position.y - screenPoint.y) > 37)
            {
                Destroy(gameObject);
                itemBox.botb = false;
            }
        }
    }
}
