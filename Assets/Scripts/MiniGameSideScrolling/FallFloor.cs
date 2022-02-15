using UnityEngine;

//落ちる床
//カメラの座標を取得
//一定距離まで近づいたら落ちる
//カメラは前に進むから後ろから順番に落ちていく事を想定して作ってるやつ
//落ち方　上に少し上がってから左から落ちていく感じ。
public class FallFloor : MonoBehaviour
{
    public GameObject Camera;//カメラの座標を使うのでゲームカメラD&D
    public float downLange = 3.0f;
    private float downSpeed = 10.0f;
    private bool downMove = false;
    private bool upMove = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //カメラの位置より落下板が後ろなら
        if (gameObject.transform.position.z < Camera.transform.position.z - downLange)
        {
            //一定値まで板が上がったら落とす
            if (downMove == true)
            {
                gameObject.transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
                gameObject.transform.Rotate(0.0f, 0.0f, 30.0f * Time.deltaTime);
            }
            else
            {
                if (downMove == false)
                {
                    gameObject.transform.Translate(0.0f, 0.1f, 0.0f);
                }

                if (1.0f < gameObject.transform.position.y)
                {
                    downMove = true;
                }
            }
        }
    }
}