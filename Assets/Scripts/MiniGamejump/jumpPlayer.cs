using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpPlayer : MonoBehaviour
{
    //--パラメータ
    public Text TextPoint;

    //自分が何Pか
    int _plNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //何Pか指定してオフセットを作成
    /*public void SetPlNum(int num)
    {
        _plNum = num;

        switch (num)
        {
            case 1:
                _plOffset = new Vector3(-OFFSET_DURATION, 0.0f, OFFSET_DURATION);
                break;
            case 2:
                _plOffset = new Vector3(OFFSET_DURATION, 0.0f, OFFSET_DURATION);
                break;
            case 3:
                _plOffset = new Vector3(-OFFSET_DURATION, 0.0f, -OFFSET_DURATION);
                break;
            case 4:
                _plOffset = new Vector3(-OFFSET_DURATION, 0.0f, OFFSET_DURATION);
                break;
        }
    }*/
}
