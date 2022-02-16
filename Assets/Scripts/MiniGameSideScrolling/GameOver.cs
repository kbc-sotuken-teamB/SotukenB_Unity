using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ゲームが終わったときの処理。
//オブジェクトをプレファブ化して、終わったタイミングで出す。

public class GameOver : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float leftMax = -304.3f;
    [SerializeField] Vector3 startPos = new Vector3(600.0f, 255.0f, 0.0f);
    [SerializeField] Vector3 moveSpeed = new Vector3(-5.0f,0.0f,0.0f);
    // Start is called before the first frame update
    void Start()
    {
        image.transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(image.transform.position.x > leftMax)
        {
            image.transform.Translate(moveSpeed);
        }
        
    }
}
