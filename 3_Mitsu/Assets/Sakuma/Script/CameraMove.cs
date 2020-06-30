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

            rigidbody2D.transform.position = pos;

        }
    }

    //対象オブジェのセット
    public void SeeSet(GameObject target)
    {
        targetTransform = target.transform;
        rigidbody2D.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -10);
    }
}
