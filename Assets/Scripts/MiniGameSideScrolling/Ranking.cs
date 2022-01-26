using UnityEngine;
using UnityEngine.SceneManagement;

//死んだ数+ゴールした数がプレイヤー人数を超えたらシーンを切り替えるスクリプト。
public class Ranking : MonoBehaviour
{
    public int winScore = 10;            //スコア。恐らく結構調整するのでパブリック変数にしてあります。
    public int loseScore = 3;          　//負けても何故か得点が追加されます。０点だと悲しくない？
    private int remainigPlayer = 4;      //残り人数
    private MainGameData mainGameData;   //メインゲームデータ。ゲーム終了時に得点を加算するときに使う。
    private SSGoal goal;                 //ゴールクラス。ゴールしたヤツの判定。

    // Start is called before the first frame update
    private void Start()
    {
        goal = GetComponent<SSGoal>();
        mainGameData = new MainGameData();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(remainigPlayer);
    }

    public void PlayerIsWinOrDead()
    {
        remainigPlayer--;//残りプレイヤー数を減らす
        if (remainigPlayer <= 0)//残りプレイヤー数が0以下になったら。
        {
            for (int i = 0; i < 4; i++)
            {
                if (goal.IsWinner(i) == true)
                {
                    mainGameData.SMainData.Points[i] += winScore; //ゴールしていたら得点を加算
                }
                else
                {
                    mainGameData.SMainData.Points[i] += loseScore; //ゴールしていなかったら得点を減算
                }
            }
            SceneManager.LoadScene("MainGameScene");//メインゲームに戻るぜ。
        }
    }
}