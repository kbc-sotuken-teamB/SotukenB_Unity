using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*ゲームマネージャー*/

//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class GeminGameManager : MonoBehaviour
{
    //--プレハブ
    public GameObject CoinPrefab;
    public GameObject BombPrefab;
    //降ってくるアイテム
    public GameObject Item;

    //--定数
    //降ってくる間隔
    float FALL_DURATION = 2.0f;
    //出現するY座標(高さ)
    float SPAWN_Y = 10.0f;
    //時間切れになる時間
    float TIME_END = 60.0f;
    //X座標、範囲指定定数
    float MIN_X_POS = -8.0f;
    float P2_LEFT_X_POS = 9.0f;
    float P3_LEFT_X_POS = 27.0f;
    float P4_LEFT_X_POS = 45.0f;
    float MAX_X_POS = 62.0f;
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
    float FORM_X_POS = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeFall += Time.deltaTime;
        //降ってくる間隔になったら
        if(_timeFall >= FALL_DURATION)
        {
            //発生場所を決定する
            FORM_X_POS = GetRandomXPos();

            //ランダムにボムかコインか決定
            _ranItem = Random.Range(0, 5);
            if(_ranItem != 1)
            {
                //生成
                Item = Instantiate(CoinPrefab);
            }
            else
            {
                //生成
                Item = Instantiate(BombPrefab);
            }
            //X座標をランダムで決定
            Item.transform.position = new Vector3(FORM_X_POS, 6.0f, 0);
            
            

            //クールタイムをリセット
            _timeFall = 0.0f;
        }


        //時間切れで終了

    }

    private float GetRandomXPos()
    {
        return Random.Range(MIN_X_POS, P2_LEFT_X_POS - 1.0f);
    }
}
