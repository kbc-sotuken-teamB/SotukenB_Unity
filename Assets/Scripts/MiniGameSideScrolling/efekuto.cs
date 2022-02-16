using UnityEngine;
using System.Collections;
//落下時溶岩に突っ込んだ時のエフェクトとなります
//座標が溶岩以下になったら発生
public class efekuto : MonoBehaviour
{

	//　出現させるエフェクト
	[SerializeField]
	private GameObject effectObject;
	//　エフェクトを消す秒数
	[SerializeField]
	private float deleteTime;
	//　エフェクトの出現位置のオフセット値
	[SerializeField]
	private float offset;
	//　出現させるエフェクト
	[SerializeField]
	private GameObject effectObject2;
	//　エフェクトを消す秒数
	[SerializeField]
	private float deleteTime2;
	//　エフェクトの出現位置のオフセット値
	[SerializeField]
	private float offset2;
	//プレイヤーをセットorその他落下して燃やす物が在れば。
	public GameObject target;
	//1度だけ再生するときに使うフラグ。
	private bool IsPlay = false;
	//エフェクトの大きさ
	Vector3 efSize = Vector3.one;
	Vector3 efSize2 = Vector3.one;	
	Vector3 efSizeTransScale = new Vector3(0.01f, 0.01f, 0.01f);
	// Use this for initialization
	void Start()
	{
		////　ゲームオブジェクト登場時にエフェクトをインスタンス化
		//var instantiateEffect = GameObject.Instantiate(effectObject, target.transform.position, target.transform.rotation) as GameObject;
		//Destroy(instantiateEffect, deleteTime);
		//Debug.Log(target.transform.position);
	}
	void Update()
    {
		BombSound();
	}
	void BombSound()
    {
		if (target.transform.position.y < -3 && IsPlay == false)
		{
			var instantiateEffect = GameObject.Instantiate(effectObject2, target.transform.position, target.transform.rotation) as GameObject;
			Destroy(instantiateEffect, deleteTime2);
			IsPlay = true;
			GameObject obj = (GameObject)Resources.Load("bomSound");
			; Instantiate(obj, Vector3.zero, Quaternion.identity);
		}
		efSize -= efSizeTransScale;
		effectObject.transform.localScale = efSize;
	}
}