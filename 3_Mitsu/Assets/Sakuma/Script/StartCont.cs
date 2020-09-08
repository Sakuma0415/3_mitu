using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartCont : MonoBehaviour
{

    float time = 4;
    [SerializeField]
    Text text;
    [SerializeField]
    AudioSource audioSource;
    string tet = "";
    [SerializeField ]
    AudioClip sound1;
    private void Start()
    {
        tet = "";
    }

    // Update is called once per frame
    void Update()
    {
        


        time -= Time.deltaTime;
        if (time > 3)
        {
            text.text = "";
        }else if (time > 2)
        {
            text.text = "3";
        }else if (time > 1)
        {
            text.text = "2";
        }else if(time > 0)
        {
            text.text = "1";
        }
        if(tet!=text.text)
        {
            audioSource.PlayOneShot(sound1);
        }

        tet = text.text;



        if (time < 0)
        {
            text.text = "";
            GameStatus.Instance.ChangeGameMode(GameStatus.GameMode.Play);
            audioSource.Play();
            this.enabled = false;
        }
    }
}
