using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminGameManager : MonoBehaviour
{
    //--プレハブ
    public GameObject CoinPrefab;
    public GameObject BombPrefab;

    //--定数
    //降ってくる間隔
    float FALL_DURATION = 2.0f;
    //出現するY座標(高さ)
    float SPAWN_Y = 10.0f;
    //時間切れになる時間
    float TIME_END = 60.0f;
    //X座標端の絶対値
    float MAX_X_POS = 8.0f;

    //--メンバ変数
    //現在の降ってくるクールタイム
    float _timeFall = 0.0f;
    //現在のミニゲーム時間　時間切れ判定用
    float _nowTime = 0.0f;

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
            //ランダムにボムかコインか決定
            
            //X座標をランダムで決定

            //生成


            //クールタイムをリセット
            _timeFall = 0.0f;
        }


        //時間切れで終了

    }
}
