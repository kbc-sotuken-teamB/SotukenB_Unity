using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//とりあえずプレイヤーが入力で自由に動けるコントローラー
//1P:↑←↓→
//2P:WASD
//3P:TFGH
//4P:IJKL
//をUnityプロジェクト設定のInputに登録しました
//（ゲームパッドっぽい設定も一応したのでパッドでも動くかも）

//Input.GetAxis("1PHorizontal") で1Pの左右軸　"1PVertical"で上下軸　が取得できます

//インスペクタービューでPlayerNumにこれは何Pなのか指定してね！

public class PlayerController : MonoBehaviour
{
    //--パラメータ
    //何Pか指定
    public int PlayerNum;

    //--定数
    //スピード
    const float SPEED = 3.0f;

    //--メンバ変数
    //プレイヤーごとの入力を名前指定で取得するために使う
    //「○PHorizontal」
    string _plNumTextHorizontal;
    //「○PVertical」
    string _plNumTextVertical;
    //キャラコン
    CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        //キャラコン取得
        _controller = GetComponent<CharacterController>();
        //「○PHorizontal」「○PVertical」のテキスト作成
        _plNumTextHorizontal = PlayerNum.ToString() + "PHorizontal";
        _plNumTextVertical = PlayerNum.ToString() + "PVertical";
    }

    // Update is called once per frame
    void Update()
    {
        //入力で移動
        MovePlayer();
    }

    void MovePlayer()
    {
        //左右上下入力取得
        float moveX = Input.GetAxis(_plNumTextHorizontal);
        float moveZ = Input.GetAxis(_plNumTextVertical);
        //ベクター作成
        Vector3 moveDirection = new Vector3(moveX, 0.0f, moveZ);
        //速度とデルタタイム乗算
        moveDirection *= SPEED * Time.deltaTime;

        //キャラコンでMove
        _controller.Move(moveDirection);
    }
}
