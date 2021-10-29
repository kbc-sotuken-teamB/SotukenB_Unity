using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//デバッグ用

public class TestLoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    //ミニゲームのシーンにボタン貼り付けてこの関数を呼ぶとメインゲームに戻れる
    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}
