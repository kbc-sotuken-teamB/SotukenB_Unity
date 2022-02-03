using UnityEngine;

//落ちる床
//カメラの座標を取得
//一定距離まで近づいたら落ちる
//カメラは前に進むから後ろから順番に落ちていく事を想定して作ってるやつ

public class FallFloor : MonoBehaviour
{
    private Vector3 mainus = new Vector3(-1.0f, -5.0f, -1.0f);
    public GameObject Camera;//カメラの座標を使うのでゲームカメラD&D
    public float downLange = 1.0f;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.transform.position.z < Camera.transform.position.z- downLange)
        {
            gameObject.transform.Translate(Vector3.down * 5 * Time.deltaTime);
            gameObject.transform.Rotate(0.0f, 0.0f, 30.0f* Time.deltaTime);
        }
    }
}