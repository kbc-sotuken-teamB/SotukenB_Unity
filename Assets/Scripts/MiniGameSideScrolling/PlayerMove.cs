using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //--パラメータ
    //何Pか指定
    public int PlayerNum;

    //--定数
    //スピード
    const float SPEED = 3.0f;
    //スタン時間設定
    const float STAN_DURATION = 3.0f;

    //--メンバ変数

    //現在のスタンクールタイム
    float _stanTime = 0.0f;
    //スタン状態か？
    bool _isStan = false;
    //死亡判定
    bool _isDead = false;

    //プレイヤーごとの入力を名前指定で取得するために使う
    //「○PHorizontal」
    string _plNumTextHorizontal;
    string _plNumTextVertical;
    //キャラコン
    CharacterController _controller;
    // Start is called before the first frame update
    void Start()
    {
        //キャラコン取得
        _controller = GetComponent<CharacterController>();
        //「○PHorizontal」のテキスト作成
        _plNumTextHorizontal = PlayerNum.ToString() + "PHorizontal";
        _plNumTextVertical = PlayerNum.ToString() + "PVertical";

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

    void MovePlayer()
    {
        //左右入力取得、速度乗算
        float moveX = Input.GetAxis(_plNumTextHorizontal) * SPEED * Time.deltaTime;
        float moveZ = Input.GetAxis(_plNumTextVertical) * SPEED * Time.deltaTime;
        //キャラコンでMove
        _controller.Move(new Vector3(moveX, 0.0f, moveZ));
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
}
