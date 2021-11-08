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
//ランダムにミニゲームを決定して、ミニゲームスタートボタンを押したらシーン遷移

//ゲームデータの保持…
//データ保持用スクリプト(MainGameData)に　プレイヤーの位置と持ち物だけ保持
//MainGameのスタートでデータロード　保持データがあれば続きを再現する



//Start→(LoadParam)
//→CheckNextPlayer(ターン終了ならミニゲームへ)→NextPlayerCoroutine→PlayerTurnCoroutine
//→enIdle→PressA→enDiceRoll→DiceRoll→NextStateCoroutine→enMovePlayer→MovePlayer→(マス判定)
//→CheckNextPlayer(ターン終了ならミニゲームへ)…


public class MainGame : MonoBehaviour
{
    //--参照
    public Text AnnounceText;
    //プレイヤー親
    public GameObject PlayersParent;

    //ダイス
    public GameObject Dice;
    //マスの親オブジェクト(フォルダ代わり)　ここから順番にマスを取得
    public GameObject SquaresParent;

    //A
    public GameObject TextA;

    //「ミニゲーム◯◯を開始します」ってUIのパネル
    public GameObject PanelBeginMiniGame;

    public Text TextTurn;

    //ステート
    //1P操作待機状態、2P…
    //サイコロが振られる、プレイヤーが出た目の数進むアニメーション中などのプレイヤー操作不可状態

    enum EnMainGameState
    {
        enWait,
        enDiceRoll,
        enMovePlayer,
        enIdle,
        //enNextPlayer
    }

    //現在のステート
    EnMainGameState _mainState = EnMainGameState.enWait;

    //これに今何Pのターンか取得してくっつける
    const string BUTTON_A = "PButtonA";

    //ミニゲームシーン名
    const string SCENE_GEMIN = "MinGameGEMIN";
    const string SCENE_SCROLL = "MiniGameSideScrolling";
    const string SCENE_JUMP = "MiniGamejump";
    //ミニゲームシーンの配列 ランダムインデックスで呼ぶ
    string[] _miniGameScenes = { SCENE_GEMIN, SCENE_SCROLL, SCENE_JUMP };

    //ミニゲーム名　「ミニゲーム◯◯を開始します」
    string[] MINIGAME_TITLE = { "下民暮らし", "らん・RUN・ラン", "JUMPアスレチック" };

    int _currentTurn;

    //今何Pのターンか
    int _currentPlayer = 0;
    //bool _isIdle = true;
    //「○PButtonA」
    string _plInputTextA;

    //クールタイム
    float _coolTime = 0.0f;

    //マス
    Transform[] _squares;
    //プレイヤー
    GameObject[] _players;
    //追従カメラ
    CameraFollowPlayer _cameraScript;
    //次のミニゲーム
    int _minigameInd = -1;

    Vector3 _dicePosOffset;

    bool _isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        //サイコロの初期位置取得
        _dicePosOffset = Camera.main.transform.position - Dice.transform.position;

        //マスを上から順に取得 仮リストに入れて
        List<Transform> tmp = new List<Transform>();
        tmp.AddRange(SquaresParent.GetComponentsInChildren<Transform>());
        //一番最初の要素(親オブジェクトを除去)して
        tmp.RemoveAt(0);
        //配列に格納
        _squares = tmp.ToArray();

        //GetComponentsInChildrenは上から順に取得するので
        //ちゃんと順番通りにマスが格納されてる　デバッグ用
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

        //保持していたデータロード
        LoadParam();

        //ターン更新
        TextTurn.text = "ターン" + _currentTurn;

        //カメラを現在プレイヤーに合わせる
        _cameraScript.ChangeFollow(_currentPlayer);

