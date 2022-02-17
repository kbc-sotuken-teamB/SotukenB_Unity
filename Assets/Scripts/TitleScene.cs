using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    AudioSource audioSource;
    public bool isFade;
    public double FadeInSeconds = 5.0f;
    bool IsFadeIn = false;
    float FadeDeltaTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //とりあえず何かキーを押すかクリックで遷移
        //if (Input.anyKey || Input.GetMouseButtonUp(0))

        //1PのAボタン（zキー）で
        if(Input.GetButtonUp("1PButtonA") || Input.GetMouseButtonUp(0))
        {
            IsFadeIn = true;
        }

        if (IsFadeIn)
        {
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime >= FadeInSeconds)
            {
                //FadeDeltaTime = FadeInSeconds;
                //IsFadeIn = false;

                
            }
            audioSource.volume = (float)(0.2f - FadeDeltaTime / FadeInSeconds * 0.1f);

            if(audioSource.volume <= 0.05f)
            {
                //ゲームシーンに遷移
                SceneManager.LoadScene("MainGameScene");
            }
        }
    }
}
