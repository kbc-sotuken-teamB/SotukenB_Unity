using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    float speed = 0.007f;
    private Vector3 size = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        size = gameObject.transform.localScale;//現在の大きさを代入
        size.z = size.z - speed;
        gameObject.transform.localScale = size;
    }
}
