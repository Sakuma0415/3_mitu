using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Machine : MonoBehaviour
{

    int line = 0;
    float[] lineTime = new float[1];
    bool[] IsLine = new bool[1];
    public int stack = 0;
    public int end = 0;
    [SerializeField]
    GameObject[] GageObj;
    [SerializeField]
    Image[] images;
    [SerializeField]
    Text contText;
    [SerializeField]
    float siftTime =10;
    [SerializeField]
    BearMaster bearMaster;

    bool sw = false;

    void Start()
    {
        line = 1;
        lineTime[0] = 0;
        for(int i=0;i< GageObj.Length; i++)
        {

            GageObj[i].SetActive(i<line);
        }
    }
    
    void Update()
    {

        contText.text  = stack.ToString();
        
        for (int j = 0; j < IsLine.Length; j++)
        {
            if (!IsLine[j]&&stack >0)
            {
                IsLine[j] = true;
                stack--;
            }
        }


        if (sw && Input.GetKeyDown(KeyCode.Space)) 
        {

            StackH();
        }

        if (Input .GetKeyDown(KeyCode.Q))
        {
            LinePlus();
        }

        if (GameStatus.Instance.gameMode == GameStatus .GameMode .Play )
        {
            sysGo();
        }

        int bearCont = end / 3;

        bearMaster.spawnLevel = bearCont;
    }

    void sysGo()
    {
        for (int j = 0; j < IsLine.Length; j++)
        {
            if (IsLine[j])
            {
                lineTime[j] += Time.deltaTime;
            }

            if(lineTime[j]> siftTime)
            {
                lineTime[j] = 0;
                IsLine[j] = false;
                end += 1;
            }
            images[j].fillAmount = lineTime[j] / siftTime;
        }
    }



    public void LinePlus()
    {
        if (line < 3)
        {
            line += 1;

            Array.Resize(ref lineTime, lineTime.Length + 1);
            lineTime[lineTime.Length - 1] = 0;

            Array.Resize(ref IsLine, IsLine.Length + 1);
            IsLine[IsLine.Length - 1] = false;

            for (int i = 0; i < GageObj.Length; i++)
            {
                GageObj[i].SetActive(i < line);
            }
        }
    }

    public void StackH()
    {
        for(int i=0;i<ItemList.Instance .itemList.Length; i++)
        {
            if (ItemList.Instance.itemList[i]!= -1)
            {
                if (ItemList.Instance.itemDataList[ItemList.Instance.itemList[i]].itemName == "ハチの巣")
                {
                    stack++;
                    ItemList.Instance.itemList[i] = -1;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            sw = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
                sw = false;
        }
    }

}
