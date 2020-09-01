using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMoney : MonoBehaviour
{
    public ItemList itemList;
    private Text earnMoneyText;
    private int firstscore = 0;
    private int gameScore;
    // Start is called before the first frame update
    void Start()
    {
        earnMoneyText.text = firstscore.ToString();
        itemList = GetComponent<ItemList>();
        gameScore = itemList.okane;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        if(firstscore<gameScore)
        {
            firstscore += 100;
            earnMoneyText.text = firstscore.ToString();
        }
    }
}
