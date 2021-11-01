﻿using System.Collections.Generic;
using UnityEngine;
using System.Threading;
//障害物のクラス。めっちゃFind使ってます。重い死ねってなったら解決法教えて下し亜。
public class SSObstacle : MonoBehaviour
{
    //private bool isCreate = false;//障害物を作るかどうかのフラグ。いらないかもしれない。
    private int yokoNum = 5;      //障害物を横に並べる数
    private int yokoMax = 4;      //障害物横の最大数
    private int tateNum = 10;      //障害物縦の数
    private Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private const float moveZ = 7.0f;//縦にずらす距離。
    private const float moveX = 3.0f;//1マスの横のサイズ。
    public GameObject obstract;
    private bool Start()
    {
        for (int i = 0; i < tateNum; i++)//縦
        {
            startPosition.z += moveZ;//初期座標のままだとプレイヤーと被るので少し前に持っていく
            startPosition.x = -10.5f;//初期座標を先のコードでずらしてるから戻す。
            int createCount = 0;
            for (int j = 0; j < yokoNum; j++)//横
            {
                if (createCount < yokoMax && BoolRandom() == true)
                {
                    obstract =　(GameObject)Instantiate(obstract, startPosition, Quaternion.identity);//座標と回転を設定してインスタンスを生成
                    createCount++;
                }
                startPosition.x += moveX;//1マス分横にずらす
            }
        }
        return true;
    }

    private void Update()
    {
    }

    /// <summary>
    /// bool型の乱数
    /// </summary>
    /// <returns>bool型の乱数</returns>
    private bool BoolRandom()
    {
        return Random.Range(0, 2) == 0;
    }
}