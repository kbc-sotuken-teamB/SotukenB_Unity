using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    時間経過で消去するスクリプト
 */
public class DeleteScript : MonoBehaviour
{
   
    //死ぬまでの時間
    float DELETE_TIME = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DELETE_TIME -= Time.deltaTime;
        if (DELETE_TIME <= 0.0f)
        {
            Destroy(this.gameObject);
        }

        //Vector3 pPos = GameObject.Find("player").transform.position;
        //Vector3 itemPos = this.gameObject.transform.position;

    }
}
