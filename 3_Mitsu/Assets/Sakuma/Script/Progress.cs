using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///ゲームの進行度を管理するクラス
/// </summary>
public class Progress : MonoBehaviour
{

    //Instance
    static public Progress Instance;
    //進行度
    public int Degree;
    //制限時間
    public float LimitTime = 0;

    bool endC = false;

    //アクションシーン開始時の進行度の初期化
    private void Awake()
    {
        endC = false;
        Instance = new Progress();
        Instance.Degree = 0;
        Instance .LimitTime = 180;
    }

    private void Update()
    {

        Instance.LimitTime -= Time.deltaTime;

        if (Instance.LimitTime < 0 && endC == false)
        {
            endC = true;
            GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Stop);
        }

        if(Instance.LimitTime < -3)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

}
