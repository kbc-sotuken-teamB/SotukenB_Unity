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
        PlayerDeath();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    //プレイヤーが死んだとき
    private bool[] deathPlayer = new bool[4];
    private void PlayerDeath()
    {
        for (int i = 0; i < playerMax; i++)
        {
            if (player[i].transform.position.y < -100)
            {
                deathPlayer[i] = true;
                if (
                    deathPlayer[0] == true &&
                    deathPlayer[1] == true &&
                    deathPlayer[2] == true &&
                    deathPlayer[3] == true)
                {
                    LoadNextScene();
                }
            }
        }
    }
}