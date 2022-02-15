using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerText : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    Vector3 pos = new Vector3(-0.5f,0.0f,1.5f);
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + pos;
        
    }

    void BillBord()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
