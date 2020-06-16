using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの挙動を管理するクラス
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public PlayerStatus playerStatus { set; private get; }=new PlayerStatus();

    [Header("設定項目")]
    //プレイヤーの画像のオブジェ
    [SerializeField]
    GameObject PlayerImage;
    //プレイヤーのアニメーション
    [SerializeField]
    PlayerAnime playerAnime;
    //Private

    //プレイヤーの角度
    float angle=0;
    //プレイヤーの速度
    float spead = 0;
    //プレイヤーの緩急に使うタイマー
    float speadTime = 0;

    private void Start()
    {
        Init();
    }

    //初期化
    void Init()
    {
        angle = 0;
        spead = 0;
        speadTime = 0;
        PlayerImage.transform.eulerAngles = new Vector3(0, 0, 180 + angle);
    }

    void Update()
    {
        //プレイヤーの入力の方向
        Vector2 MoveVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) { MoveVector.y += 1; }
        if (Input.GetKey(KeyCode.A)) { MoveVector.x -= 1; }
        if (Input.GetKey(KeyCode.S)) { MoveVector.y -= 1; }
        if (Input.GetKey(KeyCode.D)) { MoveVector.x += 1; }


        if(MoveVector != Vector2.zero)
        {
            speadTime = (speadTime + Time.deltaTime / playerStatus.speadmaximumTime) > 1 ? 1 : speadTime + Time.deltaTime / playerStatus.speadmaximumTime;
        }
        else
        {
            speadTime = (speadTime - Time.deltaTime / playerStatus.speadmaximumTime) < 0 ? 0 : speadTime - Time.deltaTime / playerStatus.speadmaximumTime;
        }

        spead = Mathf.Lerp(0, playerStatus.spead, speadTime);

        playerAnime.IsWalk = (spead > 0);

        if (spead>0)
        {
            float moveAngle = Mathf.Atan2(MoveVector.y, MoveVector.x)*Mathf.Rad2Deg ;
            Debug.Log(moveAngle);

            if (MoveVector != Vector2.zero)
            {
                angle = Mathf.LerpAngle(angle, moveAngle, 0.3f);
            }

            PlayerImage.transform.eulerAngles = new Vector3(0,0,180+angle);
            transform.position += new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0)* spead * Time.deltaTime ;
        }

    }



}
