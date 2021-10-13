using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSWallMove : MonoBehaviour
{
    public int PlayerNumber;

    public static Vector3 wallPosition = new Vector3();//壁の座標
    const float SPEED = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.Translate(new Vector3(0.0f, 0.0f, 1 * SPEED * Time.deltaTime));
    }
    //プレイヤーを殺すなり除外するなりの処理。多分スコアを残すために場所を変えるだけの処理になるんじゃないかな
    void KillPlayer()
    {
        
    }

    string playerName = "Player";
   
    void FindPlayer()
    {

        //_plNumTextHorizontal = PlayerNum.ToString() + "PHorizontal";
        //名前の付け方やばくて草
        GameObject[] player = {
            GameObject.Find(playerName+"0"),
            GameObject.Find(playerName+"1"),
            GameObject.Find(playerName+"2"),
            GameObject.Find(playerName+"3")
        };
    }
}
