using UnityEngine;
using System;
//障害物を生成するクラス
public class SSObstacle : MonoBehaviour
{
    Vector3 startPosition = new Vector3(-6.0f, 0.0f, 10.0f);
    [SerializeField]float addX;
    [SerializeField]float addZ;
    public GameObject obstract; //邪魔ブロック
    System.Random rand = new System.Random();         //ランダム変数
    [SerializeField] int tateMax;//縦最大数
    [SerializeField] int yokoMax;// 横最大数
    private bool Start()
    {
        //座標と回転を設定してインスタンスを生成
        for(int j = 0;j < tateMax; j++)//楯列は13
        {
            int randInt = rand.Next(0, yokoMax);
            int count = 0;
            for (int i = 0; i < yokoMax; i++)//横列は4
            {
                if (count != randInt)
                {
                    obstract = (GameObject)Instantiate(obstract, startPosition, Quaternion.identity);
                }
                count++;
                startPosition.x += addX;
            }
            startPosition.x = -6.0f;
            startPosition.z += addZ;
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
        return UnityEngine.Random.Range(0, 2) == 0;
    }
}