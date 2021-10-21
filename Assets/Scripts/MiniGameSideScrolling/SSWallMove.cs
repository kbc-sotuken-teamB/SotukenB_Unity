using UnityEngine;

//ありえんコード汚いです。すみません。

public class SSWallMove : MonoBehaviour
{
    //配列とfor文を知らぬ男。
    public int PlayerNumber;

    private GameObject player0;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject wall;
    private const float SPEED = 3.0f;

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
        Vector3 pPos0 = player0.transform.position;
        Vector3 pPos1 = player1.transform.position;
        Vector3 pPos2 = player2.transform.position;
        Vector3 pPos3 = player3.transform.position;
        Vector3 deathPos = new Vector3(0.0f, -1000.0f, 0.0f);

        if (player0.transform.position.z < wall.transform.position.z)
        {
            player0.GetComponent<CharacterController>().Move(deathPos);
        }

        if (player1.transform.position.z < wall.transform.position.z)
        {
            player1.GetComponent<CharacterController>().Move(deathPos);
        }

        if (player2.transform.position.z < wall.transform.position.z)
        {
            player2.GetComponent<CharacterController>().Move(deathPos);
        }

        if (player3.transform.position.z < wall.transform.position.z)
        {
            player3.GetComponent<CharacterController>().Move(deathPos);
        }
    }

    private string playerName = "Player";

    private void Find()
    {
        player0 = GameObject.Find(playerName + "0");//プレイヤー0
        player1 = GameObject.Find(playerName + "1");//プレイヤー1
        player2 = GameObject.Find(playerName + "2");//プレイヤー2
        player3 = GameObject.Find(playerName + "3");//プレイヤー3
        wall = GameObject.Find("Wall");             //迫りくる壁。毒ガスっぽい何かになるかもしれない。
    }
}