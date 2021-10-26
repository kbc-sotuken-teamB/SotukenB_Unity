using UnityEngine;

//障害物のクラス。めっちゃFind使ってます。重い死ねってなったら解決法教えて下し亜。
public class SSObstacle : MonoBehaviour
{
    private int OBSTRACT_MAX = 25;//妨害箱の最大数
    private GameObject[] obstract;
    private GameObject wall;

    //Start is called before the first frame update
    private void Start()
    {
        obstract = new GameObject[OBSTRACT_MAX];

        wall = GameObject.Find("Wall");

        for (int i = 0; i < OBSTRACT_MAX; i++)          //オブジェクトの最大数廻す
        {
            int num = i + 1;                            //オブジェクトの名前を1番から作ったバカがいると聞いて。
            string myName = "Obstacle" + num.ToString();//オブジェクトの名前完成
            obstract[i] = GameObject.Find(myName);      //Find関数使ってるだけ
        }
        RandomSelectMove();                             //ランダムに障害物を上にずらす関数。ランダム生成でよくね。
    }

    // Update is called once per frame
    private void Update()
    {
        //アップデートに何もないってま！？
    }

    //ランダムに箱を最大5×4まで障害物として配置します。数は変わります。
    private void RandomSelectMove()
    {
        int countMax = 4;
        int count2 = 0;
        System.Random random = new System.Random();
        for (int i = 0; i < 5; i++)//障害物縦列
        {
            count2++;
            int count = 0;
            for (int j = 0; j < 5; j++)//障害物横列
            {
                int rand = random.Next(random.Next(0, 2));              //0か1をランダムで選ぶ
                if (count <= countMax)                                  //箱を出した数が4つ以下なら
                {
                    Vector3 pos = obstract[count2].transform.position;  //どうやらC#では直接構造体の変数を弄れないと聞いて。
                    if (rand == 0)
                    {
                        pos.y = 0;                                      //Y=0座標です。0だと妨害される高さなんで。

                        count++;                                        //カウントを進める
                    }
                    obstract[count2].transform.position = pos;          //座標の更新。
                }
            }
        }
    }
}