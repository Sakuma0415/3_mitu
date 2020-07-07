using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{

    [SerializeField]
    RectTransform[] BoxPos;
    [SerializeField]
    GameObject bot;

    public bool botb = false;
    public GameObject botc;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&!botb)
        {
            // クリックしたスクリーン座標
            var screenPoint = Input.mousePosition;

            //Debug.Log(screenPoint);

            for(int i = 0; i < BoxPos.Length; i++)
            {
                //Debug.Log(BoxPos[i].position );

                if(Mathf.Abs ( BoxPos[i].position.x- screenPoint.x)<82&& Mathf.Abs(BoxPos[i].position.y - screenPoint.y) < 82&&ItemList .Instance.itemList[i]!=-1)
                {
                    Debug.Log(i);
                    botc= Instantiate(bot, screenPoint, Quaternion.identity, transform);
                    botc.GetComponent<ItemLost>().lostNum = i;
                    botc.GetComponent<ItemLost>().itemBox  = this;
                    botb = true;
                }
            }
        }

    }
}
