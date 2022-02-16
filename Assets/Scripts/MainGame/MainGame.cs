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

    public GameObject DiceArea;
    //マスの親オブジェクト(フォルダ代わり)　ここから順番にマスを取得
    public GameObject SquaresParent;

    //A
    public GameObject TextA;

    //「ミニゲーム◯◯を開始します」ってUIのパネル
    public GameObject PanelBeginMiniGame;
    public GameObject PanelGameEnd;

    public Text TextTurn;

    /*//コインのオブジェクト
    private GameObject coin;
    //宝箱のオブジェクト
    private GameObject chest;
    //インスタンス
    private GameObject instance;*/
    //コインアップの効果音
    public AudioClip soundCoinUp;
    //コインダウンの効果音
    public AudioClip soundCoinDown;
    //サイコロの効果音
    public AudioClip soundDice;
    //サイコロの決定
    public AudioClip soundDic;
    //クリック音
    public AudioClip soundClick;
    
    AudioSource audioSource;

    //テキスト背景バーのオブジェクト
    public GameObject image;

    //ステート
    //1P操作待機状態、2P…
    //サイコロが振られる、プレイヤーが出た目の数進むアニメーション中などのプレイヤー操作不可状態

    enum EnMainGameState
    {
        enWait,
        enDiceRoll,
        enDiceEnd,
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

    const float FILL_AMOUNT_OFFSET = 1.57f;

    //ミニゲームシーンの配列 ランダムインデックスで呼ぶ
    string[] _miniGameScenes = { SCENE_GEMIN, SCENE_SCROLL, SCENE_JUMP };

    //ミニゲーム名　「ミニゲーム◯◯を開始します」
    string[] MINIGAME_TITLE = { "下民暮らし", "らん・RUN・ラン", "JUMPアスレチック" };

    int _currentTurn;

    //今何Pのターンか 1～4
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


    int _diceNum;

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

        /*//コインオブジェクトの取得
        coin = (GameObject)Resources.Load("coins");
        chest = (GameObject)Resources.Load("chest_open");*/
        //audioComponentを取得
        audioSource = GetComponent<AudioSource>();
        //テキストバーを取得
        //image = GameObject.Find("TextBar");
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

            case EnMainGameState.enDiceEnd:
                DiceEndMove();
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

    void DiceRotation()
    {
        Dice.transform.Rotate(new Vector3(0, -1.9f, 2), Space.Self);
    }

    //Aボタンでサイコロ振る待機
    void PressA()
    {
        //サイコロ回転
        DiceRotation();
        //Aボタン押したら
        if (Input.GetButtonUp(_plInputTextA))
        {
            //Aボタンテキスト消す
            TextA.SetActive(false);
            //サイコロが動くステートへ
            _mainState = EnMainGameState.enDiceRoll;
            //クールタイムをリセット（仮なので時間でサイコロ止まる）
            _coolTime = 0.0f;
            //サイコロの重力をオンにして動かす
            Dice.GetComponent<Rigidbody>().useGravity = true;
            Dice.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
            //サイコロの効果音
            audioSource.PlayOneShot(soundDice);
        }
    }

    //ダイスが振られる
    /*void DiceRoll()
    {
        //クールタイム加算
        _coolTime += Time.deltaTime;
        //1秒ぐらい経ったらとりあえずサイコロ動作終わり
        if (_coolTime > 3f)
        {
            //サイコロを止める
            Dice.GetComponent<Rigidbody>().useGravity = false;
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
    }*/

    void DiceRoll()
    {
        //クールタイム加算
        _coolTime += Time.deltaTime;

        //Debug.Log(Dice.GetComponent<Rigidbody>().velocity.magnitude);

        //少しの間まだ回転しててもらう
        if(_coolTime < 1.0f)
        {
            //サイコロ回転
            DiceRotation();
        }
        //止まったら もしくは10秒経ったら
        else if (Dice.GetComponent<Rigidbody>().velocity.magnitude < 0.0001f || _coolTime > 10f)
        {
            //サイコロの効果音を再生
            audioSource.PlayOneShot(soundDic);
            //サイコロ決定
            StartCoroutine(DiceEnd());
        }
    }

    int GetNumber()
    {
        int result = 0;

        float innerProductX = Vector3.Dot(Dice.transform.right, Vector3.up);
        float innerProductY = Vector3.Dot(Dice.transform.up, Vector3.up);
        float innerProductZ = Vector3.Dot(Dice.transform.forward, Vector3.up);

        Debug.Log("X:" + innerProductX);
        Debug.Log("Y:" + innerProductY);
        Debug.Log("Z:" + innerProductZ);

        if ((Mathf.Abs(innerProductX) > Mathf.Abs(innerProductY)) && (Mathf.Abs(innerProductX) > Mathf.Abs(innerProductZ)))
        {
            // X軸が一番近い
            if (innerProductX > 0f) result = 2;
            else result = 5;
        }
        else if ((Mathf.Abs(innerProductY) > Mathf.Abs(innerProductX)) && (Mathf.Abs(innerProductY) > Mathf.Abs(innerProductZ)))
        {
            // Y軸が一番近い
            if (innerProductY > 0f) result = 4;
            else result = 3;
        }
        else
        {
            // Z軸が一番近い
            if (innerProductZ > 0f) result = 1;
            else result = 6;
        }

        return result;
    }

    void DiceEndMove()
    {
        if (Dice.GetComponent<Dice>().Move())
        {
            //メッセージ
            AnnounceText.text = _currentPlayer + "Pは" + _diceNum + "マス進みます";
            Debug.Log(_currentPlayer + "P: " + _diceNum);
            //ImageというコンポーネントのfillAmountを取得して操作する
            image.GetComponent<Image>().fillAmount = 0.3f * FILL_AMOUNT_OFFSET;
            //プレイヤー取得
            MainPlayer player = _players[_currentPlayer - 1].GetComponent<MainPlayer>();
            //プレイヤーの現在マス取得
            int plSquare = player.CurrentSquare;

            //プレイヤーの移動対象の座標リストにする
            List<Vector3> targetPosList = new List<Vector3>();
            for (int i = 1; i <= _diceNum && plSquare + i < _squares.Length; i++)
            {
                //マスの座標を移動対象座標として追加
                targetPosList.Add(_squares[plSquare + i].position);
            }

            //プレイヤー移動初期化
            player.InitMove(targetPosList);

            //コルーチンでプレイヤーが進むステートへ
            StartCoroutine(NextPlayerStateCoroutine());
        }
    }

    private IEnumerator DiceEnd()
    {
        _mainState = EnMainGameState.enWait;

        yield return new WaitForSeconds(0.3f);

        //サイコロの出目
        //int dice = Random.Range(7, 7);
        //int dice = Random.Range(1, 7);
        _diceNum = GetNumber();

        Vector3 diceAngle = Vector3.zero;

        switch (_diceNum)
        {
            case 1:
                diceAngle = new Vector3(0, 180, 0);
                break;
            case 2:
                diceAngle = new Vector3(0, 90, 0);
                break;
            case 3:
                diceAngle = new Vector3(90, 0, 0);
                break;
            case 4:
                diceAngle = new Vector3(-90, 0, 0);
                break;
            case 5:
                diceAngle = new Vector3(0, -90, 0);
                break;
            case 6:
                break;
        }


        //サイコロを止める
        Dice.GetComponent<Rigidbody>().isKinematic = true;
        //サイコロを見えなくする
        //Dice.SetActive(false);
        //サイコロの回転をリセット
        //Dice.transform.rotation = Quaternion.identity;

        Dice.GetComponent<Dice>().InitMove(Vector3.one - _dicePosOffset, diceAngle);

        //出た目の位置へ回転
        //Dice.transform.eulerAngles = _diceAngle;
        //初期位置
        //Dice.transform.position = Camera.main.transform.position - _dicePosOffset;

        Debug.Log("diceEnd");

        _mainState = EnMainGameState.enDiceEnd;
    }

    private IEnumerator NextStateCoroutine(EnMainGameState nextState, float wait = 1.0f)
    {
        //待機ステートにする
        _mainState = EnMainGameState.enWait;
        //1秒待って
        yield return new WaitForSeconds(wait);
        //次のステートへ
        _mainState = nextState;

    }

    private IEnumerator NextPlayerStateCoroutine()
    {
        //待機ステートにする
        _mainState = EnMainGameState.enWait;
        //1秒待って
        yield return new WaitForSeconds(1.0f);

        //サイコロ
        Dice.GetComponent<Rigidbody>().isKinematic = false;
        Dice.GetComponent<Rigidbody>().useGravity = false;
        //サイコロを見えなくする
        Dice.SetActive(false);
        DiceArea.SetActive(false);
        //サイコロの回転をリセット
        Dice.transform.rotation = Quaternion.identity;

        //次のステートへ
        _mainState = EnMainGameState.enMovePlayer;
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
            yield return new WaitForSeconds(2.0f);
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
        Vector3 dicePos = Camera.main.transform.position - _dicePosOffset;
        Dice.transform.position = dicePos;
        dicePos.y += 1.0f;
        DiceArea.transform.position = dicePos;
        Dice.SetActive(true);
        DiceArea.SetActive(true);

        //◯PのAボタンのキー設定
        _plInputTextA = _currentPlayer.ToString() + BUTTON_A;

        //1Pのターンなら
        if (_currentPlayer == 1)
        {
            //ターン表示更新
            TextTurn.text = "ターン" + _currentTurn;
        }

        //◯Pのターン！
        Debug.Log("Next:" + _currentPlayer + "P");
        AnnounceText.text = _currentPlayer + "Pのターン！";
        //ImageというコンポーネントのfillAmountを取得して操作する
        image.GetComponent<Image>().fillAmount = 0.2f * FILL_AMOUNT_OFFSET;
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
                    //ImageというコンポーネントのfillAmountを取得して操作する
                    image.GetComponent<Image>().fillAmount = 0.52f * FILL_AMOUNT_OFFSET;
                    //コイン追加　とりあえず10
                    player.AddCoin(10);
                    //コインの効果音
                    audioSource.PlayOneShot(soundCoinUp);
                    
                    break;
                //コインマイナスマスなら
                case "SquareMinuCoin":
                    AnnounceText.text = "コインマイナスマス… " + _currentPlayer + "Pは5コイン没収！！";
                    //ImageというコンポーネントのfillAmountを取得して操作する
                    image.GetComponent<Image>().fillAmount = 3f * FILL_AMOUNT_OFFSET;
                    player.MinusCoin(5);
                    //コインの効果音
                    audioSource.PlayOneShot(soundCoinDown);

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

        //コインの効果音
        audioSource.PlayOneShot(soundCoinUp);   

        //1秒待って
        yield return new WaitForSeconds(1.0f);

        //ゴールしたプレイヤーをスタートに戻す
        AnnounceText.text = _currentPlayer + "Pはスタートに戻ります";
        player.transform.position = _squares[0].position;
        player.ApplyOffset();
        player.CurrentSquare = 0;

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
        _currentPlayer = 1;
        //データセーブ
        SaveParam();

        //1秒待って
        yield return new WaitForSeconds(1.0f);

        //AnnounceText.text = "ターン終了！ミニゲームを開始します！";
        //ImageというコンポーネントのfillAmountを取得して操作する
        //image.GetComponent<Image>().fillAmount = 0.6f;
        //ミニゲームを準備する
        //InitMinigame();

        //ターン20で終わり
        if(_currentTurn >= 10)
        {
            PanelGameEnd.SetActive(true);
            //1秒待って
            yield return new WaitForSeconds(1.0f);
            //スコアシーンへ
            SceneManager.LoadScene("ScoreScene");
        }
        else
        {
            StartCoroutine(NextPlayerCoroutine());
        }
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
        //クリック音の効果音
        audioSource.PlayOneShot(soundClick);
        //ミニゲームシーンを呼び出す
        SceneManager.LoadScene(_miniGameScenes[_minigameInd]);
    }


}
