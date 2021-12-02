using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*ゲームマネージャー*/

//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class GeminGameManager : MonoBehaviour
{
    //ゲームステート{enWaitMode→待機モード、enGameMode→ゲームモード、enEndMode→終了モード}
    enum EnGameMode
    {
        enWaitMode,
        enGameMode,
        enEndMode
    }
    EnGameMode miniGameMode = EnGameMode.enGameMode;

    //--プレハブ
    public GameObject CoinPrefab;
    public GameObject BombPrefab;
    //降ってくるアイテム
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;

    //--定数
    //降ってくる間隔
    float FALL_DURATION = 2.0f;
    //出現するY座標(高さ)
    float SPAWN_Y = 10.0f;
    //X座標、範囲指定定数
    float MIN_X_POS = -9.0f;
    float P2_LEFT_X_POS = 9.0f;
    float P3_LEFT_X_POS = 27.0f;
    float P4_LEFT_X_POS = 45.0f;
    float MAX_X_POS = 63.0f;
    //PlayerのX座標
    float P1_INIT_POS = 0.0f;
    float P2_INIT_POS = 18.0f;
    float P3_INIT_POS = 36.0f;
    float P4_INIT_POS = 54.0f;

    //--メンバ変数
    //現在の降ってくるクールタイム
    float _timeFall = 0.0f;
    //現在のミニゲーム時間　時間切れ判定用
    float _nowTime = 0.0f;
    //ボムorコイン
    int _ranItem = 0;
    //アイテム発生位置
    float P1_FORM_X_POS = 0.0f;
    float P2_FORM_X_POS = 0.0f;
    float P3_FORM_X_POS = 0.0f;
    float P4_FORM_X_POS = 0.0f;

    //スコア更新用オブジェクト
    GameObject scoreGameObj;

    // Start is called before the first frame update
    void Start()
    {
        //終了時のスコア取得用オブジェクト
        scoreGameObj = GameObject.Find("ScoreManager");
    }

    // Update is called once per frame
    void Update()
    {
        switch(miniGameMode)
        {
            case EnGameMode.enWaitMode:
                break;
            
            case EnGameMode.enGameMode:
                _timeFall += Time.deltaTime;
                //降ってくる間隔になったら
                if (_timeFall >= FALL_DURATION)
                {
                    //発生場所を決定する
                    P1_FORM_X_POS = GetRandomXPos(MIN_X_POS, P2_LEFT_X_POS);
                    P2_FORM_X_POS = GetRandomXPos(P2_LEFT_X_POS, P3_LEFT_X_POS);
                    P3_FORM_X_POS = GetRandomXPos(P3_LEFT_X_POS, P4_LEFT_X_POS);
                    P4_FORM_X_POS = GetRandomXPos(P4_LEFT_X_POS, MAX_X_POS);

                    //ランダムにボムかコインか決定
                    _ranItem = Random.Range(0, 5);
                    if (_ranItem != 1)
                    {
                        //生成
                        Item1 = Instantiate(CoinPrefab);
                        Item2 = Instantiate(CoinPrefab);
                        Item3 = Instantiate(CoinPrefab);
                        Item4 = Instantiate(CoinPrefab);
                    }
                    else
                    {
                        //生成
                        Item1 = Instantiate(BombPrefab);
                        Item2 = Instantiate(BombPrefab);
                        Item3 = Instantiate(BombPrefab);
                        Item4 = Instantiate(BombPrefab);
                    }
                    //X座標をランダムで決定
                    Item1.transform.position = new Vector3(P1_FORM_X_POS, 6.0f, 0);
                    Item2.transform.position = new Vector3(P2_FORM_X_POS, 6.0f, 0);
                    Item3.transform.position = new Vector3(P3_FORM_X_POS, 6.0f, 0);
                    Item4.transform.position = new Vector3(P4_FORM_X_POS, 6.0f, 0);



                    //クールタイムをリセット
                    _timeFall = 0.0f;

                   //miniGameMode = EnGameMode.enEndMode;
                }
                break;

            case EnGameMode.enEndMode:
                //ミニゲームが終了したので順位の判定を行う
                


                break;
        }



        //時間切れで終了

    }

    private float GetRandomXPos(float min, float max)
    {
        return Random.Range(min + 1.0f, max - 1.0f);
    }

    public void ChangeGameMode()
    {
        miniGameMode = EnGameMode.enEndMode;
    }
}
