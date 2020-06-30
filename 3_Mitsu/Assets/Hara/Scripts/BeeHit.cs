using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHit : MonoBehaviour
{
    /// <summary>
    /// 蜂とプレイヤーの接触フラグ
    /// </summary>
    public bool PlayerHit { private set; get; } = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerHit = false;
    }
}