        //プレイヤーのターン開始
        CheckNextPlayer();
        //StartCoroutine(PlayerTurn());
    }

    // Update is called once per frame
    void Update()
    {
        switch (_mainState)
        {
            case EnMainGameState.enIdle:
                //Aボタンでサイコロ振る待機
                //1P:z 2P:b 3P:1 4P:5
                //ボタン押される判定
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

            /*case EnMainGameState.enNextPlayer:
                CheckNextPlayer();
                break;*/

                //何もしない
            case EnMainGameState.enWait:

            default:
                break;
        }
    }

    //保持していたデータロード
    void LoadParam()
    {
        //シーン跨いで保持されるメインゲームデータ
        //MainGameData mainGameData = MainGameData.Instance;
        MainGameData.SMainGameData mainGameData = MainGameData.Instance.SMainData;

        _currentPlayer = mainGameData.CurrentPlayer;
        _currentTurn = mainGameData.CurrentTurn;

        for (int i = 0; i < _players.Length; i++)
        {
            MainPlayer pl = _players[i].GetComponent<MainPlayer>();

            //現在のマス
            pl.CurrentSquare = mainGameData.CurrentSquares[i];

            //プレイヤーの現在マスが最大マスならゴールしてる
            pl.IsGoal = pl.CurrentSquare == _squares.Length - 1;

            //現在位置
            pl.Position = _squares[pl.CurrentSquare].position;
            pl.ApplyOffset();

            //現在のポイント
            pl.Point = mainGameData.Points[i];
        }
    }


    //Aボタンでサイコロ振る待機
    void PressA()
    {
        //Aボタン押したら
        if (Input.GetButtonUp(_plInputTextA))
        {
            //Aボタンテキスト消す
            TextA.SetActive(false);
            //サイコロが動くステートへ
            _mainState = EnMainGameState.enDiceRoll;
            //クールタイムをリセット（仮なので時間でサイコロ止まる）
            _coolTime = 0.0f;
            //サイコロのkinematicを解除して動かす
            Dice.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //ダイスが振られる
    void DiceRoll()
    {
        //クールタイム加算
        _coolTime += Time.deltaTime;
        //1秒ぐらい経ったらとりあえずサイコロ動作終わり
        if (_coolTime > 1.5f)
        {
            //サイコロを止める
            Dice.GetComponent<Rigidbody>().isKinematic = true;
            //サイコロを見えなくする
            Dice.SetActive(false);
            //サイコロの回転をリセット
            Dice.transform.rotation = Quaternion.identity;

            Debug.Log("diceEnd");


            //サイコロの出目　（仮）ランダム　本物ではサイコロの上面を判定する　たぶんできる
            //int dice = Random.Range(7, 7);
            int dice = Random.Range(1, 7);
            AnnounceText.text = _currentPlayer + "Pは" + dice + "マス進みます" ;
            Debug.Log(_currentPlayer + "P: " + dice);

            //プレイヤー取得
            MainPlayer player = _players[_currentPlayer - 1].GetComponent<MainPlayer>();
            //プレイヤーの現在マス取得
            int plSquare = player.CurrentSquare;

            //プレイヤーの移動対象の座標リストにする
            List<Vector3> targetPosList = new List<Vector3>();
            for(int i = 1; i <= dice && plSquare + i < _squares.Length; i++)
            {
                //マスの座標を移動対象座標として追加
                targetPosList.Add(_squares[plSquare + i].position);
            }

            //プレイヤー移動初期化
            player.InitMove(targetPosList);

            //コルーチンでプレイヤーが進むステートへ
            StartCoroutine(NextStateCoroutine(EnMainGameState.enMovePlayer));
        }
    }

    private IEnumerator NextStateCoroutine(EnMainGameState nextState)
    {
        //待機ステートにする
        _mainState = EnMainGameState.enWait;
        //1秒待って
        yield return new WaitForSeconds(1f);
        //次のステートへ
        _mainState = nextState;

    }

    IEnumerator NextPlayerCoroutine()
    {
        //待機ステートにする
        _mainState = EnMainGameState.enWait;

        if (_isFirst)
        {
            //初回のみ待たない
            _isFirst = false;
        }
        else
        {
            //1秒待って
            yield return new WaitForSeconds(1.0f);
        }
        //カメラを現在プレイヤーに合わせる
        _cameraScript.ChangeFollow(_currentPlayer);
        //プレイヤーのターン開始コルーチン
        StartCoroutine(PlayerTurnCoroutine());
    }

    //プレイヤーのターン開始
    IEnumerator PlayerTurnCoroutine()
    {
        yield return null;
        //1フレーム待ってカメラが合わさってから
        //カメラから一定の位置にサイコロを置く
        Dice.transform.position = Camera.main.transform.position - _dicePosOffset;
        Dice.SetActive(true);

        //◯PのAボタンのキー設定
        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

        //◯Pのターン！
        Debug.Log("Next:" + _currentPlayer + "P");
        AnnounceText.text = _currentPlayer + "Pのターン！";
        //Aボタン入力を促すただの「A」だけのテキスト表示　クソ雑　本来ボタンアイコンとかにするべき
        TextA.SetActive(true);

        //サイコロ待機へ
        _mainState = EnMainGameState.enIdle;
    }

    //プレイヤーが進む
    void MovePlayer()
    {
        //プレイヤーの移動呼ぶ　true返ってきたら移動完了なので
        if (_players[_currentPlayer - 1].GetComponent<MainPlayer>().Move())
        {
            Debug.Log("moveEnd");


            //--ここでマスのイベントチェック
            MainPlayer player = _players[_currentPlayer - 1].GetComponent<MainPlayer>();
            int current = player.CurrentSquare;

            //プレイヤーの現在マスが最大マスならゴールしてる
            if(current == _squares.Length - 1)
            {
                //player.IsGoal = true;
                StartCoroutine(GoalCoroutine());
                return;
            }

            //複数イベントマス作るときはswitchにする

            switch (_squares[current].gameObject.tag)
            {
                //コインマスなら
                case "SquareAddCoin":
                    AnnounceText.text = "コインマス！ " + _currentPlayer + "Pは10コインゲット！";
                    //コイン追加　とりあえず10
                    player.AddCoin(10);
                    break;

                    //ミニゲームマスなら
                case "SquareMinigame":
                    AnnounceText.text = "ミニゲームマス！";
                    SaveParam();
                    //ミニゲームを開始
                    InitMinigame();
                    return;
            }

            //次のプレイヤーへ
            CheckNextPlayer();
            //_mainState = EnMainGameState.enNextPlayer;
        }
    }

    IEnumerator GoalCoroutine()
    {
        _mainState = EnMainGameState.enWait;

        Debug.Log(_currentPlayer + "P is goal!");
        AnnounceText.text = _currentPlayer + "Pゴール！ 100ポイントゲット！";
        //ボーナス100ポイント
        MainPlayer player = _players[_currentPlayer - 1].GetComponent<MainPlayer>();
        player.AddCoin(100);

        //1秒待って
        yield return new WaitForSeconds(1.0f);

        //ゴールしたプレイヤーをスタートに戻す
        AnnounceText.text = _currentPlayer + "Pはスタートに戻ります";
        player.transform.position = _squares[0].position;
        player.ApplyOffset();
        player.CurrentSquare = 0;

        //次のプレイヤーへ
        CheckNextPlayer();
    }

    //次のプレイヤーをチェック (関数の最後で呼んでほしい)
    void CheckNextPlayer()
    {
        _currentPlayer++;

        //4以下なら次のプレイヤーがサイコロを振る
        if (_currentPlayer <= 4)
        {
            StartCoroutine(NextPlayerCoroutine());
        }
        //5になったら全員終わったので
        else
        {
            StartCoroutine(TurnEndCoroutine());
        }
    }

    IEnumerator TurnEndCoroutine()
    {
        _mainState = EnMainGameState.enWait;

        //ターン増加
        _currentTurn++;
        //カレントプレイヤーを0に戻して
        _currentPlayer = 0;
        //データセーブ
        SaveParam();

        //1秒待って
        yield return new WaitForSeconds(1.0f);

        AnnounceText.text = "ターン終了！ミニゲームを開始します！";
        //ミニゲームを準備する
        InitMinigame();
    }

    void InitMinigame()
    {
        //ミニゲームをランダムで決める
        _minigameInd = Random.Range(0, _miniGameScenes.Length);
        //ミニゲーム開始ボタンのUI表示
        PanelBeginMiniGame.SetActive(true);
        //テキストを表示
        PanelBeginMiniGame.transform.Find("TextBeginMiniGame").GetComponent<Text>().text
            = "ミニゲーム" + MINIGAME_TITLE[_minigameInd] + "を開始します";

        //ミニゲームスタートボタン待機へ
        _mainState = EnMainGameState.enWait;
    }

    //データセーブ
    void SaveParam()
    {
        Debug.Log("DataSave");

        //宣言
        int[] squares = new int[_players.Length];
        int[] points = new int[_players.Length];
        //forで回して取得
        for (int i = 0; i < _players.Length; i++)
        {
            MainPlayer pl = _players[i].GetComponent<MainPlayer>();
            squares[i] = pl.CurrentSquare;
            points[i] = pl.Point;
        }

        //構造体
        MainGameData.SMainGameData mainGameData = new MainGameData.SMainGameData
        {
            CurrentSquares = squares,
            Points = points,
            CurrentPlayer = _currentPlayer,
            CurrentTurn = _currentTurn,
        };

        //セーブデータオブジェクトに渡してセーブ
        //MainGameData.Instance.SaveParam(squares, points);
        MainGameData.Instance.SaveParam(mainGameData);
    }

    //一周後現れるミニゲームスタートボタンを押したらミニゲーム開始
    public void BeginMiniGameButtonOnClick()
    {
        //ミニゲームシーンを呼び出す
        SceneManager.LoadScene(_miniGameScenes[_minigameInd]);
    }


}
