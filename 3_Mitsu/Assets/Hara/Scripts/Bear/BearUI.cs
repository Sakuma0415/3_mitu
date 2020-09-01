using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BearUI : MonoBehaviour
{
    [SerializeField, Tooltip("熊の注意画像")] private Image image = null;

    private Coroutine coroutine = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        image.gameObject.SetActive(false);
    }

    /// <summary>
    /// 熊のUI表示の処理
    /// </summary>
    public void BearUIAction()
    {
        if(coroutine != null) { return; }
        coroutine = StartCoroutine(UICoroutine());
    }

    /// <summary>
    /// 熊のUI表示のコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator UICoroutine()
    {
        float time;

        int count = 0;
        while(count < 3)
        {
            if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(true);
                count++;
            }

            time = 0;
            while(time < 0.5f)
            {
                if(GameStatus.Instance.gameMode == GameStatus.GameMode.Play)
                {
                    time += Time.deltaTime;
                }
                yield return null;
            }
        }

        time = 0;
        while(time < 1.0f)
        {
            if (GameStatus.Instance.gameMode == GameStatus.GameMode.Play)
            {
                time += Time.deltaTime;
            }
            yield return null;
        }

        Vector3 diff = image.transform.localPosition;
        Vector3 to = diff + Vector3.right * 700;

        while(Vector3.Distance(diff, to) > 1.0f)
        {
            diff = Vector3.MoveTowards(image.gameObject.transform.localPosition, to, 2.5f);

            if (GameStatus.Instance.gameMode == GameStatus.GameMode.Play)
            {
                image.transform.localPosition = diff;
            }
            
            yield return null;
        }

        image.gameObject.SetActive(false);
        image.gameObject.transform.localPosition = to - Vector3.right * 700;

        coroutine = null;
    }
}
