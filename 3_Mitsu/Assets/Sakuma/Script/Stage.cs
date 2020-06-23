using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの管理クラス
/// </summary>
public class Stage : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    void OnDrawGizmosSelected()
    {

        Gizmos.color = new Color(1f, 0, 0, 1);
        Gizmos.DrawWireCube(new Vector3 (0,0,0), new Vector3(30,30,1));
    }
}
