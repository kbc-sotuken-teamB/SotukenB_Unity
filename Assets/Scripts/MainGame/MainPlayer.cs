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

    //ゴールした？
    bool _isGoal = false;
    public bool IsGoal { get { return _isGoal; } set { _isGoal = value; } }

    //Sound再生
    public AudioClip soundWolkPray;
    AudioSource audioSource;

    //アニメーション再生
    private Animator animator;
    
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
    float _yPos = 0.0f;
    //みんなマスの中央だと重なるので四隅に寄ってもらう
    //けどデフォルトでそれだと見栄えが微妙だったので同じマスに重なったときだけズレてもらう処理にするか
    Vector3 _plOffset = Vector3.zero;

    bool _isFast = false;

    int count = 0;


    //--定数
    //何秒で移動するかのデフォルト (とりあえず1.0なので存在する意味はないけど)
    const float MOVE_DURATION = 1.0f;
    //オフセットの間隔
    const float OFFSET_DURATION = 0.66f;


    // Start is called before the first frame update
    void Start()
    {
        //y位置取得　0だと埋まるので
        //_yPos = transform.position.y;

        //ポイントをテキストに反映
        ApplyPointText();

        //audioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pキーで移動早送り
        _isFast = Input.GetKey(KeyCode.P);
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
        float deltaTime = Time.deltaTime;
        //早送り
        if (_isFast){
            deltaTime *= 2.0f;
        }
        _moveTime = Mathf.Min(MOVE_DURATION, _moveTime + deltaTime);

        //y位置調整やオフセット
        Vector3 targetPos = _targetPosList[_targetPosListInd];
        targetPos.y = _yPos;
        targetPos += _plOffset;

        //線形補間移動
        //プレイヤーtransform直動かしだけどメインゲームでは当たり判定とかも使わないだろうし大丈夫だと思う
        //プレイヤーから次の座標までのベクトル
        Vector3 moveSpeed = targetPos - transform.position;
        moveSpeed.Normalize();
        transform.position += moveSpeed * 0.07f;
        //transform.position = Vector3.Lerp(_oldPos, targetPos, _moveTime / MOVE_DURATION);
        Vector3 diff = transform.position - _oldPos;   //前回からどこに進んだかをベクトルで取得
        _oldPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
        }

        float dist = Vector3.Distance(targetPos, transform.position);
        if (_isMove == true)
        {
            //移動処理？アニメーション再生、効果音の再生
            //足音を再生
            if (count == 0) audioSource.PlayOneShot(soundWolkPray);
            count = 1;
            
            animator.SetFloat("Speed", 0.2f);
        }


        //移動終わったら
        //if (_moveTime == MOVE_DURATION)
        if (dist < 0.1f)
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

                //アニメーション停止
                animator.SetFloat("Speed", 0.0f);
                count = 0;
                audioSource.Stop();
                diff = new Vector3(0, 0, -1);
                transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
                //移動終わったtrueを返す
                return true;
            }
            //（このタイミングで次のマスに進み始めるまでに間を置くことも可能）
        }

        //まだ移動終わってない　継続
        return false;
    }


    public void AddCoin(int coin)
    {     // 再生中か？
        animator.SetTrigger("Get");
        /*AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // layerNo:Base Layer == 0
        while (stateInfo.normalizedTime < 1.0f)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }*/
        
        _point += coin;
        ApplyPointText();
        
    }
    public void MinusCoin(int coin)
    {
        if (_point > 0)
        {
            _point -= coin;
        }
        ApplyPointText();
    }
    //テキスト更新
    void ApplyPointText()
    {
        TextPoint.text = _plNum + "P\n" + _point + "Point";
    }

}
