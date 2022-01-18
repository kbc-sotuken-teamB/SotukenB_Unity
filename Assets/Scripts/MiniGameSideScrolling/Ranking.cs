using UnityEngine;
using UnityEngine.SceneManagement;

//死んだ数+ゴールした数がプレイヤー人数を超えたらシーンを切り替えるスクリプト。
public class Ranking : MonoBehaviour
{
    public int[] Score;            //スコア。恐らく調整するのでパブリック変数にしてあります。
    public GameObject ranking;      //ランキングを管理してるゲームオブジェクトを格納する変数
    private int remainigPlayer = 4;//残り人数
    private MainGameData mainGameData;//メインゲームデータ。なんだこれは。
    private SSGoal goal;

    // Start is called before the first frame update
    private void Start()
    {
        goal = GetComponent<SSGoal>();
        mainGameData = new MainGameData();
    }

    // Update is called once per frame
    private void Update()
    {
        NextScene();
    }

    public void HerasuRemainingPlayer()
    {
        remainigPlayer--;
        if (remainigPlayer <= 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (goal.IsWinner(i) == true)
                {
                    mainGameData.SMainData.Points[i] += 50;
                }
            }
        }
    }

    private void NextScene()
    {
        if (remainigPlayer <= 0)
        {
            SceneManager.LoadScene("MainGameScene");
        }
    }
}