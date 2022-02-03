using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーを落下させるだけの予定。
//もしかしたら死ぬときの動きもここに書くかもしれない
public class Rakka : MonoBehaviour
{
    public GameObject obj;//落下させたいオブジェクトをD&D
    public float speed = 1.0f;   //速度入れてね。
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Translate(Vector3.up * -1 * speed);
    }
}
