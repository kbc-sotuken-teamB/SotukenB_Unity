using UnityEngine;

//落ちる床
//カメラの座標を取得
//一定距離まで近づいたら落ちる
//カメラは前に進むから後ろから順番に落ちていく事を想定して作ってるやつ
//落ち方　上に少し上がってから左から落ちていく感じ。
public class FallFloor : MonoBehaviour
{
    public GameObject Camera;//カメラの座標を使うのでゲームカメラD&D
    public float downLange = 1.0f;
    bool downMove = false;
    bool upMove = false;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.transform.position.z < Camera.transform.position.z - downLange)
        {
            if (1.0f < gameObject.transform.position.y)
            {
                gameObject.transform.Translate(Vector3.down * 5 * Time.deltaTime);
                gameObject.transform.Rotate(0.0f, 0.0f, 30.0f * Time.deltaTime);
            }
            else
            {
                gameObject.transform.Translate(0.0f, 0.1f, 0.0f);
            }
        }
    }
    void Move()
    {
        
       
    }
}