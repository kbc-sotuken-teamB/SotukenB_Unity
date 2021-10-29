using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーに追従するカメラ
public class CameraFollowPlayer : MonoBehaviour
{
    //--参照
    //プレイヤーの親オブジェクト
    public GameObject PlayersParent;
    //1マス目がいい感じにカメラの真ん中にあるので目安位置マーカーとして使う
    public GameObject TargetPosition;

    //カメラ位置＋オフセット＝追従カメラ
    Vector3 _cameraOffset;

    GameObject[] _players;
    int _plCount;
    int _followPlNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー数取得
        _plCount = PlayersParent.transform.childCount;
        _players = new GameObject[_plCount];
        for (int i = 0; i < _plCount; i++)
        {
            //プレイヤーを取得
            _players[i] = PlayersParent.transform.GetChild(i).gameObject;
        }

        _cameraOffset = transform.position - TargetPosition.transform.position;
        //~プレイヤーの高さ
        _cameraOffset.y -= 0.86f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _players[_followPlNum - 1].transform.position + _cameraOffset;
    }

    public void ChangeFollow(int plNum)
    {
        _followPlNum = plNum;

        //パッと切り替わる
        //カメラスライドしていって切り替わる演出とかしたかったらここから
        //やらないなら単に_followPlNum変数のパブリックセッター作った方がいいです
    }
}
