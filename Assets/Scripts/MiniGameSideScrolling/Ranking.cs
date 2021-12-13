using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Ranking : MonoBehaviour
{
    private int remainigPlayer = 4;
    public int[] Score;
    MainGameData mainGameData;
    SSGoal goal;
    // Start is called before the first frame update
    void Start()
    {
        goal = GetComponent<SSGoal>();
        mainGameData = new MainGameData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HerasuRemainingPlayer()
    {
        remainigPlayer--;
        if (remainigPlayer <= 0)
        {
            for(int i =0; i < 4; i++)
            {
                if(goal.IsWinner(i) == true)
                {
                    mainGameData.SMainData.Points[i] += 50;
                }
            }
            SceneManager.LoadScene("MainGameScene");
        }
    }

    public int GetRemainingPlayer()
    {
        return remainigPlayer;
    }
}
