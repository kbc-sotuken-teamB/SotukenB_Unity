using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//1P 2P…　みたいなテキストを生成してプレイヤーの頭にくっつけるやつ

public class PlayerNumTextManager : MonoBehaviour
{
    public GameObject PlayersParent;
    public Text PlNumTextPrefab;
    public GameObject CanvasChild;

    GameObject[] _players;
    Text[] _plNumTexts;

    Color[] _plColor = new Color[]{ Color.red, Color.blue, Color.yellow, Color.green };

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーを取得
        int plCount = PlayersParent.transform.childCount;
        _players = new GameObject[plCount];
        _plNumTexts = new Text[plCount];
        for (int i = 0; i < plCount; i++)
        {
            _players[i] = PlayersParent.transform.GetChild(i).gameObject;

            //テキストを生成
            _plNumTexts[i] = Instantiate(PlNumTextPrefab);
            //キャンバスの下に
            _plNumTexts[i].transform.SetParent(CanvasChild.transform);
            //アンカー反映しての位置0真ん中
            _plNumTexts[i].transform.localPosition = new Vector3(0, 0, 0);
            _plNumTexts[i].color = _plColor[i];
            _plNumTexts[i].text = "▼" + ( i + 1) +"P";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのスクリーン上の位置を取得して追従

        



    }
}
