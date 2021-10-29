using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

//プレイヤー

//落ちてくる物にぶつかったらタグで分岐
//コインならポイントアップ
//爆弾ならスタン、ポイントダウン

//スタン待ち時間・ポイントはプレイヤー各自管理


//----他スクリプト

//--ゲームマネージャ
//制限時間管理　ゲーム終了時に各プレイヤーのポイントを判定
//コインとボムをランダムに降らせる　終わったら止める

//--UI
//各プレイヤーのポイントを表示
//ポイント増減したタイミングでプレイヤーがsetを呼んで更新する


//普通に並べてるだけだと左右干渉するんで離して置いて画面分割するか
//並べたままで仕切り置いてもいいか？　端に寄りまくると隣のレーンに降ってきたやつ横取りできちゃいそうから駄目か

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class GeminPlayer : MonoBehaviour
{
    //--パラメータ
    //何Pか指定
    public int PlayerNum;

    //--定数
    //スピード
    const float SPEED = 5.5f;
    //スタン時間設定
    const float STAN_DURATION = 3.0f;

    //--メンバ変数
    //現在のポイント
    int _point = 0;
    //現在のスタンクールタイム
    float _stanTime = 1.0f;
    //スタン状態か？
    bool _isStan = false;

    //プレイヤーごとの入力を名前指定で取得するために使う
    //「○PHorizontal」
    string _plNumTextHorizontal;
    //キャラコン
    CharacterController _controller;
    //スコア更新用オブジェクト
    GameObject scoreObj;

    // Start is called before the first frame update
    void Start()
    {
        //キャラコン取得
        _controller = GetComponent<CharacterController>();
        //「○PHorizontal」のテキスト作成
        _plNumTextHorizontal = PlayerNum.ToString() + "PHorizontal";
        //スコア更新用オブジェクト
        scoreObj = GameObject.Find("ScoreManager");
    }

    // Update is called once per frame
    void Update()
    {
        //スタンしてなかったら
        if (!_isStan)
        {
            //移動可能
            MovePlayer();
        }
        //スタンしてたら
        else
        {
            //移動不可　スタン時間処理
            CheckStan();
        }
    }

    //入力で左右に動く
    void MovePlayer()
    {
        //左右入力取得、速度乗算
        float moveX = Input.GetAxis(_plNumTextHorizontal) * SPEED * Time.deltaTime;
        //キャラコンでMove
        _controller.Move(new Vector3(moveX, 0.0f, 0.0f));
    }

    //スタン時間の処理
    void CheckStan()
    {
        //スタン時間減らす
        _stanTime -= Time.deltaTime;
        //0になったらスタン状態解除
        if (_stanTime <= 0.0f)
        {
            _isStan = false;
            Debug.Log("stanDissabled");
        }
    }

    //--Catchスクリプトから呼ばれる
    //コインをキャッチ
    public void CatchCoin()
    {
        if (!_isStan)
        {
            //ポイント加算
            scoreObj.GetComponent<ScoreScript>().AddScore(PlayerNum - 1);

            Debug.Log("point:" + _point);
        }
    }
    //爆弾をキャッチ
    public void CatchBomb()
    {
        //ポイント減算
        scoreObj.GetComponent<ScoreScript>().SubScore(PlayerNum - 1);

        //スタン中にする
        _isStan = true;
        //スタン時間初期化
        _stanTime = STAN_DURATION;

        Debug.Log("stan");
    }

}
