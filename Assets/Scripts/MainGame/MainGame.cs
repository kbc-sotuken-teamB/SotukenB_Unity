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

//ミニゲーム遷移してもゲームデータ保持しとかないとな

//プレイヤー各個のスクリプトで移動してもらったほうがいいな


//今のところダイスが変な動きするし　プレイヤー移動は申し訳程度の1P単純前進だけど
//まあやりたいことできる間（ステート）はできたのでいいでしょう

//あとミニゲームに急に行くので　もうちょっと間と
//スタートボタン押したらミニゲーム開始するって準備画面もいる

//マスの座標を目的地に指定して　一つ一つ　目的地配列にして進んでいってもらおうか

public class MainGame : MonoBehaviour
{
    //とりあえずデバッグで動かすだけのプレイヤー1
    public GameObject pl;
    //ダイス
    public GameObject Dice;

    public GameObject Squares;

    //ステート
    //1P操作待機状態、2P…
    //サイコロが振られる、プレイヤーが出た目の数進むアニメーション中などのプレイヤー操作不可状態

    //プレイヤーの操作可能状態、操作不可状態のオンオフで、カレントプレイヤーの変数用意しといたらいいかな

    enum EnMainGameState
    {
        enDiceRoll,
        enMovePlayer,
        enIdle
    }

    //現在のステート
    EnMainGameState _mainState = EnMainGameState.enIdle;

    //これに今何Pのターンか取得してくっつける
    const string BUTTON_A = "PButtonA";

    //ミニゲームシーン名
    const string SCENE_GEMIN = "MiniGameGeminScene";
    const string SCENE_SCROLL = "MiniGameSideScrolling";
    const string SCENE_JUMP = "MiniGameJumpAthleticScene";
    //ミニゲームシーンの配列 ランダムインデックスで呼ぶ
    string[] _miniGameScenes = { SCENE_GEMIN, SCENE_SCROLL/*, SCENE_JUMP*/ };

    //今何Pのターンか
    int _currentPlayer = 1;
    bool _isIdle = true;
    //「○PButtonA」
    string _plInputTextA;

    //クールタイム
    float _coolTime = 0.0f;

    //ダイスの初期位置
    Vector3 _dicePos;

    //プレイヤーの移動前位置
    Vector3 _plPosOld;
    //プレイヤーの移動先位置
    Vector3 _targetPos;

    //マス
    Transform[] _squares;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("First:" + _currentPlayer + "P");

        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;
        //サイコロの初期位置取得しておく
        _dicePos = Dice.transform.position;

        //マスを上から順に取得 仮リストに入れて
        List<Transform> tmp = new List<Transform>();
        tmp.AddRange(Squares.GetComponentsInChildren<Transform>());
        //一番最初の要素(親オブジェクトを除去)して
        tmp.RemoveAt(0);
        //配列に格納
        _squares = tmp.ToArray();

        //でばっぐ ちゃんと上から順に入ってる
        for(int i = 0; i < _squares.Length; i++)
        {
            Debug.Log(_squares[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_mainState)
        {
            case EnMainGameState.enIdle:
                //Aボタンでサイコロ振る待機
                //1P:z 2P:b 3P:1 4P:5
                PressA();
                break;

            case EnMainGameState.enDiceRoll:
                //ダイス振るアニメーション
                DiceRoll();
                break;

            case EnMainGameState.enMovePlayer:
                //進む
                MovePlayer();
                break;


            default:
                break;
        }

    }

    //Aボタンでサイコロ振る待機
    void PressA()
    {
        if (Input.GetButtonUp(_plInputTextA))
        {
            //サイコロが動くステート
            _mainState = EnMainGameState.enDiceRoll;
            //クールタイムをリセット（仮なので時間でサイコロ止まる）
            _coolTime = 0.0f;
            //サイコロの重力を有効にして仮動作してもらう
            Dice.GetComponent<Rigidbody>().useGravity = true;


        }
    }

    //ダイスが振られる
    void DiceRoll()
    {
        //クールタイム加算
        _coolTime += Time.deltaTime;
        //1秒立ったらとりあえず終わり
        if (_coolTime > 1.0f)
        {
            //重力オフ
            Dice.GetComponent<Rigidbody>().useGravity = false;
            //サイコロを初期位置へ
            Dice.transform.position = _dicePos;

            Debug.Log("diceEnd");


            //サイコロの出目　（仮）ランダム　本物ではサイコロの上面を判定する
            int dice = Random.Range(1, 6);
            Debug.Log(_currentPlayer + "P: " + dice);

            MainPlayer player = pl.GetComponent<MainPlayer>();
            int plSquare = player.CurrentSquare;

            List<Vector3> targetPosList = new List<Vector3>();
            for(int i = 0; i < dice; i++)
            {
                targetPosList.Add(_squares[plSquare + i].position);
            }

            player.InitMove(targetPosList);


            //クールタイムリセット
            /*_coolTime = 0.0f;

            //現在のプレイヤー位置保持
            _plPosOld = pl.transform.position;
            //移動先(仮)は適当に
            _targetPos = _plPosOld;
            _targetPos.z += 2.0f;*/

            //プレイヤーが進むステートへ
            _mainState = EnMainGameState.enMovePlayer;
        }

    }

    //プレイヤーが進む
    void MovePlayer()
    {
        //プレイヤーの移動呼ぶ
        if (pl.GetComponent<MainPlayer>().Move())
        {
            //移動終わったら
            Debug.Log("moveEnd");

            //次のプレイヤー
            _currentPlayer += 1;

            //4以下なら次のプレイヤーがサイコロを振る
            if (_currentPlayer <= 4)
            {
                _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

                Debug.Log("Next:" + _currentPlayer + "P");

                //サイコロ待機へ
                _mainState = EnMainGameState.enIdle;
            }
            //5になったら全員終わったので
            else
            {
                //ランダムでミニゲームを呼び出す
                SceneManager.LoadScene(_miniGameScenes[Random.Range(0, _miniGameScenes.Length - 1)]);
            }
        }
    }

    /*void MovePlayer()
    {

        //Debug.Log("move");
        //--出た目の数進む
        //--プレイヤーごとのカレントマスデータも記録しないとな
        //--踏んだマスに応じてイベント

        //クールタイム加算
        _coolTime = Mathf.Min(1.0f, _coolTime + Time.deltaTime);

        //線形補間移動
        pl.transform.position = Vector3.Lerp(_plPosOld, _targetPos, _coolTime);


        //移動が終わったら
        if (_coolTime == 1.0f)
        {
            Debug.Log("moveEnd");

            //次のプレイヤー
            _currentPlayer += 1;

            //4以下なら次のプレイヤーがサイコロを振る
            if (_currentPlayer <= 4)
            {
                _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

                Debug.Log("Next:" + _currentPlayer + "P");

                //サイコロ待機へ
                _mainState = EnMainGameState.enIdle;
            }
            //5になったら全員終わったので
            else
            {
                //ランダムでミニゲームを呼び出す
                //SceneManager.LoadScene(_miniGameScenes[Random.Range(0,2)]);
                SceneManager.LoadScene(_miniGameScenes[Random.Range(0,1)]);
                //（まだ一つしかないのでとりあえず下民呼ぶ）
                //SceneManager.LoadScene(_miniGameScenes[0]);
            }

        }
    }*/


}
