using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//データ保持用
//プレイヤーそれぞれの現在マスとポイント、アイテムのデータのみ保持
//MainGame開始時にロードして再配置する

public class MainGameData : MonoBehaviour
{
    public static MainGameData Instance
    {
        get; private set;
    }

    //構造体
    public struct SMainGameData
    {
        //プレイヤーたちの現在マス
        public int[] CurrentSquares;
        //プレイヤーたちの所持ポイント
        public int[] Points;
        //今何Pのターンか
        public int CurrentPlayer;
        //今何ターン目か
        public int CurrentTurn;
    }

    SMainGameData _sMainGameData;
    public SMainGameData SMainData { get { return _sMainGameData; } }

    //これ構造体使う必要あるか？
    //外から読み込むときMainGameDataのgameObjectとかと混ざってゴチャッとせず分かりやすくなった


    //プレイヤーたちの現在マス
    /*int[] _currentSquares = Enumerable.Repeat(0, 4).ToArray();
    public int[] CurrentSquares { get { return _currentSquares; } }
    //プレイヤーたちの所持ポイント
    int[] _points = Enumerable.Repeat(0, 4).ToArray();
    public int[] Points { get { return _points; } }*/


    private void Awake()
    {
        //既に存在したら消す
        if(Instance != null)
        {
            Debug.Log("single destroy");
            Destroy(gameObject);
            return;
        }

        //構造体初期化
        _sMainGameData = new SMainGameData
        {
            //0埋め
            CurrentSquares = Enumerable.Repeat(0, 4).ToArray(),
            //10埋め
            Points = Enumerable.Repeat(10, 4).ToArray(),
            //0
            CurrentPlayer = 0,
            //1
            CurrentTurn = 1,
        };

        Instance = this;
        //シーン遷移しても消えないようにする
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //データセーブ　メインゲームから他シーンへの遷移前に呼ぶ
    //セーブ項目多くなったら構造体にしようか
    /*public void SaveParam(int[] currentSquares, int[] points)
    {
        for(int i = 0; i < currentSquares.Length; i++)
        {
            _currentSquares[i] = currentSquares[i];
            _points[i] = points[i];
        }
    }*/
    public void SaveParam(SMainGameData mainGameData)
    {
        _sMainGameData = mainGameData;
    }
}
