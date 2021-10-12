using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//メインゲーム全体のスクリプト

//1PのAボタン（zキー）押されたら

//サイコロが振られる
//→操作不可状態に遷移
//出た目を判定
//出た目の数進む
//踏んだマスに応じてイベント

//1Pが振り終わったら→2P→3P→4P

//全員終わったらミニゲームへ
//ランダムにミニゲームを決定してシーン遷移

public class MainGame : MonoBehaviour
{
    //ステート
    //1P操作待機状態、2P…
    //サイコロが振られる、プレイヤーが出た目の数進むアニメーション中などのプレイヤー操作不可状態

    //プレイヤーの操作可能状態、操作不可状態のオンオフで、カレントプレイヤーの変数用意しといたらいいかな


    //これに今何Pのターンか取得してくっつける
    const string BUTTON_A = "PButtonA";

    //ミニゲームシーン名
    const string SCENE_GEMIN = "MiniGameGeminScene";
    const string SCENE_JUMP = "MiniGameJumpAthleticScene";
    //ミニゲームシーンの配列 ランダムインデックスで呼ぶ
    string[] _miniGameScenes = { SCENE_GEMIN, SCENE_JUMP };

    //今何Pのターンか
    int _currentPlayer = 1;
    bool _isIdle = true;
    //「○PButtonA」
    string _plInputTextA;

    // Start is called before the first frame update
    void Start()
    {
        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;
    }

    // Update is called once per frame
    void Update()
    {
        //操作待ち（サイコロ振り待ち）
        //ステートに変えたほうがええか
        if (_isIdle)
        {
            //Aボタンでサイコロを振る
            //1P:z 2P:b 3P:1 4P:5
            if (Input.GetButtonUp(_plInputTextA))
            {
                _isIdle = false;
            }
        }
        else
        {
            //サイコロが振られる
            int dice = Random.Range(1, 6);

            Debug.Log(_currentPlayer + "P: " + dice);
            //出た目の数進む
            //踏んだマスに応じてイベント

            //_currentPlayer = (_currentPlayer + 1) % 5;
            _currentPlayer += 1;

            //4以下なら次のプレイヤー
            if(_currentPlayer <= 4)
            {
                _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

                Debug.Log("Next:" + _currentPlayer + "P");

                _isIdle = true;
            }
            //5になったら全員終わったので
            else
            {
                //ランダムでミニゲームを呼び出す
                //SceneManager.LoadScene(_miniGameScenes[Random.Range(0,2)]);
                //（まだ一つしかないのでとりあえず下民呼ぶ）
                SceneManager.LoadScene(_miniGameScenes[0]);
            }
        }
    }
}
