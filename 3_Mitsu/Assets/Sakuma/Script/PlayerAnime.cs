using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションを管理するクラス
/// </summary>
public class PlayerAnime : MonoBehaviour
{
    [SerializeField]
    Texture[] sprites;
    [SerializeField]
    Material material;
    [SerializeField]
    float stepTime=0;

    //歩き中かどうか
    public bool IsWalk = false;
    //歩き中の時間
    float walkTime=0;

    void Update()
    {
        if (IsWalk)
        {
            walkTime += Time.deltaTime;
            material.SetTexture("_MainTex", sprites[((int)(walkTime/ stepTime)%2)+1]);
        }
        else
        {
            walkTime = 0;
            material.SetTexture("_MainTex", sprites[0]);
        }
    }
}
