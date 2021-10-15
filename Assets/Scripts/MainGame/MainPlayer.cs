using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メインゲームのプレイヤースクリプト

//自分の今いるマスとか保持して
//各自で移動アニメーションして

public class MainPlayer : MonoBehaviour
{
    //今いるマスのインデックス　分岐なし前提
    int _currentSquare = 0;
    public int CurrentSquare { get { return _currentSquare; } }

    //プレイヤーの移動前位置
    Vector3 _oldPos;
    //プレイヤーの移動先位置
    Vector3 _targetPos;

    //何秒で移動するか とりあえず1だと存在する意味はないけど
    const float MOVE_DURATION = 1.0f;
    //移動アニメーションの進行時間
    float _moveTime;

    //移動先座標リスト
    List<Vector3> _targetPosList;
    //移動先リストの何個目か
    int _targetPosListInd;

    bool _isMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //移動対象リスト渡して移動開始
    public void InitMove(List<Vector3> targetPosList)
    {
        //移動前位置保持
        _oldPos = transform.position;
        //リストをコピー
        _targetPosList = new List<Vector3>(targetPosList);
        _targetPosListInd = 0;
        _moveTime = 0.0f;
        _isMove = true;
    }

    //メインゲームスクリプトから呼ぶ
    public bool Move()
    {
        //移動しないなら帰る (本来来ないけどInitMoveされずにMove呼んだりしたらここで帰る)
        if (!_isMove)
        {
            Debug.LogWarning("_isMove:falseの状態でMoveが呼ばれました");
            return true;
        }


        //移動進行度加算
        _moveTime = Mathf.Min(MOVE_DURATION, _moveTime + Time.deltaTime);

        //線形補間移動
        //プレイヤーtransform直動かしだけどメインゲームでは当たり判定とかも使わないだろうし大丈夫だと思う
        transform.position = Vector3.Lerp(
            _oldPos, _targetPosList[_targetPosListInd], _moveTime / MOVE_DURATION);

        //移動終わったら
        if(_moveTime == MOVE_DURATION)
        {
            _moveTime = 0.0f;
            _oldPos = transform.position;
            _currentSquare++;

            //移動対象インクリメント(前置)して
            //もし配列外になったなら
            if (++_targetPosListInd >= _targetPosList.Count)
            {
                //終わり
                _isMove = false;
                //移動終わったtrueを返す
                return true;
            }
            //（このタイミングで次のマスに進み始めるまでに間を置くことも可能）
        }

        //まだ移動終わってない　継続
        return false;
    }

}
