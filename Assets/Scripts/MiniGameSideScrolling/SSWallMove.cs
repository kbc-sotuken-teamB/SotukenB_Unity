using UnityEngine;

public class SSWallMove : MonoBehaviour
{
    public int PlayerNumber;
    private GameObject[] player;
    private GameObject wall;
    private bool[] deathFlag = new bool[4];
    private const float SPEED = 3.0f;

    //各順位のスコアです。0番目を1位として使います。
    public int[] RankingScore = new int[4];
    private int ranking = 3;//最下位から。
    private bool[] nandakore = new bool[4];
    // Start is called before the first frame update
    private void Start()
    {
        Find();
    }

    // Update is called once per frame
    private void Update()
    {
        KillPlayer();
        wall.transform.Translate(new Vector3(0.0f, 0.0f, 1 * SPEED * Time.deltaTime));//壁を前に進める
   
    }

    //プレイヤーを殺すなり除外するなりの処理。死亡エフェクトとともにプレイヤーをカメラ外にぶん投げます。エフェクトの実装は座して待て。
    private void KillPlayer()
    {
        //各プレイヤーの座標と死んだときの座標
        Vector3 deathPos = new Vector3(0.0f, -500.0f, 0.0f);
        float zSize = 2.0f;

        for (int i = 0; i < 4; i++)
        {
            if (player[i].transform.position.z < wall.transform.position.z + zSize
                && deathFlag[i] == false
                )
            {
                player[i].GetComponent<CharacterController>().Move(deathPos);
                deathFlag[i] = true;
                
                //player[i].SetScore(RankingScore[ranking]);
                ranking--;
            }
        }
    }
        
    private void Find()
    {
        string playerName = "Player";
        player = new GameObject[5];
        for (int i = 0; i < 4; i++)
        {
            string num = i.ToString();
            player[i] = GameObject.Find(playerName + num);
        }
        wall = GameObject.Find("Wall");             //迫りくる壁。毒xガスっぽい何かになるかもしれない。
    }
}