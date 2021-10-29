using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//メインゲームのプレイヤースクリプト

//自分の今いるマスとか保持して
//各自で移動アニメーションして


//Startの　テキストアップデートは　メインゲームのデータロード後に呼んでほしいので
//Playerはメインゲームが生成するようにした方が安定
//プレイ人数の変更にも対応できるし
//…まあミニゲームもあるし面倒くさいし別に人数は変更しなくていいか

//というかUnityはスクリプトの実行順指定できるのか～

public class MainPlayer : MonoBehaviour
{
    //--パラメータ
    //とりあえず割り当てで　めんどくさいので本当は検索とか　メインゲームに持たせとくとか　自分で生成して持っとくとかの方がいい
    public Text TextPoint;


    //----メンバ変数

    //--公開情報

    //自分が何Pか
    int _plNum = 0;

    //今いるマスのインデックス　(分岐なし前提でスタートから連番)
    int _currentSquare = 0;
    public int CurrentSquare { get { return _currentSquare; } set { _currentSquare = value; } }
    public Vector3 Position { set { transform.position = value;} }

    //所持ポイント
    int _point = 0;
    public int Point { get { return _point; } set { _point = value; } }

    //所持アイテム
    //todo

    //--内部値

    //プレイヤーの移動前位置
    Vector3 _oldPos;
    //プレイヤーの移動先位置
    //Vector3 _targetPos;

    //移動アニメーションの進行時間
    float _moveTime;

    //移動先座標リスト
    List<Vector3> _targetPosList;
    //移動先リストの何個目か
    int _targetPosListInd;

    //移動中か
    bool _isMove;

    //y位置　Startで取得　このカプセルモデルだと埋まるのでとりあえず　なくなるかも
    float _yPos = 0.86f;
    //みんなマスの中央だと重なるので四隅に寄ってもらう
    //けどデフォルトでそれだと見栄えが微妙だったので同じマスに重なったときだけズレてもらう処理にするか
    Vector3 _plOffset = Vector3.zero;


    //--定数
    //何秒で移動するか (とりあえず1.0なので存在する意味はないけど)
    const float MOVE_DURATION = 1.0f;
    //オフセットの間隔
    const float OFFSET_DURATION = 0.66f;


    // Start is called before the first frame update
    void Start()
    {
        //y位置取得　0だと埋まるので
        //_yPos = transform.position.y;

        ApplyPointText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //オフセット適用
    public void ApplyOffset()
    {
        Vector3 pos = transform.position + _plOffset;
        pos.y = _yPos;

        transform.position = pos;
    }

    //何Pか指定してオフセットを作成
    public void SetPlNum(int num)
    {
        _plNum = num;

        switch (num)
        {
            case 1:
                _plOffset = new Vector3(-OFFSET_DURATION, 0.0f, OFFSET_DURATION);
                break;
            case 2:
                _plOffset = new Vector3(OFFSET_DURATION, 0.0f, OFFSET_DURATION);
                break;
            case 3:
                _plOffset = new Vector3(-OFFSET_DURATION, 0.0f, -OFFSET_DURATION);
                break;
            case 4:
                _plOffset = new Vector3(OFFSET_DURATION, 0.0f, -OFFSET_DURATION);
                break;
        }
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

        //y位置調整やオフセット
        Vector3 targetPos = _targetPosList[_targetPosListInd];
        targetPos.y = _yPos;
        //targetPos += _plOffset;

        //線形補間移動
        //プレイヤーtransform直動かしだけどメインゲームでは当たり判定とかも使わないだろうし大丈夫だと思う
        transform.position = Vector3.Lerp(_oldPos, targetPos, _moveTime / MOVE_DURATION);

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


    public void AddCoin(int coin)
    {
        _point += coin;
        ApplyPointText();
    }

    //テキスト更新
    void ApplyPointText()
    {
        TextPoint.text = _plNum + "P\n" + _point + "Point";
    }

}
