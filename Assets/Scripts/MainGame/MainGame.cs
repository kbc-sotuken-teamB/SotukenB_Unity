using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Text AnnounceText;

    //とりあえずデバッグで動かすだけのプレイヤー1
    //public GameObject pl;

    //とりあえずプレイヤー配列　フォルダ代わりの親オブジェクト作ってぶちこんだ方がいいかも
    //public GameObject[] Players;

    //プレイヤー親
    public GameObject PlayersParent;

    //ダイス
    public GameObject Dice;
    //マスの親オブジェクト(フォルダ代わり)　ここから順番にマスを取得
    public GameObject SquaresParent;

    //A
    public GameObject TextA;

    //
    public GameObject PanelBeginMiniGame; 

    //ステート
    //1P操作待機状態、2P…
    //サイコロが振られる、プレイヤーが出た目の数進むアニメーション中などのプレイヤー操作不可状態

    //プレイヤーの操作可能状態、操作不可状態のオンオフで、カレントプレイヤーの変数用意しといたらいいかな

    enum EnMainGameState
    {
        enWait,
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
    //const string SCENE_JUMP = "MiniGameJumpAthleticScene";
    //ミニゲームシーンの配列 ランダムインデックスで呼ぶ
    string[] _miniGameScenes = { SCENE_GEMIN, SCENE_SCROLL/*, SCENE_JUMP*/ };

    string[] MINIGAME_TITLE = { "下民暮らし", "Run!Run!Run!", "ジャンプアスレチック" };

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
    //Vector3 _plPosOld;
    //プレイヤーの移動先位置
    //Vector3 _targetPos;

    //マス
    Transform[] _squares;

    GameObject[] _players;

    CameraFollowPlayer _cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("First:" + _currentPlayer + "P");
        AnnounceText.text = "1Pのターン！";
        TextA.SetActive(true);

        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;
        //サイコロの初期位置取得しておく
        _dicePos = Dice.transform.position;

        //マスを上から順に取得 仮リストに入れて
        List<Transform> tmp = new List<Transform>();
        tmp.AddRange(SquaresParent.GetComponentsInChildren<Transform>());
        //一番最初の要素(親オブジェクトを除去)して
        tmp.RemoveAt(0);
        //配列に格納
        _squares = tmp.ToArray();

        //でばっぐ ちゃんと上から順にマスが入ってる
        /*for(int i = 0; i < _squares.Length; i++)
        {
            Debug.Log(_squares[i].name);
        }*/

        //プレイヤーを取得
        _players = new GameObject[PlayersParent.transform.childCount];
        for(int i = 0; i < PlayersParent.transform.childCount; i++)
        {
            _players[i] = PlayersParent.transform.GetChild(i).gameObject;
            _players[i].GetComponent<MainPlayer>().SetPlNum(i + 1);
        }

        _cameraScript = Camera.main.GetComponent<CameraFollowPlayer>();
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

                //何もしない
            case EnMainGameState.enWait:

            default:
                break;
        }

    }

    //Aボタンでサイコロ振る待機
    void PressA()
    {
        if (Input.GetButtonUp(_plInputTextA))
        {
            TextA.SetActive(false);
            //サイコロが動くステート
            _mainState = EnMainGameState.enDiceRoll;
            //クールタイムをリセット（仮なので時間でサイコロ止まる）
            _coolTime = 0.0f;
            //サイコロのkinematicを解除して動かす
            Dice.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private IEnumerator NextState(EnMainGameState nextState)
    {
        _mainState = EnMainGameState.enWait;
        yield return new WaitForSeconds(1f);
        _mainState = nextState;

    }

    //ダイスが振られる
    void DiceRoll()
    {
        //クールタイム加算
        _coolTime += Time.deltaTime;
        //1秒立ったらとりあえず終わり
        if (_coolTime > 1.0f)
        {
            //サイコロを止める
            Dice.GetComponent<Rigidbody>().isKinematic = true;
            //サイコロを初期位置へ
            Dice.transform.position = _dicePos;
            Dice.transform.rotation = Quaternion.identity;

            Debug.Log("diceEnd");


            //サイコロの出目　（仮）ランダム　本物ではサイコロの上面を判定する
            int dice = Random.Range(1, 6);
            AnnounceText.text = _currentPlayer + "Pは" + dice + "マス進みます" ;
            Debug.Log(_currentPlayer + "P: " + dice);

            MainPlayer player = _players[_currentPlayer - 1].GetComponent<MainPlayer>();
            int plSquare = player.CurrentSquare;

            List<Vector3> targetPosList = new List<Vector3>();
            for(int i = 1; i <= dice; i++)
            {
                targetPosList.Add(_squares[plSquare + i].position);
            }

            //プレイヤー移動初期化
            player.InitMove(targetPosList);


            //プレイヤーが進むステートへ
            //_mainState = EnMainGameState.enMovePlayer;
            //コルーチン
            StartCoroutine(NextState(EnMainGameState.enMovePlayer));
        }

    }

    IEnumerator NextPlayer()
    {
        _mainState = EnMainGameState.enWait;

        yield return new WaitForSeconds(1f);

        _cameraScript.ChangeFollow(_currentPlayer);

        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

        Debug.Log("Next:" + _currentPlayer + "P");
        AnnounceText.text = _currentPlayer + "Pのターン！";
        TextA.SetActive(true);

        //サイコロ待機へ
        _mainState = EnMainGameState.enIdle;

    }

    //プレイヤーが進む
    void MovePlayer()
    {
        //プレイヤーの移動呼ぶ
        if (_players[_currentPlayer - 1].GetComponent<MainPlayer>().Move())
        {
            //移動終わったら
            Debug.Log("moveEnd");

            //次のプレイヤー
            _currentPlayer += 1;

            //4以下なら次のプレイヤーがサイコロを振る
            if (_currentPlayer <= 4)
            {
                StartCoroutine(NextPlayer());
            }
            //5になったら全員終わったので
            else
            {
                int miniGameInd = Random.Range(0, _miniGameScenes.Length);

                PanelBeginMiniGame.SetActive(true);

                PanelBeginMiniGame.transform.Find("TextBeginMiniGame").GetComponent<Text>().text
                    = "ミニゲーム" + MINIGAME_TITLE[miniGameInd] + "を開始します";


                _mainState = EnMainGameState.enWait;
                
                //ランダムでミニゲームを呼び出す
                //SceneManager.LoadScene(_miniGameScenes[Random.Range(0, _miniGameScenes.Length)]);
            }
        }
    }
}

    //--出た目の数進む
    //--プレイヤーごとのカレントマスデータも記録しないとな
    //--踏んだマスに応じてイベント

