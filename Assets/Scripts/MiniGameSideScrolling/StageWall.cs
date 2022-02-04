using UnityEngine;

//Z方向に複製しまくるヤツ。壁作るのに使ってる。
//オブジェクトの配置がめんどくさくなってしまった奴。
public class StageWall : MonoBehaviour
{
    public GameObject obj;
    private Vector3 pos = new Vector3(-10.0f, -0.5f, -3.0f);
    public int kabeNoKazu = 20;
    private int posNum = 2;

    // Start is called before the first frame update
    private void Start()
    {
        for (int j = 0; j < posNum; j++)
        {
            for (int i = 0; i < kabeNoKazu; i++)
            {
                GameObject wallObj = Instantiate(obj);
                wallObj.name = "wall" + i;
                wallObj = (GameObject)Instantiate(wallObj, pos, Quaternion.identity);
                pos.z += 4.0f;
            }
            pos.x += 20.0f;
            pos.z = 0.0f;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}