using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMoney : MonoBehaviour
{
    private Text earnMoneyText;
    private int firstscore = 0;
    private int gameScore = 100;
    // Start is called before the first frame update
    void Start()
    {
        earnMoneyText.text = firstscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        if(firstscore<gameScore)
        {
            firstscore += 1;
            earnMoneyText.text = firstscore.ToString();
        }
    }
}
