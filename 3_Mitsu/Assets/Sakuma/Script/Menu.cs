using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menuボタンの管理クラス
/// </summary>
public class Menu : MonoBehaviour
{

    //メニューのON/OFF
    [SerializeField]
    bool IsMenu = false;
    //メニューのON/OFF
    [SerializeField]
    bool IsAnime = false;
    //
    [SerializeField]
    Image MaskImage;
    [SerializeField]
    GameObject MgameObject;
    void Update()
    {
        if (!IsAnime&&Input.GetKeyDown (KeyCode.Tab))
        {
            if (IsMenu)
            {
                StartCoroutine("FadeOut");
            }
            else
            {
                if (GameStatus.Instance.gameMode == GameStatus.GameMode.Play)
                {
                    StartCoroutine("FadeIn");
                }
            }
        }
    }

    IEnumerator FadeIn()
    {
        GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Pause);
        MaskImage.enabled = true;
        float time = 0;
        IsAnime = true;
        MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, 0);
        while (time < 0.5f)
        {
            MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, time);
            time += Time.deltaTime;
            yield return null;
        }
        MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, 0.5f);
        IsAnime = false ;
        IsMenu = true;
        MgameObject.SetActive(true);
        ItemBox itemBox = MgameObject.GetComponent<ItemBox>();
        itemBox.botb = false;
        Destroy(itemBox.botc); 
        yield return null;
    }

    IEnumerator FadeOut()
    {
        MaskImage.enabled = true;
        float time = 0;
        IsAnime = true;
        MgameObject.SetActive(false);
        MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, 0.5f);
        while (time < 0.5f)
        {
            MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, (0.5f-time));
            time += Time.deltaTime;
            yield return null;
        }
        MaskImage.color = new Color(MaskImage.color.r, MaskImage.color.g, MaskImage.color.b, 0);
        IsAnime = false;
        IsMenu = false;
        MaskImage.enabled = false;
        GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Play);
        
        yield return null;
    }
}
