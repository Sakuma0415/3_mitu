using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button Startbutton;
    // Start is called before the first frame update
    void Start()
    {
        Startbutton = GetComponent<Button>();

        Startbutton.onClick.AddListener(() =>
        {
            //シーンの名前を変えよう。
            //SceneManager.LoadScene("MainScene");

            Debug.Log("ボタンが押された");
        });
    }


}
