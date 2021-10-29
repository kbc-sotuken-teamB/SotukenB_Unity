using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpCamera : MonoBehaviour
{

    //　カメラの分割方法
    public enum SplitCameraMode
    {
        horizontal,
        vertical,
        square
    };

    public SplitCameraMode mode;    //　カメラの分割方法

    //　プレイヤーを写すそれぞれのカメラ
    public Camera Camera1P;
    public Camera Camera2P;
    public Camera Camera3P;
    public Camera Camera4P;

    // Start is called before the first frame update
    void Start()
    {
        //　４プレイヤー用の4分割
        if (mode == SplitCameraMode.square)
        {
            //　カメラのViewPortRectの変更
            Camera1P.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
            Camera2P.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            Camera3P.rect = new Rect(0f, 0f, 0.5f, 0.5f);
            Camera4P.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
