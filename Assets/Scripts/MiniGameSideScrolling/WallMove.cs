using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public int PlayerNumber;
    public static Vector3 wallPosition = new Vector3();//壁の座標
    const float SPEED = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.Translate(new Vector3(0.0f, 0.0f, 1 * SPEED * Time.deltaTime));
    }

    void KillPlayer()
    {
        
    }
    string player;
    void FindPlayer()
    {
        //_plNumTextHorizontal = PlayerNum.ToString() + "PHorizontal";
        player = PlayerNumber.ToString() + "Player";
    }
}
