using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの状態を管理するクラス
/// </summary>
public class GameStatus : MonoBehaviour
{
    //Instance
    static public GameStatus Instance;

    //ゲームモード
    public enum GameMode
    {
        Start=0,
        Play,
        Stop,
        Anime,
        Pause
    }

    //現在のゲームモード
    public GameMode gameMode { private set; get; }=0;

    //初期化
    private void Init()
    {
        gameMode = GameMode.Play;
    }

    //開始時の処理
    private void Awake()
    {
        Instance = new GameStatus();
        Instance.Init();
    }

    //ゲームモード更新
    public void ChangeGameMode(GameMode change)
    {
        gameMode = change;
        ChangeToGameMode(change);
    }

    //ゲームモード更新時の処理
    private void ChangeToGameMode(GameMode change)
    {
        Debug.Log("ゲームモードを"+gameMode+"に変更");
        switch(change)
        {
            case GameMode.Start:
                break;
            case GameMode.Play:
                break;
            case GameMode.Pause:
                break;
        }
    }

}
