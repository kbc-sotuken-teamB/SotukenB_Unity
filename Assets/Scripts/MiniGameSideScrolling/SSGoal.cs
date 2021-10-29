using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SSGoal : MonoBehaviour
{
    private GameObject[] player;//プレイヤー
    private GameObject  goal;
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー4人を探す
        string playerName = "Player";
        player = new GameObject[5];
        for (int i = 0; i < 4; i++)
        {
            string num = i.ToString();
            player[i] = GameObject.Find(playerName + num);
        }
        //ゴールを探す。
        goal = GameObject.Find("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (player[i].transform.position.z > goal.transform.position.z)
            {
                SceneManager.LoadScene("MainGameScene");
            }
        }
    }


}
