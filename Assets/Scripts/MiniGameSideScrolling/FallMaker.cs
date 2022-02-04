using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//落ちる床量産クラス
public class FallMaker : MonoBehaviour
{
    public int FloorNum = 20;
    public GameObject fallFloor;
    private Vector3 startPosition = new Vector3(0.0f,-1.5f,-3.0f);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < FloorNum; i++)
        {
            GameObject fall = Instantiate(fallFloor);
            fall.name = "fallFloor"+i;
            fall = (GameObject)Instantiate(fall, startPosition, Quaternion.identity);//座標と回転を設定してインスタンスを生成
            startPosition.z += 4.0f;
        }
    }

    private GameObject Instantiate(GameObject fall, float startPosition, Quaternion identity)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
