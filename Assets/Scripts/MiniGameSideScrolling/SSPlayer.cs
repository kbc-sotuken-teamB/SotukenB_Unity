using UnityEngine;
public class SSPlayer: MonoBehaviour
{
    //--パラメータ
    //何Pか指定
    public int PlayerNum = 0;
    //--定数
    //スピード
    private const float SPEED = 3.0f;

    //--メンバ変数
    //死んでる？
    private bool _isDead = false;

    //キャラコン
    private CharacterController _controller;
    void MakeName()
    {
        string player = "Player"+ PlayerNum;
    }
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        
        if (_isDead)
        {
            
        }
    }
}