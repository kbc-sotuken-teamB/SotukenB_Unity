using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    float MOVE_DURATION = 0.4f;
    float _moveTime;

    bool _moveEnable = false;

    Vector3 _oldPos;
    Vector3 _targetPos;

    Quaternion _oldRot;
    Quaternion _targetRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void InitMove(Vector3 targetPos, Vector3 targetAngle)
    {
        _oldPos = transform.position;
        _oldRot = transform.rotation;

        _targetPos = targetPos;
        _targetRot = Quaternion.Euler(targetAngle);

        _moveTime = 0.0f;
        _moveEnable = true;
    }

    public bool Move()
    {
        if (!_moveEnable)
        {
            return true;
        }

        _moveTime = Mathf.Min(MOVE_DURATION, _moveTime + Time.deltaTime);

        transform.position = Vector3.Lerp(_oldPos, _targetPos, _moveTime / MOVE_DURATION);
        transform.rotation = Quaternion.Slerp(_oldRot, _targetRot, _moveTime / MOVE_DURATION);


        //移動終わったら
        if (_moveTime == MOVE_DURATION)
        {
            _moveTime = 0.0f;
            _moveEnable = false;

            return true;
        }

        return false;
    }



}
