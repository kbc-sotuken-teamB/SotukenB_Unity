using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//強制横スクロール用カメラ。横に寄っていくだけ。

//ミニゲームでのカメラどうしよう

public class SSCameraMove : MonoBehaviour
{
    public static Vector3 cameraPosition = new Vector3();//カメラの座標
    const float SPEED = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject camera = GameObject.Find("Main Camera");
       camera.transform.Translate(new Vector3(0.0f, 1 * SPEED * Time.deltaTime,0.0f));
    }
}