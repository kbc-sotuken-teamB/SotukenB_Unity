using UnityEngine;

//縦スクロール中に追加されるプレイヤーの色々
public class ScrollPlayer : MonoBehaviour
{
    public GameObject goalObj;              //ゴールのオブジェクト
    public GameObject rankingObj;           //ランキングを管理してるオブジェクト
    private Ranking ranking;                //↑のスクリプト。
    private bool isRakka = false;           //落下した？
    private bool isGoal = false;            //ゴールした？
    private bool isEnd = false;             //プレイヤーが動く必要がない状態（死んだかゴールしたか。
    private float rakkaSpeed = 5.0f;        //落ちる速度
    private const float deathHight = -6.0f; //死ぬ高さ。多分固定。
    private MainGameData mainGameData;      //メインゲームのデータ、得点の加減算に使う。
    [SerializeField] private int number = 0;//このプレイヤーの番号。
    [SerializeField] private int Point = 10;//得点

    // Start is called before the first frame update
    private void Start()
    {
        ranking = rankingObj.GetComponent<Ranking>();
    }

    // Update is called once per frame
    private void Update()
    {
        Rakka();//落下処理
        IsGoal();//ゴールした時の処理
        IsDeath();//落ちた時の処理
    }

    private void Rakka()//下に落ちるだけ。キャラコンでなんかありそう。
    {
        this.gameObject.transform.Translate(0.0f, -1.0f, 0.0f);
    }

    private void IsGoal()//ゴールの座標を過ぎ去ればクリア。
    {
        if (goalObj.transform.position.z < gameObject.transform.position.z
            && isGoal == false
            && isRakka == false)
        {
            ranking.PlayerIsEnd(number);
            mainGameData.SMainData.Points[number] += Point;
            isGoal = true;
            isEnd = true;
        }
    }

    private void IsDeath()//座標Yが一定値を下回ったら死ぬ。
    {
        if (gameObject.transform.position.y < deathHight
            && isRakka == false)
        {
            ranking.PlayerIsEnd(number);
            isRakka = true;
            isEnd = true;
            //mainGameData.SMainData.Points[number] += Point / 2;
        }
    }
    void EndPlayerMove()
    {
        if(isEnd == true)
        {
            gameObject.transform.Translate(0.0f, 0.0f, -5.0f);
        }
    }
}