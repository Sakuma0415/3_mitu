using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの動きを管理するクラス
/// </summary>
public class CameraMove : MonoBehaviour
{
    //見るオブジェの座標
    Transform targetTransform;
    [SerializeField]
    Rigidbody2D rigidbody2D;
    [SerializeField]
    Vector2 size = Vector2.zero;
    [SerializeField]
    LayerMask layerMask;

    //フレームワーク
    private void FixedUpdate()
    {
        if(GameStatus .Instance.gameMode ==GameStatus.GameMode.Play && targetTransform != null)
        {
            Vector3 pos= Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, -10),
                new Vector3(targetTransform.position.x, targetTransform.position.y, -10),
                0.05f);

            if(Physics2D.BoxCastAll(transform.position, new Vector2(17.77778f, 10f), 0, Vector2.right, (pos .x- transform.position.x),layerMask ).Length > 0)
            {
                pos.x = transform.position.x;
            }
            if (Physics2D.BoxCastAll(transform.position, new Vector2(17.77778f, 10f), 0, Vector2.up, (pos.y - transform.position.y), layerMask).Length > 0)
            {
                pos.y = transform.position.y;
            }

            //if( Physics2D.OverlapBoxAll(pos, new Vector2(17.77778f, 10f), 0, layerMask).Length <= 0)
            //{
            rigidbody2D.transform.position = pos;
            //}
                

        }
    }

    //対象オブジェのセット
    public void SeeSet(GameObject target)
    {
        targetTransform = target.transform;
        rigidbody2D.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -10);
    }
}
