using UnityEngine;
using UnityEngine.SceneManagement;

//死んだ数+ゴールした数がプレイヤー人数を超えたらシーンを切り替えるスクリプト。
public class Ranking : MonoBehaviour
{
    public GameObject[] playerObj = new GameObject[4];
    ScrollPlayer[] scrollPlayer = new ScrollPlayer[4];
    int count = 0;
    bool[] EndPlayer = new bool[4];
    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            scrollPlayer[i] = playerObj[i].GetComponent<ScrollPlayer>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        EndPlayerCount();
    }

    public void NextScene()
    {
        if(count >= 4)
        {
            SceneManager.LoadScene("MainGameScene");//メインゲームに戻るぜ。
        }
    }
    void EndPlayerCount()
    {
        for(int i = 0; i < 4; i++)
        {
            if(EndPlayer[i] == false)
            {
                EndPlayer[i] = scrollPlayer[i].GetIsEnd();
                count++;
            }
        }
    }
}