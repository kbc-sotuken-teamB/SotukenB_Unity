using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラの挙動

//メインゲームでのフリーカメラ

//ミニゲームでのカメラどうしよう


public class CameraMove : MonoBehaviour
{

    public static Vector3 cameraPosition = new Vector3();//カメラの座標
    float padLStick_X = 0.0f;
    float padLStick_Y = 0.0f;
    const float SPEED = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PadUpdate()
    {
        padLStick_X = Input.GetAxis("Horizontal");
        padLStick_Y = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        PadUpdate();
        MainGameFreeCamera();
    }
    void MainGameFreeCamera()
    {

        
        GameObject camera = GameObject.Find("Main Camera");

        camera.gameObject.transform.Translate(cameraPosition);
    }
}
