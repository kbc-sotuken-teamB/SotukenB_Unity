using UnityEngine;

/// <summary>
/// ゴールまたは全員死亡時にメインゲームに戻るよう書いています
/// 死ぬほど頭悪ポン太郎なので良い書き方あったら教えてください
/// </summary>
public class SSGoal : MonoBehaviour
{
    public GameObject[] player;//プレイヤー
    private GameObject goal;
    private int playerMax = 4;
    private bool[] isWin = new bool[4];
    private Ranking rank;

    // Start is called before the first frame update
    private void Start()
    {
        //ゴールを探す。
        goal = GameObject.Find("Goal");
        rank = GetComponent<Ranking>();
    }

    // Update is called once per frame

    private void Update()
    {

        IsGoal();
    }

    //プレイヤーが死んだとき
    private bool[] isGoalOrDead = new bool[4];

    private void IsGoal()
    {
        for (int i = 0; i < playerMax; i++)
        {
            if (player[i].transform.position.z > goal.transform.position.z && isWin[i] == false)
            {
                rank.PlayerIsWinOrDead();

                isWin[i] = true;
            }
        }
    }

}