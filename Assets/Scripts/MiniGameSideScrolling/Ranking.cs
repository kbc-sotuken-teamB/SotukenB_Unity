using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//死んだ数+ゴールした数がプレイヤー人数を超えたらシーンを切り替えるスクリプト。
public class Ranking : MonoBehaviour
{
    [SerializeField]  GameObject[] scrollPlayer;
    ScrollPlayer[] scrollPlayers = new ScrollPlayer[4];
    [SerializeField] Image image;
    [SerializeField] Vector3 leftMax = new Vector3(0.0f,0.0f,0.0f);
    [SerializeField] Vector3 startPos = new Vector3(600.0f, 255.0f, 0.0f);
    [SerializeField] Vector3 moveSpeed = new Vector3(-5.0f, 0.0f, 0.0f);
    [SerializeField] float delay = 0;
    MainGameData.SMainGameData mainGameData;// = MainGameData.Instance.SMainData;
    bool imageMoveEnd = false;
    bool[] isPlayerEnd = new bool[4];
    int count = 0;
    int winScore = 10;
    int loseScore = 3;
    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            scrollPlayers[i] = scrollPlayer[i].GetComponent<ScrollPlayer>();
        }
        mainGameData = MainGameData.Instance.SMainData;
    }

    // Update is called once per frame
    private void Update()
    {
        if (count >= 4)
        {
            MoveImage();
        }
        if (imageMoveEnd)
        {
            delay -= Time.deltaTime;//動き終わった後のディレイ
            if (delay < 0)
            {
                AddPoint();
                SceneManager.LoadScene("MainGameScene");//メインゲームに戻るぜ。
            }
        }
    }

    private bool[] flag = new bool[4];

    public void PlayerIsEnd(int myNum)
    {
        isPlayerEnd[myNum] = true;
        if (flag[myNum] == false)
        {
            count++;
            flag[myNum] = true;
        }
    }

    public void MoveImage()
    {
        if (image.transform.position.x > leftMax.x)//Imageの移動処理
        {
            image.transform.Translate(moveSpeed);
        }
        else
        {
            imageMoveEnd = true;
        }
    }

    void AddPoint()
    {
        
        //MainGameData.SMainGameData mainGameData = MainGameData.Instance.SMainData;
        
        for(int i= 0; i < 4; i++)
        {
            if(scrollPlayers[i].GetIsWin())
            {
                mainGameData.Points[i] = winScore;
            }
            else
            {
                mainGameData.Points[i] = loseScore;
            }
        }
    }
}