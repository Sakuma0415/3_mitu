using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartCont : MonoBehaviour
{

    float time = 3;
    [SerializeField]
    Text text;


    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time > 2)
        {
            text.text = "3";
        }else if (time > 1)
        {
            text.text = "2";
        }else if(time > 0)
        {
            text.text = "1";
        }

        if (time < 0)
        {
            text.text = "";
            GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Play);
            
        }
    }
}
