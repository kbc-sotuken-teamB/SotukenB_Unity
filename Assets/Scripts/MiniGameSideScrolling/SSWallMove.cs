using UnityEngine;

//wallMoveとか書いておきながら
//動かすコード以外も書いているかもしれない致命的なコード。
public class SSWallMove : MonoBehaviour
{
    //public変数。
    public GameObject[] player;

    //private変数。
    private GameObject RankingObj;

    private Ranking rank;
    private GameObject wall;
    private bool[] deathFlag = new bool[4];
    private const float SPEED = 3.0f;

    // Start is called before the first frame update
    private object Start()
    {
        wall = GameObject.Find("Wall");             //迫りくる壁。毒xガスっぽい何かになるかもしれない。
        RankingObj = GameObject.Find("Ranking");
        rank = RankingObj.GetComponent<Ranking>();
        return true;
    }

    // Update is called once per frame
    private void Update()
    {
        KillPlayer();
    }

    //プレイヤーを殺すなり除外するなりの処理。死亡エフェクトとともにプレイヤーをカメラ外にぶん投げます。エフェクトの実装は座して待て。
    private void KillPlayer()
    {
        //各プレイヤーの座標と死んだときの座標
        Vector3 deathPos = new Vector3(0.0f, 21000.0f, 0.0f);
        float zSize = 2.0f;

        for (int i = 0; i < 4; i++)
        {
            if (player[i].transform.position.z < wall.transform.position.z + zSize && deathFlag[i] == false)
            {
                player[i].GetComponent<CharacterController>().Move(deathPos);

                rank.PlayerIsWinOrDead();
                deathFlag[i] = true;
            }
        }
    }
}