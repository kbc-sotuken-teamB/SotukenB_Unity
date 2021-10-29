using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject score1P = null; // Textオブジェクト
    public GameObject score2P = null; // Textオブジェクト
    public GameObject score3P = null; // Textオブジェクト
    public GameObject score4P = null; // Textオブジェクト
    public GameObject Timer   = null; // Text_Timer

    /*
    public int score_num1P = 0; // スコア変数
    public int score_num2P = 0; // スコア変数
    public int score_num3P = 0; // スコア変数
    public int score_num4P = 0; // スコア変数
    */

    public int[] score = new int[4] { 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
