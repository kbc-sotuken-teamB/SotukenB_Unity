using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゴールまたは全員死亡時にメインゲームに戻るよう書いています
/// 死ぬほど頭悪ポン太郎なので良い書き方あったら教えてください
/// </summary>
public class SSGoal : MonoBehaviour
{
    private GameObject[] player;//プレイヤー
    private GameObject goal;
    private int playerMax = 4;
    private bool[] winnerNum = new bool[4];

    // Start is called before the first frame update
    private void Start()
    {
        //プレイヤー4人を探す
        string playerName = "Player";
        player = new GameObject[5];
        for (int i = 0; i < 4; i++)
        {
            string num = i.ToString();//変数iを文字に変換
            player[i] = GameObject.Find(playerName + num);//プレイヤーを名前検索
        }
        //ゴールを探す。
        goal = GameObject.Find("Goal");
    }

    // Update is called once per frame

    private void Update()
    {
        GoalOrDead();
    }

    //プレイヤーが死んだとき
    private bool[] isGoalOrDead = new bool[4];

    private void GoalOrDead()
    {
        for (int i = 0; i < playerMax; i++)
        {
            if (player[i].transform.position.y < -100   //プレイヤーが死んだとき(Y座標を下げてるので
                || player[i].transform.position.z > goal.transform.position.z//プレイヤーがゴールより奥に行ったとき
                )
            {
                isGoalOrDead[i] = true;
                if (
                    isGoalOrDead[0] == true &&
                    isGoalOrDead[1] == true &&
                    isGoalOrDead[2] == true &&
                    isGoalOrDead[3] == true)
                {
                    LoadNextScene();
                }
            }
        }
    }
    private void IsGoal()
    {
        
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}