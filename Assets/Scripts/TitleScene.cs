using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //とりあえず何かキーを押すかクリックで遷移
        if (Input.anyKey || Input.GetMouseButtonUp(0))
        {
            //ゲームシーンに遷移する処理
            //とりあえずミニゲームシーンへ　後でメインゲームシーンとかへに差し替えてね
            SceneManager.LoadScene("MiniGameGeminScene");
        }
    }
}
