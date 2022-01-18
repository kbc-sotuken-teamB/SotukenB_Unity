using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラ等々一定速度で一方向に動くヤツ
public class Scroll : MonoBehaviour
{
    public GameObject scrollObject = new GameObject();
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrollObject.transform.Translate(0.0f, scrollSpeed * 1 * Time.deltaTime, 0.0f);
    }
}
