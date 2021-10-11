using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが落ちてくる物をキャッチする判定　かごのスクリプト
public class GeminPlayerCatch : MonoBehaviour
{
    //プレイヤーのスクリプト
    GeminPlayer _geminPlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        //親のプレイヤースクリプトを取得
        _geminPlayerScript = transform.parent.gameObject.GetComponent<GeminPlayer>();
    }

    //何かにぶつかった　キャッチした
    private void OnTriggerEnter(Collider other)
    {
        //デバッグ　ぶつかった
        Debug.Log("hit");

        //コイン
        if(other.gameObject.tag == "Coin")
        {
            //プレイヤーのコインキャッチを呼ぶ　ポイント加算
            _geminPlayerScript.CatchCoin();
        }
        //爆弾
        else if(other.gameObject.tag == "Bomb")
        {
            //プレイヤーの爆弾キャッチを呼ぶ　ポイント減算、スタン
            _geminPlayerScript.CatchBomb();
        }
        //どちらでもないなら
        else
        {
            //この後の削除処理は通らず帰る
            return;
        }

        //ぶつかったアイテムを削除　(爆発、キラキラ演出に置き換えるかも
        //コイン、爆弾に演出→Destroyする関数用意して呼ぶか　めんどくさい、オブジェクトのデストラクタとかできるんかな)
        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
