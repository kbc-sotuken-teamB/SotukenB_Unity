using UnityEngine;
public class SSWallMove : MonoBehaviour
{
    public int PlayerNumber;
    public GameObject[] player;
    private GameObject wall;
    Ranking rank;
    private bool[] deathFlag = new bool[4];
    private const float SPEED = 3.0f;
    // Start is called before the first frame update
    private object Start()
    {
        //Find();
        wall = GameObject.Find("Wall");             //迫りくる壁。毒xガスっぽい何かになるかもしれない。
        //rank = gameObject.GetComponent(typeof(GameObject)) as Ranking;
        rank = gameObject.GetComponent<Ranking>();
        Debug.Log(rank);
        return true;
    }

    // Update is called once per frame
    private void Update()
    {
        KillPlayer();
        wall.transform.Translate(new Vector3(0.0f, 0.0f, SPEED * Time.deltaTime));//壁を前に進める
   
    }

    //プレイヤーを殺すなり除外するなりの処理。死亡エフェクトとともにプレイヤーをカメラ外にぶん投げます。エフェクトの実装は座して待て。
    private void KillPlayer()
    {
        //各プレイヤーの座標と死んだときの座標
        Vector3 deathPos = new Vector3(500.0f, -0.0f, 0.0f);
        float zSize = 2.0f;

        for (int i = 0; i < 4; i++)
        {
            if (player[i].transform.position.z < wall.transform.position.z + zSize
                && deathFlag[i] == false
                )
            {
                player[i].GetComponent<CharacterController>().Move(deathPos);
                deathFlag[i] = true;
                rank.HerasuRemainingPlayer();
            }
        }
    }
}