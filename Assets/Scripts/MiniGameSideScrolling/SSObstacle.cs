using UnityEngine;

//障害物のクラス。めっちゃFind使ってます。重い死ねってなったら解決法教えて下し亜。
public class SSObstacle : MonoBehaviour
{
    //private const int OBSTRACT_MAX = 5;//妨害箱の最大数
    //private GameObject[] obstract = new GameObject[5];
    //private GameObject wall = new GameObject();

    //Start is called before the first frame update
    private void Start()
    {
       // RandomSelectMove();
        //obstract[1] = GameObject.Find("Obstacle" + 0);
        //obstract[2] = GameObject.Find("Obstacle" + 1);
        //obstract[3] = GameObject.Find("Obstacle" + 2);
        //obstract[4] = GameObject.Find("Obstacle" + 3);
        //obstract[5] = GameObject.Find("Obstacle" + 4);

        //wall = GameObject.Find("Wall");
    }

    // Update is called once per frame
    private void Update()
    {
        //Iranaikamo();
    }

    private void Iranaikamo()
    {
        //if (wall.transform.position.z > obstract[1].transform.position.z)
        //{
        //    RandomSelectMove();
        //}
    }

    //ランダムに箱を最大4つまで障害物として配置します。
    private void RandomSelectMove()
    {
        //int countMax = 4;
        //int count = 0;

        //System.Random random = new System.Random();
        //for (int i = 0; i < OBSTRACT_MAX; i++)
        //{
        //    int rand = random.Next(random.Next(0, 1));//0or1をランダムで作成
        //    if (count <= countMax)//箱を出した数が4つ以下なら
        //    {
        //        Vector3 pos = obstract[i].transform.position;
        //        if (rand == 0)
        //        {
        //            pos.y = 3;

        //            count++;//カウントを進める
        //        }
        //        pos.z += 10.0f;
        //        obstract[i].transform.position = pos;
        //    }
        //}
    }
}