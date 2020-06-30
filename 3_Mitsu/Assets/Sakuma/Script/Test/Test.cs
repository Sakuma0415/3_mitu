using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameStatus.Instance.ChangeGameMode(GameStatus.GameMode .Play );
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Pause );
        }
        Debug.Log(GameStatus.Instance.gameMode);
    }
}
