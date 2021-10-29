using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//データ保持用

public class MainGaveData : MonoBehaviour
{
    public static MainGaveData InstanceMainGameData
    {
        get; private set;
    }


    int[] _currentSquares = Enumerable.Repeat(0, 4).ToArray();
    int[] _points = Enumerable.Repeat(0, 4).ToArray();



    private void Awake()
    {
        if(InstanceMainGameData != null)
        {
            Debug.Log("single destroy");
            Destroy(gameObject);
            return;
        }

        InstanceMainGameData = this;
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


    public void SaveParam(int[] currentSquares, int[] points)
    {
        for(int i = 0; i < currentSquares.Length; i++)
        {
            _currentSquares[i] = currentSquares[i];
            _points[i] = points[i];
        }
    }

    //ロードはこっちからじゃなくて　メインゲーム側から取得した方がいいかな
    public void LoadParam()
    {

    }


}
