using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//データ保持用

public class MainGameData : MonoBehaviour
{
    public static MainGameData Instance
    {
        get; private set;
    }


    int[] _currentSquares = Enumerable.Repeat(0, 4).ToArray();
    public int[] CurrentSquares { get { return _currentSquares; } }
    int[] _points = Enumerable.Repeat(0, 4).ToArray();
    public int[] Points { get { return _points; } }


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("single destroy");
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    //セーブする
    public void SaveParam(int[] currentSquares, int[] points)
    {
        for(int i = 0; i < currentSquares.Length; i++)
        {
            _currentSquares[i] = currentSquares[i];
            _points[i] = points[i];
        }
    }

    //ロードはこっちからじゃなくて　メインゲーム側から取得した方がいいかな
    /*public void LoadParam()
    {

    }*/


}
