using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public GameObject score1P = null; // Textオブジェクト
    public GameObject score2P = null; // Textオブジェクト
    public GameObject score3P = null; // Textオブジェクト
    public GameObject score4P = null; // Textオブジェクト
    public GameObject Timer   = null; // Text_Timer

    public int[] score = new int[4] { 0, 0, 0, 0 };

    //時間切れ
    float TIME_END = 10.0f;

    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GeminGameManager");
    }

    // Update is called once per frame
    void Update()
    {
        TIME_END -= Time.deltaTime;

        // オブジェクトからTextコンポーネントを取得
        Text score_text1P = score1P.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text1P.text = "1P Score:" + score[0];
        // オブジェクトからTextコンポーネントを取得
        Text score_text2P = score2P.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text2P.text = "2P Score:" + score[1];
        // オブジェクトからTextコンポーネントを取得
        Text score_text3P = score3P.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text3P.text = "3P Score:" + score[2];
        // オブジェクトからTextコンポーネントを取得
        Text score_text4P = score4P.GetComponent<Text>();
        // テキストの表示を入れ替える
        score_text4P.text = "4P Score:" + score[3];
        
        // オブジェクトからTextコンポーネントを取得
        Text timer_text = Timer.GetComponent<Text>();
        

        if(TIME_END <= 0)
        {
            SceneManager.LoadScene("MainGameScene");

            //gameManager.GetComponent<GeminGameManager>().ChangeGameMode();
        }
        else
        {
            // テキストの表示を入れ替える
            timer_text.text = TIME_END.ToString("N2");
        }
    }

    public void AddScore(int num)
    {
        score[num]++;
    }

    public void SubScore(int num)
    {
        score[num]--;
    }

    
}
