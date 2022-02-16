using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//▼1P ▼2P　みたいな表示テキスト生成してプレイヤーの頭にくっつけるやつ

public class PlayerNumTextManager : MonoBehaviour
{
    //--参照
    //プレイヤーの親オブジェクト
    public GameObject PlayersParent;
    //テキストprefab
    public Text PlNumTextPrefab;
    //テキストを格納する親オブジェクト(キャンバス内)
    public GameObject TextParent;

    //--メンバ変数
    //プレイヤーオブジェクト
    GameObject[] _players;
    //生成したテキスト
    Text[] _plNumTexts;
    //色
    Color[] _plColor = new Color[]{ Color.red, Color.blue, Color.yellow, Color.green };
    //プレイヤーの数
    int _plCount;

    //この数字の分上に表示する
    const float OFFSET_Y = 2.3f;

    const int FONT_SIZE = 50;

    Vector2 sizeXY = new Vector2(500.0f, 500.0f);

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー数取得
        _plCount = PlayersParent.transform.childCount;
        //配列を作成
        _players = new GameObject[_plCount];
        _plNumTexts = new Text[_plCount];
        for (int i = 0; i < _plCount; i++)
        {
            //プレイヤーを取得
            _players[i] = PlayersParent.transform.GetChild(i).gameObject;

            //テキストを生成
            _plNumTexts[i] = Instantiate(PlNumTextPrefab);
            //キャンバスの下に親子付け
            _plNumTexts[i].transform.SetParent(TextParent.transform);
            //アンカー反映しての位置0真ん中初期化　(この工程は別になくてもいい)
            _plNumTexts[i].transform.localPosition = new Vector3(0, 0, 0);
            //色
            _plNumTexts[i].color = _plColor[i];
            //テキスト「▼1P」
            _plNumTexts[i].text = "▼" + ( i + 1) +"P";
            //テキストサイズ
            _plNumTexts[i].fontSize = FONT_SIZE;
            _plNumTexts[i].resizeTextMaxSize = FONT_SIZE;
            _plNumTexts[i].GetComponent<RectTransform>().sizeDelta = sizeXY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのスクリーン上の位置を取得して追従
        //メインカメラ取得
        Camera camera = Camera.main;

        for(int i = 0; i < _plCount; i++)
        {
            //プレイヤーの位置取得
            Vector3 posW = _players[i].transform.position;
            //頭上に表示したいので少し上に
            posW.y += OFFSET_Y;

            //スクリーン座標に変換してテキストの位置に代入
            _plNumTexts[i].transform.position = camera.WorldToScreenPoint(posW);
        }
    }
}
