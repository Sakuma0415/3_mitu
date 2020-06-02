using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeControl : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度"), Range(1, 5)] private float speed = 1.0f;

    private Vector3 movePos = Vector3.zero;

    /// <summary>
    /// 移動座標の設定
    /// </summary>
    public Vector3 MovePos { set { movePos = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 移動方向によって向きを変える
        if (transform.position.x < movePos.x)
        {
            // 反転
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            // 正方向
            if (transform.position.x != movePos.x) { transform.rotation = Quaternion.Euler(Vector3.zero); }
        }

        // 移動処理
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movePos.x, movePos.y, 0), speed * Time.deltaTime);
    }
}
