using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private Button resultButton;
    // Start is called before the first frame update
    void Start()
    {
        resultButton = GetComponent<Button>();

        resultButton.onClick.AddListener(() =>
        {
            //シーンの名前を変えよう。
            //SceneManager.LoadScene("TitleScene");

            Debug.Log("リザルトのボタンが押された");
        });
    }


}
