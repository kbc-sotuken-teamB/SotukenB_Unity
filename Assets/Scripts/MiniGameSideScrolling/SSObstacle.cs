using UnityEngine;

//障害物を生成するクラス
public class SSObstacle : MonoBehaviour
{
    Vector3 startPosition = new Vector3(-6.0f, 0.0f, 10.0f);
    public GameObject obstract;

    private bool Start()
    {
        //座標と回転を設定してインスタンスを生成
        for(int i = 0; i < 13; i++) {
            
        }
        obstract = (GameObject)Instantiate(obstract, startPosition, Quaternion.identity);
       
        return true;
    }

    private void Update()
    {
        int a = 0;
        if (a == 0)
        {
        }
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