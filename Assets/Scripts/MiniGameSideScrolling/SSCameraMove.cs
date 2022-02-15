using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//強制横スクロール用カメラ。横に寄っていくだけ。

//ミニゲームでのカメラどうしよう

public class SSCameraMove : MonoBehaviour
{
    public static Vector3 cameraPosition = new Vector3();//カメラの座標
    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    Vector3 moveSpeed = new Vector3(0.0f, 0.0f, 1.0f);

    [SerializeField]
    float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position += new Vector3(dx * Time.deltaTime, 0, 0);
        mainCamera.transform.position += new Vector3(0.0f, 0.0f, speed*Time.deltaTime);
    }
}